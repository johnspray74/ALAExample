using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// FileReader is supposed to read data from a file into a DataTable for further operation.
    /// Supported file format: csv, txt, xls, xlsx
    /// </summary>
    public class FileReader : IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";
        public string EncodingFormat = "iso-8859-1";

        // ports
        private List<IDataFlow<DataTable>> fileContent;
        private ITableDataFlow destinationDataFlow;
        private IDataFlow<string> fileName;
        private IDataFlow<string> fileCreationDate;
        private IDataFlow<string> message;

        // fields
        private string fileFullPath;
        private string fileNameNoExt;
        private DataTable dataTable = new DataTable();

        // ctor
        public FileReader() { }

        // implementation of IDataFlow<string>
        string IDataFlow<string>.Data
        {
            set
            {
                if (File.Exists(value)) fileFullPath = value;

                fileNameNoExt = Path.GetFileNameWithoutExtension(fileFullPath);
                if (fileName != null) fileName.Data = fileNameNoExt;
                if (fileCreationDate != null) fileCreationDate.Data = new FileInfo(fileFullPath).CreationTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                var fileExtension = Path.GetExtension(fileFullPath)?.ToUpper();

                switch (fileExtension)
                {
                    case ".CSV":
                        dataTable = ReadFromCsv(fileFullPath);
                        break;

                    case ".XLS":
                        dataTable = ReadFromXls(fileFullPath);
                        break;

                    case ".XLSX":
                        dataTable = ReadFromXlsx(fileFullPath);
                        break;

                    case ".TXT":
                        dataTable = ReadFromTxt(fileFullPath);
                        break;

                    default:
                        if (message != null)
                        {
                            message.Data = fileFullPath;
                            message.Data = "Not supported file format.";
                        }
                        Debug.WriteLine("Not supported file format when reading " + fileFullPath);
                        break;
                }

                // get empty information about the table, this could be wire to Collection to generate messages
                if (dataTable.Rows.Count == 0)
                {
                    if (message != null)
                    {
                        message.Data = fileFullPath;
                        message.Data = "CSV file is empty.";
                    }
                    Debug.WriteLine("Warning: Empty file when reading from csv in FileParser: " + fileFullPath);
                }

                foreach (var dataFlow in fileContent)
                {
                    dataFlow.Data = dataTable;
                }
            }
        }

        // methods
        // Read from csv
        private DataTable ReadFromCsv(string csvPath)
        {
            // initialise table with file name
            var table = new DataTable {TableName = fileNameNoExt};

            // create file stream reader
            using (
                var sr = new StreamReader(
                    new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                    Encoding.GetEncoding(EncodingFormat)))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();

                    // import a line in the file with constraints
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

            return table;
        }

        // Read from text
        private DataTable ReadFromTxt(string txtPath)
        {
            return ReadFromCsv(txtPath);
        }

        // read from xls
        private DataTable ReadFromXls(string xlsPath)
        {
            // initialise table with file name
            var table = new DataTable { TableName = fileNameNoExt };

            // using ExcelDataReader library
            using (var stream = File.Open(xlsPath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var columnCount = reader.FieldCount;

                    while (reader.Read())
                    {
                        DataRow row = table.NewRow();

                        for (int colNum = 0; colNum < columnCount; colNum++)
                        {
                            if (!table.Columns.Contains("Column" + colNum))
                                table.Columns.Add("Column" + colNum, typeof(string));

                            string value = reader.GetValue(colNum)?.ToString().Trim();

                            if (value != null) row[table.Columns[colNum]] = value;
                        }

                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }

        // read from xlsx
        private DataTable ReadFromXlsx(string xlsxPath)
        {
            return ReadFromXls(xlsxPath);
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
    }

}
