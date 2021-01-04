using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>A counter that can be incremented by an IEvent, or have its value set by an IDataFlow&lt;int&gt;.</para>
    /// <para>Ports:</para>
    /// <para>1. IEvent incrementCount: Increments the counter by 1.</para>
    /// <para>2. IDataFlow&lt;int&gt; resetToValue: Sets the counter value.</para>
    /// <para>2. IDataFlowB&lt;string&gt; countAsString: Returns the current count as a string.</para>
    /// </summary>
    public class Counter : IEvent, IDataFlow<int>, IDataFlowB<string>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private int currentCount = 0;

        // Ports
        private IDataFlow<int> count;

        /// <summary>
        /// <para>A counter that can be incremented by an IEvent, or have its value set by an IDataFlow&lt;int&gt;.</para>
        /// </summary>
        public Counter() { }

        // IEvent implementation
        void IEvent.Execute()
        {
            currentCount++;
            if (count != null) count.Data = currentCount;
            DataChanged?.Invoke();
        }

        // IDataFlow<int> implementation
        int IDataFlow<int>.Data
        {
            set
            {
                currentCount = value;
                DataChanged?.Invoke();
                if (count != null) count.Data = currentCount;
            }
        }

        // IDataFlowB<string> implementation
        public event DataChangedDelegate DataChanged;
        
        string IDataFlowB<string>.Data { get => currentCount.ToString(); }
    }
}
