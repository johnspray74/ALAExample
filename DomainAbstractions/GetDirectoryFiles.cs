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
    /// Outputs a list of all filenames (that can be filtered) in a given directory.
    /// Implemented:
    /// IDataFlow&lt;string&gt; "directoryPath": Full directory path
    /// 
    /// Field:
    /// IDataFlow&lt;List&lt;string&gt;&gt; "filenamesOutput": List of all (filtered) filenames in the given directory
    /// </summary>
    public class GetDirectoryFiles : IDataFlow<string>
    {
        // Properties
        public string InstanceName = "Default";
        public List<string> Filters = new List<string>();

        // Private fields
        private List<string> filteredFilenames;

        // Outputs
        private IDataFlow<List<string>> filenamesOutput;

        public GetDirectoryFiles() { }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                if (filenamesOutput == null) return;

                string[] filenames;
                if (Filters.Count == 0)
                {
                    filenamesOutput.Data = Directory.GetFiles(value).ToList<string>();
                }
                else
                {
                    foreach (var filter in Filters)
                    {
                        filenames = Directory.GetFiles(value, filter);
                        foreach (string filename in filenames)
                        {
                            filteredFilenames.Add(filename);
                        }
                    }
                    filenamesOutput.Data = filteredFilenames;
                }
            }
        }
    }
}
