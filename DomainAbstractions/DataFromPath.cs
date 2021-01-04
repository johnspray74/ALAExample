using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Extracts all content in a file as a string.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; filePath: The path of the file to extract from.</para>
    /// <para>2. IDataFlowB&lt;string&gt; dataOutputB: Returns the data in the file as a string.</para>
    /// <para>3. IDataFlow&lt;string&gt; dataOutput: Sends out the data in the file as a string.</para>
    /// </summary>
    public class DataFromPath : IDataFlow<string>, IDataFlowB<string>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private string path = "";
        private string data = "";

        // Ports
        private IDataFlow<string> dataOutput;

        /// <summary>
        /// <para>Extracts all content in a file as a string.</para>
        /// </summary>
        /// <param name="path"></param>
        public DataFromPath(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
            {
                this.path = path;
                data = GetData(path);
                DataChanged?.Invoke();
            }
        }

        private string GetData(string path)
        {
            string fileData = "";

            try
            {
                fileData = File.ReadAllText(path);
            }
            catch
            {

            }


            return fileData;
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                path = value;
                data = GetData(path);
                if (dataOutput != null) dataOutput.Data = data;
                DataChanged?.Invoke();
            }
        }

        // IDataFlowB<string> implementation
        string IDataFlowB<string>.Data { get => data; }
        public event DataChangedDelegate DataChanged;
    }
}
