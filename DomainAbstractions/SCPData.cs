using Foundation;
using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Know about SCP commands of the connected SCP device to retrieve session data and convert to an ITableDataflow
    /// SCP commands are like {FH} for header, {FN} for rows of data, device responds with like [982000000123456,123.5,2018-11-23]
    /// TBD: create caching abstraction so we don't have to do slow reloading data from the device e.g. when displying data and then getting the data a second time to import to a file 
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow inputOutputTableData: input/output port which can be source - getting session data (retrieving from device) or destination - putting session data to device
    /// 2. IRequestResponseDataFlow<string, string> requestResponseDataFlow: both way serial connection to the device using SCPProtocol commands (SCP is a request/response protocol)
    /// 3. IArbitrator arbitrator: arbitrator for allowing only one thing at a time to use the device
    /// </summary>
    public class SCPData : ITableDataFlow // inputOutputTableData
    {
        // Configurations
        // Instance name gnerally identifies the instance when debugging
        // This one is also used to identify who is using the arbitrator port
        public string InstanceName { set; get; } = "anonymous";

        // a batch is how many records we get at a time from the device, default is 26
        // smaller makes overhead of moving lots of data inefficient
        // too big makes higher latency to get more data
        public int batchSize { set; get; }  = 26;


        // ports
        // ITableDataFlow 
        private IRequestResponseDataFlow<string, string> SCPPort;
        private IArbitrator arbitrator;



        // private fields
        private long rowId = 1;
        private List<FieldHeader> headers = new List<FieldHeader>();
        private Dictionary<string, Dictionary<int, string>> customFields;


        /// <summary>
        /// Fetch session data by sending a series SCP commands to device. Such as {FH} for header, {FN} for row.
        /// </summary>
        public SCPData() { }
        
        // ITableDataFlow implementation --------------------------------------------------------------------------------------------------------
        private DataTable currentDataTable = new DataTable();


        DataTable ITableDataFlow.DataTable
        {
            get => currentDataTable;
        }
        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }


        // devices have dynamic columns
        // get column header names

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // devices can have so called custom fields which are like enums, their data is actually numbers, but these numbers represent a finite number of strings.
            // used e.g for fields that represents things like animal genders, breeeds, etc
            // These string sets are read from the device using the {SOCU} command
            // {SOCU} returns the total number of string sets
            // {SOCUx} returns the name of the field that the strings apply to, and the number of strings. e.g. Breed,3
            // {SOCUx,y} returns the yth string e.g Angus, Fiesian or Hereford
            if (customFields == null)
            {
                await arbitrator.Request(InstanceName);

                customFields = new Dictionary<string, Dictionary<int, string>>();
                int customFieldCount = int.Parse(await SCPPort.SendRequest("{SOCU}"));

                for (int i = 0; i < customFieldCount; i++)
                {
                    Dictionary<int, string> fields = new Dictionary<int, string>();
                    string[] response = (await SCPPort.SendRequest("{SOCU" + i + "}")).Split(',');
                    string fieldName = response[0];
                    int numberVal = int.Parse(response[1]);

                    for (int j = 1; j <= numberVal; j++)
                    {
                        string valueName = await SCPPort.SendRequest("{SOCU" + i + "," + j + "}");
                        fields.Add(j, valueName);
                    }

                    customFields.Add(fieldName, fields);
                }

                arbitrator.Release(InstanceName);
            }

            currentDataTable = new DataTable();

            // devices have field header names and field types which are fetched with {FU} command
            // {FH} returns nt<name> where n is a field number, t is a field type
            // we only want the name
            string header = null;
            List<string> headerList = new List<string>();


            await arbitrator.Request(InstanceName);
            header = await SCPPort.SendRequest("{FH}");
            while (!string.IsNullOrEmpty(header))
            {
                headerList.Add(header.Substring(3));
                header = await SCPPort.SendRequest("{FH}");
            }

            arbitrator.Release(InstanceName);

            foreach (var h in headerList)
            {
                if (!currentDataTable.Columns.Contains(h))
                {
                    currentDataTable.Columns.Add(h);
                }
            }

            // add a column in the table called index, which will just be sequenctial record number starting from 0

            if (!currentDataTable.Columns.Contains("index")) currentDataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
            currentDataTable.TableName = "";
        }



        // get a batch of records from teh device using the {FM} command
        // The {FGDD} command sets the device up for downloading session data rtaher than lifedata or other tables
        // The {FD} command starts from the first record
        // The {FRn} command selects record n
        // {FNn} gets n records in the form [1,data,data,data;2,data;data,data]

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            int rowStartIndex = currentDataTable.Rows.Count;

            await arbitrator.Request(InstanceName);
            await SCPPort.SendRequest("{FGDD}");

            // set the cursor to the last record
            await SCPPort.SendRequest("{FD}");  // (redundant?)
            await SCPPort.SendRequest("{FR" + rowStartIndex + "}"); 
            string content = await SCPPort.SendRequest("{FN" + batchSize + "}");
            arbitrator.Release(InstanceName);

            if (!string.IsNullOrEmpty(content))
            {
                string[] records = content.Split(';');
                foreach (var r in records)
                {
                    string[] datas = r.Split(',');
                    // add one record to datatable
                    if (datas.Length >= currentDataTable.Columns.Count)
                    {
                        DataRow row = currentDataTable.NewRow();
                        for (var i = 0; i < currentDataTable.Columns.Count - 1; i++)
                        {
                            DataColumn column = currentDataTable.Columns[i];
                            string field = datas[i+1];

                            if (customFields.ContainsKey(column.ColumnName))
                            {
                                // try get custom field from the dictionary
                                // if there is some sort of error, skip and just display the value
                                try
                                {
                                    field = customFields[column.ColumnName][int.Parse(field)];
                                }
                                catch
                                {
                                }
                            }

                            row[column.ColumnName] = field;
                        }
                        if (currentDataTable.Columns.Contains("index"))
                        {
                            row["index"] = rowId++;
                        }
                        currentDataTable.Rows.Add(row);
                    }
                }
            }

            return new Tuple<int, int>(rowStartIndex, currentDataTable.Rows.Count);
        }


        // uploading records to device ----------------------------------------------------------
        private int sessionId;
        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            // retrieve metadata from table header
            // should reallly be changed - we don't need to know the session index
            // as we will just add it to a new session anyway
            // also don't need the field headers from the 3000 format - we can get this from a regular
            // csv file headers and then call {FH} to get all headers - remove the first 3 characters and you get the
            // CSV file header
            string[] metaData = currentDataTable.TableName.Split(';');

            if (metaData.Length < 3)
            {
                throw new ArgumentException($"Table did not have enough metadata");
            }

            // you can thank CSVFileReaderWriter for this :)
            // to be changed when CSV file is refactored
            string sessionName = metaData[1].Replace("Name: ", "");
            string sessionDate = metaData[2].Replace("Date: ", "");

            await arbitrator.Request(InstanceName);
            await SCPPort.SendRequest("{FGDD}"); // select session data

            // find the end of the session list
            // add one to the end to create new session
            while (true)
            {
                string response = await SCPPort.SendRequest("{FL}");
                if (String.IsNullOrEmpty(response)) break;
                sessionId = Int32.Parse(response) + 1;
            }

            await SCPPort.SendRequest("{FF" + sessionId + "}"); // select session

            // retrieve headers
            // convert into field headers
            headers.Clear();
            string header;
            while (true)
            {
                header = await SCPPort.SendRequest("{FH}");
                if (String.IsNullOrEmpty(header)) break;

                try
                {
                    FieldHeader fieldHeader = FieldHeader.FromString(header);
                    if (currentDataTable.Columns.Contains(fieldHeader.Name))
                    {
                        headers.Add(fieldHeader);
                    }
                }
                // if we can't parse the header just ignore it
                catch (ArgumentException)
                {
                }
            }

            // set name and date of session
            await SCPPort.SendRequest("{FPNA" + sessionId + "," + sessionName + "}");
            await SCPPort.SendRequest("{FPDA" + sessionId + "," + sessionDate + "}");

            arbitrator.Release(InstanceName);
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (firstRowIndex >= lastRowIndex)
            {
                getNextPage?.Invoke();
                return;
            }

            await arbitrator.Request(InstanceName);

            await SCPPort.SendRequest("{FGDD}"); // select session data
            await SCPPort.SendRequest("{FF" + sessionId + "}"); // select session index

            // build headers to send
            // basically their IDs joined together with commas
            string uploadHeaders = String.Join(",", from FieldHeader header in headers select header.Id);
            await SCPPort.SendRequest("{FI" + uploadHeaders + "}"); // send upload headers

            for (int i = firstRowIndex; i < lastRowIndex; i++)
            {
                DataRow row = currentDataTable.Rows[i];
                
                // select data from row in same order as headers
                // join with commas
                string uploadData = String.Join(",", from FieldHeader header in headers select row[header.Name].ToString());
                await SCPPort.SendRequest("{FU" + uploadData + "}");
            }

            arbitrator.Release(InstanceName);
            getNextPage?.Invoke();
        }
    }




    /// <summary>
    /// Describes a field header from a device.
    /// All fields have a type (see FieldType), identifier (for device side) and a name (user friendly).
    /// </summary>
    class FieldHeader
    {
        public string Id { get => id; }
        public FieldType Type { get => type; }
        public string Name { get => name; }
        public bool IsLifeData { get => id.StartsWith("F"); }

        private string id;
        private FieldType type;
        private string name;

        /// <summary>
        /// Defines the type of field. 
        /// </summary>
        public enum FieldType
        {
            Numeric,
            Alphanumeric,
            Custom,
            Date,
            Time
        }



        private static readonly Dictionary<char, FieldType> fieldTypes = new Dictionary<char, FieldType>()
        {
            { 'N', FieldType.Numeric },
            { 'A', FieldType.Alphanumeric },
            { 'O', FieldType.Custom },
            { 'D', FieldType.Date },
            { 'T', FieldType.Time }
        };

        /// <summary>
        /// Describes a field header from a device.
        /// All fields have a type (see FieldType), identifier (for device side) and a name (user friendly).
        /// </summary>
        private FieldHeader(string id, FieldType type, string name)
        {
            this.id = id;
            this.type = type;
            this.name = name;
        }

        /// <summary>
        /// Converts a field stringt to a FieldHeader object.
        /// </summary>
        /// <param name="field">The field string to convert to an object.</param>
        /// <returns>A field header object.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the field string is invalid and could not be converted.</exception>
        public static FieldHeader FromString(string field)
        {
            string fieldId = field.Substring(0, 2);
            char fieldTypeC = field.ElementAt(2);
            string fieldName = field.Substring(3);

            try
            {
                FieldType fieldType = fieldTypes[fieldTypeC];

                return new FieldHeader(fieldId, fieldType, fieldName);
            }
            catch
            {
                throw new ArgumentException("Unable to parse the given field.");
            }
        }
    }

}
