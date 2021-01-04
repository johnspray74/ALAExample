using ProgrammingParadigms;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// Compares two values. Evaluates the expression {inputOne}>{inputTwo}, and sets the return
    /// data to be either true or false.
    /// ----------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlowB<T> inputOne: first input
    /// 2. IDataFlowB<T> inputTwo: second input
    /// 3. IDataFlow<bool> output: whether first input is greater than second input
    /// 4. IDataFlowB<bool> outputB: whether first input is greater than second input 
    /// </summary>
    /// <typeparam name="T">The type of value to compare. Must implement IComparable.</typeparam>
    class GreaterThan<T> : IDataFlowB<bool> where T : IComparable // outputB
    {
        // properties
        public string InstanceName = "Default";
        public T InputOne { set => valueOne = value; }
        public T InputTwo { set => valueTwo = value; }

        // ports
        private IDataFlowB<T> inputOne;
        private IDataFlowB<T> inputTwo;
        private IDataFlow<bool> output;

        // private fields
        private T valueOne;
        private T valueTwo;
        private bool result;

        /// <summary>
        /// Compares two values. Evaluates the expression {inputOne}>{inputTwo}, and sets the return
        /// data to be either true or false.
        /// </summary>
        public GreaterThan() { }

        /// <summary>
        /// Sets up the listeners for the input ports.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (inputOne != null)
            {
                inputOne.DataChanged += () =>
                {
                    valueOne = inputOne.Data;
                    Compare();
                };
            }

            if (inputTwo != null)
            {
                inputTwo.DataChanged += () =>
                {
                    valueTwo = inputTwo.Data;
                    Compare();
                };
            }
        }

        /// <summary>
        /// Compares the two values and updates the output ports.
        /// </summary>
        private void Compare()
        {
            result = valueOne.CompareTo(valueTwo) > 0 ? true : false;
            if (output != null) output.Data = result;
            DataChanged?.Invoke();
        }

        // IDataFlowB implementation
        bool IDataFlowB<bool>.Data { get => result; }
        public event DataChangedDelegate DataChanged;
    }
}
