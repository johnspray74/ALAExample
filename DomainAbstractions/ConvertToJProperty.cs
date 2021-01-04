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
    /// Populates a JProperty with two elements, with the first element being a string and the second element being a JToken containing any data type.
    /// These two elements can be converted directly from any Tuple<string, T>, or pushed individually from an IDataFlow<string> and an IDataFlow<JToken>.
    /// Ports:
    /// 1. IDataFlow<Tuple<string, T>> tupleInput
    /// 2. IDataFlow<string> propertyKey
    /// 3. IDataFlow<JToken> propertyValue
    /// 4. IDataFlow<JProperty> jPropertyOutput
    /// </summary>
    public class ConvertToJProperty<T> : IDataFlow<Tuple<string, T>>, IDataFlow<string>, IDataFlow<JToken>   
    {
        // Properties
        public string InstanceName = "Default";
        public string Key
        {
            get => key;
            set => key = value;
        }

        // Private fields
        private string key;
        private JToken jToken;

        // Ports
        private IDataFlow<JProperty> jPropertyOutput;

        /// <summary>
        /// Populates a JProperty with two elements, with the first element being a string and the second element being a JToken containing any data type.
        /// These two elements can be pushed individually from an IDataFlow<string> and an IDataFlow<JToken>, or converted directly from any Tuple<string, T>.
        /// </summary>
        public ConvertToJProperty() { }

        public void CreateAndPushJProperty(string s, JToken jt)
        {
            jPropertyOutput.Data = new JProperty(s, jt);
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                key = value;
                if (key != null && jToken != null) CreateAndPushJProperty(key, jToken);
            }
        }

        // IDataFlow<JToken> implementation
        JToken IDataFlow<JToken>.Data
        {
            set
            {
                jToken = value;
                if (key != null && jToken != null) CreateAndPushJProperty(key, jToken);
            }
        }

        // IDataFlow<Tuple<string, T>> implementation
        Tuple<string, T> IDataFlow<Tuple<string, T>>.Data
        {
            set
            {
                key = value.Item1;
                jToken = new JValue(value.Item2); // JValue is implicitly cast to a JToken
                if (key != null && jToken != null) CreateAndPushJProperty(key, jToken);
            }
        }

    }
}