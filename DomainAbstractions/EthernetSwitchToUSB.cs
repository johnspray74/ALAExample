using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// This abstraction is used to switch specific devices like XR5000 from Ethernet mode to USB mass storage mode.
    /// Step 1: Scan all Ethernet adapters.
    /// Step 2: Try to switch device into USB mode.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent startEthernetScanning: port to initiate scanning of all Ethernet adapters and to begin switching device mode
    /// 2. IEvent ethernetSwitchCompleted: Ethernet mode switch completed event
    /// </summary>
    public class EthernetSwitchToUSB : IEvent // startEthernetScanning
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // ports ---------------------------------------------------------------
        private IEvent ethernetSwitchCompleted;

        // private fields ---------------------------------------------------------------
        private string subnetIp = "192.168.7.0"; // Default subnet ip address is 192.168.7.0 for XR5000
        private string deviceIp = "192.168.7.1";
        private string switchURL = "http://192.168.7.1:9000/settings?usb_mode=USB_MassStorage";

        private NetworkInterface[] oldNetworkInterfaces;

        /// <summary>
        /// Scan and switch devices from Ethernet mode to USB mode, default setting is for XR5000.
        /// </summary>
        public EthernetSwitchToUSB() { }
        public EthernetSwitchToUSB(string subnetIpString, string deviceIpString, string switchUrlString)
        {
            // ip address validation
            if ((subnetIpString.Count(c => c == '.') != 3) &&
                IPAddress.TryParse(subnetIpString, out IPAddress subnetOutAddress)) 
            {
                subnetIp = subnetIpString;
            }

            if ((deviceIpString.Count(c => c == '.') != 3) &&
                IPAddress.TryParse(deviceIpString, out IPAddress deviceOutAddress))
            {
                deviceIp = deviceIpString;
            }

            // url validation
            if (Uri.TryCreate(switchUrlString, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)) 
            {
                switchURL = switchUrlString;
            }
        }

        // IEvent implementation  -----------------------------------------------------------------
        // can wires from Timer (continuously scan) or MainWindow application (scan only once)
        void IEvent.Execute()
        {
            // step 1: get existed ethernet adapters
            var newNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            System.Diagnostics.Debug.WriteLine($"Found {newNetworkInterfaces.Length} Ethernet adapters.");

            // step 2: check old ethernet adapter array, when find new one the switch
            foreach (var newNetworkInterface in newNetworkInterfaces)
            {
                var needReSwitch = true;

                if (oldNetworkInterfaces != null)
                {
                    foreach (var oldNetworkInterface in oldNetworkInterfaces)
                    {
                        if (!string.Equals(oldNetworkInterface.Id, newNetworkInterface.Id)) continue;
                        needReSwitch = false;
                        break;
                    }
                }

                if (!needReSwitch) continue;
                
                SwitchEthernetToUsb(newNetworkInterface);
            }

            // step 3: start drive scan
            ethernetSwitchCompleted?.Execute();

            // step 4: record interface array
            // oldNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        }

        // private methods  -----------------------------------------------------------------
        /// <summary>
        /// Send switch request to the device.
        /// </summary>
        /// <param name="adapter"> Ethernet adapter need to check and switch. </param>
        private void SwitchEthernetToUsb(NetworkInterface adapter)
        {
            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
            UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
            foreach (var uni in uniCast)
            {
                if ((uni.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) &&
                    (IPAddress.IsLoopback(uni.Address) == false))
                {
                    IPAddress ip = uni.Address;
                    IPAddress subnetMask = uni.IPv4Mask;
                    if ((subnetMask != null) &&
                        IPAddress.Equals(IPAddress.Parse(subnetIp), GetNetworkAddress(ip, subnetMask)))
                    {
                        Ping pingSender = new Ping();
                        IPAddress deviceAddress = IPAddress.Parse(deviceIp);
                        PingReply reply = pingSender.Send(deviceAddress);

                        if (reply.Status == IPStatus.Success)
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(switchURL);
                            request.Proxy = new WebProxy();
                            request.Method = "POST";
                            request.Timeout = 100;

                            try
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    $"==========> Switch Ethernet adapter {adapter.Description}.");

                                using (var response = request.GetResponse())
                                {
                                }

                            }
                            catch (WebException ex)
                            {
                                if (ex.Status == WebExceptionStatus.Timeout)
                                {
                                    System.Diagnostics.Debug.WriteLine("==========> WebExceptionStatus" + ex.Message);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get subnet IP address with IP address and subnet mask.
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="subnetMask">Subnet mask of the IP address</param>
        /// <returns></returns>
        // ref: https://docs.microsoft.com/en-us/archive/blogs/knom/ip-address-calculations-with-c-subnetmasks-networks
        private IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }
    }
}
