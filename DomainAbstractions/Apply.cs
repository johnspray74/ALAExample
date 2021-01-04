using System;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Applies a lambda on an input of type T1 and returns an output of type T2.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow<T1> input: The input to the lambda.</para>
    /// <para>2. IDataFlowB<T2> getOutputFrom_bPort: The oputput from the lambda.</para>
    /// <para>3. IDataFlow<T2> output: The output from the lambda.</para>
    /// </summary>
    public class Apply<T1, T2> : IDataFlow<T1>, IDataFlowB<T2> // input, getOutputFrom_bPort
    {
        // Properties
        public string InstanceName = "Default";
        public delegate T2 LambdaDelegate(T1 x);
        public LambdaDelegate Lambda;
        public bool LatchData;

        // Private fields
        private T2 storedValue;

        // Ports
        private IDataFlow<T2> output;

        /// <summary>
        /// <para>Applies a lambda on an input of type T1 and returns an output of type T2.</para>
        /// <param name="latchData">Whether to latch the input when it goes null.</param>
        /// </summary>
        public Apply(bool latchData = false)
        {
            LatchData = latchData;
        }

        /// <summary>
        /// Calls the given lambda and sets the data.
        /// </summary>
        /// <param name="value">The data to act upon.</param>
        private void SetData(T1 value)
        {
            if (LatchData && value == null) return;

            try
            {
                storedValue = Lambda(value);
            }
            catch (Exception e)
            {
                Libraries.Logging.Log(e);
            }

            DataChanged?.Invoke();
            if (output != null && storedValue != null) output.Data = storedValue;
        }

        // IDataFlow<T1> implementation
        T1 IDataFlow<T1>.Data { set => SetData(value); }

        // IDataFlowB<T2> implementation
        T2 IDataFlowB<T2>.Data { get => storedValue; }

        public event DataChangedDelegate DataChanged;
    }
}