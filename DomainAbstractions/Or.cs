using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Or operator that checks for any inputs to be true to output true and outputs a false if all inputs are false.
    /// Uses a List of IDataFlowB for a fan in.
    /// Only pushes to the output port if the result is different from previous e.g. previously was a 'true' from all the inputs,
    /// and now after data is changed, and is now a 'false' result, then it will push. Otherwise if the result remains as 'true' even
    /// when data is changed, it will not push to the output port.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. List<IDataFlowB<bool>> listOfInputs: list of inputs that will be listened for data changed
    /// 2. IDataFlow<bool> booleanResult: boolean output value after Or is performed on the entire list of inputs
    /// </summary>

    public class Or
    {
        // Properties
        public string InstanceName = "Default";

        // Ports
        private List<IDataFlowB<bool>> listOfInputs;
        private IDataFlow<bool> booleanResult;

        // Private fields
        private Dictionary<int, bool> logicalInputCache = new Dictionary<int, bool>();
        private bool initialiseFlag = true;
        /// <summary>
        /// Or operator that checks for any inputs to be true to output true and outputs a false if all inputs are false.
        /// Uses a List of IDataFlowB for a fan in.
        /// </summary>
        public Or() { }

        private void PostWiringInitialize()
        {
            foreach (IDataFlowB<bool> input in listOfInputs)
            {
                logicalInputCache.Add(input.GetHashCode(), false);

                input.DataChanged += () =>
                {
                    var lastStatus = logicalInputCache.Any(kvp => kvp.Value == true);

                    if (logicalInputCache.ContainsKey(input.GetHashCode()))
                    {
                        logicalInputCache[input.GetHashCode()] = input.Data;
                    }
                    else
                    {
                        throw new ArgumentException("No valid input in cache dictionary.");
                    }

                    var newStatus = logicalInputCache.Any(kvp => kvp.Value == true);

                    if (newStatus != lastStatus || initialiseFlag)
                    {
                        booleanResult.Data = newStatus;

                        initialiseFlag = false;
                    }
                };
            }
        }

    }
}
