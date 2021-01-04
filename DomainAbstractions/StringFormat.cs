using ProgrammingParadigms;
using System.Collections.Generic;

namespace DomainAbstractions
{
    /// <summary>
    /// Similiar to other formatting string functions found in other languages.
    /// Takes a string property which contains C# style data insertion points
    /// e.g. "Data={0}, Moredata={1}"
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. List<IDataFlowB<string>> dataFlowBsList: fan in for strings to be inserted at the insertion points according to their index numbers. ORDERING OF CONNECTION IS IMPORTANT.
    /// 2. IDataFlow<string> dataFlowOutput: formatted string output (outputs every time the input data has been changed)
    /// </summary>
    public class StringFormat
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private List<IDataFlowB<string>> dataFlowBsList = new List<IDataFlowB<string>>();
        private IDataFlow<string> dataFlowOutput;

        // private fields
        private string format;

        /// <summary>
        /// Formats a string based on a literal string and some input parameters.
        /// </summary>
        /// <param name="literal">the literal string</param>
        public StringFormat(string literal)
        {
            format = literal;
        }

        private void PostWiringInitialize()
        {
            foreach (var f in dataFlowBsList)
            {
                f.DataChanged += DataChanged;
            }
        }

        private void DataChanged()
        {
            object[] paras = new object[dataFlowBsList.Count];
            for (var i = 0; i < paras.Length; i++)
            {
                paras[i] = dataFlowBsList[i].Data;
            }
            dataFlowOutput.Data = string.Format(format, paras);
        }        
    }
}
