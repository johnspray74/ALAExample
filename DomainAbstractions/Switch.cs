using ProgrammingParadigms;
using System;
using System.Collections.Generic;

namespace DomainAbstractions
{
    /// <summary>
    /// Acts as a switch statement with ports. Takes a list of IDataFlowB inputs and runs a given
    /// lambda on each input, and compares the result of the lambda to the given compare value, which
    /// can be given through the IDataFlow implementation. A default can also be given through
    /// the IDataFlowB defaultInput property. The output is then pushed out via the IDataFlow output
    /// port and the IDataFlowB implementation.
    /// ----------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<C> compareValue: The value to compare to.
    /// 2. IDataFlowB<T> outputB: The output from the switch statement.
    /// 3. List<IDataFlowB<T>> inputs: list of inputs to the switch statement.
    /// 4. IDataFlow<T> output: The output from the switch statement.
    /// 5. IDataFlowB<T> defaultInput: default value for the switch statement.
    /// </summary>
    /// <typeparam name="T">The type that we are switching.</typeparam>
    /// <typeparam name="C">The value that we are comparing. Must implement IComparable.</typeparam>
    class Switch<T, C> : IDataFlow<C>, IDataFlowB<T> where C : IComparable // compareValue, outputB
    {
        // properties
        public string InstanceName = "Default";
        public delegate C LambdaDelegate(T x);
        public LambdaDelegate Lambda;
        public C[] compareValues = new C[] { };

        // ports
        private List<IDataFlowB<T>> inputs = new List<IDataFlowB<T>>();
        private IDataFlow<T> output;
        private IDataFlowB<T> defaultInput;

        // private properties
        private T switchDefault;
        private T outputData;
        private C compareValue;

        /// <summary>
        /// Acts as a switch statement with ports. Takes a list of IDataFlowB inputs and runs a given
        /// lambda on each input, and compares the result of the lambda to the given compare value, which
        /// can be given through the IDataFlow implementation. A default can also be given through
        /// the IDataFlowB defaultInput property. The output is then pushed out via the IDataFlow output
        /// port and the IDataFlowB implementation.
        /// </summary>
        public Switch() { }

        /// <summary>
        /// Adds data changed listeners for the inputs.
        /// </summary>
        private void PostWiringInitialize()
        {
            foreach (IDataFlowB<T> input in inputs)
            {
                input.DataChanged += () => HandleSwitch();
            }

            HandleSwitch();
        }

        /// <summary>
        /// Performs the "switch" statement.
        /// </summary>
        private void HandleSwitch()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                C compareTo;
                IDataFlowB<T> input = inputs[i];
                if (input.Data == null) continue;

                if (Lambda != null)
                {
                    compareTo = Lambda(input.Data);
                }
                else
                {
                    compareTo = compareValues[i];
                }

                if (compareTo.Equals(compareValue))
                {
                    SetOutputs(inputs[i].Data);
                    return;
                }
            }

            if (defaultInput != null) SetOutputs(defaultInput.Data);
        }

        /// <summary>
        /// Sets the outputs of the switch.
        /// </summary>
        /// <param name="value">The value to set.</param>
        private void SetOutputs(T value)
        {
            outputData = value;
            if (output != null) output.Data = outputData;
            DataChanged?.Invoke();
        }

        // IDataFlow<C> implementation
        C IDataFlow<C>.Data { set { compareValue = value;  HandleSwitch(); } }

        // IDataFlowB<T> implementation
        T IDataFlowB<T>.Data { get => outputData; }
        public event DataChangedDelegate DataChanged;
    }
}
