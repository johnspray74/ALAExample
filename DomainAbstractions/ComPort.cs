using System.Collections.Generic;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// ComPort abstraction is in charge of detecting the COM ports with device connected to it, writing to the selected COM port and reading from the selected COM port.
    /// </summary>
    public class COMPort : IDataFlow<string>, IDataFlow<char> // scpCommand, scpResult
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        //ports ---------------------------------------------------------------
        private IDataFlow<char> charFromPort;
        private IDataFlow<char> charToPort;

        public COMPort() {}
  

        // IDataFlow<string> implementation -------------------------------------------------------
        // When we receive back a string of the selected COM Port
        string IDataFlow<string>.Data
        {
            set
            {
                var _fireAndForget = WriteToPortAsync(value);
            }
        }

        public char Data 
        { 
            set
            {
                charFromPort.Data = value;
            }
        }

        private async Task WriteToPortAsync(string command)
        {
            foreach (var c in command)
            {
                charToPort.Data = c;
            }
        }
    }
}
