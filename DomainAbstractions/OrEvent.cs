using ProgrammingParadigms;
using System;
using System.Collections.Generic;

namespace DomainAbstractions
{
    /// <summary>
    /// A logical OR gate for events. When one IEventB fires, an IEvent logicalOutput will fire.
    /// The gate will only fire ONCE until after it has been reset with an IEvent NEEDNAME,
    /// which then the process will repeat.
    /// ---------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Resets the gate.
    /// 2. IEventB NEEDNAME: The logical output for the gate.
    /// 3. List<IEventB> logicalInputs: The logical inputs for the gate.
    /// 4. IEvent logicalOutput: The logical output for the gate.
    /// </summary>
    class OrEvent : IEvent, IEventB
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private List<IEventB> logicalInputs;
        private IEvent logicalOutput;

        // private fields
        private bool hasFired = false;

        /// <summary>
        /// A logical OR gate for events. When one IEventB fires, an IEvent logicalOutput will fire.
        /// The gate will only fire ONCE until after it has been reset with an IEvent NEEDNAME,
        /// which then the process will repeat.
        /// </summary>
        public OrEvent() { }

        /// <summary>
        /// Sets up listeners for the events.
        /// </summary>
        private void PostWiringInitialize()
        {
            foreach (IEventB _event in logicalInputs)
            {
                _event.EventHappened += () =>
                {
                    if (hasFired) return;
                    logicalOutput?.Execute();
                    EventHappened?.Invoke();
                    hasFired = true;
                };
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // reset the gate
            hasFired = false;
        }

        // IEventB implementation
        public event CallBackDelegate EventHappened;
    }
}
