using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>A unique collection of items of type T, i.e. a set.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; element: A single element to be added to the collection (if it doesn't already exist in it)</para>
    /// <para>2. IDataFlow&lt;List&lt;T&gt;&gt; addCollection: A collection that will undergo a set union with the existing collection</para>
    /// <para>3. IDataFlow&lt;HashSet&lt;T&gt;&gt; outputAsHashSet: The collection as a HashSet&lt;T&gt;</para>
    /// <para>4. IDataFlow&lt;List&lt;T&gt;&gt; outputAsList: The collection as a List&lt;T&gt;</para>
    /// </summary>

    public class UniqueCollection<T> : IDataFlow<T>, IDataFlow<List<T>>, IEvent
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private HashSet<T> collection = new HashSet<T>();

        // Ports
        private IDataFlow<HashSet<T>> outputAsHashSet;
        private IDataFlow<List<T>> outputAsList;

        /// <summary>
        /// A unique collection of items of type T, i.e. a set.
        /// </summary>
        public UniqueCollection() { }

        public void Test()
        {
            collection.EnumerablePrinter();
        }

        private void Output()
        {
            if (outputAsHashSet != null) outputAsHashSet.Data = collection;
            if (outputAsList != null) outputAsList.Data = collection.ToList();
        }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                collection.Add(value);
                Output();
            }
        }

        // IDataFlow<List<T>> implementation
        List<T> IDataFlow<List<T>>.Data
        {
            set
            {
                foreach (T element in value)
                {
                    collection.Add(element);
                }

                Output();
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // Test();
            collection.Clear();
        }

    }
}