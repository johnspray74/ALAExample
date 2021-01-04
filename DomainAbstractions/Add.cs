using ProgrammingParadigms;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// The expression itself is code like {1}+{2} which adds the input from ports 1 and 2 and outputs the result as a string
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports
    /// 1. IDataFlowB<string> inputOne: first input as string
    /// 2. IDataFlowB<string> inputTwo: second input as string
    /// 3. IDataFlow<string> outputResult: output summation result as a string
    /// </summary>
    public class Add
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";
        public int input1 { set => firstInput = value; }
        public int input2 { set => secondInput = value; }

        // ports ---------------------------------------------------------------
        private IDataFlowB<string> inputOne;
        private IDataFlowB<string> inputTwo;
        private IDataFlow<string> outputResult;

        // private fields ---------------------------------------------------------------
        private int firstInput = default(int);
        private int secondInput = default(int);

        /// <summary>
        /// Adds two inputs and outputs the summation result as a string
        /// </summary>
        public Add() { }

        private void PostWiringInitialize()
        {
            if(inputOne != null)
            {
                inputOne.DataChanged += () =>
                {
                    try
                    {
                        firstInput = Int32.Parse(inputOne.Data);
                    }
                    catch (Exception)
                    {}

                    if (checkBothReceived())
                    {
                        outputResult.Data = (firstInput + secondInput).ToString();
                    }
                };
            }

            if(inputTwo != null)
            {
                inputTwo.DataChanged += () =>
                {
                    try
                    {
                        secondInput = Int32.Parse(inputTwo.Data);
                    }
                    catch (Exception)
                    {}

                    if (checkBothReceived())
                    {
                        outputResult.Data = (firstInput + secondInput).ToString();
                    }

                };
            }

        }

        private bool checkBothReceived()
        {
            return (firstInput != default(int) && secondInput != default(int));
        }
    }
}
