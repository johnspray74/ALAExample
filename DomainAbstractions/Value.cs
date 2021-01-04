using ProgrammingParadigms;
using System.Collections.Generic;

namespace DomainAbstractions
{
    /// <summary>
    /// [DEPRECATED]
    /// Can have multiple IDataFlow inputs and outputs
    /// As implied by the way the IDataFlow interfaces works,
    /// stores the value when an event comes from an input, and emits events to the outputs.
    /// All the outputs can read the value at any time.
    /// </summary>
    public class Value : IDataFlow<bool>, IEvent
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private List<IEvent> eventOutputs = new List<IEvent>();
        private List<IDataFlow<bool>> dataFlowOutputs = new List<IDataFlow<bool>>();

        /// <summary>
        /// Store and fan-out values and events.
        /// </summary>
        public Value() { }

        // IDataFlow<T> implementation -------------------------------------------
        private bool data;
        bool IDataFlow<bool>.Data { set => Fanout(value); }

        // IEvent implementation -------------------------------------------------
        void IEvent.Execute() => Fanout(data);

        // private methods -------------------------------------------------------
        private void Fanout(bool value)
        {
            data = value;
            if (value)
            {
                foreach (var e in eventOutputs) e.Execute();
                foreach (var d in dataFlowOutputs) d.Data = value;
            }
        }
    }
}
