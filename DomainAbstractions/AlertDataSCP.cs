using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Manages the Alerts of the device, e.g. alerts data records count, downloading alert data from the connected device by sending SCP commands.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports
    /// 1. ITableDataFlow inputOutputTableData: input managing the data as pages and headers seperately as well as provides a united interface for transacting.
    /// 2. IEvent startAlertsCount : input for fetching the records count when it's triggered by IDataFlow<bool>
    /// 3. IDataFlow<bool> isDeviceConnected: boolean input to show what device is connected
    /// 4. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol to communicate with the device
    /// 5. IArbitrator arbitrator: take the responsibility of arranging all the command in a sequential order
    /// 6. List<IDataFlow<string>> alertCountFanOut: sends the alert data record count to all abstractions that are connected to this port
    /// </summary>
    public class AlertDataSCP : ITableDataFlow, IEvent, IDataFlow<bool> // inputOutputTableData, startAlertsCount, isDeviceConnected
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private List<IDataFlow<string>> alertCountFanOut = new List<IDataFlow<string>>();

        // private fields
        private int recordCount = 0, pageSize = 26;
        private long rowId = 1;
        private bool cacheFlag = false;
        private bool isConnected = false;

        /// <summary>
        /// Manages the Alerts Data of the device, e.g. alert data records count, downloading alert
        /// data from the connected device.
        /// </summary>
        public AlertDataSCP() { }

        // ITableDataFlow implementation ---------------------------------------------------------
        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;
        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // select life data file
            await arbitrator.Request(InstanceName);
            var _fireAndForgot = await requestResponseDataFlow.SendRequest("{FGLD}");

            if (dataTable.Columns.Count > 0 && dataTable.Rows.Count > 0)
            {
                cacheFlag = true;
                arbitrator.Release(InstanceName);
            }
            else
            {
                dataTable = new DataTable();
                dataTable.TableName = "Alert Data";

                // Need to find SCP command to find alert headings for all devices for more abstraction
                dataTable.Columns.Add("EID");
                dataTable.Columns.Add("Alert");
                dataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
                arbitrator.Release(InstanceName);
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
                return new Tuple<int, int>(0, dataTable.Rows.Count);
            }

            if (dataTable.Rows.Count >= recordCount)
            {
                // all records has been loaded, return no new record indexs
                return new Tuple<int, int>(dataTable.Rows.Count - 1, dataTable.Rows.Count - 1);
            }

            await arbitrator.Request(InstanceName);
            int startRowIndex = dataTable.Rows.Count;
            string content = await requestResponseDataFlow.SendRequest("{URA" + startRowIndex + "}");

            if (!string.IsNullOrEmpty(content))
            {
                string[] records = content.Split(';');
                foreach (var r in records)
                {
                    string[] fields = r.Split(',');

                    // add one record to datatable
                    DataRow row = dataTable.NewRow();
                    for (var i = 0; i < dataTable.Columns.Count-1; i++)
                    {
                        row[dataTable.Columns[i].ColumnName] = fields[i];
                    }

                    row["index"] = rowId++;
                    dataTable.Rows.Add(row);

                }
            }

            arbitrator.Release(InstanceName);

            return new Tuple<int, int>(startRowIndex, dataTable.Rows.Count);
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }
        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate callBack) { throw new NotImplementedException(); }

        private async Task GetNumberOfAlertsAsync()
        {
            await arbitrator.Request(InstanceName);
            string record = await requestResponseDataFlow.SendRequest("{CRNA}"); // {CRNA} SCP command to get the number of alerts
            arbitrator.Release(InstanceName);

            if (record != null)
            {
                recordCount = Convert.ToInt32(record);
                foreach (IDataFlow<string> d in alertCountFanOut) d.Data = record;
            }
        }

        // IDataFlow<bool> implementations
        bool IDataFlow<bool>.Data
        {
            set
            {
                isConnected = value;
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            if (isConnected)
            {
                var _fireAndForget = GetNumberOfAlertsAsync();
            }
        }
    }
}
