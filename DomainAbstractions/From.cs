using System;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// From abstraction is supposed to support query SELECT operation, assign table name for query.
    ///
    /// Input ports:
    /// 1. ITableDataFlow: interface between abstractions implemented ITableDataFlow
    ///
    /// Output ports:
    /// 1. ITableDataFlow sourceDataFlow: used for source ITableDataFlow wire
    /// 2. ITableDataFlow destinationDataFlow: used for destination ITableDataFlow wire
    ///
    /// e.g. new From("table name")
    /// </summary>
    public class From : ITableDataFlow
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private ITableDataFlow sourceDataFlow;
        private ITableDataFlow destinationDataFlow;

        // fields
        private DataTable dataTable = new DataTable();
        private int currentIndex = 0;
        private bool queryMode = false;
        private JObject fromContainer = new JObject();
        private string table;
        private Tuple<int, int> sourceTuple;


        // ctor
        public From(string queryTable) { table = queryTable; }
        
        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // request operating mode with end point.
            if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

            if (queryMode)
            {
                // check whether received JObject from other instances
                if (queryOperation != null) fromContainer = queryOperation as JObject;
                // initialise "SELECT"-"From" property
                InitialiseFromTable(fromContainer);
                // add from information into JObject
                AddFromTable(fromContainer, table);
                // pass JObject through ITableDataFlow chain
                if (sourceDataFlow != null) await sourceDataFlow.GetHeadersFromSourceAsync(fromContainer);
                dataTable = sourceDataFlow?.DataTable;
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            // var tuple = await sourceDataFlow.GetPageFromSourceAsync();

            if (queryMode)
            {
                sourceTuple = await sourceDataFlow.GetPageFromSourceAsync();
                dataTable = sourceDataFlow?.DataTable;
                return sourceTuple;
            }

            foreach (DataRow r in sourceDataFlow.DataTable.Rows) dataTable.ImportRow(r);
            return sourceTuple;

            // var tuple = await sourceDataFlow.GetPageFromSourceAsync();
            //
            // var tmpIndex = currentIndex;
            //
            // if (dataTable.Rows.Count < sourceDataFlow.DataTable.Rows.Count)
            // {
            //     for (var i = dataTable.Rows.Count; i < sourceDataFlow.DataTable.Rows.Count; i++)
            //     {
            //         dataTable.ImportRow(sourceDataFlow.DataTable.Rows[i]);
            //     }
            // }
            //
            // currentIndex = dataTable.Rows.Count;
            //
            // if (tmpIndex >= dataTable.Rows.Count)
            //     return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);
            //
            // return new Tuple<int, int>(tmpIndex, dataTable.Rows.Count);

            // if (!queryMode && currentIndex == 0)
            // {
            //     foreach (DataRow row in sourceDataFlow.DataTable.Rows)
            //         dataTable.ImportRow(row);
            // }
            //
            // var tmpIndex = currentIndex;
            //
            // if (tmpIndex >= dataTable.Rows.Count)
            //     return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);
            //
            // currentIndex = dataTable.Rows.Count;
            // return new Tuple<int, int>(tmpIndex, dataTable.Rows.Count);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            if (sourceDataFlow == null && destinationDataFlow == null) return true;
            return sourceDataFlow?.RequestQuerySupport() ?? true;
        }

        // private methods
        /// <summary>
        ///  Initialise JObject query container with "SELECT"-"From"
        /// </summary>
        /// <param name="container">Query operation container</param>
        private void InitialiseFromTable(JObject container)
        {
            if (container.ContainsKey("SELECT"))
            {
                JObject selectObj = (JObject)container["SELECT"];

                if (!selectObj.ContainsKey("From"))
                {
                    selectObj.Add(new JProperty("From",new JValue(string.Empty)));
                }
            }
            else
            {
                container.Add(new JProperty("SELECT", new JObject(new JProperty("From", new JValue(string.Empty)))));
            }
        }
        /// <summary>
        /// Add select from information into JObject container
        /// </summary>
        /// <param name="container">Query operation container</param>
        /// <param name="tableName">Columns selected</param>
        private void AddFromTable(JObject container, string tableName)
        {
            var fromTable = (JValue) container["SELECT"]["From"];
            fromTable.Value = tableName;
        }
    }
}
