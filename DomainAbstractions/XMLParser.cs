using ProgrammingParadigms;
using System.IO;
using System.Xml.Serialization;

namespace DomainAbstractions
{
    /// <summary>
    /// Takes a string of XML data and parses it into struct(s).
    /// --------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<string> "NEEDNAME": The XML string to parse.
    /// 2. IDataFlowB<T> "NEEDNAME": The unserialised XML object.
    /// 3. IDataFlow<T> parsedObject: The unserialised XML object.
    /// </summary>
    /// <typeparam name="T">The base class to unserialise the XML file into.</typeparam>
    class XMLParser<T> : IDataFlow<string>, IDataFlowB<T>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<T> parsedObject;

        // private
        private string xmlData = default;
        private T output = default;
        private readonly XmlSerializer serializer;

        /// <summary>
        /// Takes a string of XML data and parses it into struct(s).
        /// </summary>
        public XMLParser()
        {
            serializer = new XmlSerializer(typeof(T));
        }

        // IDataFlow implementation
        string IDataFlow<string>.Data
        {
            set
            {
                xmlData = value;
                ParseData();
            }
        }

        // IDataFlowB implementation
        T IDataFlowB<T>.Data
        {
            get => output;
        }

        public event DataChangedDelegate DataChanged;

        /// <summary>
        /// Deserialize the given string of XML data.
        /// </summary>
        private void ParseData()
        {
            using (StringReader reader = new StringReader(xmlData))
            {
                output = (T)serializer.Deserialize(reader);
            }

            // no output port
            if (parsedObject != null)
            {
                parsedObject.Data = output;
            }

            DataChanged?.Invoke();
        }
    }
}
