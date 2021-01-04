using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// SessionFileReaderWriter abstraction is used to read a csv file into a DataTable.
    /// Input ports:
    /// 1. IDataFlow<string>: file path to csv file.
    /// 2. ITableDataFlow: interface to pass data with other abstractions implemented ITableDataFlow interface.
    /// Output ports:
    /// 
    /// </summary>
    /// TODO: modify to replace CSVFileReaderWriter
    public class SessionFileReaderWriter : IDataFlow<string>, ITableDataFlow
    {
        // Instance name
        public string InstanceName = "Default";
        public string EncodingFormat = "iso-8859-1";
        public string SessionFileFormat = "3000Format";
        // 0: append text, 1: csv file, 2: no header, do nothing, 3: 3000 format, 4: minda format, 5: EID only
        public int FileType { set=> fileFormatIndex = value; }
        // ports
        private IDataFlow<bool> dataFlowOpenOrCloseProgressWindow;
        private IDataFlow<string> dataFlowFilePath;
        private IDataFlow<string> dataFlowRecordsTotalCount;

        // private fields
        private string filePath;
        private DataTable dataTable = new DataTable();
        private int fileFormatIndex = 1;
        private StringBuilder contentBuilder = new StringBuilder();
        private int sessionFileDataIndex;
        private int currentIndex = 0;

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => filePath = value; }

        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            sessionFileDataIndex = SessionFileFormat.Equals("3000Format") ? 5 : 1;

            if (!string.IsNullOrEmpty(filePath))
            {
                dataTable.Rows.Clear();
                dataTable.Columns.Clear();
                currentIndex = 0;

                try
                {
                    dataTable = GetDataTableFromCsv(filePath.ToLower());
                    if (dataFlowRecordsTotalCount != null)
                        dataFlowRecordsTotalCount.Data = (dataTable.Rows.Count - sessionFileDataIndex).ToString();
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {

            if ((dataTable.Rows.Count > sessionFileDataIndex) 
                && (currentIndex < dataTable.Rows.Count)) // actual data in session file
            {
                currentIndex = dataTable.Rows.Count;
                return new Tuple<int, int>(0, dataTable.Rows.Count);
            }

            return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);

        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            if (dataFlowOpenOrCloseProgressWindow != null)
                dataFlowOpenOrCloseProgressWindow.Data = true;

            contentBuilder.Clear();
            var headers = new List<string>();

            switch (fileFormatIndex)
            {
                case 0:
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        if (!c.Prefix.Equals("hide")) 
                            headers.Add(c.ToString());
                    }

                    contentBuilder.Append(string.Join(",", headers));
                    contentBuilder.Append(Environment.NewLine);
                    break;
                case 1:
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        if (!c.Prefix.Equals("hide"))
                            headers.Add(c.ToString());
                    }
                    contentBuilder.Append(string.Join(",", headers));
                    contentBuilder.Append(Environment.NewLine);
                    break;
                case 2:
                    break;
                case 3: 
                    // TODO: new implementation TBC
                    break;
                case 4:
                    // TODO: new implementation TBC
                    break;
                default:
                    break;
            }
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            switch (fileFormatIndex)
            {
                case 0:
                    break;
                case 1:
                    for (var i = firstRowIndex; i < lastRowIndex; i++)
                    {
                        foreach (DataColumn c in dataTable.Columns)
                        {
                            if (!c.Prefix.Equals("hide"))
                                contentBuilder.Append(dataTable.Rows[i][c.ColumnName] + ",");
                        }
                        contentBuilder.Remove(contentBuilder.Length - 1, 1);
                        contentBuilder.Append(Environment.NewLine);
                    }
                    break;
                case 2:
                    break;
                case 3:
                    for (var i = firstRowIndex; i < lastRowIndex; i++)
                    {
                        foreach (DataColumn c in dataTable.Columns)
                        {
                            if (!c.Prefix.Equals("hide"))
                                contentBuilder.Append(dataTable.Rows[i][c.ColumnName] + ",");
                        }
                        contentBuilder.Remove(contentBuilder.Length - 1, 1);
                        contentBuilder.Append(Environment.NewLine);
                    }
                    break;
                case 4:
                    break;
                case 5:
                    for (var i = firstRowIndex; i < lastRowIndex; i++)
                    {
                        DataRow r = dataTable.Rows[i];
                        contentBuilder.Append(r["EID"]);
                        contentBuilder.Append(Environment.NewLine);
                    }
                    break;
                default:
                    break;
            }

            string directoryPath = Path.GetDirectoryName(filePath);
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(filePath, contentBuilder.ToString());
            contentBuilder.Clear();

            if (dataFlowOpenOrCloseProgressWindow != null)
                dataFlowOpenOrCloseProgressWindow.Data = false;
            if (dataFlowFilePath != null)
                dataFlowFilePath.Data = filePath;
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() { return false; }

        // private methods
        /// <summary>
        /// Read a csv format session file as a DataTable
        /// </summary>
        /// <param name="fileName"> file path to the session file</param>
        /// <returns>return a DataTable for upload use</returns>
        private DataTable GetDataTableFromCsv(string fileName)
        {
            var table = new DataTable();
            using (
                var sr = new StreamReader(
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                    Encoding.GetEncoding(EncodingFormat)))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();

                    string[] values = ImportValuesFromLine(line);

                    DataRow row = table.NewRow();
                    for (int colNum = 0; colNum < values.Length; colNum++)
                    {
                        if (!table.Columns.Contains("Column" + colNum.ToString()))
                            table.Columns.Add("Column" + colNum.ToString(), typeof(string));
                        string value = values[colNum];
                        row[table.Columns[colNum]] = value;
                    }
                    table.Rows.Add(row);
                }
            }
            RemoveTopEmptyRows(table);
            if (table.Rows.Count == 0)
                throw new FileLoadException("Empty file: " + fileName);
            return table;
        }

        /// <summary>
        /// Read one line in the file and validate content
        /// </summary>
        /// <param name="line"> a line of text read from session file</param>
        /// <returns> string array contains all contents in the line</returns>
        private string[] ImportValuesFromLine(string line)
        {
            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator; //CSV field separator
            const string textDelimitingCharacter = "\""; //Text delimiting character
            const string commaReplaceString = "<ԊḞ₩>"; //replacement for the comma in line


            while (line.Contains(textDelimitingCharacter)) // contains the line a '"' ?
            {
                //getting the index of the next opening tag ',"' (or '"' if it's the beginning of line)
                int startIdx = line.IndexOf(separator + textDelimitingCharacter, StringComparison.Ordinal);
                if (line.StartsWith(textDelimitingCharacter))
                {
                    startIdx = 0;
                }
                // Once all delimiters are processed, if there are " as data inside a field break out
                if (startIdx < 0)
                {
                    break;
                }

                //getting the index of the next closing tag '",' (or '"' if it's the end of line)
                bool isEndOfLine = false;
                int endIdx = line.IndexOf(textDelimitingCharacter + separator, StringComparison.Ordinal);
                if (endIdx == -1 && line.EndsWith(textDelimitingCharacter))
                {
                    isEndOfLine = true;
                    endIdx = line.Length;
                }

                string tmpLine = string.Empty;

                //everything in front of the text field
                tmpLine += line.Substring(0, startIdx);


                string tmp = line.Substring(startIdx + 1, endIdx - startIdx - (isEndOfLine ? 1 : 0));
                //clean the field
                tmp = tmp.Replace(textDelimitingCharacter, "");
                //replace , with commaReplaceString
                tmp = tmp.Replace(separator, commaReplaceString);

                //add a , in front and behind the text
                if (startIdx != 0)
                {
                    tmpLine += separator;
                }
                tmpLine += tmp;
                if (!isEndOfLine)
                {
                    tmpLine += separator;
                }

                //everything behind the text field
                tmpLine += line.Substring(endIdx + (isEndOfLine ? 0 : 2), line.Length - endIdx - (isEndOfLine ? 0 : 2));
                line = tmpLine;
            }

            //import in string array
            string[] values = line.Split(new[] { separator }, StringSplitOptions.None).Select(a => a.Trim()).ToArray();

            //replace commaReplaceString with ,
            for (int i = 0; i <= values.GetUpperBound(0); i++)
            {
                if (values[i].Contains(commaReplaceString))
                {
                    values[i] = values[i].Replace(commaReplaceString, separator);
                }
            }
            return values;
        }

        /// <summary>
        /// Remove top empty rows in session DataTable
        /// </summary>
        /// <param name="dataTable"> session DataTable read from session file</param>
        private void RemoveTopEmptyRows(DataTable dataTable)
        {
            List<DataRow> emptyRows = new List<DataRow>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!IsEmptyRow(row))
                    break;
                emptyRows.Add(row);
            }

            foreach (DataRow row in emptyRows)
                dataTable.Rows.Remove(row);
        }

        /// <summary>
        /// Check whether a DataRow is empty or not
        /// </summary>
        /// <param name="dr">one DataRow in a DataTable</param>
        /// <returns>true when DataRow is empty, false if not</returns>
        private bool IsEmptyRow(DataRow dr)
        {
            if (dr == null)
                return true;
            else
                foreach (DataColumn col in dr.Table.Columns)
                    if (dr[col] != DBNull.Value && !String.IsNullOrEmpty(dr[col].ToString()))
                        return false;
            return true;
        }
    }
}
