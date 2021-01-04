using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// DeviceDrive abstraction is used to find all available USB devices matching information listed in deviceInfoMap dictionary.
    /// Within the dictionary contains all available devices and their db file paths with format e.g. XR5000-SerialNumber, E:\\db
    /// 
    /// SZ: Uncomment usbDrivePathDict when need all available USB devices.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent startScanningUSBDevices: Event to begin scanning USB devices.
    /// 2. IDataFlow<string> usbDeviceName: the first found USB device name without serial number, e.g. XR5000
    /// 3. IDataFlow<string> usbDeviceDbPath: the first found USB device db file path, e.g. E:\\db
    /// 4. IDataFlow<string> deviceDrivePath: The path to the USB device drive.
    /// 5. IDataFlow<string> deviceFirmwareVersion: The current firmware version of the scale.
    /// </summary>
    public class DeviceDrive : IEvent // startScanningUSBDevices
    {
        private struct ScaleInfo
        {
            public string DbPath { get; set; }
            public string Version { get; set; }

            public ScaleInfo(string dbPath, string version)
            {
                DbPath = dbPath;
                Version = version;
            }
        }

        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // ports ---------------------------------------------------------------
        private IDataFlow<string> usbDeviceName;
        private IDataFlow<string> usbDeviceDbPath;
        private IDataFlow<string> deviceDrivePath;
        private IDataFlow<string> deviceFirmwareVersion;
        // private IDataFlow<Dictionary<string, ScaleInfo>> usbDrivePathDict; // DeviceType, drive path, e.g. <"XR5000-1c:ba:8c:a8:35:5d", @"E:\db"> not in use now

        // private fields
        // New device information can be assigned from Application or added in this default file.
        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // || Key         || string[]                                                                                                                     ||
        // || device type || VolumeLabel | database file name | device info file name | device model property | device model content | device SN property ||
        private readonly Dictionary<string, string[]> deviceInfoMap = new Dictionary<string, string[]>()
        {
            {"XR5000", new string[6]{"TRU-TEST", "db", "polaris.xml", "model", "XR5000", "serialNumber" } },
            {"ID5000", new string[6]{"TRU-TEST", "db", "polaris.xml", "model", "ID5000", "serialNumber" } }
        };

        private string deviceName = "";

        /// <summary>
        /// Find all available USB devices matching information listed in deviceInfoMap dictionary, output device name and db file path.
        /// </summary>
        public DeviceDrive() { }

        public DeviceDrive(Dictionary<string, string[]> deviceInfoMapDictionary)
        {
            foreach (var kvp in deviceInfoMapDictionary)
            {
                if (kvp.Value.Length != deviceInfoMap[deviceInfoMap.Keys.ElementAt(0)].Length)
                {
                    throw new ArgumentException("Input device information map is not valid.");
                }
            }

            deviceInfoMap = deviceInfoMapDictionary;
        }

        // IEvent implementation  ---------------------------------------------------------------
        void IEvent.Execute()
        {
            var scanUsbDriveDict = ScanUsbDrive();

            if (scanUsbDriveDict.Count > 0 && !scanUsbDriveDict.ContainsKey(deviceName))
            {
                System.Diagnostics.Debug.WriteLine($"==========> Found {scanUsbDriveDict.Count} USB devices.");
                // usbDrivePathDict.Data = scanUsbDriveDict; // This dictionary may be used in the future to show current available devices
                var info = scanUsbDriveDict[scanUsbDriveDict.Keys.ElementAt(0)];
                deviceName = scanUsbDriveDict.Keys.ElementAt(0);
                if (usbDeviceDbPath != null) usbDeviceDbPath.Data = info.DbPath;
                if (deviceDrivePath != null) deviceDrivePath.Data = Regex.Match(info.DbPath, @"[A-Za-z].*\\").Value;
                if (deviceFirmwareVersion != null) deviceFirmwareVersion.Data = info.Version;
                usbDeviceName.Data = deviceName.Split('-')[0];
            }
            else if (!scanUsbDriveDict.Any())
            {
                System.Diagnostics.Debug.WriteLine("==========> No USB devices found.");

                usbDeviceDbPath.Data = null;
                usbDeviceName.Data = null;
                deviceName = "";
            }
        }

        /// <summary>
        /// Scan all available USB devices matching information listed in deviceInfoMap.
        /// </summary>
        /// <returns> a dictionary contains all available devices and their db file paths with format e.g. XR5000-SerialNumber, E:\\db </returns>
        private Dictionary<string, ScaleInfo> ScanUsbDrive()
        {
            var usbDrivePathDictionary = new Dictionary<string, ScaleInfo>();
            var scanCIMV2 = true;

            try
            {
                List<DriveInfo> driveInfos = new List<DriveInfo>();
                ConnectionOptions opts = new ConnectionOptions();
                ManagementScope scope = new ManagementScope(@"\\.\root\cimv2", opts);
                SelectQuery diskQuery = new SelectQuery("SELECT * FROM Win32_LogicalDisk WHERE (MediaType = null OR MediaType = 11 OR MediaType = 12)");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(diskQuery);
                ManagementObjectCollection diskObjColl = searcher.Get();

                foreach (var obj in diskObjColl)
                {
                    var mobj = (ManagementObject) obj;
                    driveInfos.Add(new DriveInfo(mobj["Name"].ToString()));
                }

                return usbDrivePathDictionary = IdentifyDevice(driveInfos);
            }
            catch (ManagementException ex)
            {
                scanCIMV2 = false;
                Debug.WriteLine(ex.Message + "Cannot search local drives when scanning.");
            }
            // A windows computer can get into a state where the CIMV2 cache is corrupted, if this is the case then datalink
            // will never be able to find locally connected drives. A computer in this state will never be able to connect to
            // either a 5000 or ERS until this cache is rebuilt.
            // 
            // Resolution for this problem:
            // https://blogs.msdn.microsoft.com/vsnetsetup/2017/04/03/vs-2017-invalid-classsystem-management-managementexception-throwwithextendedinfomanagementstatus-errorcode/
            if (!scanCIMV2)
            {
                try
                {
                    var driveInfos = DriveInfo.GetDrives();
                    return usbDrivePathDictionary = IdentifyDevice(driveInfos);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    Debug.WriteLine(ex.Message + "Cannot search local drives when scanning.");
                }
            }

            return usbDrivePathDictionary;
        }

        /// <summary>
        /// Scan a drive to see whether it matches device info.
        /// </summary>
        /// <param name="allDrives"> input drive labels </param>
        /// <returns> Return valid drive path when info matches, return null when does not. </returns>
        private Dictionary<string, ScaleInfo> IdentifyDevice(IEnumerable<DriveInfo> allDrives)
        {
            var deviceIndexDictionary = new Dictionary<string, ScaleInfo>();

            // begin search every device listed in the deviceInfoMap
            foreach (var infoMapKvp in deviceInfoMap)
            {
                string deviceType = infoMapKvp.Key; // e.g. XR5000
                string[] deviceInfo = infoMapKvp.Value; // device info
                
                // When device information map structure changes, change this part.
                string deviceVolumeLabel = deviceInfo[0]; // TRU-TEST
                string dbFileName = deviceInfo[1]; // db
                string deviceInfoFileName = deviceInfo[2]; // polaris.xml
                string deviceModel = deviceInfo[3]; // model
                string deviceModelInfo = deviceInfo[4]; // XR5000 or ""
                string deviceSn = deviceInfo[5]; // serialNumber

                // define drive scan type
                DriveType driverTypeCheck = DriveType.Removable;

                // get all removable drives to search
                var driveInfos = allDrives.Where(
                    d => d.DriveType == driverTypeCheck && d.IsReady && d.VolumeLabel == deviceVolumeLabel);

                // begin scan each drive for db file, polaris file and model content info.
                foreach (DriveInfo driver in driveInfos)
                {
                    string dbFilePath = Path.Combine(driver.RootDirectory.Name, dbFileName);
                    string dbWorkingFilePath = Path.Combine(driver.RootDirectory.Name, "db_working"); // support scanning device feature when updating db
                    string deviceXmlPath = Path.Combine(driver.RootDirectory.Name, deviceInfoFileName);

                    // both file exist in this drive
                    if ((File.Exists(dbFilePath) && File.Exists(deviceXmlPath)) || (File.Exists(dbWorkingFilePath) && File.Exists(deviceXmlPath)))
                    {
                        // read xml content
                        XElement polaris = XElement.Load(deviceXmlPath);

                        // get object content in xml
                        IEnumerable<string> model = from item in polaris.Elements(deviceModel)
                            select item.Value;
                        string deviceModelName = model.First(); // get model name in model element

                        // get object content in xml
                        IEnumerable<string> serialNumber = from item in polaris.Elements(deviceSn)
                            select item.Value;
                        string deviceSerialNumber = serialNumber.First(); // get serial number in serialNumber element

                        // get object content in xml
                        IEnumerable<string> version = from item in polaris.Elements("version") select item.Value;
                        string deviceVersion = version.First();

                        // create device index pair
                        string deviceIndexDictKey = $"{deviceType}-{deviceSerialNumber}";
                        string deviceDbIndexDictValue = dbFilePath;

                        // insert device index pair into dictionary
                        if (String.Equals(deviceModelName, deviceModelInfo) && 
                            !deviceIndexDictionary.ContainsKey(deviceIndexDictKey))
                        {
                            deviceIndexDictionary.Add(deviceIndexDictKey, new ScaleInfo(deviceDbIndexDictValue, deviceVersion));
                        }
                    }
                }
            }

            return deviceIndexDictionary;
        }
    }
}
