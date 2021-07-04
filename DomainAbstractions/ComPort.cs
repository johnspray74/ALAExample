using System.Collections.Generic;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>
    /// ComPort abstraction normally talks to physical COM ports for serial I/O except but we modified this one to be a virual COM port so that we could wire a simulated external device to it. 
    /// ComPort is an ALA domain abstraction (see AbstractionLayeredArchitecture.md for more details)
    /// Abstraction description follows:
    /// Comport has a pair of DataFlow ports for interfacing to the rest of the application, one for input and one for output
    /// As its virtual COM port it has another pair of DataFlow ports. It bassically passes individual characters through
    /// ------------------------------------------------------------------------------------------------------------------
    /// Configurations: (configurations are for use by the application when it instantiates a domain abstraction)
    /// None except the usual InstanceName
    /// ------------------------------------------------------------------------------------------------------------------
    /// </para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; inputStringToBeSentOutOnTheCOMPort: input that takes a string to be output to the COM port</para>
    /// <para>2. IDataflow&lt;char&gt; outputForCharactersReceiveFromTheCOMPort: output for characters as they are received from the COM port</para>
    /// <para>3. IDataflow&lt;char&gt; virtualComPortTx: output that transmits characters as a virtual COM port</para>
    /// <para>4. IDataflow&lt;char&gt; virtualCOMPortRx: input that receives characters as a vitual COM port</para>
    /// <summary>
    public class COMPort : IDataFlow<string>, IDataFlow<char> // inputStringToBeSentOutOnTheCOMPort, virtualCOMPortRx
    {
        // configurations ---------------------------------------------------------------
        public string InstanceName = "Default";

        //ports ---------------------------------------------------------------
        private IDataFlow<char> outputForCharactersReceiveFromTheCOMPort;
        private IDataFlow<char> virtualComPortTx;  // connects to a simulated external hardware device

        public COMPort() {}
  

        // Implement IDataFlow<string> -------------------------------------------------------
        string IDataFlow<string>.Data
        {
            // When we receive a string to be transmitted out the COM port, serial tranmits can take time for every character, so start an asychronous function to output one character at a time 
            set
            {
                var _fireAndForget = WriteToPortAsync(value);
            }
        }


        private async Task WriteToPortAsync(string command)
        {
            foreach (var c in command)
            {
                virtualComPortTx.Data = c;
            }
        }





        // Implement virtualCOMPortRx -------------------------------------------------------
        char IDataFlow<char>.Data 
        { 
            set
            {
                outputForCharactersReceiveFromTheCOMPort.Data = value;
            }
        }


    }
}
