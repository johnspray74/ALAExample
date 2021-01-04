using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>A controller that pulls from multiple sources to produce a JObject. Each input that is not of type JProperty will be converted into a JProperty before being added to the JObject.</para>
    /// <para>Ports:</para>
    /// <para>1. IEvent start: begin the process.</para>
    /// <para>2. IDataFlowB&lt;JObject&gt; baseJObject: a base JObject to add new properties to.</para>
    /// <para>3. List&lt;IDataFlowB&lt;Tuple &lt;string, string&gt;&gt;&gt; stringPairs: a list of string pairs to pull from.</para>
    /// <para>4. List&lt;IDataFlowB&lt;Tuple &lt;string, object&gt;&gt;&gt; objectPairs: a list of object pairs to pull from.</para>
    /// <para>5. List&lt;IDataFlowB&lt;Tuple &lt;string, JToken&gt;&gt;&gt; JTokenPairs: a list of JToken pairs to pull from.</para>
    /// <para>6. List&lt;IDataFlowB&lt;JProperty&gt;&gt; jProperties: a list of pre-formatted JProperties that can be pulled from and added directly to the object.</para>
    /// </summary>

    public class CreateJObject : IEvent
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private JObject jObject = new JObject();

        // Ports
        private IDataFlowB<JObject> baseJObject;
        private List<IDataFlowB<Tuple<string, string>>> stringPairs;
        private List<IDataFlowB<Tuple<string, object>>> objectPairs;
        private List<IDataFlowB<Tuple<string, JToken>>> jTokenPairs;
        private List<IDataFlowB<JProperty>> jProperties;
        private IDataFlow<JObject> outputAsJObject;
        private IDataFlow<string> outputAsJSON;

        /// <summary>
        /// <para>A controller that pulls from multiple sources to produce a JObject. Each input that is not of type JProperty will be converted into a JProperty before being added to the JObject.</para>
        /// </summary>
        public CreateJObject() { }

        private void Create()
        {
            jObject = new JObject();

            if (baseJObject != null) jObject = baseJObject.Data;
            if (stringPairs != null)
            {
                foreach (IDataFlowB<Tuple<string, string>> b in stringPairs)
                {
                    var stringPair = b.Data;
                    if (stringPair.Item1 != null && !jObject.ContainsKey(stringPair.Item1) && stringPair.Item2 != null) jObject.Add(stringPair.Item1, stringPair.Item2);
                }
            }

            if (objectPairs != null)
            {
                foreach (IDataFlowB<Tuple<string, object>> b in objectPairs)
                {
                    var objectPair = b.Data;
                    if (objectPair.Item1 != null && !jObject.ContainsKey(objectPair.Item1) && objectPair.Item2 != null) jObject.Add(objectPair.Item1, JObject.FromObject(objectPair.Item2));
                }
            }

            if (jTokenPairs != null)
            {
                foreach (IDataFlowB<Tuple<string, JToken>> b in jTokenPairs)
                {
                    var jTokenPair = b.Data;
                    if (jTokenPair.Item1 != null && !jObject.ContainsKey(jTokenPair.Item1) && jTokenPair.Item2 != null) jObject.Add(jTokenPair.Item1, jTokenPair.Item2);
                }
            }

            if (jProperties != null)
            {
                foreach (IDataFlowB<JProperty> b in jProperties)
                {
                    var jProperty = b.Data;
                    if (jProperty.Name != null && !jObject.ContainsKey(jProperty.Name) && jProperty.Value != null) jObject.Add(jProperty);
                }
            }
        }

        private void Push()
        {
            string json = JsonConvert.SerializeObject(jObject);
            if (outputAsJObject != null) outputAsJObject.Data = jObject;
            if (outputAsJSON != null) outputAsJSON.Data = JsonConvert.SerializeObject(jObject);
        }

        private void Test()
        {
            Tuple<int, double, string, List<string>, object> testInstances = new Tuple<int, double, string, List<string>, object>(10, 10.001, "test", new List<string>() {"a", "b"}, new {val = 1, str = "two"});
            var testObj = JObject.FromObject(testInstances as object);
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // Test();
            Create();
            Push();
        }

    }
}