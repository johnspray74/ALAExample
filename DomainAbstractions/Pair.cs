using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>An abstraction that contains a pair as a Tuple. When it receives an IEvent or an incoming Item1, Item2 is extracted and the tuple is made ready to be sent out.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T1&gt; Item1: The first element in the pair. Pulls Item2 when this is received.</para>
    /// <para>2. IEvent start: The start event. Pulls Item2 when this is received.</para>
    /// <para>3. IDataFlowB&lt;Tuple&lt;T1, T2&gt;&gt; pairOutputB: Returns the pair as a Tuple.</para>
    /// <para>4. IDataFlowB&lt;T2&gt; Item2: The second item in the pair.</para>
    /// <para>5. IDataFlow&lt;Tuple&lt;T1, T2&gt;&gt; pairOutput: Sends out the pair as a Tuple.</para>
    /// </summary>
    public class Pair<T1, T2> : IEvent, IDataFlow<T1>, IDataFlowB<Tuple<T1, T2>>
    {
        // Properties
        public string InstanceName = "Default";
        public T1 Item1
        {
            get => item1;
            set => item1 = value;
        }

        // Private fields
        private T1 item1;
        private Tuple<T1, T2> tuple;

        // Ports
        private IDataFlowB<T2> Item2;
        private IDataFlow<Tuple<T1, T2>> pairOutput;

        /// <summary>
        /// <para>An abstraction that contains a pair as a Tuple. When it receives an IEvent or an incoming Item1, Item2 is extracted and the tuple is made ready to be sent out.</para>
        /// </summary>
        public Pair() { }

        private void Create()
        {
            tuple = new Tuple<T1, T2>(item1 ?? default, Item2 != null ? Item2.Data : default);
            DataChanged?.Invoke();
        }

        private void Push()
        {
            if (pairOutput != null) pairOutput.Data = tuple;
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            Create();
            Push();
        }

        // IDataFlow<T1> implementation
        T1 IDataFlow<T1>.Data
        {
            set
            {
                item1 = value;
                Create();
                Push();
            }
        }

        // IDataFlowB<T2> implementation
        public event DataChangedDelegate DataChanged;

        Tuple<T1, T2> IDataFlowB<Tuple<T1, T2>>.Data
        {
            get
            {
                Create();
                return tuple;
            }
        }
    }
}
