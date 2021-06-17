using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// An abstraction which sends SCP commands to connected SCP device to get session list meta data of date, index, name, count, description.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow inputOutputTableData: currently only supported as a source data table for retrieving the session list information from the connected SCP device
    /// 2. IRequestResponseDataFlow<string, string> requestResponseDataFlow: usage of the request response of the SCPProtocol to communicate with the device
    /// 3. IArbitrator arbitrator: arbitrator for ordering SCP commands
    /// 4. IDataFlow<string> sessionListCount: outputs the session list count as a string
    /// </summary>
    public class SCPSessions : ITableDataFlow, IDataFlow<string> // inputOutputTableData
    {
        // properties
        // public string InstanceName = "Default";
        public string InstanceName;

        // ports
        private IRequestResponseDataFlow<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;

        // private fields 
        private DataTable sessionListDataTable;
        private List<string> sessionFileNumber; // List that contians the indexes of each session data
        private int indexOfList = 0; // the index of the sessionIndexList above (NOT TO BE CONFUSED WITH THE ACTUAL SESSION INDEX)

        /// <summary>
        /// Synchronize the session files and their relative meta data e.g. date, record count from the connected device.
        /// </summary>
        public SCPSessions() { }

        // ITableDataFlow implementation -----------------------------------------------------------------
        DataTable ITableDataFlow.DataTable => sessionListDataTable;
        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        string IDataFlow<string>.Data 
        {
            set
            {
                var _fireAndForgot = SetSessionIndexAsync(value);
            }
        }

        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        private async Task SetSessionIndexAsync(string index)
        {
            // select the corresponding session file in the connected device
            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGDD}");
            await requestResponseDataFlow.SendRequest("{FF" + index + "}");
            arbitrator.Release(InstanceName);
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // initialize data  
            sessionFileNumber = new List<string>();
            sessionListDataTable = new DataTable();
            indexOfList = 0;

            // add table headers, prefix "hide" means the column will not show          
            sessionListDataTable.Columns.Add("checkbox" );
            sessionListDataTable.Columns.Add("description");
            sessionListDataTable.Columns.Add(new DataColumn("date") { Prefix = "hide" });
            sessionListDataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
            sessionListDataTable.Columns.Add(new DataColumn("name") { Prefix = "hide" });
            sessionListDataTable.Columns.Add(new DataColumn("count") { Prefix = "hide" });

            // start to fetch file index from device
            await arbitrator.Request(InstanceName);
            await requestResponseDataFlow.SendRequest("{FGDD}");

            string index = null;
            // fetch session file index list
            index = await requestResponseDataFlow.SendRequest("{FL}");
            while (!string.IsNullOrEmpty(index))
            {
                sessionFileNumber.Add(index);
                index = await requestResponseDataFlow.SendRequest("{FL}");
            }
            arbitrator.Release(InstanceName);

            int lastIndex = int.Parse(sessionFileNumber[sessionFileNumber.Count - 1]) + 1;
            sessionFileNumber.Add((lastIndex + 1).ToString());
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (indexOfList >= sessionFileNumber.Count - 1)
            {
                return new Tuple<int, int>(indexOfList, indexOfList);
            }
            else
            {
                string name = null;
                string date = null;
                string record = null;

                await arbitrator.Request(InstanceName);
                try
                {
                    name = await requestResponseDataFlow.SendRequest("{FPNA" + sessionFileNumber[indexOfList] + "}");
                    date = await requestResponseDataFlow.SendRequest("{FPDA" + sessionFileNumber[indexOfList] + "}");
                    record = await requestResponseDataFlow.SendRequest("{FPNR" + sessionFileNumber[indexOfList] + "}");
                }
                catch (TaskCanceledException tsc)
                {
                    System.Diagnostics.Debug.WriteLine("3 seconds no response from device.");
                }
                arbitrator.Release(InstanceName);

                DataRow row = sessionListDataTable.NewRow();
                row["name"] = name;
                row["checkbox"] = false;
                row["index"] = sessionFileNumber[indexOfList];
                row["count"] = record;
                row["date"] = date;
                row["description"] = name + "\n" + date + " (" + record + " records)";
                sessionListDataTable.Rows.Add(row);
                indexOfList += 1;

                return new Tuple<int, int>(indexOfList - 1, indexOfList);
            }
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }
        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) { throw new NotImplementedException(); }
    }
}
