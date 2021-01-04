using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Converts any value of type T to an object. This uses the "as" keyword, so if the conversion is unsuccessful, the output will be null. Also supports JToken output.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; input: the value to convert. </para>
    /// <para>2. IDataFlow&lt;object&gt; outputAsObject: the instance as an object. </para>
    /// <para>3. IDataFlow&lt;JToken&gt; outputAsJToken: the instance as a JToken. </para>
    /// </summary>

    public class AsObject<T> : IDataFlow<T>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields

        // Ports
        private IDataFlow<object> outputAsObject;
        private IDataFlow<JToken> outputAsJToken;

        /// <summary>
        /// <para>Converts any value of type T to an object. This uses the "as" keyword, so if the conversion is unsuccessful, the output will be null. Also supports JToken output.</para>
        /// </summary>
        public AsObject() { }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                var valueAsObject = value as object;
                valueAsObject.LogDataChange(InstanceName, "outputAsObject", $"IDataFlow<{typeof(T)}>");
                if (outputAsObject != null) outputAsObject.Data = valueAsObject;

                var jsonSerializer = JsonSerializer.CreateDefault();
                JToken jToken;
                using (JTokenWriter jsonWriter = new JTokenWriter())
                {
                    jsonSerializer.Serialize(jsonWriter, valueAsObject);
                    jToken = jsonWriter.Token!;
                }

                jToken.LogDataChange(InstanceName, "outputAsJToken", $"IDataFlow<{typeof(T)}>");
                outputAsJToken.Data = jToken;
            }
        }

    }
}