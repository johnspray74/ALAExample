using ProgrammingParadigms;
using System;
using TruTest.Comms.Connection;
using TruTest.Comms.Connection.Detection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO.Ports;
using System.Runtime.Remoting.Channels;

namespace DomainAbstractions
{
    /// <summary>
    /// ComPortAdapter abstraction is in charge of detecting the COM ports with device connected to it, writing to the selected COM port and reading from the selected COM port.
    /// It sends a list of COM ports to SCPDeviceSense class and in return will receive a selected COM port to be initialised for communication to the COM port.
    /// Every time there is a new selected COM port, it is closed if it is the same one or opened if not.
    /// While no device is connected, every 3 seconds a new list of COM ports (only if it has changed from the previous list) will be sent to SCPDeviceSense.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent startFetchingComPorts: IEvent that starts the COMPortAdapter class to begin creating the list of COM ports for further use
    /// 2. IDataFlow<string> scpCommand: a complete SCP command as a string to be written to the selected COM port
    /// 3. IDataFlow<bool> deviceConnected: input of status of whether the device is connected
    /// 4. IDataFlow<List<string>> listOfPorts: output list of string with all the COM ports
    /// 5. IDataFlowB<string> selectedCOM: input of a selected COM port to be communicated to
    /// 6. IDataFlow<char> charFromPort: single char read from the to be pushed
    /// 7. IDataFlowB<bool> lockComPort: Locks the COM port adapter to prevent connecting while flashing firmware.
    /// </summary>
    public class COMPortAdapter : IEvent, IDataFlow<string> // startFetchingComPorts, scpCommand
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        //ports ---------------------------------------------------------------
        private IDataFlow<List<string>> listOfPorts;
        private IDataFlowB<string> selectedCOM;
        private IDataFlow<char> charFromPort;
        private IDataFlowB<bool> lockComPort;

        // private fields ---------------------------------------------------------------
        private SerialComPort currentSerialPort;
        private ComPortDetection detection;
        private Dictionary<string, SerialComPort> comPortDictionary;
        private bool detectionHandlerSet = false;
        private bool comPortLocked = false;

        /// <summary>
        /// Detects list of COM Ports and is in charge of writing and reading from the selected COM port.
        /// </summary>
        public COMPortAdapter()
        {
            detection = new ComPortDetection();
            
            comPortDictionary = new Dictionary<string, SerialComPort>();
        }

        // when the selectedCOM has changed, check if the COM wanting to be opened is same as the SerialPort
        private void PostWiringInitialize()
        {
            if (lockComPort != null)
            {
                lockComPort.DataChanged += () =>
                {
                    // the lock state has changed
                    if (comPortLocked != lockComPort.Data)
                    {
                        comPortLocked = lockComPort.Data;

                        // check if com port should be locked and if serial port is open
                        if (comPortLocked && currentSerialPort != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"ComPortAdapter {InstanceName}: COM port locked, {currentSerialPort.PortName} will be closed");
                            currentSerialPort?.Close();
                        }
                        else if (! comPortLocked)
                        {
                            System.Diagnostics.Debug.WriteLine($"ComPortAdapter {InstanceName}: COM port unlocked, attempting reconnect");
                            FetchListOfComPorts();
                        }
                    }
                };
            }

            selectedCOM.DataChanged += () =>
            {
                // don't attempt to reconnect if the com port is locked
                if (comPortLocked) return;

                var serialPort = new SerialPort(selectedCOM.Data);

                // first run where currentSerialPort has not been initialised or if it is a new Port
                if (this.currentSerialPort == null || this.currentSerialPort.PortName != serialPort.PortName)
                {
                    this.currentSerialPort?.Close(); //if new Port, check to close it first
                    this.currentSerialPort = new SerialComPort(serialPort);
                    System.Diagnostics.Debug.WriteLine($"Current serial port name: ({this.currentSerialPort.PortName}) and new serial com port: ({serialPort.PortName})");
                }

                // check if the currentSerialPort is available then Open the port and Subscribe
                if (!this.currentSerialPort.IsOpen && selectedCOM.Data.Equals(this.currentSerialPort.PortName))
                {
                    try
                    {
                        this.currentSerialPort.Open();
                        this.currentSerialPort.DataReceived.Subscribe(this.ReadFromPort);

                        // if (comPortDictionary.ContainsKey(selectedCOM.Data))
                        // {
                        //     comPortDictionary.Remove(selectedCOM.Data);
                        //     comPortDictionary.Add(selectedCOM.Data, this.currentSerialPort);
                        // }
                        // else
                        // {
                        //     comPortDictionary.Add(selectedCOM.Data, this.currentSerialPort);
                        // }
                    }
                    catch (Exception e)
                    {

                        // Throws an exception when the COMPort name doesn't start with "COM"
                    }

                }
                else // else currentSerialPort is not available
                {
                    System.Diagnostics.Debug.WriteLine($"ComPortAdapter port has been disconnected. {currentSerialPort.PortName} will now be closed.");

                    currentSerialPort.Close();
                    // comPortDictionary.Remove(selectedCOM.Data);
                }
            };
        }

        /// <summary>
        /// Sets up the listener for the list of COM ports from the COM port detector.
        /// When triggered, it will also update the `listOfPorts` port but will
        /// only add the listener once.
        /// </summary>
        private void FetchListOfComPorts()
        {
            if (listOfPorts != null) listOfPorts.Data = detection.PortNames.ToList();

            if (! detectionHandlerSet)
            {
                detection.PropertyChanged += (sender, args) =>
                {
                    if (listOfPorts != null) listOfPorts.Data = detection.PortNames.ToList();
                };
                detectionHandlerSet = true;
            }
        }

        // IEvent implementation -------------------------------------------------------
        // Begins searching for a list of COMPorts
        void IEvent.Execute() => FetchListOfComPorts();

        // IDataFlow<string> implementation -------------------------------------------------------
        // When we receive back a string of the selected COM Port
        string IDataFlow<string>.Data
        {
            set
            {
                var _fireAndForget = WriteToPortAsync(value);
            }
        }

        private async Task WriteToPortAsync(string command)
        {
            bool successfulWrite = currentSerialPort == null ? false : currentSerialPort.Write(command); // before writing to the port check if open

            if (!successfulWrite)
            {
                await Task.Delay(3500); // wait for timeout
            }
        }

        //private method ---------------------------------------------------------
        // method that communicates with the connected COMPort and reads data
        private void ReadFromPort(byte b)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                charFromPort.Data = (char)b;
            }));
        }

    }
}
