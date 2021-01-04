using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>[DEPRECATED: Replaced by Collection&lt;T&gt;]</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;&gt;</para>
    /// </summary>
    public class ToList<T> : IDataFlow<T>, IEvent
    {
        // Properties
        public string InstanceName = "Default";
        public int OutputLength = -1; // Output when the list reaches this length, -1 for output at any length, -2 to disable auto-output
        public bool OutputOnEvent = false;

        // Private fields
        List<T> list = new List<T>();

        // Ports
        private IDataFlow<List<T>> listOutput;

        /// <summary>
        /// 
        /// </summary>
        public ToList() { }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                list.Add(value);
                if ((OutputLength == -1 || list.Count == OutputLength) && listOutput != null)
                {
                    listOutput.Data = list;
                }
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            var temp = list.Select(s => s).ToList();
            list.Clear();
            if (listOutput != null && OutputOnEvent) listOutput.Data = temp;
        }
    }
}
