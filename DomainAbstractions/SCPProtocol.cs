using ProgrammingParadigms;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DomainAbstractions
{
// TBD replace the Debug.WriteLines with an outputLogging port owning its own delegate type (same is in Wiring.cs)
// Application must wired it to somewhere such as an instance of logging.cs

    /// <summary>
    /// This abstraction is used to interact with the device using SCP commands and all the request will be asynchronized. All the scp command will be sent 
    /// through this abstraction to the devices and the returns of the devices will be handled here to composite a complete string, 
    /// which will then be sent back through the Task.
    /// 
    /// Notice: The reason we need a buffer in this class because the device only returns byte data which is not a complete return for the caller.
    /// The byte is passed in as a char and decoded using ASCII to be checked for special symbols of "[" and "]" and therefore extracting the response within.
    /// 
    /// Note: This class is not in charge of preventing race conditions and therefore it is expected that each Request and Response from the client side
    /// should be arbitrated by the Arbitrator class
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<char> charFromSource: sources one char from the source (COMs)
    /// 2. IRequestResponseDataFlow<string, string> SCPRequestResponse: implementation of the IRequestResponse of managing SCP commands
    /// 3. IDataFlow<string> scpCommand: string output of the SCP response from the device
    /// </summary>

    public class SCPProtocol : IDataFlow<char>, IRequestResponseDataFlow<string, string> // charFromSource, SCPRequestResponse
    {
        //properties
        public string InstanceName;

        // ports
        private IDataFlow<string> scpCommand;

        // private fields
        private TaskCompletionSource<string> scpResponseTask;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Request and response of a SCP command to a connected device, handling the response from the device no matter it is a 
        /// normal response or a error message.
        /// </summary>
        public SCPProtocol() { }

        // IDataFlow<char> implementation ------------------------------------------------------
        char IDataFlow<char>.Data
        {
            set
            {
                ParseIncomingCharacters(value);
            }
        }

        // IResponseDataFlow<string, string> implementation ------------------------------------
        Task<string> IRequestResponseDataFlow<string, string>.SendRequest(string command)
        {
            scpResponseTask = new TaskCompletionSource<string>();

            if (command.StartsWith("{") && command.EndsWith("}"))
            {
                scpCommand.Data = command; // this line will tell COMPortAdapter to write to the Port
                diagnosticOutput?.Invoke("SCPProtocol.cs: Sending: " + command);
            }
            else
            {
                throw new Exception("DeviceSimulator: illegal command: " + command + ", command should start with '{' and end with '}'");
            }

            StartTimer(scpResponseTask);

            return scpResponseTask.Task;
        }

        // store the temporary return data from the device
        private string incomingBuffer;
        private void ParseIncomingCharacters(char character)
        {
            switch (character)
            {
                case '^':
                    {
                        cancellationTokenSource?.Dispose();
                        diagnosticOutput?.Invoke($"SCPProtocol.cs: Response: {character}");
                        scpResponseTask.TrySetResult(Char.ToString(character));
                    }
                    break;
                case '[':
                    incomingBuffer = null;
                    break;
                case ']':
                    {
                        string tempString = incomingBuffer;
                        incomingBuffer = null;
                        cancellationTokenSource?.Dispose();
                        diagnosticOutput?.Invoke($"SCPProtocol.cs: Response: {tempString}");
                        scpResponseTask.TrySetResult(tempString);
                    }
                    break;
                case ')':
                    {
                        if (!string.IsNullOrEmpty(incomingBuffer) && incomingBuffer.StartsWith("("))
                        {
                            string error = incomingBuffer + character;
                            diagnosticOutput?.Invoke(error);
                            incomingBuffer = null;
                            cancellationTokenSource?.Dispose();
                            scpResponseTask.TrySetException(new Exception(String.Format($"Command execution failed. Error message ({error})")));
                        }
                    }
                    break;
                default:
                    incomingBuffer += character;
                    break;
            }
        }

        // private methods ------------------------------------------------------------------------------
        private void StartTimer(TaskCompletionSource<string> tcs)
        {
            int timeout = 2000;
            cancellationTokenSource = new CancellationTokenSource(timeout); // countdown begins during the call of the constructor
            var cancellationToken = cancellationTokenSource.Token;

            //after the 4 seconds when CancellationTokenSource is cancelled, this method is called
            cancellationToken.Register(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    diagnosticOutput?.Invoke("SCPProtocol: 2 second timeout. Cancelling task.");
                    tcs.TrySetCanceled();
                }
            });
        }




        public delegate void DiagnosticOutputDelegate(string output);
        private static DiagnosticOutputDelegate diagnosticOutput;
        public static DiagnosticOutputDelegate DiagnosticOutput { get => diagnosticOutput; set => diagnosticOutput = value; }

    }
}
