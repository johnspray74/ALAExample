using ProgrammingParadigms;
using System;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// Checks the validity of a given Intel HEX firmware file:
    /// 1. Checks the file extension (`hex`)
    /// 2. Calculates the checksum of each line and compares with expected.
    /// -------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Triggers the checking of validity. If the path is not set, it will
    ///                     wait until it is set then check.
    /// 2. IDataFlow<string> NEEDNAME: The path to the firmware to check.
    /// 3. IDataFlow<bool> validity: Boolean based on if the firmware is valid or not.
    /// 4. IDataFlow<IntelHexCheckValidity.InvalidReason> reason: The reason it is not valid, if `validity` has already said it is not.
    /// 5. IEvent isValid: Fired when the hex file is valid.
    /// 6. IEvent notValid: Fired when the hex file is not valid.
    /// </summary>
    class IntelHexCheckValidity : IEvent, IDataFlow<string>
    {
        public enum InvalidReason
        {
            NOT_HEX,
            CORRUPT
        }

        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<bool> validity;
        private IDataFlow<InvalidReason> reason;
        private IEvent isValid;
        private IEvent notValid;

        // private fields
        private string filePath = default;
        private bool isHexFileValid;
        private bool eventWasCalled = false;

        /// <summary>
        /// Checks the validity of a given Intel HEX firmware file.
        /// </summary>
        public IntelHexCheckValidity(string filePath = default)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Runs checks on the given Intel HEX firmware file. Sets the validity response.
        /// </summary>
        void ExecuteCheck()
        {
            if (filePath == default)
            {
                eventWasCalled = true;
                return;
            }

            eventWasCalled = false;
            isHexFileValid = (CheckFileExtension() && CheckContents());

            if (validity != null)
            {
                validity.Data = isHexFileValid;
            }

            if (isHexFileValid)
            {
                isValid?.Execute();
            }
            else
            {
                notValid?.Execute();
            }
        }

        /// <summary>
        /// Checks that the given file path has a `.hex` file extension.
        /// </summary>
        /// <returns>Whether the file path has a `.hex` file extension.</returns>
        private bool CheckFileExtension()
        {
            if (! filePath.EndsWith(".hex"))
            {
                reason.Data = InvalidReason.NOT_HEX;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Iterates through the hex file and calculates the checksum of each line, ensuring that the
        /// file is not corrupt.
        /// </summary>
        /// <returns>Whether the file is not corrupt.</returns>
        private bool CheckContents()
        {
            string s;

            using (StreamReader sr = new StreamReader(filePath))
            {
                // iterate over every line
                while (sr.Peek() >= 0)
                {
                    s = sr.ReadLine();

                    if (s != null)
                    {
                        // check line begins with colon
                        if (! s.StartsWith(":"))
                        {
                            reason.Data = InvalidReason.CORRUPT;
                            return false;
                        }

                        // retrieve checksum and convert from hex (the last two characters)
                        byte checksum = Convert.ToByte(s.Substring((s.Length - 2), 2), 16);
                        s = s.Substring(1).Remove((s.Length - 3), 2); // remove colon and checksum

                        // calculate the checksum of the line
                        byte[] line = ConvertStringToByteArray(s);
                        byte sum = 0;

                        foreach (byte b in line) sum += b;
                        sum = (byte) ~sum;
                        sum++;

                        // check that the checksums match, if not it is corrupt 
                        if (sum != checksum)
                        {
                            reason.Data = InvalidReason.CORRUPT;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Converts a given hexadecimal string to an array of bytes.
        /// </summary>
        /// <param name="eidString">A string of hexadecimal bytes.</param>
        /// <returns>An array of bytes representing the original string.</returns>
        private Byte[] ConvertStringToByteArray(string eidString)
        {
            int resultLength = eidString.Length / 2;
            byte[] result = new Byte[resultLength];

            int max = eidString.Length + resultLength - 4;

            for (int sp = 2; sp <= max; sp += 3)
            {
                eidString = eidString.Insert(sp, ",");
            }

            string[] hexValuesNationalSplit = eidString.Split(',');

            for (int i = 0; i < resultLength; i++)
            {
                result[i] = Convert.ToByte(hexValuesNationalSplit[i], 16);
            }
            return result;
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

        // IEvent implementation
        void IEvent.Execute()
        {
            ExecuteCheck();
        }
    }
}