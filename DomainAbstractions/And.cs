using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Outputs a boolean value of true when all of its inputs are true. Null inputs are treated as false.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. List<IDataFlowB<bool>> logicalInputs: list of boolean inputs
    /// 2. IDataFlow<bool> logicalOutput: boolean output result
    /// </summary>

    public class And
    {
        // Properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // Ports ---------------------------------------------------------------
        private List<IDataFlowB<bool>> logicalInputs;
        private IDataFlow<bool> logicalOutput;

        // Private fields ---------------------------------------------------------------
        // private bool output = true;
        private Dictionary<int, bool> logicalInputCache = new Dictionary<int, bool>();
        private bool initialiseFlag = true;

        /// <summary>
        /// Outputs a boolean value of true when all of its inputs are true. Null inputs are treated as false.
        /// </summary>
        public And() { }

        private void PostWiringInitialize()
        {
            foreach (IDataFlowB<bool> input in logicalInputs)
            {
                logicalInputCache.Add(input.GetHashCode(), false);

                input.DataChanged += () =>
                {
                    var lastStatus = logicalInputCache.All(kvp => kvp.Value == true);

                    if (logicalInputCache.ContainsKey(input.GetHashCode()))
                    {
                        logicalInputCache[input.GetHashCode()] = input.Data;
                    }
                    else
                    {
                        throw new ArgumentException("No valid input in cache dictionary.");
                    }

                    var newStatus = logicalInputCache.All(kvp => kvp.Value == true);

                    if(newStatus != lastStatus || initialiseFlag)
                    {
                        logicalOutput.Data = newStatus;

                        initialiseFlag = false;
                    }
                };
            }
        }
    }
}
