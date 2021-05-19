using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Converts any kind of IDataFlow (once arrived) to an IEvent. 
    /// The Generic Type 'T' should be assigned when it is instantiated.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> inputData: input data of generic type
    /// 2. IEvent eventOutput: output event
    /// </summary>
    public class ConvertToEvent<T> : IDataFlow<T> // inputData
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // port ---------------------------------------------------------------
        private IEvent eventOutput;

        /// <summary>
        /// Converts any kind of IDataFlow to an IEvent.
        /// </summary>
        public ConvertToEvent() { }

        // IDataFlow<T> implementation -----------------------------------------------------------------
        T IDataFlow<T>.Data { set => eventOutput.Execute(); }
    }
}
