using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Libraries;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Map operator for ITableDataFlow (ITableDataFlow decorator) which keeps the selected columns by the giving property 'Columns' and removes all other columns.
    /// Analogous to LINQ Select for IEnumerable or IObservable.
    /// </summary>
    public class Select : ITableDataFlow, IDataFlow<DataTable>, IIterator<Tuple<string, string, string, string>>
    {
        // properties
        public string InstanceName = "Default";
        public string[] Columns;

        // outputs
        private ITableDataFlow tableDataFlow;
        private IDataFlow<DataTable> dataFlowDataTable;

        private IIterator<ITableDataFlow> iterator;

        /// <summary>
        /// Takes an ITableDataFlow and keeps certain columns.
        /// </summary>
        public Select() { }

        // IDataFlow<DataTable> implementation
        private DataTable dataTableHeader;
        DataTable IDataFlow<DataTable>.Data { set => dataTableHeader = value; }

        // IIterator<dynamic> implementation
        bool IIterator<Tuple<string, string, string, string>>.Finished => iterator.Finished;

        async Task<Tuple<string, string, string, string>> IIterator<Tuple<string, string, string, string>>.Next()
        {
            // First read the entire input on the iterator in case it is connected to a CSVFileReaderWriter

            ITableDataFlow tableDataFlow = await iterator.Next();
            await tableDataFlow.GetHeadersFromSourceAsync();
            Tuple<int, int> tuple = await tableDataFlow.GetPageFromSourceAsync();
            while (tuple.Item1 < tuple.Item2)
            {
                tuple = await tableDataFlow.GetPageFromSourceAsync();
            }
            
            DataRow r = dataTableHeader.Rows[0]; // KL: 
            return new Tuple<string, string, string, string>(
                r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString());
        }

        void IIterator<Tuple<string, string, string, string>>.Reset()
        {
            iterator.Reset();
        }

        // ITableDataFlow implementation ---------------------------------------------------
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
            await tableDataFlow.GetHeadersFromSourceAsync();

            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            dataTable.TableName = tableDataFlow.DataTable.TableName;

            foreach (DataColumn c in tableDataFlow.DataTable.Columns)
            {
                if (Columns.Contains(c.ColumnName))
                {
                    dataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            Tuple<int, int> tuple = await tableDataFlow.GetPageFromSourceAsync();
            foreach (DataRow r in tableDataFlow.DataTable.Rows) dataTable.ImportRow(r);
            // output the data table
            if (dataFlowDataTable != null) dataFlowDataTable.Data = dataTable;
            return tuple;
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            if (tableDataFlow == null) return;

            tableDataFlow.DataTable.Rows.Clear();
            tableDataFlow.DataTable.Columns.Clear();
            tableDataFlow.DataTable.TableName = dataTable.TableName;

            foreach (DataColumn c in dataTable.Columns)
            {
                if (Columns.Contains(c.ColumnName))
                {
                    tableDataFlow.DataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }
            }

            await tableDataFlow.PutHeaderToDestinationAsync();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (tableDataFlow == null) return;
            
            for (var i = firstRowIndex; i < lastRowIndex; i++)
            {
                tableDataFlow.DataTable.ImportRow(dataTable.Rows[i]);
            }

            // output the data table
            if (dataFlowDataTable != null) dataFlowDataTable.Data = dataTable;

            tableDataFlow.DataTable.LogDataChange($"{(InstanceName != "Default" ? InstanceName : "(No instance name) select")} tableDataFlow DataTable");


            await tableDataFlow.PutPageToDestinationAsync(firstRowIndex, lastRowIndex, getNextPage);
        }
    }
}
