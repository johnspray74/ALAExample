using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Groups a collection of Tuples together as a Dictionary.</para>
    /// <para>Ports:</para>
    /// <para>1. IEvent start: Starts the process.</para>
    /// <para>2. List&lt;IDataFlow&lt;Tuple&lt;T1, T2&gt;&gt;&gt; pairs: The input pairs.</para>
    /// <para>3. IDataFlow&lt;Dictionary&lt;T1, T2&gt;&gt; listOutput: Outputs the pairs as a dictionary.</para>
    /// </summary>
    public class ConvertPairsToDictionary<T1, T2> : IEvent
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private Dictionary<T1, T2> dict = new Dictionary<T1, T2>();

        // Ports
        private List<IDataFlowB<Tuple<T1, T2>>> pairs;
        private IDataFlow<Dictionary<T1, T2>> dictOutput;

        /// <summary>
        /// <para>Groups a collection of Tuples together as a Dictionary.</para>
        /// </summary>
        public ConvertPairsToDictionary() { }

        // IEvent implementation
        void IEvent.Execute()
        {
            foreach (var pair in pairs)
            {
                dict[pair.Data.Item1] = pair.Data.Item2;
            }

            if (dictOutput != null) dictOutput.Data = dict;
        }
    }
}
