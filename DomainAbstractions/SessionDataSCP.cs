using Libraries;
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
    /// Sends SCP commands to the connected SCP device to retrieve session data and store as a data table.
    /// SCP commands are like {FH} for header, {FN} for row, device responds with like [982000000123456,123.5,2018-11-23]
    /// There is a Dictionary to store cached session data tables.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent clear: Clears the session data cache.
    /// 2. IDataFlow<string> currentSessionIndex: sets the current session index to cache the session table data (not needing to reload it)
    /// 3. ITableDataFlow inputOutputTableData: input/output port which can be source - getting session data (retrieving from device) or destination - putting session data (uploading to device)
    /// 4. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol
    /// 5. IArbitrator arbitrator: arbitrator for ordering SCP commands
    /// </summary>
    public class SessionDataSCP : IEvent, IDataFlow<string>, ITableDataFlow // clear, currentSessionIndex, inputOutputTableData
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private IDataFlow<DateTime> sessionCreationDate;

        // cache, the loaded session data will be stored in cache so we don't need to load again when we select the session
        private Dictionary<string, DataTable> cachedSessionData = new Dictionary<string, DataTable>();
        private Dictionary<string, int> sessionRecordsCount = new Dictionary<string, int>();
        private Dictionary<string, DateTime> sessionCreationDates = new Dictionary<string, DateTime>();

        // private fields
        // how many records are there in a page. default is 26
        private int pageSize = 26;
        private long rowId = 1;
        private List<FieldHeader> headers = new List<FieldHeader>();
        private Dictionary<string, Dictionary<int, string>> customFields;

        // this flag is used when a session file is selected and there exsits cached data, then the cached data will be returned
        // when GetPageFromSource is called. However, there still exists another situation that the user wants to load next page
        // when calling GetPageFromSource, then the flag should be false and it will return the data of the next page.
        private bool cacheFlag = false;

        /// <summary>
        /// Fetch session data by sending a series SCP commands to device. Such as {FH} for header, {FN} for row.
        /// </summary>
        public SessionDataSCP() { }
        
        // IEvent implementation
        void IEvent.Execute()
        {
            headers.Clear();
            sessionRecordsCount.Clear();
            cachedSessionData.Clear();
            currentDataTable = new DataTable();
        }

        // IDataFlow<DataRow> implementation ----------------------------------------------------------------------
        // store current session primary key
        private string currentSessionPrimaryKey;
        string IDataFlow<string>.Data
        {
            set
            {
                Debug.WriteLine("set session file id: " + value + "--------------------------------");
                currentSessionPrimaryKey = value;

                if (!cachedSessionData.ContainsKey(value))
                {
                    currentDataTable = new DataTable();
                    cachedSessionData[value] = currentDataTable;
                }
            }
        }

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

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            string primaryKey = currentSessionPrimaryKey;
            // select the corresponding session file in the connected device
            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGDD}");
            await requestResponseDataFlow.SendRequest("{FF" + primaryKey + "}");
            arbitrator.Release(InstanceName);

            if (customFields == null)
            {
                await arbitrator.Request(InstanceName);

                customFields = new Dictionary<string, Dictionary<int, string>>();
                int customFieldCount = int.Parse(await requestResponseDataFlow.SendRequest("{SOCU}"));

                for (int i = 0; i < customFieldCount; i++)
                {
                    Dictionary<int, string> field = new Dictionary<int, string>();
                    string[] response = (await requestResponseDataFlow.SendRequest("{SOCU" + i + "}")).Split(',');
                    string fieldName = response[0];
                    int numberVal = int.Parse(response[1]);

                    for (int j = 1; j <= numberVal; j++)
                    {
                        string valueName = await requestResponseDataFlow.SendRequest("{SOCU" + i + "," + j + "}");
                        field.Add(j, valueName);
                    }

                    customFields.Add(fieldName, field);
                }

                arbitrator.Release(InstanceName);
            }

            if (cachedSessionData.ContainsKey(primaryKey)
                && cachedSessionData[primaryKey].Rows.Count > 0
                && cachedSessionData[primaryKey].Columns.Count > 0)
            {
                // cache exsits
                cacheFlag = true;
                currentDataTable = cachedSessionData[primaryKey];
            }
            else
            {
                // cache DataTable for current session
                currentDataTable = new DataTable();
                cachedSessionData[primaryKey] = currentDataTable;

                // start to fetch header
                string header = null;
                List<string> headerList = new List<string>();

                await arbitrator.Request(InstanceName);
                header = await requestResponseDataFlow.SendRequest("{FH}");
                while (!string.IsNullOrEmpty(header))
                {
                    headerList.Add(header.Substring(3));
                    header = await requestResponseDataFlow.SendRequest("{FH}");
                }
                string name = await requestResponseDataFlow.SendRequest("{FPNA" + primaryKey + "}");
                string date = await requestResponseDataFlow.SendRequest("{FPDA" + primaryKey + "}");
                string count = await requestResponseDataFlow.SendRequest("{FPNR" + primaryKey + "}");

                arbitrator.Release(InstanceName);

                foreach (var h in headerList)
                {
                    if (!currentDataTable.Columns.Contains(h))
                    {
                        currentDataTable.Columns.Add(h);
                    }
                }

                if (!currentDataTable.Columns.Contains("index")) currentDataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });

                sessionRecordsCount[primaryKey] = Convert.ToInt32(count);
                currentDataTable.TableName = string.Format("FileNo: {0};Name: {1};Date: {2}", primaryKey, name, date);
                if (DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime dateTime))
                {
                    sessionCreationDates[primaryKey] = dateTime;
                }
            }

            sessionCreationDates.TryGetValue(primaryKey, out var sessionDate);
            if (sessionCreationDate != null) sessionCreationDate.Data = sessionDate;
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            // cache exist and the current table is going to be displayed
            if (cacheFlag)
            {
                await arbitrator.Request(InstanceName);
                await requestResponseDataFlow.SendRequest("{FGDD}");
                await requestResponseDataFlow.SendRequest("{FF" + currentSessionPrimaryKey  + "}");
                arbitrator.Release(InstanceName);

                cacheFlag = false;
                return new Tuple<int, int>(0, currentDataTable.Rows.Count);
            }

            if (sessionRecordsCount.ContainsKey(currentSessionPrimaryKey) 
                && currentDataTable.Rows.Count >= sessionRecordsCount[currentSessionPrimaryKey])
            {
                // all rows has been loaded, so just return the cached data
                return new Tuple<int, int>(currentDataTable.Rows.Count, currentDataTable.Rows.Count);
            }

            int rowStartIndex = currentDataTable.Rows.Count;

            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGDD}");

            // set the cursor to the last record
            await requestResponseDataFlow.SendRequest("{FD}");  // {FD} first to start downloading from the first record
            await requestResponseDataFlow.SendRequest("{FR" + rowStartIndex + "}"); // {FRn} resets back to this record number inside the selected session or life data
            string content = await requestResponseDataFlow.SendRequest("{FN" + pageSize + "}");
            arbitrator.Release(InstanceName);

            if (!string.IsNullOrEmpty(content))
            {
                string[] records = content.Split(';');
                foreach (var r in records)
                {
                    string[] fields = r.Split(',');
                    // add one record to datatable
                    if (fields.Length >= currentDataTable.Columns.Count)
                    {
                        DataRow row = currentDataTable.NewRow();
                        for (var i = 0; i < currentDataTable.Columns.Count - 1; i++)
                        {
                            DataColumn column = currentDataTable.Columns[i];
                            string field = fields[i+1];

                            if (customFields.ContainsKey(column.ColumnName))
                            {
                                // try get custom field from the dictionary
                                // if there is some sort of error, skip and just
                                // display the value
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
            await requestResponseDataFlow.SendRequest("{FGDD}"); // select session data

            // find the end of the session list
            // add one to the end to create new session
            while (true)
            {
                string response = await requestResponseDataFlow.SendRequest("{FL}");
                if (String.IsNullOrEmpty(response)) break;
                sessionId = Int32.Parse(response) + 1;
            }

            await requestResponseDataFlow.SendRequest("{FF" + sessionId + "}"); // select session

            // retrieve headers
            // convert into field headers
            headers.Clear();
            string header;
            while (true)
            {
                header = await requestResponseDataFlow.SendRequest("{FH}");
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
            await requestResponseDataFlow.SendRequest("{FPNA" + sessionId + "," + sessionName + "}");
            await requestResponseDataFlow.SendRequest("{FPDA" + sessionId + "," + sessionDate + "}");

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

            await requestResponseDataFlow.SendRequest("{FGDD}"); // select session data
            await requestResponseDataFlow.SendRequest("{FF" + sessionId + "}"); // select session index

            // build headers to send
            // basically their IDs joined together with commas
            string uploadHeaders = String.Join(",", from FieldHeader header in headers select header.Id);
            await requestResponseDataFlow.SendRequest("{FI" + uploadHeaders + "}"); // send upload headers

            for (int i = firstRowIndex; i < lastRowIndex; i++)
            {
                DataRow row = currentDataTable.Rows[i];
                
                // select data from row in same order as headers
                // join with commas
                string uploadData = String.Join(",", from FieldHeader header in headers select row[header.Name].ToString());
                await requestResponseDataFlow.SendRequest("{FU" + uploadData + "}");
            }

            arbitrator.Release(InstanceName);
            getNextPage?.Invoke();
        }
    }
}
