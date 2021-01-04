using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// 1. IDataFlow<string> currentSessionIndex: sets the current session index to cache the session table data (not needing to reload it)
    /// 2. ITableDataFlow inputOutputTableData: input/output port which can be source - getting session data (retrieving from device) or destination - putting session data (uploading to device)
    /// 3. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol
    /// 3. IArbitrator arbitrator: arbitrator for ordering SCP commands
    /// </summary>
    public class SessionDataSCP : IDataFlow<string>, ITableDataFlow // currentSessionIndex, inputOutputTableData
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;

        // cache, the loaded session data will be stored in cache so we don't need to load again when we select the session
        private Dictionary<string, DataTable> cachedSessionData = new Dictionary<string, DataTable>();
        private Dictionary<string, int> sessionRecordsCount = new Dictionary<string, int>();

        // private fields
        // how many records are there in a page. default is 26
        private int pageSize = 26;
        private long rowId = 1;

        // this flag is used when a session file is selected and there exsits cached data, then the cached data will be returned
        // when GetPageFromSource is called. However, there still exists another situation that the user wants to load next page
        // when calling GetPageFromSource, then the flag should be false and it will return the data of the next page.
        private bool cacheFlag = false;

        /// <summary>
        /// Fetch session data by sending a series SCP commands to device. Such as {FH} for header, {FN} for row.
        /// </summary>
        public SessionDataSCP() { }

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
                

            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            // cache exist and the current table is going to be displayed
            if (cacheFlag)
            {
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
                            row[currentDataTable.Columns[i].ColumnName] = fields[i + 1];
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
        //private bool is2Fields = false;
        private string uploadSessionPrimaryKey;
        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            string tableName = currentDataTable.TableName;
            DataColumnCollection columns = currentDataTable.Columns;

            string[] meta = currentDataTable.TableName.Split(';');
            if (meta.Length < 4) return;
   
            uploadSessionPrimaryKey = currentSessionPrimaryKey;
            string fileName = meta[1].Length > 6 ? meta[1].Substring(6) : "session-file";
            fileName = fileName.IndexOf(",") > 0 ? fileName.Substring(0, fileName.IndexOf(",")) : fileName;
            string date = meta[2].Length > 15 ? meta[2].Substring(6, 10) : DateTime.Now.ToString("dd/MM/yyyy");

            string[] fieldIDs = meta[3].Split(',');
            string letterFieldId = "";

            for(int i = 0; i < fieldIDs.Length; i++)
            {
                if (i != fieldIDs.Length - 1)
                {
                    letterFieldId += fieldIDs[i].Substring(0,2) + ",";
                }
                else
                {
                    letterFieldId += fieldIDs[i].Substring(0, 2);
                }
            }

            //is2Fields = false;

            currentDataTable.TableName = tableName;
            foreach (DataColumn c in columns)
            {
                if (!currentDataTable.Columns.Contains(c.ColumnName))
                {
                    currentDataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }
            }

            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGDD}"); // to operate on Session Data 
            await requestResponseDataFlow.SendRequest("{FF" + uploadSessionPrimaryKey + "}");
            await requestResponseDataFlow.SendRequest("{FC}"); // this to clear the current 
            await requestResponseDataFlow.SendRequest("{FPNA" + uploadSessionPrimaryKey + "," + fileName + "}");
            await requestResponseDataFlow.SendRequest("{FPDA" + uploadSessionPrimaryKey + "," + date + "}");
            arbitrator.Release(InstanceName);

            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FI" + letterFieldId + "}");
            arbitrator.Release(InstanceName);

            //KL: commented out the is2field as unsure what this is for?
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"{ex.Message}");
            //    await requestResponseDataFlow.SendRequest("{FIDW,F1}");
            //    is2Fields = true;
            //}

        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (firstRowIndex >= lastRowIndex) // all records has been uploaded, release arbitrator source
            {
                sessionRecordsCount[uploadSessionPrimaryKey] = lastRowIndex;
                //cachedSessionData.Remove(uploadSessionPrimaryKey);
                getNextPage?.Invoke();
                return;
            }

            //string headers = is2Fields ? "Weight,EID" : "IDV,Weight,Draft,IDE";

            string headers = "";
            for(int i = 0; i < currentDataTable.Columns.Count; i++)
            {
                DataColumn c = currentDataTable.Columns[i];

                if(i != currentDataTable.Columns.Count - 1)
                {
                    headers += currentDataTable.Columns[i] + ",";
                }
                else
                {
                    headers += currentDataTable.Columns[i];
                }
            }

            //StringBuilder sb = new StringBuilder();
            for (var i = firstRowIndex; i < lastRowIndex; i++)
            {
                DataRow r = currentDataTable.Rows[i];

                string rowInfo = String.Join(",", r.ItemArray);

                // KL: Rosman way of uploading multiple rows at a time however does not work for XRS2
                //foreach (var h in headers.Split(','))
                //{
                //    sb.AppendFormat("{0},", r[h]);
                //}
                //sb.Remove(sb.Length - 1, 1);
                //sb.Append(";");

                await arbitrator.Request(InstanceName);
                await requestResponseDataFlow.SendRequest("{FU"+ rowInfo + "}");
                arbitrator.Release(InstanceName);

            }
            //string uploadCommand = "{FU" + sb.Remove(sb.Length - 1, 1).ToString() + "}";

            //await arbitrator.Request(InstanceName);
            //await requestResponseDataFlow.SendRequest(uploadCommand);
            //arbitrator.Release(InstanceName);

            getNextPage?.Invoke();
        }
    }
}
