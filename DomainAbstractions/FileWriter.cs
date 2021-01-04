using ProgrammingParadigms;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// Abstraction to write to a file, provided with file location, name and data from wiring.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: incoming event to execute the file writer
    /// 2. IDataFlowB<string> fileLocation: input string of the file location
    /// 3. IDataFlowB<string> fileName: input string of the file name
    /// 4. IDataFlowB<string> fileData: input string of the data wanting to be written to the file
    /// </summary>
    public class FileWriter : IEvent //start
    {
        // Properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // Ports ---------------------------------------------------------------
        private IDataFlowB<string> fileLocation;
        private IDataFlowB<string> fileName;
        private IDataFlowB<string> fileData;

        /// <summary>
        /// 
        /// </summary>
        public FileWriter() { }

        // IEvent implementation ---------------------------------------------------------
        void IEvent.Execute()
        {
            string location = fileLocation.Data;
            if (!location.EndsWith(@"\")) location += @"\";
            File.WriteAllText(location + fileName.Data, fileData.Data.ToString());
        }
    }
}
