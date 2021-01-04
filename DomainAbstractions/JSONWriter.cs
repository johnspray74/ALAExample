using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Newtonsoft.Json;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Writes an input of type T to a file, at the provided file path, as JSON.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; input: The value to be written to a file.</para>
    /// </summary>
    public class JSONWriter<T> : IDataFlow<T>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private string path;

        /// <summary>
        /// <para>Writes an input of type T to a file, at the provided file path, as JSON.</para>
        /// </summary>
        /// <param name="path"></param>
        public JSONWriter(string path)
        {
            this.path = path;
        }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, value);
                }
            }
        }
    }
}
