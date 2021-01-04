using ProgrammingParadigms;
using System;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// Retrieves the firmware version of the connected EID.
    /// ----------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Event to get the EID version.
    /// 2. IRequestResponseDataFlow<string, string> requestResponseDataFlow:  is in charge of interacting with the device.
    /// 3. IArbitrator arbitrator: take the responsibility of arranging all the command in a sequential order.
    /// 4. IDataFlow<string> versionOutput: The EID version output.
    /// 5. IEvent gotVersion: Fired when the version has been retireved.
    /// </summary>
    class EidGetFirmwareVersion : IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private IDataFlow<string> versionOutput;
        private IEvent gotVersion;

        /// <summary>
        /// Retrieves the firmware version of the connected EID.
        /// </summary>
        public EidGetFirmwareVersion() { }

        // IEvent implementation -----------------------------------------------------------------
        // Normally, this mehod will not be declared with "async", the reason using "async" is that 
        // it calls other method with "await", which requires an "async" key word to decorate the method
        async void IEvent.Execute()
        {
            // Wait for arbitrator and get firmware version
            await arbitrator.Request(InstanceName);
            string version = await requestResponseDataFlow.SendRequest("{VA}");
            arbitrator.Release(InstanceName);

            // can't set output if it is null
            if (versionOutput != null)
            {
                versionOutput.Data = version;
            }

            gotVersion?.Execute();
        }
    }
}
