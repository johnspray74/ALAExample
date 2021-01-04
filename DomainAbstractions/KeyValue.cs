using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>A decorator for any abstraction that has an IDataFlow&lt;Dictionary&lt;string, T&gt;&gt; output.</para>
    /// <para>------------------------------------------------------------------------------------------------------------------</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; "NEEDNAME": input string that wants to be added to the internal dictionary (only adds unique values)</para>
    /// <para>2. IDataFlow&lt;Dictionary&lt;string, T&gt;&gt; "NEEDNAME": input port that instatiates the internal dictionary</para>
    /// <para>3. IDataFlow&lt;T&gt; valueOutput: generic output of the value that matches the key</para>
    /// </summary>
    public class KeyValue<T> : IDataFlow<string>, IDataFlow<Dictionary<string, T>>
    {
        // Properties -------------------------------------------------------
        public string InstanceName = "Default";
        public string Key = "";
        public List<string> Keys = new List<string>();
        public Dictionary<string, T> Dict = new Dictionary<string, T>();

        // Private fields
        private bool keyNotFound = false;

        // Ports
        private List<IDataFlow<T>> valueOutputs;
        private IDataFlow<Dictionary<string, T>> dictOutput;
        private IDataFlow<bool> keyNotFoundOutput;

        /// <summary>
        /// <para>A decorator for any abstraction that has an IDataFlow&lt;Dictionary&lt;string, T&gt;&gt; output.</para>
        /// </summary>
        public KeyValue(string key = null)
        {
            if (!string.IsNullOrEmpty(key)) Key = key;
        }
        
        // IDataFlow<string> implementation ------------------------------------------------
        string IDataFlow<string>.Data
        {
            set
            {
                if (Dict.ContainsKey(value) && valueOutputs != null) valueOutputs.FirstOrDefault().Data = Dict[value];
            }
        }

        // IDataFlow<Dictionary<string, T>> implementation ------------------------------------------------
        Dictionary<string, T> IDataFlow<Dictionary<string, T>>.Data
        {
            set
            {
                if (valueOutputs == null) return;
                if (!string.IsNullOrEmpty(Key)) Keys.Add(Key);
                for (int i = 0; i < Keys.Count; i++)
                {
                    if (i >= valueOutputs.Count) break;
                    if (value != null && value.ContainsKey(Keys[i]))
                    {
                        valueOutputs[i].Data = value[Keys[i]]; 
                    }
                    else
                    {
                        keyNotFound = true;
                    }
                }
                if (keyNotFoundOutput != null) keyNotFoundOutput.Data = keyNotFound;

                if (dictOutput != null) dictOutput.Data = value;
            }
        }
    }
}
