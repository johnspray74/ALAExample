using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// A simple class to request and release a slot in an arbitrator queue. It is triggered with an
    /// IEvent and a 'ready' event is sent when it is time to perform actions. The event can then be
    /// finished with an IEventB release event.
    /// --------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME:         To be fired when the work has been done to release.
    /// 2. IEventB request:         Requests a place in the arbitrator queue.
    /// 3. IArbitrator arbitrator:  The arbitrator in use.
    /// 4. IEvent ready:            Fired when it is our turn to perform actions.
    /// </summary>
    class RequestArbitrator : IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IEventB request;
        private IArbitrator arbitrator;
        private IEvent ready;

        // private fields
        private bool eventHappened = false;
        private bool requestAccepted = false;

        /// <summary>
        /// A simple class to request and release a slot in an arbitrator queue. It is triggered with an
        /// IEvent and a 'ready' event is sent when it is time to perform actions. The event can then be
        /// finished with an IEventB release event.
        /// </summary>
        public RequestArbitrator() { }

        /// <summary>
        /// Sets up the listener for the IEventB request event.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (request != null)
            {
                request.EventHappened += async () =>
                {
                    // don't call twice
                    if (eventHappened) return;

                    eventHappened = true;
                    await arbitrator.Request(InstanceName);
                    requestAccepted = true;
                    ready?.Execute();
                };
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // if our request hasn't been accepted yet we can't release
            if (!requestAccepted) return;
            arbitrator.Release(InstanceName);
            eventHappened = false;
            requestAccepted = false;
        }
    }
}
