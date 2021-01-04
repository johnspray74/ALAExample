using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// The EventGate is used to control whether an event pushes through or not by using the latch of the gate (blocking and storing an IEvent).
    /// When an IDataFlow<bool> event arrives, the gate will be turned on or off with a boolean value. (true = open, false = closed)
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent incoming: when the IEvent inputs comes in, it does not go to the eventOutput directly, instead it checks the LatchInput (the gate is open or closed), then decides to let the event go or just store here.
    /// 2. IDataFlow<bool> latchInput: input to control the LatchInput, when it is true and there is an event received and stored, the eventOutput will be executed, otherwise it just closes the gate.
    /// 3. IEventB NEEDNAME: input event through an IEventB port
    /// 4. IEvent eventOutput: event that outputs when the latch is true
    //// </summary>

    public class EventGate : IEvent, IDataFlow<bool> // incoming, latchInput
    {
        // properties ---------------------------------------------------------------
        public bool LatchInput = false;
        public bool LatchEvent = true;
        public string InstanceName = "Default";

        // ports ---------------------------------------------------------------
        private IEventB eventInput;
        private IEvent eventOutput;

        /// <summary>
        /// Controls the latch of the gate to let an event go through or not.
        /// </summary>
        public EventGate() { }

        /// <summary>
        /// Sets up the listener for the IEventB input event.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (eventInput != null)
            {
                eventInput.EventHappened += () => InputEventHappened();
            }
        }

        /// <summary>
        /// Triggered when either of the input events happen.
        /// </summary>
        private void InputEventHappened()
        {
            if (LatchInput)
            {
                eventOutput?.Execute();
                EventHappened?.Invoke();
            }
            else if (LatchEvent)
            {
                eventRecieved = true;
            }
        }

        // IDataFlow<bool> implementation -----------------------------------------------------------------
        bool IDataFlow<bool>.Data
        {
            set
            {
                LatchInput = value;

                if (value && eventRecieved)
                {
                    eventRecieved = false;
                    eventOutput?.Execute();
                    EventHappened?.Invoke();
                }                
            }
        }

        // IEvent implementation -----------------------------------------------------------------
        private bool eventRecieved = false;
        void IEvent.Execute() => InputEventHappened();

        // IEventB implementation
        public event CallBackDelegate EventHappened;
    }
}
