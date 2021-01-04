using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Toggles a boolean output on receiving an IEvent.
    /// Default boolean is false so the output will first be true but property can be set to true to begin with.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent inputEvent: input event that is wanted to be changed to a boolean output
    /// 2. IDataFlow<bool> booleanOutput: result boolean output
    /// </summary>
    public class ConvertEventToBoolean : IEvent
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";
        public bool KeepTrue = false;
        public bool InitialBoolean { set => boolean = value; }

        // ports -----------------------------------------------------------------
        private IDataFlow<bool> booleanOutput;

        // private fields -----------------------------------------------------------------
        private bool boolean = false;

        /// <summary>
        /// Toggles a boolean output on receiving an IEvent.
        /// </summary>
        public ConvertEventToBoolean() { }

        // IEvent implementation ------------------------------------------------------
        void IEvent.Execute()
        {
            boolean = KeepTrue ? true : !boolean;
            booleanOutput.Data = boolean;
        }
    }
}
