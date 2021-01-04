using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace DomainAbstractions
{
    public class ConvertHttpResponseToDictionary : IDataFlow<HttpResponseMessage>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private Dictionary<string, string> dict;

        // Ports
        private IDataFlow<Dictionary<string, string>> dictOutput;

        public ConvertHttpResponseToDictionary() { }

        public async Task ParseAsync(HttpResponseMessage response)
        {
            dict = new Dictionary<string, string>();

            string responseString = await response.Content.ReadAsStringAsync();
            dynamic uploadResult = JsonConvert.DeserializeObject(responseString);
            var error = uploadResult.error;
            Debug.WriteLine(error.innererror);
        }

        // IDataFlow<HttpResponseMessage> implementation
        HttpResponseMessage IDataFlow<HttpResponseMessage>.Data
        {
            set
            {
                var fireAndForget = ParseAsync(value);
                if (dictOutput != null) dictOutput.Data = dict;
            }
        }
    }
}
