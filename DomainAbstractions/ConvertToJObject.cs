using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DomainAbstractions
{
    /// <summary>
    /// Creates and outputs a JObject from either an input object or a List<JProperty>.
    /// Ports:
    /// 1. IDataFlow<object> objectInput
    /// 2. IDataFlow<List<JProperty>> listInput
    /// 3. IDataFlow<JObject> output
    /// </summary>
    public class ConvertToJObject : IDataFlow<object>, IDataFlow<List<JProperty>>
    {
        // Properties
        public string InstanceName = "Default";

        // Ports
        private IDataFlow<JObject> output;

        /// <summary>
        /// Creates and outputs a JObject from either an input object or a List<JProperty>.
        /// </summary>
        public ConvertToJObject() { }

        // IDataFlow<T> implementation
        object IDataFlow<object>.Data
        {
            set
            {
                output.Data = JObject.FromObject(value);   
            }
        }

        // IDataFlow<List<JProperty>> implementation
        List<JProperty> IDataFlow<List<JProperty>>.Data
        {
            set
            {
                var jObject = new JObject();
                foreach (var jProperty in value)
                {
                    if (!jObject.ContainsKey(jProperty.Name)) jObject.Add(jProperty);
                }

                output.Data = jObject;
            }
        }
    }
}
