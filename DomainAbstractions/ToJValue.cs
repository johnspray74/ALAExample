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
    /// <para>Converts a value of type T into a JValue.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; inputValue: the value to convert.</para>
    /// <para>2. IDataFlow&lt;T&gt; outputAsJValue: the converted JValue.</para>
    /// </summary>

    public class ToJValue<T> : IDataFlow<T>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields

        // Ports
        private IDataFlow<JValue> outputAsJValue;

        /// <summary>
        /// 
        /// </summary>
        public ToJValue() { }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                if (outputAsJValue != null)
                {
                    try
                    {
                        outputAsJValue.Data = new JValue(value);
                        var a = JObject.FromObject(value as object);
                    }
                    catch (Exception e)
                    {
                        outputAsJValue.Data = new JValue("Data failed to be packaged as a JValue");
                    }
                }
            }
        }

    }
}