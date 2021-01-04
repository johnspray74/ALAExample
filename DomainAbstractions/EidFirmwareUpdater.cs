using Libraries;
using ProgrammingParadigms;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Performs a firmware update on an EID device. Starts the ISP bootloader on the EID and calls the firmware
    /// updater sub-process.
    /// --------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME:                                                     Starts the firmware update.
    /// 2. IDataFlow<string> NEEDNAME:                                          Provides the COM port that the device is on.
    /// 3. IRequestResponseDataFlow<string, string> requestResponseDataFlow:    Implementation of the IRequestResponse of managing SCP commands.
    /// 4. IArbitrator arbitrator:                                              Arbitrates the SCP requests.
    /// 5. IDataFlowB<string> firmwarePathPort:                                 Provides the path to the firmware to upload. Is not checked if it exists.
    /// 6. IDataFlow<bool> comPortShouldClose:                                  Whether the COM port should be closed by the current program, so that the
    ///                                                                         firmware updater can access the port.
    /// 7. IDataFlow<double> progressPercentage:                                The progress percentage from 0 to 200, where 0 is not started, 100 is programming finished
    ///                                                                         and verifying starting, 200 is verifying finished.
    /// 8. IDataFlow<string> statusString:                                      The current staus of the updater. Will display the percentage and if it is programming/verifying,
    ///                                                                         and will display the error message if there is an error.
    /// 9. IEvent updateSuccess:                                                Called when the update succeeds.
    /// 10: IEvent updateFailed:                                                Called when the update fails.
    /// </summary>
    class EidFirmwareUpdater : IEvent, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private IDataFlowB<string> firmwarePathPort;
        private IDataFlow<bool> comPortShouldClose;
        private IDataFlow<double> progressPercentage;
        private IDataFlow<string> statusString;
        private IEvent updateSuccess;
        private IEvent updateFailed;

        // private fields
        private bool updaterRunning = false;
        private string deviceID;
        private string comPort = default;
        private string firmwarePath = default;

        /// <summary>
        /// Performs a firmware update on an EID device. Starts the ISP bootloader on the EID and calls the firmware
        /// updater sub-process.
        /// </summary>
        /// <param name="deviceID">The microcontroller device ID. Default is LPC1767.</param>
        /// <param name="firmwarePath">The path to the firmware to upload. Can also be given through IDataFlowB<string> firmwarePathPort.</param>
        public EidFirmwareUpdater(string deviceID = default, string firmwarePath = default)
        {
            this.deviceID = deviceID == default ? Libraries.Constants.XRS2SRS2DeviceID : deviceID;
            this.firmwarePath = firmwarePath;
        }

        /// <summary>
        /// Sets up the listener for IDataFlowB<string> firmwarePathPort.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (firmwarePathPort != null)
            {
                firmwarePathPort.DataChanged += () => firmwarePath = firmwarePathPort.Data;
            }
        }

        /// <summary>
        /// Starts the update task.
        /// </summary>
        private async Task UpdateFirmware()
        {
            // no firmware path - not ready
            // don't run while we are already running
            if (firmwarePath == default || updaterRunning) return;

            Logging.Log($"EidFirmwareUpdater {InstanceName} starting update");
            updaterRunning = true;

            // set the EID in ISP programming mode
            if (requestResponseDataFlow != null)
            {
                try
                {
                    await arbitrator.Request(InstanceName);
                    await requestResponseDataFlow.SendRequest("{VU}");
                    arbitrator.Release(InstanceName);
                }
                catch (Exception)
                { }
            }

            if (comPortShouldClose != null) comPortShouldClose.Data = true;
            await Task.Delay(2000); // wait 2 secs to ensure the COM port closes

            Console.WriteLine($"{GetUpdaterPath()} {GetUpdaterArguments()}");

            Process updaterProcess = new Process()
            {
                StartInfo =
                {
                    FileName = GetUpdaterPath(),
                    Arguments = GetUpdaterArguments(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                },
                EnableRaisingEvents = true
            };

            updaterProcess.OutputDataReceived += (object sender, DataReceivedEventArgs e) => System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.Data == null) return;
                string line = e.Data;

                Logging.Log($"EidFirmwareUpdater {InstanceName} {line}");

                // error - the program will exit after this line
                // provide it to the error dialog
                if (line.Contains("Exception"))
                {
                    statusString.Data = line;
                    return;
                }

                if (line.Contains("Erasing"))
                {
                    statusString.Data = "Erasing old firmware...";
                    return;
                }
                
                if (line.Contains("ProgressP"))
                {
                    double percentage = 0;

                    // extract percentage from line - if we run into an error, default to 0
                    try
                    {
                        percentage = Convert.ToDouble(line.Substring(12));
                    }
                    catch (Exception)
                    { }

                    statusString.Data = $"Programming: {percentage}%";
                    progressPercentage.Data = percentage;
                }

                if (line.Contains("ProgressV"))
                {
                    double percentage = 0;

                    // extract percentage from line - if we run into an error, default to 0
                    try
                    {
                        percentage = Convert.ToDouble(line.Substring(12));
                    }
                    catch (Exception)
                    { }

                    statusString.Data = $"Verifying: {percentage}%";
                    progressPercentage.Data = 100 + percentage;
                }
            }));

            updaterProcess.Exited += (object sender, System.EventArgs e) => System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                updaterRunning = false;
                if (comPortShouldClose != null) comPortShouldClose.Data = false;

                // there was an error
                if (updaterProcess.ExitCode > 0)
                {
                    Logging.Log($"EidFirmwareUpdater {InstanceName} update failed with code {updaterProcess.ExitCode}");
                    updateFailed?.Execute();
                }
                else
                {
                    Logging.Log($"EidFirmwareUpdater {InstanceName} update success");
                    updateSuccess?.Execute();
                }

                updaterProcess.Dispose();
            }));

            bool started = updaterProcess.Start();

            if (! started)
            {
                Logging.Log($"EidFirmwareUpdater {InstanceName} COULD NOT START PROCESS");
                await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusString.Data = "Could not start the updater process.";
                    updateFailed?.Execute();
                }));
            }
            else
            {
                Logging.Log($"EidFirmwareUpdater {InstanceName} subprocess started");
                updaterProcess.BeginOutputReadLine();
            }
        }

        /// <summary>
        /// Builds the path to the updater executable.
        /// </summary>
        /// <returns>Path to updater executable.</returns>
        private string GetUpdaterPath()
        {
            // test script that reads log file
            //return @"C:\Users\David.cole\Projects\DatalinkTest\bin\Debug\net5.0\DatalinkTest.exe";

            // get the base directory of the executable
            string baseDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            
            //// for debugging purposes - grab the executable from the UpgradeXrsFirmware project
            //if (baseDirectory.EndsWith(@"bin\Debug"))
            //{
            //    baseDirectory += @"\..\..\..\UpgradeXrsFirmware\bin\Debug";
            //}

            return @$"{baseDirectory}\UpgradeXrsFirmware.exe";
        }

        /// <summary>
        /// Builds the arguments for the updater executable.
        /// </summary>
        /// <returns>Arguments for the update executable in string format.</returns>
        private string GetUpdaterArguments()
        {
            return $"{comPort} \"{firmwarePath}\" {deviceID}";
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            Task _ = UpdateFirmware();
        }

        // IDataFlow implementation
        string IDataFlow<string>.Data
        {
            set
            {
                // don't change the COM port after we have closed it
                if (updaterRunning) return;
                comPort = value;
            }
        }
    }
}
