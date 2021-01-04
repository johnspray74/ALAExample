using ProgrammingParadigms;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// The expression itself is code like {1}+{2} which adds the input from ports 1 and 2 
    /// Port left is the output
    /// Port 1 is input 1
    /// Port 2 is input 2
    /// 
    /// KL TBD: need to make more general, also add check for first number bigger
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow inputOne: output of row count
    /// 2. ITableDataFlow: input 
    /// </summary>
    public class Subtract
    {
        // properties
        public string InstanceName = "Default";
        public int input1 { set => firstInput = value; }
        public int input2 { set => secondInput = value; }

        // ports
        private IDataFlowB<string> inputOne;
        private IDataFlowB<string> inputTwo;
        private IDataFlow<string> outputResult;

        // private fields
        private int firstInput = default(int);
        private int secondInput = default(int);

        public Subtract() { }

        private void PostWiringInitialize()
        {
            if (inputOne != null)
            {
                inputOne.DataChanged += () =>
                {
                    firstInput = Int32.Parse(inputOne.Data);

                    if (checkBothReceived())
                    {
                        outputResult.Data = (firstInput - secondInput).ToString();
                        //firstInput = default(int);
                    }
                };
            }

            if (inputTwo != null)
            {
                inputTwo.DataChanged += () =>
                {
                    secondInput = Int32.Parse(inputTwo.Data);

                    if (checkBothReceived())
                    {
                        outputResult.Data = (firstInput - secondInput).ToString();
                        //firstInput = default(int);
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
