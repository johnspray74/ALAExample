using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Manages the Life Data (animal information over it's life time) of the device,
    /// e.g. life data records count, uploading life data to connected device (destination) or downloading life data from the connected device (source).
    /// Works in a similar way as the SessionDataSCP, except sends Life Data related SCP commands to
    /// the connected device to get data such as total records count, every row of the Life Data etc.
    /// For each interaction with the device, the IArbitrator will take the responsibility of 
    /// arranging all the command in a sequential order and the IRequestResponseDataFlow is in charge
    /// of interacting with the device.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow inputOutputTableData: input/output port which can be source - getting (retrieving life data from device) or destination - putting life data (uploading life data to device)
    /// 2. IEvent startLifeDataCount: start fetching the life data record count
    /// 3. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol
    /// 4. IArbitrator arbitrator: arbitrator for ordering SCP commands
    /// 5. List<IDataFlow<string>> lifeDataCountFanOut: sends the life data record count to all abstractions that is connected to this port
    /// </summary>
    public class LifeDataSCP : ITableDataFlow, IEvent // inputOutputTableData, startLifeDataCount
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";

        // outputs -----------------------------------------------------------------
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private List<IDataFlow<string>> lifeDataCountFanOut = new List<IDataFlow<string>>();

        // private fields -----------------------------------------------------------------
        private int recordCount = 0, pageSize = 26;
        private long rowId = 1;
        private bool cacheFlag = false;

        /// <summary>
        /// Manages the Life Data of the device, e.g. life data records count, downloading life
        /// data from the connected device.
        /// </summary>
        public LifeDataSCP() { }


        // IEvent implementation -----------------------------------------------------------------
        // Normally, this mehod will not be declared with "async", the reason using "async" is that 
        // it calls other method with "await", which requires an "async" key word to decorate the method
        async void IEvent.Execute()
        {
            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGLD}");
            string record = await requestResponseDataFlow.SendRequest("{FE}");
            arbitrator.Release(InstanceName);

            if (record != null)
            {
                recordCount = Convert.ToInt32(record);
                foreach (IDataFlow<string> d in lifeDataCountFanOut) d.Data = record;
            }
        }

        // ITableDataFlow implementation ---------------------------------------------------------
        private DataTable currentDataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => currentDataTable;
        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() { throw new NotImplementedException(); }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // select life data file
            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGLD}");
            arbitrator.Release(InstanceName);

            if (currentDataTable.Columns.Count > 0 && currentDataTable.Rows.Count > 0)
            {
                cacheFlag = true;
            }
            else
            {
                currentDataTable = new DataTable();
                currentDataTable.TableName = "Life Data";

                // start to fetch header
                string header = null;
                await arbitrator.Request(InstanceName);
                header = await requestResponseDataFlow.SendRequest("{FH}");
                while (!string.IsNullOrEmpty(header))
                {
                    currentDataTable.Columns.Add(header.Substring(3));
                    header = await requestResponseDataFlow.SendRequest("{FH}");
                }
                arbitrator.Release(InstanceName);
                currentDataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });

            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (cacheFlag)
            {
                // This statement must be in front of return,
                // because when the records are saved to files, it will keep callling this function to load the next batch.
                // If the cacheFlag still be true, it will return the cached data instead of next batch.
                cacheFlag = false;
                return new Tuple<int, int>(0, currentDataTable.Rows.Count);
            }

            if (currentDataTable.Rows.Count >= recordCount)
            {
                // all records has been loaded, return no new record indexs
                return new Tuple<int, int>(currentDataTable.Rows.Count - 1, currentDataTable.Rows.Count - 1);
            }

            await arbitrator.Request(InstanceName);
            int startRowIndex = currentDataTable.Rows.Count;
            await requestResponseDataFlow.SendRequest("{FD}");
            await requestResponseDataFlow.SendRequest("{FR" + startRowIndex + "}");
            string content = await requestResponseDataFlow.SendRequest("{FN" + pageSize + "}");
            arbitrator.Release(InstanceName);

            if (!string.IsNullOrEmpty(content))
            {
                string[] records = content.Split(';');
                foreach (var r in records)
                {
                    string[] fields = r.Split(',');

                    // add one record to datatable
                    DataRow row = currentDataTable.NewRow();
                    for (var i = 0; i < currentDataTable.Columns.Count - 1; i++)
                    {
                        row[currentDataTable.Columns[i].ColumnName] = fields[i + 1];
                    }

                    row["index"] = rowId++;
                    currentDataTable.Rows.Add(row);
                }
            }

            return new Tuple<int, int>(startRowIndex, currentDataTable.Rows.Count);
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }
        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate callBack) { throw new NotImplementedException(); }        
    }
}
