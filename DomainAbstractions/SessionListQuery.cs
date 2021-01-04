using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    public class SessionListQuery : ITableDataFlow
    {
        // public properties
        public string InstanceName = "Default";

        // ports
        private ITableDataFlow sourceDataFlow;
        private ITableDataFlow destinationDataFlow;
        private IDataFlow<string> sessionCount;
        private IDataFlow<DataTable> dataFlowDataTable;

        // private filed
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private DataTable sessionListDataTable = new DataTable(); // used to store session list data
        private bool queryMode;
        private JObject sessionListContainer = new JObject();
        private Tuple<int, int> sourceTuple;

        // ctor
        public SessionListQuery() { }

        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => sessionListDataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // request operating mode with end point.
            if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

            if (queryMode)
            {
                if (sourceDataFlow != null)
                {
                    await sourceDataFlow.GetHeadersFromSourceAsync(sessionListContainer);
                    sessionListDataTable = sourceDataFlow.DataTable;
                    sessionListDataTable.Columns["date"].Prefix = "hide";
                    sessionListDataTable.Columns["index"].Prefix = "hide";
                    sessionListDataTable.Columns["name"].Prefix = "hide";
                    sessionListDataTable.Columns["count"].Prefix = "hide";
                }

                // process with data table and fill sessionListDataTable
                foreach (DataRow dataRow in sessionListDataTable.Rows)
                {
                    try
                    {
                        dataRow["date"] = epoch.AddSeconds(long.Parse(dataRow["date"].ToString())).ToString("dd/MM/yyyy");
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"SessionListQuery {InstanceName} session missing date {e.Message}");
                        dataRow["date"] = "";
                    }

                    dataRow["description"] = $"{dataRow["name"]}\n{dataRow["date"]} ({dataRow["count"]} records)";
                }
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            sourceTuple = await sourceDataFlow.GetPageFromSourceAsync();
            if (sessionCount != null) sessionCount.Data = sessionListDataTable.Rows.Count.ToString();

            // output the data table
            if (dataFlowDataTable != null) dataFlowDataTable.Data = sessionListDataTable;

            return sourceTuple;
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            return sourceDataFlow?.RequestQuerySupport() ?? true;
        }
    }
}
