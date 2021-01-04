using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// XmlReaderWriter abstraction is supposed to read from and write to XML files, support reading a list of elements from a file or files in a directory.
    /// Output two types of data, one is lists of data, particularly, the first list would be the output names for those elements.
    /// The other output type is DataTable in ITableDataFlow, can be accessed via Get method as data source.
    /// Implemented IDataFlow<string> and ITableDataFlow to support different scenarios.
    ///
    /// Input ports:
    /// 1. IDataFlow<string>: directory path or file path
    /// 2. ITableDataFlow: can be used as data source for transact
    /// Output ports:
    /// 1. IDataFlow<List<string>> xmlReadInfo: First come up element inner text read from xml file according to the selected element.
    /// 2. IDataFlow<string> totalCount: records count for the XML reading task.
    ///
    /// Example to use the abstraction:
    /// XmlReaderWriter XR5000FavouriteSettingXmlReader = new XmlReaderWriter(
    /// new List<string>() {"Name", "Description", "LastModifiedDateTime"},
    /// new List<string>() {"Name", "Description", "Modified Time"},
    /// ".ttfav");
    /// 
    /// TODO-1: Now only support get the first come up element content from a xml file, later modification to support XML element path as: Header1/Header2/Header3/Element1, ... (index format not fixed yet)
    /// TODO-2: Implement methods to support write information into xml files
    /// </summary>
    public class XmlReaderWriter : IDataFlow<string>, ITableDataFlow
    {
        // public
        public string InstanceName = "Default";
        public bool OutputHeader = true;

        // ports
        private IDataFlow<List<string>> xmlReadInfo; // First output would be the OutputNames, then would be the information from XML file
        private IDataFlow<string> totalCount;

        // private fields
        private List<string> elementList;
        private List<string> outputNameList;
        private DataTable xmlInnerTextTable = new DataTable();
        private string xmlFileLocation;
        private string xmlFileExtension;
        private bool dataSent = false;

        // constructor
        /// <summary>
        /// XmlReaderWriter abstraction is supposed to read from and write to XML files, support reading a list of elements from a file or files in a directory.
        /// Output two types of data, one is lists of data, particularly, the first list would be the output names for those elements.
        /// The other output type is DataTable in ITableDataFlow, can be accessed via Get method as data source.
        /// </summary>
        /// <param name="elements"> Element names want to read from xml files. </param>
        /// <param name="outputNames"> Names to show in a data table. </param>
        /// <param name="extension"> Extension for the xml files, format is like ".xml" or ".ttfav". </param>
        public XmlReaderWriter(List<string> elements, List<string> outputNames, string extension = ".xml")
        {
            if (!outputNames.Any() || elements.Count != outputNames.Count) throw new ArgumentException("Length of elements and names does not match.");
            elementList = elements;
            outputNameList = outputNames;
            xmlFileExtension = extension;
        }


        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                xmlInnerTextTable.Rows.Clear();
                xmlInnerTextTable.Columns.Clear();

                xmlFileLocation = value;
                if(xmlReadInfo != null && OutputHeader) xmlReadInfo.Data = outputNameList;

                xmlInnerTextTable.Rows.Clear();
                xmlInnerTextTable.Columns.Clear();

                foreach (var colName in outputNameList)
                {
                    xmlInnerTextTable.Columns.Add(colName);
                }

                ReadXmlToDataTable(xmlFileLocation, xmlInnerTextTable, elementList);

                if (totalCount != null) totalCount.Data = xmlInnerTextTable.Rows.Count.ToString();

            }
        }

        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => xmlInnerTextTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            dataSent = false;

            // create a DataTable contains column names as output names
            xmlInnerTextTable.Rows.Clear();
            xmlInnerTextTable.Columns.Clear();

            foreach (var colName in outputNameList)
            {
                xmlInnerTextTable.Columns.Add(colName);
            }

            // output column names as first output list
            if (xmlReadInfo != null) xmlReadInfo.Data = outputNameList;
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            int startRow;
            int finalRow;

            if (!dataSent)
            {
                ReadXmlToDataTable(xmlFileLocation, xmlInnerTextTable, elementList);
                startRow = 0;
                finalRow = xmlInnerTextTable.Rows.Count;
            }
            else
            {
                finalRow = xmlInnerTextTable.Rows.Count;
                startRow = finalRow;
            }

            dataSent = true;
            return new Tuple<int, int>(startRow, finalRow);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) { throw new NotImplementedException(); }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        // private methods
        /// <summary>
        /// Read xml elements inner texts from a directory or a file. Check xmlFilePath parameter, read all xml files if it is a directory or a specific file if it is a XML file.
        /// </summary>
        /// <param name="xmlFilePath">The path to xml files, can be a directory or a file. </param>
        /// <param name="dataTable"> Data table used to contain information selected from xml files. </param>
        /// <param name="elements"> Element names want to read from xml files. </param>
        private void ReadXmlToDataTable(string xmlFilePath, DataTable dataTable, List<string> elements)
        {
            List<string> tmpReadList = new List<string>();

            if (IsDirectory(xmlFilePath)) // path is a directory
            {
                var sourceFolder = new DirectoryInfo(xmlFileLocation);
                foreach (var fileInfo in sourceFolder.GetFiles($"*{xmlFileExtension}"))
                {
                    tmpReadList.Clear();

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileInfo.FullName);

                    foreach (var element in elements)
                    {
                        var elementNodeList = xmlDoc.GetElementsByTagName(element);
                        var elementContent = elementNodeList.Count == 0 ? string.Empty : elementNodeList[0].InnerText;
                        tmpReadList.Add(elementContent);
                    }

                    if (xmlReadInfo != null) xmlReadInfo.Data = tmpReadList;

                    var tmpRow = dataTable.NewRow();
                    tmpRow.ItemArray = tmpReadList.ToArray();
                    dataTable.Rows.Add(tmpRow);
                }
            }
            else // path is a XML file
            {
                tmpReadList.Clear();
                var xmlFile = new FileInfo(xmlFilePath);
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile.FullName);

                foreach (var element in elements)
                {
                    var elementNodeList = xmlDoc.GetElementsByTagName(element);
                    tmpReadList.Add(elementNodeList.Count == 0 ? string.Empty : elementNodeList[0].InnerText);
                }

                if (xmlReadInfo != null) xmlReadInfo.Data = tmpReadList;

                var tmpRow = dataTable.NewRow();
                tmpRow.ItemArray = tmpReadList.ToArray();
                dataTable.Rows.Add(tmpRow);
            }

        }

        /// <summary>
        /// Check whether a path is a directory or a file.
        /// </summary>
        /// <param name="path"> Path need to be checked. </param>
        /// <returns> True if path is a directory, false if not. </returns>
        private bool IsDirectory(string path)
        {
            var attr = File.GetAttributes(path);
            return attr.HasFlag(FileAttributes.Directory);
        }
    }
}
