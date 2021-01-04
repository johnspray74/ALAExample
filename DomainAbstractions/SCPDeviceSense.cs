using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;

namespace DomainAbstractions
{
    /// <summary>
    /// SCPDeviceSense is in charge of looping through each of the COM ports in the list and sending {ZA1} commands for response while no device is connected yet.
    /// Once a COM port receives a response of a carat character (^), it connects to the COM port and notifies that a device is connected.
    /// Then to that responsive COM port, it will further send other SCP commands to retrieve device name as well as turning on the acknowledgements and error
    /// responses back from the device. When the device is disconnected it will output null string for the device name and resumes polling for devices on all ports.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<List<string>> listOfCOMPorts: input list of COM ports to be looped and checked for response
    /// 2. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol
    /// 3. IDataFlow<string> selectedCOMPort: out of the selected COM port wanting communication with
    /// 4. IDataFlow<bool> deviceConnected: boolean for whether device is connected
    /// 5. IDataFlow<string> connectedDeviceName: output connected device name accquired from SCP commands
    /// 6. IArbitrator arbitrator: arbitrator for ordering SCP commands
    /// </summary>
    public class SCPDeviceSense : IDataFlow<List<string>> // listOfCOMPorts
    {
        // properties -----------------------------------------------------------------
        public string InstanceName;

        // ports -----------------------------------------------------------------
        private IRequestResponseDataFlow_B<string, string> SCPRequestResponse_B;
        private IDataFlow<bool> connected;
        private IDataFlow<string> selectedCOMPort;
        private IDataFlow<string> connectedDeviceName;
        private IArbitrator arbitrator;

        // private fields -----------------------------------------------------------------
        private List<string> newListOfPorts;
        private bool isDeviceConnected;
        private string deviceName = null;
        private string connectedPort;
        private bool isLoopingThroughPorts = false;


        /// <summary>
        /// Loops through COM list to find device connected response and acquires device name using SCP commands. Also turns on acknowledgements and error responses.
        /// </summary>
        public SCPDeviceSense() { isDeviceConnected = false; }

        // IDataFlow<List<string>> implementation ---------------------------------------------------
        List<string> IDataFlow<List<string>>.Data
        {
            set
            {
                newListOfPorts = value;

                if (!isLoopingThroughPorts)
                {
                    var _fireAndForget = StartProcessAsync();
                }

            }
        }

        // private method ---------------------------------------------------------
        // this method starts the process of looping through each of the ports and sending commands for response
        private async Task StartProcessAsync()
        {
            bool completedLoop = false;
            int counter = 0;

            if (!isDeviceConnected)
            {
                while (!completedLoop)
                {
                    try
                    {
                        List<string> listOfPorts = newListOfPorts.Select(s => s).ToList(); // Create a new copy of the list to avoid interference from other scopes
                        if (listOfPorts.Count == 0) return;
                        listOfPorts.Reverse(); // To speed up debugging, when you have a lot of COMPorts and your desired port is closer to the end of the list
                        isLoopingThroughPorts = true;
                        foreach (string port in listOfPorts)
                        {
                            counter++;
                            if (isDeviceConnected) { completedLoop = true; break; }
                            if (!port.StartsWith("COM")) continue;
                            await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                selectedCOMPort.Data = port; // first select the port to communicate with
                            }));
                            await TryingForPortResponseAsync(port);
                            if (counter.Equals(listOfPorts.Count)) { completedLoop = true; }

                        }
                    }
                    catch (Exception e)
                    {
                        completedLoop = true;
                        Libraries.Logging.Log(e);
                    }
                }

                isLoopingThroughPorts = false;
            }

            while (isDeviceConnected)
            {
                await SendTimedResponseAsync();
            }
        }

        // private method ---------------------------------------------------------
        // method to loop through list of COM ports and when received a ^ response, it will choose current COM Port as selected one
        private async Task TryingForPortResponseAsync(string port)
        {
            try
            {
                string response = "";
                await arbitrator.Request(InstanceName);
                try
                {
                    response = await requestResponseDataFlow.SendRequest("{ZA1}");
                }
                catch (TaskCanceledException e)
                {
                    System.Diagnostics.Debug.WriteLine("3 seconds no response from device.");
                    return; // return to the next port
                }

                System.Diagnostics.Debug.WriteLine($"TryingForPortResponse: response was ({response})");
                arbitrator.Release(InstanceName);

                if (response.Equals("^"))
                {
                    //deviceConnected.Data = true;
                    isDeviceConnected = true;
                    connectedPort = port;
                    await GetDeviceNameAsync();
                }
            }
            catch (TaskCanceledException tce)
            {
                System.Diagnostics.Debug.WriteLine("Arbitrator 5 second timeout.");
                return; // return to the next port
            }
        }

        // private method ---------------------------------------------------------
        // method to send SCP commands for the device name
        private async Task GetDeviceNameAsync()
        {

            // after 2 seconds of device name not being set, means no device is connected
            string firmware = "";

            await arbitrator.Request(InstanceName);
            try
            {
                firmware = await requestResponseDataFlow.SendRequest("{ZF}"); // {ZF} SCP command for Firmware features


                if (firmware.Equals("Reader") || firmware.Equals("Reader,Alerts")) // Check if it is SRS2 or XRS2
                {
                    deviceName = await requestResponseDataFlow.SendRequest("{ZI}");
                }
                else // else check device name for others
                {
                    deviceName = await requestResponseDataFlow.SendRequest("{ZN}");
                }

                // turn acknowledgements on with {ZA1}, turn error responses on with {ZE1}.
                await requestResponseDataFlow.SendRequest("{ZA1}");
                await requestResponseDataFlow.SendRequest("{ZE1}");
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("3 seconds no response from device. Device Name is NULL");
                isDeviceConnected = false;
                await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    connectedDeviceName.Data = null;
                }));
            }
            arbitrator.Release(InstanceName);

            if (deviceName != null)
            {
                System.Diagnostics.Debug.WriteLine($"Device connected is {deviceName}");
                await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    connectedDeviceName.Data = deviceName;
                }));
            }
        }

        // private method ---------------------------------------------------------
        // this method is to send {ZA1} to the connected device every 3 seconds to check for device connection
        private async Task SendTimedResponseAsync()
        {
            await Task.Delay(3000);
            string response = "";
            await arbitrator.Request(InstanceName);
            try
            {
                response = await requestResponseDataFlow.SendRequest("{ZA1}");
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine($"3 seconds no response from device. Device: ({deviceName}) has been disconnected. Device Name is NULL");
                isDeviceConnected = false;
                await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    connectedDeviceName.Data = null;
                    selectedCOMPort.Data = connectedPort;
                }));

                return; // return to the next port
            }

            arbitrator.Release(InstanceName);
        }

    }

}
