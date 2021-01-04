using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Serialises an object of type T into a JSON string.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; object: object to serialise</para>
    /// <para>2. IDataFlow&lt;string&gt; jsonOutput: the JSON-formatted string output</para>
    /// </summary>

    public class ToJSON<T> : IDataFlow<T>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields

        // Ports
        private IDataFlow<string> jsonOutput;

        /// <summary>
        /// 
        /// </summary>
        public ToJSON() { }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data { set => jsonOutput.Data = JsonConvert.SerializeObject(value);}

    }
}