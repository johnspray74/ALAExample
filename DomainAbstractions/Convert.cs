using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Converts a value between two types.
    /// -----------------------------------
    /// Ports:
    /// 1. IDataFlow<A> input: The input to convert.
    /// 2. IDataFlowB<B> outputB: The conversion output.
    /// 3. IDataFlow<B> output: The conversion output. 
    /// </summary>
    /// <typeparam name="A">Original type.</typeparam>
    /// <typeparam name="B">Type to convert to.</typeparam>
    class Convert<A, B> : IDataFlow<A>, IDataFlowB<B> // input, outputB
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<B> output;

        // private fields
        private B outputValue;

        /// <summary>
        /// Converts a value between two types.
        /// </summary>
        public Convert() { }

        // IDataFlow<A> implementation
        A IDataFlow<A>.Data
        {
            set
            {
                outputValue = (B)System.Convert.ChangeType(value, typeof(B));
                DataChanged?.Invoke();
                if (output != null) output.Data = outputValue;
            }
        }

        // IDataFlowB<B> implementation
        B IDataFlowB<B>.Data { get => outputValue; }
        public event DataChangedDelegate DataChanged;
    }
}