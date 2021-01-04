using ProgrammingParadigms;
using System.Collections.Generic;
using System.Linq;

namespace DomainAbstractions
{
    /// <summary>
    /// A logical AND gate for events. Once all given IEventB's fire, an IEvent logicalOutput will fire,
    /// and the gate will reset. The gate can also be reset through the IEvent NEEDNAME.
    /// -------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent reset: Resets the gate.
    /// 2. IEventB b_Output: The logical output for the gate.
    /// 2. List<IEventB> logicalInputs: The logical inputs for the gate.
    /// 3. IEvent logicalOutput: The logical output for the gate.
    /// </summary>
    class AndEvent : IEvent, IEventB // reset, b_Output
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private List<IEventB> logicalInputs;
        private IEvent logicalOutput;

        // private fields
        private Dictionary<int, bool> hasFired = new Dictionary<int, bool>();

        /// <summary>
        /// A logical AND gate for events. Once all given IEventB's fire, an IEvent logicalOutput will fire,
        /// and the gate will reset. The gate can also be reset through the IEvent NEEDNAME.
        /// </summary>
        public AndEvent() { }

        /// <summary>
        /// Sets up listeners for the events.
        /// </summary>
        private void PostWiringInitialize()
        {
            for (int i = 0; i < logicalInputs.Count; i++)
            {
                int key = i;
                IEventB _event = logicalInputs[key];
                hasFired[key] = false;

                _event.EventHappened += () =>
                {
                    System.Console.WriteLine($"AndEvent {InstanceName} Key {key} FIRED");
                    hasFired[key] = true;
                    CheckIfAllHaveFired();
                };
            }
        }

        /// <summary>
        /// Checks if all the events have been fired and fires the output event.
        /// </summary>
        private void CheckIfAllHaveFired()
        {
            foreach (bool value in hasFired.Values.ToList())
            {
                if (!value) return;
            }

            System.Console.WriteLine($"AndEvent {InstanceName} FIRING");

            logicalOutput?.Execute();
            EventHappened?.Invoke();
            Reset();
        }

        /// <summary>
        /// Resets the AND gate.
        /// </summary>
        private void Reset()
        {
            System.Console.WriteLine($"AndEvent {InstanceName} RESETTING");
            foreach (int key in hasFired.Keys.ToList())
            {
                hasFired[key] = false;
            }
        }

        // IEvent iomplementation
        void IEvent.Execute() => Reset();

        // IEventB implementation
        public event CallBackDelegate EventHappened;
    }
}
