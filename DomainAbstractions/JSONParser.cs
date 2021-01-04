﻿using System;
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
    /*
    /// Wiring example:
    /// 
    ///     AppStartEventConnector
    ///            .WireTo(new Data<string>()
    ///            {
    ///                storedData = "{" +
    ///                             "\"message\":\"The request is invalid.\"," +
    ///                             "\"modelState\":" +
    ///                                "{" +
    ///                                "\"transferModel.ClientKey\":[\"The ClientKey field is required.\"]," +
    ///                                "\"transferModel.Username\":[\"The Username field is required.\"]," +
    ///                                "\"transferModel.Password\":[\"The Password field is required.\"]" +
    ///                                "}" +
    ///                             "}"
    ///            }
    ///                .WireTo(new JSONParser()
    ///                {
    ///                    Configuration = new Dictionary<string, string>() {{ "messages" , "$..modelState.*[0]" }}
    ///                }
    ///                    .WireTo(new KeyValue<JToken>(key: "messages")
    ///                        .WireTo(new ToString<JToken>() // The JToken is a JArray, which implements IEnumerable
    ///                        // The result string is "The ClientKey field is required.\r\nThe Username field is required.\r\nThe Password field is required.\r\n"
    ///                            .WireTo(new DataFlowConnector<string>()) 
    ///                        )
    ///                    )
    ///                )
    ///            );
    */
    /// <summary>
    /// <para>Parses a JSON string. The user specifies a configuration as a dictionary where the keys are the desired field names and the values are JSONPaths.
    /// If the JSONPath property is set, then that is used instead to output a JToken.</para>
    /// <para>For more information on JSONPaths, see: https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.html and https://goessner.net/articles/JsonPath/index.html </para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; jsonInput : The JSON string to parse.</para>
    /// <para>2. IDataFlow&lt;Dictionary&lt;string, JToken&gt;&gt; parsedOutput : The parsed output as a dictionary where the values are JTokens.</para>
    /// <para>3. IDataFlow&lt;JToken&gt; jTokenOutput : A single JToken found at the given JSONPath.</para>
    /// </summary>
    public class JSONParser : IDataFlow<string>
    {
        // Properties
        public string InstanceName = "Default";
        public Dictionary<string, string> Configuration;
        public string JSONPath = "";

        // Private fields
        private string json;
        private Dictionary<string, JToken> parsedDictionary = new Dictionary<string, JToken>();
        private JToken currentJToken = default;

        // Ports
        private IDataFlow<Dictionary<string, JToken>> parsedOutput;
        private IDataFlow<JToken> jTokenOutput;

        /// <summary>
        /// <para>Parses a JSON string. The user specifies a configuration as a dictionary where the keys are the desired fieldnames and the values are JSONPaths.
        /// If the JSONPath property is set, then that is used instead.</para>
        /// <para>For more information on JSONPaths, see: https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.html and https://goessner.net/articles/JsonPath/index.html </para>
        /// </summary>
        public JSONParser() { }

        private void Test()
        {
            // string testJson = "{\"message\":\"The request is invalid.\",\"modelState\":{\"transferModel.ClientKey\":[\"The ClientKey field is required.\"],\"transferModel.Username\":[\"The Username field is required.\"],\"transferModel.Password\":[\"The Password field is required.\"]}}";
            JObject jObject = JObject.Parse(json);
            var result1 = jObject.SelectToken("message");
            var result2 = jObject.SelectToken("modelState");
            var result3 = jObject.SelectTokens("modelState['transferModel.ClientKey'][0]");
            var result4 = jObject.SelectTokens("$..modelState.*[0]");
            var arr = JArray.FromObject(result4);
            var s = arr.ToString();
        }

        private void Process()
        {
            if (Configuration != null)
            {
                foreach (var key in Configuration.Keys)
                {
                    var jToken = Extract(json, Configuration[key]);
                    parsedDictionary[key] = jToken;
                } 
            }
            else
            {
                Logging.Log($"Configuration is null for JSONParser. Extracting {JSONPath} from: {json}");
                currentJToken = Extract(json, JSONPath);
            }
            
        }

        private JToken Extract(string json, string path)
        {
            try
            {
                JArray selectedTokens = JArray.FromObject(JObject.Parse(json).SelectTokens(path));
                if (selectedTokens.Count == 0) return new JValue("") as JToken;
                var jToken = selectedTokens.Count == 1 ? selectedTokens.FirstOrDefault() as JToken : selectedTokens as JToken;
                return jToken;
            }
            catch (Exception e)
            {
                // var jToken = new JArray(new string[] {$"Error: The JToken for {key} could not be located."}) as JToken;
                return default;
            }
        }

        private void Push()
        {
            if (parsedOutput != null) parsedOutput.Data = parsedDictionary;
            if (jTokenOutput != null) jTokenOutput.Data = currentJToken;
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                json = value;
                // Test();
                Process();
                Push();
            }
        }
    }
}