using ProgrammingParadigms;
using System.Collections.Generic;

namespace DomainAbstractions
{
    /// <summary>
    /// Retrieves a value from a dictionary when given a key.
    /// -----------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<Dictionary<K,V>> NEEDNAME: The dictionary to retrieve the key from.
    /// 2. IDataFlow<V> output: The value of the key in the dictionary.
    /// </summary>
    /// <typeparam name="K">The key type of the dictionary.</typeparam>
    /// <typeparam name="V">The value type of the dictionary.</typeparam>
    class RetrieveValueFromDict<K, V> : IDataFlow<Dictionary<K, V>>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<V> output;

        // private parameters
        private K key;

        /// <summary>
        /// Retrieves a value from a dictionary when given a key.
        /// </summary>
        /// <param name="key">The key to search for in the dictionary.</param>
        public RetrieveValueFromDict(K key)
        {
            this.key = key;
        }

        // IDataFlow implementation
        Dictionary<K, V> IDataFlow<Dictionary<K, V>>.Data
        {
            set
            {
                // can't retrieve the key if it doesnt exist, also can't set the output
                // if it doesn't exist
                if (!value.ContainsKey(key) || output == null) return;
                output.Data = value[key];
            }
        }
    }
}
