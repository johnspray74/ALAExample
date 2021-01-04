using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Retrieves (e.g. favourites record count, name and description) and uploads favourites to the device by sending SCP commands to the connected device.
    /// Works in a similar way as the AlertDataSCP.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow inputOutputTableData: input managing the data as pages and headers seperately as well as provides a united interface for transacting.
    /// 2. IEvent startFavouritesCount : input for fetching the records count when it's triggered by IDataFlow<bool>
    /// 3. IDataFlow<bool> isDeviceConnected: boolean input to show what device is connected
    /// 4. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol to communicate with the device
    /// 5. IArbitrator arbitrator: take the responsibility of arranging all the command in a sequential order
    /// 6. List<IDataFlow<string>> favouritesCountFanOut: sends the favourite record count to all abstractions that is connected to this port
    /// </summary>
    public class FavouriteSetupSCP : ITableDataFlow, IEvent, IDataFlow<bool> // inputOutputTableData, startFavouritesCount, isDeviceConnected
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // outputs ---------------------------------------------------------------
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private List<IDataFlow<string>> favouritesCountFanOut = new List<IDataFlow<string>>();

        // private fields ---------------------------------------------------------------
        private int recordCount = 0, pageSize = 26;
        private long rowId = 1;
        private bool cacheFlag = false;
        private bool isConnected = false;

        /// <summary>
        /// Manages the Favourites Data of the device, e.g. favourites data record count and downloading favourites
        /// data from the connected device.
        /// </summary>
        public FavouriteSetupSCP() { }

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
                dataTable.TableName = "Favourites";

                // Need to find SCP command to find favourites headings for all devices for more abstraction
                // start to fetch header
                dataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Description");
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
            await requestResponseDataFlow.SendRequest("{FD}"); // {FD} first to start downloading from the first record
            await requestResponseDataFlow.SendRequest("{FR" + startRowIndex + "}"); // {FRn} resets back to this record number inside the selected session or life data
            string content = await requestResponseDataFlow.SendRequest("{FVD" + startRowIndex + "}");
            arbitrator.Release(InstanceName);

            if (!string.IsNullOrEmpty(content))
            {
                string[] records = content.Split(';');
                foreach (var r in records)
                {
                    string[] fields = r.Split(',');

                    // add one record to datatable
                    DataRow row = dataTable.NewRow();
                    for (var i = 0; i < dataTable.Columns.Count; i++)
                    {
                        row[dataTable.Columns[i].ColumnName] = fields[i];
                    }

                    row["index"] = rowId++;
                    dataTable.Rows.Add(row);
                }
            }

            return new Tuple<int, int>(startRowIndex, dataTable.Rows.Count);
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }
        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate callBack) { throw new NotImplementedException(); }

        // private method ---------------------------------------------------------------
        // async to send SCP command to retrieve count of favourites
        private async void GetNumberOfFavouritesAsync()
        {
            await arbitrator.Request(InstanceName);

            string faveResults = await requestResponseDataFlow.SendRequest("{FVD" + recordCount + "}");
            while (!String.IsNullOrEmpty(faveResults))
            {
                recordCount++;
                faveResults = await requestResponseDataFlow.SendRequest("{FVD" + recordCount + "}");
            }

            arbitrator.Release(InstanceName);

            foreach (IDataFlow<string> d in favouritesCountFanOut) d.Data = recordCount.ToString();
        }

        // IDataFlow<bool> implementation -----------------------------------------------------------------
        // If the DataFlowConnector for device returns true, IEvent below is executed to retrieve the count of favourites
        bool IDataFlow<bool>.Data
        {
            set
            {
                isConnected = value;
            }
        }

        // IEvent implementation -----------------------------------------------------------------
        void IEvent.Execute()
        {
            if (isConnected) GetNumberOfFavouritesAsync();
        }
    }
}
