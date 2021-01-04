using ProgrammingParadigms;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// Checks the validity of a given EID firmware file by looping through and locating the firmware code.
    /// ---------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Triggers the validation of the firmware.
    /// 1. IDataFlow<string> NEEDNAME: The path to the firmware to check.
    /// 2. IDataFlowB<string> firmwareCode: The firmware code to check for.
    /// 3. IDataFlow<bool> validity: Boolean based on if the firmware is valid or not.
    /// 4. IEvent isValid: Fired if the firmware is valid.
    /// 5. IEvent notValid: Fired if the firmware is not valid.
    /// </summary>
    class EidValidateFirmware : IEvent, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlowB<string> firmwareCodePort;
        private IDataFlow<bool> validity;
        private IEvent isValid;
        private IEvent notValid;

        // private fields
        private bool firmwareIsValid;
        private string filePath;
        private string firmwareCode;
        private bool eventWasCalled = false;

        /// <summary>
        /// Checks the validity of a given EID firmware file by looping through and locating the firmware code.
        /// </summary>
        public EidValidateFirmware(string firmwareCode = default)
        {
            this.firmwareCode = firmwareCode;
        }

        private void PostWiringInitialize()
        {
            if (firmwareCodePort != null)
            {
                firmwareCodePort.DataChanged += () =>
                {
                    firmwareCode = firmwareCodePort.Data;
                };
            }
        }

        /// <summary>
        /// Runs the check for the firmware code. Sets the validity response.
        /// </summary>
        private void ExecuteCheck()
        {
            firmwareIsValid = CheckForFirmwareCode();

            // set dataflow output
            if (validity != null)
            {
                validity.Data = firmwareIsValid;
            }

            // execute events
            if (firmwareIsValid)
            {
                isValid?.Execute();
            }
            else
            {
                notValid?.Execute();
            }
        }

        private bool CheckForFirmwareCode()
        {
            string sOld = "", s = "", line = "";

            using (StreamReader sr = new StreamReader(filePath))
            {
                // loop over each line
                while (sr.Peek() >= 0)
                {
                    s = sr.ReadLine();

                    // retrieve "data" from line:
                    // remove start code, byte count, address, record type and checksum
                    if (s != null && s.StartsWith(":10"))
                    {
                        s = s.Substring(9);
                        s = s.Substring(0, s.Length - 2);
                    }

                    // combine last line and current line
                    line = sOld + s;
                    if (line.Contains(firmwareCode))
                    {
                        return true;
                    }

                    // store current line
                    sOld = s;
                }
            }

            return false;
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            if (filePath == default)
            {
                eventWasCalled = true;
            }
            else
            {
                ExecuteCheck();
            }
        }

        // IDataFlow implementation
        string IDataFlow<string>.Data
        {
            set
            {
                filePath = value;
                if (eventWasCalled)
                {
                    ExecuteCheck();
                }
            }
        }
    }
}