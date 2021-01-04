using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.Data;

namespace DomainAbstractions
{
    //TODO: add summary and code

    /// <summary>
    /// Allows a DataRow to be imported synchronously as a List<string> while also supporting ITableDataFlow
    /// 
    /// ITableDataFlow
    /// </summary>

    public class Table : IDataFlow<List<string>>, IEvent, ITableDataFlow, IDataFlow<DataTable>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private DataTable dataTable = new DataTable();
        private bool headersHaveBeenSet = false;
        private bool dataSent = false;

        // Ports
        private IEvent rowAdded;
        private IDataFlow<DataTable> dataTableOutput;
        private IDataFlow<string> tableRowCount;

        public Table(List<string> columnNames = null)
        {
            if (columnNames != null) SetHeaders(columnNames);
        }

        public Table(params string[] columnNames)
        {
            if (columnNames != null) SetHeaders(columnNames.ToList());
        }

        private void SetHeaders(List<string> headers)
        {
            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }
            headersHaveBeenSet = true;
        }

        // IDataFlow<List<string>> implementation
        List<string> IDataFlow<List<string>>.Data
        {
            set
            {
                if (!headersHaveBeenSet)
                {
                    SetHeaders(value);
                }
                else
                {
                    if (value.Count == dataTable.Columns.Count)
                    {
                        DataRow row = dataTable.NewRow();
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            row[dataTable.Columns[i]] = value[i];
                        }
                        dataTable.Rows.Add(row);
                        rowAdded?.Execute();
                    }
                    else
                    {
                        Debug.WriteLine($"Row length {value.Count} does not match header length {dataTable.Columns.Count} when adding to Table {InstanceName}");
                    }
                    if (dataTableOutput != null) dataTableOutput.Data = dataTable;
                }
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // Clear the table's rows
            dataTable.Rows.Clear();
        }

        // ITableDataFlow implementation ------------------------------------------------------
        DataTable ITableDataFlow.DataTable => dataTable;

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() { return false; }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            dataSent = false; // reset flag to allow for another transaction
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            tableRowCount.Data = dataTable.Rows.Count.ToString();
            int finalRow = dataTable.Rows.Count;
            int startRow = !dataSent ? 0 : finalRow;
            dataSent = true;
            return new Tuple<int, int>(startRow, finalRow);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() {  }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex,
            GetNextPageDelegate getNextPage)
        {
            if (dataTableOutput != null) dataTableOutput.Data = dataTable;
        }

        // IDataFlow<DataTable> implementation
        DataTable IDataFlow<DataTable>.Data
        {
            set
            {
                dataTable.Rows.Clear();
                dataTable.Columns.Clear();
                dataTable = value.Copy();
            }
        }
    }
}
