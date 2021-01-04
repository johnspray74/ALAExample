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
    // TODO: for refactor QueryGeneratorSessionData use.
    public class SessionDataQuery : IDataFlow<string>, ITableDataFlow
    {
        // properties
        public string InstanceName = "Default";
        public string DeviceName = "XR5000";


        // ports
        private ITableDataFlow sourceDataFlow;


        // fields
        private bool queryMode;
        private JObject sessionDataContainer = new JObject();
        private DataTable dataTable = new DataTable();
        private DataTable sessionDataHeader = new DataTable();

        private Dictionary<string, DataTable> cachedSessionData = new Dictionary<string, DataTable>();
        private Dictionary<string, int> sessionRecordsCount = new Dictionary<string, int>();
        private bool cacheFlag = false;
        private string sessionIndex;


        // implementation of IDataFlow<string>
        string IDataFlow<string>.Data { set => sessionIndex = value; }

        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            if (cachedSessionData.ContainsKey(sessionIndex)
                && cachedSessionData[sessionIndex].Rows.Count > 0
                && cachedSessionData[sessionIndex].Columns.Count > 0)
            {
                // cache data table exists.
                cacheFlag = true;
                dataTable = cachedSessionData[sessionIndex];
            }
            else
            {
                dataTable = new DataTable();
                cachedSessionData[sessionIndex] = dataTable;
                if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

                if (queryMode)
                {
                    if (sourceDataFlow != null)
                    {
                        await sourceDataFlow.GetHeadersFromSourceAsync(sessionDataContainer);
                        sessionDataHeader = sourceDataFlow.DataTable;

                        foreach (DataRow headerRow in sessionDataHeader.Rows)
                        {
                            var sessionLabelname = headerRow["labelname"].ToString();
                            dataTable.Columns.Add(sessionLabelname);
                        }

                        dataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });

                        if (sessionDataHeader.Rows.Count > 0)
                        {
                            sessionRecordsCount[sessionIndex] = int.Parse(sessionDataHeader.Rows[0]["count"].ToString());
                            dataTable.TableName = $"FileNo: {sessionIndex};Name: {sessionDataHeader.Rows[0]["name"].ToString()};Date: {sessionDataHeader.Rows[0]["date"].ToString()}";
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"==========> Empty query result for session {sessionIndex}");
                            throw new ArgumentException($"Empty query result for session {sessionIndex}");
                        }
                    }
                }
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            string primaryKey = sessionIndex;
            if (cacheFlag)
            {
                cacheFlag = false;
                return new Tuple<int, int>(0, dataTable.Rows.Count);
            }
            if (sessionRecordsCount.ContainsKey(primaryKey)
                && dataTable.Rows.Count >= sessionRecordsCount[primaryKey])
            {
                return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);
            }
            int rowStartIndex = dataTable.Rows.Count;

            return new Tuple<int, int>(1,1);

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
            throw new NotImplementedException();
        }
    }
}
