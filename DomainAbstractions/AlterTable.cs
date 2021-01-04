using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainAbstractions;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// TODO: aborted, replaced by SelectColumns

    public delegate bool AlterLambdaDelegate(DataColumn col);
    public delegate bool AlterLambdaParamDelegate(DataColumn col, string param);
    public delegate bool AlterLambdaIndexParamDelegate(DataColumn col, string param, int index);

    /// <summary>
    /// AlterTable abstraction is supposed to alter columns in a DataTable based on the lambda expression.
    /// This abstraction can be used at both source and destination sides of Transact.
    /// By default, the row index is -1, means can use lambda expression to handle select columns purpose.
    /// In some cases, for example, process 3000-format session file, the actual header information is at the 4th row of the session file, we want to remove empty-value columns and duplicate-header-value columns.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<string> lambdaExpression: a lambda expression passed from other class.
    /// 2. IDataFlow<int> rowIndex: validate row index, e.g. 3000-format file, row index usually is 4.
    /// 3. ITableDataFlow : interface between abstractions implemented ITableDataFlow
    /// 4. ITableDataFlow getTableAlter: used for source ITableDataFlow wire
    /// 5. ITableDataFlow putTableAlter: used for destination ITableDataFlow wire
    ///
    /// ------------------------------------------------------------------------------------------------------------------
    /// Two implementation examples:
    /// 1. Select columns with column names: Column1, Column2, Column3
    /// .WireTo(new AlterTable(){ AlterDelegate = (DataColumn col) => return new List<string>() { "Column1", "Column2", "Column3" }.Contains(col.ColumnName); }
    /// .WireTo(SOME INSTANCE,
    /// "getTableAlter")
    /// )
    /// Description for the Linq above:
    /// 1) Create a new List<string> contains column names needed, e.g. { "Column1", "Column2", "Column3" }
    /// 2) Every time receives a column, check whether this column name exists in the column list
    /// 3) return true if exists, false if not.
    /// 
    /// 2. In some cases, for example, process 3000-format session file, the actual header information is at the 4th row of the session file,
    /// we want to remove empty-value columns and duplicate-header-value columns, e.g.:
    /// .WireTo(new AlterTable(){ AlterDelegate =
    /// (DataColumn col) =>
    /// {
    /// return col.Table.Columns.Cast<DataColumn>()
    /// .GroupBy(c => col.Table.Rows[4][c])
    /// .Select(g => g.ToList().First())
    ///     .Contains(col) // columns for distinct values in a row
    ///     && !string.IsNullOrWhiteSpace(col.Table.Rows[4][col].ToString());
    /// };})
    /// Description for the Linq above:
    /// 1) Group row[4] of the Table by cell values
    /// 2) Select the first column of each group(Non duplicate values group) and generate a list of DataColumn as Non-duplicate column list
    /// 3) Every time receives a column, will check whether this column exists in the Non-duplicate column list or not.
    /// 4) return true if exist, false if not.
    /// 
    /// A simple ref for Linq GroupBy, https://stackoverflow.com/questions/8939516/how-to-find-duplicate-record-using-linq-from-datatable
    /// </summary>
    public class AlterTable : IDataFlow<string>, IDataFlow<int>, ITableDataFlow // lambdaExpression, rowIndex
    {
        // public properties
        public string InstanceName = "Default";
        public AlterLambdaDelegate AlterDelegate;
        public AlterLambdaParamDelegate AlterParamDelegate;
        public AlterLambdaIndexParamDelegate AlterIndexParamDelegate;
            
        // ports
        private ITableDataFlow getTableAlter; // used at the GetFromSource side of Transact
        private ITableDataFlow putTableAlter; // used at the PutFromSource side of Transact

        // private fields
        private string lambdaParam;
        private int alterRowIndex = -1;
        private DataTable dataTable = new DataTable();
        private Tuple<int, int> getTuple;

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => lambdaParam = value; }

        // IDataFlow<int> implementation
        int IDataFlow<int>.Data { set => alterRowIndex = value; }
        
        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // get DataTable from source
            await getTableAlter.GetHeadersFromSourceAsync();
            getTuple = await getTableAlter.GetPageFromSourceAsync(); // In case the lambda expression use table rows
            
            // initialise local data table
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            dataTable.TableName = getTableAlter.DataTable.TableName;
            
            // add column names match lambda
            foreach (DataColumn col in getTableAlter.DataTable.Columns)
            {
                var alterCondition = AlterDelegate?.Invoke(col) ?? 
                                     (alterRowIndex > -1
                                         ? AlterIndexParamDelegate(col, lambdaParam, alterRowIndex)
                                         : AlterParamDelegate(col, lambdaParam));
            
                if (!alterCondition) continue;
            
                dataTable.Columns.Add(new DataColumn(col.ColumnName, col.DataType) {Prefix = col.Prefix});
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {

            foreach (DataRow row in getTableAlter.DataTable.Rows) dataTable.ImportRow(row);

            return getTuple;
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            if (putTableAlter == null) return;

            await putTableAlter.PutHeaderToDestinationAsync();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (putTableAlter == null) return;

            putTableAlter.DataTable.Rows.Clear();
            putTableAlter.DataTable.Columns.Clear();
            putTableAlter.DataTable.TableName = dataTable.TableName;

            foreach (DataColumn col in dataTable.Columns)
            {
                var alterCondition = AlterDelegate?.Invoke(col) ??
                                     (alterRowIndex > -1
                                         ? AlterIndexParamDelegate(col, lambdaParam, alterRowIndex)
                                         : AlterParamDelegate(col, lambdaParam));

                if (!alterCondition) continue;

                putTableAlter.DataTable.Columns.Add(new DataColumn(col.ColumnName, col.DataType) { Prefix = col.Prefix });
            }

            for (var i = firstRowIndex; i < lastRowIndex; i++)
            {
                putTableAlter.DataTable.ImportRow(dataTable.Rows[i]);
            }

            await putTableAlter.PutPageToDestinationAsync(firstRowIndex, lastRowIndex, getNextPage);
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }
    }
}
