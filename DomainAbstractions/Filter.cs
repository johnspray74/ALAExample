using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Libraries;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    public delegate bool FilterLambdaDelegate(DataRow row);
    public delegate bool FilterLambdaParamDelegate(DataRow row, string param);

    /// <summary>
    /// Generally it is an ITableDataFlow decorator which uses a lambda to filter the row data with condition.
    /// It goes through all the stored rows and returns the row which conforms to the condition of the lambda.
    /// Lambda is an expression that can use any of the columns but must result in a boolean.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow "NEEDNAME": incoming table that wants to be filtered
    /// 2. IDataFlow<string> "NEEDNAME: incoming string that defines the lamda parameter
    /// 3. ITableDataFlow tableDataFlow: filtered output table as a ITableDataFlow type
    /// 4. IDataFlow<DataTable> dataFlowDataTableOutput: filtered output table as a IDataFlow<DataTable> type
    /// </summary>
    public class Filter : ITableDataFlow, IDataFlow<string>
    {
        // properties ---------------------------------------------------------------------
        public string InstanceName = "Default";
        public FilterLambdaDelegate FilterDelegate;
        public FilterLambdaParamDelegate FilterLambdaParamDelegate;

        // ports ------------------------------------------------------------------------
        private ITableDataFlow tableDataFlow;
        private IDataFlow<DataTable> dataFlowDataTableOutput;

        // fields
        private bool dataSent = false;

        /// <summary>
        /// Filter the rows of data in an ITableDataFlow, keeping only the ones that comform to the Lambda Delegate.
        /// </summary>
        public Filter() { }

        // IDataFlow implementation ----------------------------------------------
        private string lambdaParam;
        string IDataFlow<string>.Data { set => lambdaParam = value; }

        // ITableDataFlow implmentation ---------------------------------------------------
        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;
        private DataRow currentRow;
        DataRow ITableDataFlow.CurrentRow { get => currentRow; set => currentRow = value; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            return tableDataFlow?.RequestQuerySupport() ?? false;
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation = null)
        {
            dataSent = false;

            await tableDataFlow.GetHeadersFromSourceAsync();

            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            dataTable.TableName = tableDataFlow.DataTable.TableName;

            foreach (DataColumn c in tableDataFlow.DataTable.Columns)
            {
                dataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            int startRow = 0;
            int finalRow = 0;

            Tuple<int, int> tuple = await tableDataFlow.GetPageFromSourceAsync();

            if (!dataSent)
            {
                startRow = 0;
                foreach (DataRow r in tableDataFlow.DataTable.Rows)
                {
                    bool condition = FilterDelegate != null ? FilterDelegate(r) : FilterLambdaParamDelegate(r, lambdaParam);

                    if (condition) dataTable.ImportRow(r);
                }
                dataSent = true;
            }
            else
            {
                startRow = dataTable.Rows.Count;
            }
            finalRow = dataTable.Rows.Count;

            return new Tuple<int, int>(startRow, finalRow);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            dataRows.Clear();

            if (tableDataFlow == null) return;

            tableDataFlow.DataTable.Rows.Clear();
            tableDataFlow.DataTable.Columns.Clear();
            tableDataFlow.DataTable.TableName = dataTable.TableName;

            foreach (DataColumn c in dataTable.Columns)
            {
                tableDataFlow.DataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
            }

            await tableDataFlow.PutHeaderToDestinationAsync();
        }

        private List<DataRow> dataRows = new List<DataRow>();
        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            List<DataRow> rowList = new List<DataRow>();

            for (var i = firstRowIndex; i < lastRowIndex; i++)
            {
                DataRow r = dataTable.Rows[i];
                bool condition = FilterDelegate != null ?
                    FilterDelegate(r) : FilterLambdaParamDelegate(r, lambdaParam);

                if (condition) rowList.Add(r);
            }

            if (tableDataFlow == null)
            {
                if (getNextPage != null)
                {
                    dataRows.AddRange(rowList);
                    getNextPage.Invoke();
                }
                else
                {
                    DataTable table = dataTable.Copy();
                    table.Rows.Clear();
                    dataRows.AddRange(rowList);

                    foreach (var r in dataRows) table.ImportRow(r);
                    dataFlowDataTableOutput.Data = table;
                }
            }
            else
            {
                int startIndex = tableDataFlow.DataTable.Rows.Count;
                foreach (var r in rowList) tableDataFlow.DataTable.ImportRow(r);

                if (tableDataFlow.DataTable.Rows.Count == 0 && currentRow != null)
                {
                    tableDataFlow.DataTable.ImportRow(currentRow);
                }

                if (dataFlowDataTableOutput != null)
                    dataFlowDataTableOutput.Data = tableDataFlow.DataTable;

                tableDataFlow.DataTable.LogDataChange($"{(InstanceName != "Default" ? InstanceName : "(No instance name) filter")} tableDataFlow DataTable");

                await tableDataFlow.PutPageToDestinationAsync(startIndex, tableDataFlow.DataTable.Rows.Count, getNextPage);
            }       
        }
    }
}
