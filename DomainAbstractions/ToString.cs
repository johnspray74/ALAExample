using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Newtonsoft.Json.Linq;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Converts an object of type T into a string.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; input: the input variable. All types are expected to be supported.</para>
    /// <para>2. IDataFlow&lt;string&gt; stringOutput: the input variable as a string.</para>
    /// <para></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ToString<T> : IDataFlow<T>
    {
        // Properties
        public string InstanceName = "Default";

        // Ports
        private IDataFlow<string> stringOutput;

        /// <summary>
        /// Converts an object of type T into a string.
        /// </summary>
        public ToString() { }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                if (stringOutput != null)
                {
                    string stringBuffer = "";

                    if (value is JToken)
                    {
                        var jToken = value as JToken;
                        if (jToken.Type == JTokenType.Property)
                        {
                            var jProperty = jToken as JProperty;
                            stringBuffer = $"{jProperty.Name} : {jProperty.Value}";
                        }
                        else if (jToken.Type == JTokenType.Array)
                        {
                            var sb = new StringBuilder();
                            var jArray = jToken as JArray;
                            foreach (var token in jArray)
                            {
                                sb.AppendLine(token.ToString());
                            }

                            stringBuffer = sb.ToString();
                        }
                        else
                        {
                            stringBuffer = jToken.ToString();
                        }
                    }
                    else if (value is IEnumerable)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var element in (IEnumerable)value)
                        {
                            sb.AppendLine(element.ToString());
                        }
                        stringBuffer = sb.ToString();
                    }
                    else
                    {
                        stringBuffer = value.ToString();
                    }

                    stringOutput.Data = stringBuffer; 
                }
            }
        }
    }
}
