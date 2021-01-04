using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ProgrammingParadigms
{
    /// <summary>
    /// An ITableDataFlow decorator which fans out an ITableDataFlow to mutiple ITableDataFlows
    /// </summary>
    public class TableDataFlowConnector : ITableDataFlow
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private List<ITableDataFlow> tableDataFlowList = new List<ITableDataFlow>();

        /// <summary>
        /// Fans out an ITableDataFlow to mutiple ITableDataFlows
        /// </summary>
        public TableDataFlowConnector() { }

        // ITableDataFlow implementation
        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;

        private DataRow currentRow;
        DataRow ITableDataFlow.CurrentRow {
            get => currentRow;
            set
            {
                currentRow = value;
                foreach (var t in tableDataFlowList) t.CurrentRow = value;
            }
        }

        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            throw new NotImplementedException();
        }

        Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            foreach (var t in tableDataFlowList)
            {
                t.DataTable.PrimaryKey = null;
                t.DataTable.Rows.Clear();
                t.DataTable.Columns.Clear();
                t.CurrentRow = currentRow;

                foreach (DataColumn c in dataTable.Columns)
                {
                    t.DataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }

                if (tableDataFlowList.IndexOf(t) != 0)
                {
                    await t.PutHeaderToDestinationAsync();
                }
            }

            if (tableDataFlowList.Count > 0)
            {
                await tableDataFlowList[0].PutHeaderToDestinationAsync();
            }
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            foreach (var t in tableDataFlowList)
            {
                if (t.DataTable.Rows.Count >= 1)
                {
                    t.DataTable.Clear();
                }

                for (var i = firstRowIndex; i < lastRowIndex; i++)
                {
                    t.DataTable.ImportRow(dataTable.Rows[i]);
                }

                if (tableDataFlowList.IndexOf(t) == tableDataFlowList.Count - 1)
                {
                    await t.PutPageToDestinationAsync(firstRowIndex, lastRowIndex, getNextPage);
                }
                else
                {
                    await t.PutPageToDestinationAsync(firstRowIndex, lastRowIndex, null);
                }
            }
        }
    }
}
