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
    /// <para>[DEPRECATED: Can be replaced with JSONParser]</para>
    /// <para>Extracts a dictionary from a JSON file.</para>
    /// <para>------------------------------------------------------------------------------------------------------------------</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; "NEEDNAME": The path of the JSON file.</para>
    /// <para>2. IEvent "NEEDNAME: Starts the process.</para>
    /// <para>3. IDataFlow&lt;Dictionary&lt;string, string&gt;&gt; jsonDictOutput: The output dictionary.</para>
    /// </summary>
    public class GetJSONDict : IDataFlow<string>, IEvent
    {
        // Properties ---------------------------------------------------------------------
        public string InstanceName = "Default";
        public string FilePath;

        // Ports ---------------------------------------------------------------------
        private IDataFlow<Dictionary<string, string>> jsonDictOutput;
        private IEvent fileNotFound;

        /// <summary>
        /// <para>[DEPRECATED: Can be replaced with JSONParser]</para>
        /// <para>Extracts a dictionary from a JSON file.</para>
        /// </summary>
        public GetJSONDict() { }

        // private methods ---------------------------------------------------------
        private void GetDict(string filePath)
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);
                if (fileContent == "" && fileNotFound != null) fileNotFound.Execute();
                if (jsonDictOutput != null) jsonDictOutput.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent);
            }
            catch
            {
                Debug.WriteLine($"File not found at {filePath}.");
                if (fileNotFound != null) fileNotFound.Execute();
                if (jsonDictOutput != null) jsonDictOutput.Data = new Dictionary<string, string>();
            }
        }

        // IDataFlow<string> implementation ---------------------------------------------------------
        string IDataFlow<string>.Data { set => GetDict(value); }

        // IEvent implementation ---------------------------------------------------------
        void IEvent.Execute()
        {
            GetDict(FilePath);
        }
    }
}
