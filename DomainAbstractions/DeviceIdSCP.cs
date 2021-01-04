using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Abstraction that retrieves device ID (PCB serial number) in the form of a string by sending a SCP command of {VS} from a connected device.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: incoming event to start sending SCP command to retrieve device ID
    /// 2. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol
    /// 3. IArbitrator arbitrator: arbitrator for ordering SCP commands
    /// 4. IDataFlow<string> deviceId: string output of the device id
    /// 5. IEvent eventComplete: event to execute after completion of the {VS} SCP command
    /// </summary>
    public class DeviceIdSCP : IEvent // start
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // ports ---------------------------------------------------------------
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private IDataFlow<string> deviceId;
        private IEvent eventComplete;

        /// <summary>
        /// An abstraction for getting the ID (PCB serial number) of a device by sending SCP command.
        /// </summary>
        public DeviceIdSCP() { }

        // IEvent implementation ----------------------------------------------------------------------------------------
        async void IEvent.Execute()
        {
            await arbitrator.Request(InstanceName);
            string id = await requestResponseDataFlow.SendRequest("{VS}"); // Get, set PCB serial number.
            arbitrator.Release(InstanceName);

            deviceId.Data = id;
            eventComplete?.Execute();
        }
    }
}
