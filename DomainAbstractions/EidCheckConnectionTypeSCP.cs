using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Gets the connection type of the EID device. Returned in an enum ConnectionType.
    /// ----------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent "NEEDNAME": To be called when we need to get the connection type.
    /// 2. IRequestResponseDataFlow<string, string> requestResponseDataFlow:  is in charge of interacting with the device.
    /// 3. IArbitrator arbitrator: take the responsibility of arranging all the command in a sequential order.
    /// 4. IDataFlow<EidCheckConnectionTypeSCP.ConnectionType> connectionType: The connection type of the device.
    /// </summary>
    class EidCheckConnectionTypeSCP : IEvent
    {
        public enum ConnectionType
        {
            UNKNOWN,
            USB,
            BT
        }
        
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private IDataFlow<ConnectionType> connectionType;

        /// <summary>
        /// Gets the connection type of the EID device. Returns the connection type with an enum EidCheckConnectionTypeSCP.ConnectionType.
        /// </summary>
        public EidCheckConnectionTypeSCP() { }

        // IEvent implementation -----------------------------------------------------------------
        // Normally, this mehod will not be declared with "async", the reason using "async" is that 
        // it calls other method with "await", which requires an "async" key word to decorate the method
        async void IEvent.Execute()
        {
            ConnectionType type = ConnectionType.UNKNOWN;

            // Retrieve connection type from the EID
            try
            {
                await arbitrator.Request(InstanceName);
                string rawType = await requestResponseDataFlow.SendRequest("{UC}");
                arbitrator.Release(InstanceName);

                // Convert string response to enum
                switch (rawType)
                {
                    case "USB":
                        type = ConnectionType.USB;
                        break;
                    case "BT":
                        type = ConnectionType.BT;
                        break;
                }
            }
            catch
            { }

            connectionType.Data = type;
        }
    }
}