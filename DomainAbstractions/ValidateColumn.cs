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
    /// [DEPRECATED]
    /// Reads a csv containing a single column and outputs a list of the non-empty entries, along with two lists of indices: one for non-empty entries, and one for empty entries
    /// </summary>
    public class ValidateColumn : IDataFlow<string>
    {
        // Properties
        public string InstanceName = "Default";

        // Outputs
        private IDataFlow<List<string>> validEntriesOutput;
        private IDataFlow<List<int>> validEntriesIndicesOutput;
        private IDataFlow<List<int>> invalidEntriesIndicesOutput;

        public ValidateColumn() { }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                string[] csvContent = File.ReadAllLines(value);
                var validEntries = new List<string>();
                var validEntriesIndices = new List<int>();
                var invalidEntriesIndices = new List<int>();

                for (int i = 1; i < csvContent.Length; i++) // Start at i = 1 to ignore the header row
                {
                    if (csvContent[i].Length < 15)
                    {
                        invalidEntriesIndices.Add(i);
                    }
                    else
                    {
                        validEntries.Add(csvContent[i]);
                        validEntriesIndices.Add(i);
                    }
                }
                if (validEntriesOutput != null) validEntriesOutput.Data = validEntries;
                if (validEntriesIndicesOutput != null) validEntriesIndicesOutput.Data = validEntriesIndices;
                if (invalidEntriesIndicesOutput != null) invalidEntriesIndicesOutput.Data = invalidEntriesIndices;
            }
        }
    }
}
