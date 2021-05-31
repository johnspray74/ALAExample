using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class SCPSense : IEvent // listOfCOMPorts
    {
        // properties -----------------------------------------------------------------
        public string InstanceName;

        // ports -----------------------------------------------------------------
        private IRequestResponseDataFlow<string, string> requestResponseDataFlow;
        private IDataFlow<bool> IsDeviceConnected;
        private IArbitrator arbitrator;

        // private fields
        private bool _isDeviceConnected = false;
        private bool _keepDetectingDevice = false;

        /// <summary>
        /// Loops through COM list to find device connected response and acquires device name using SCP commands. Also turns on acknowledgements and error responses.
        /// </summary>
        public SCPSense() { }

        // IEvent implementation ---------------------------------------------------
        void IEvent.Execute()
        {
            var _fireAndForgot = StartProcessAsync();
        }

        // private method ---------------------------------------------------------
        // this method starts the process of looping through each of the ports and sending commands for response
        private async Task StartProcessAsync()
        {
            if (_isDeviceConnected)
                return;

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
                    if (!_isDeviceConnected)
                        _keepDetectingDevice = _isDeviceConnected = IsDeviceConnected.Data = true;

                    while (_keepDetectingDevice)
                        await SendTimedResponseAsync();
                }
                else
                {
                    _keepDetectingDevice = false;
                }
            }
            catch (TaskCanceledException tce)
            {
                System.Diagnostics.Debug.WriteLine("Arbitrator 5 second timeout.");
                return; // return to the next port
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
                if (response != "")
                {
                    if (!_isDeviceConnected)
                        _isDeviceConnected = IsDeviceConnected.Data = true;
                }
                else
                {
                    if (_isDeviceConnected)
                        _isDeviceConnected = IsDeviceConnected.Data = false;
                }
            }
            catch (TaskCanceledException)
            {
                
            }

            arbitrator.Release(InstanceName);
        }
    }
}
