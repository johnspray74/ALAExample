using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    public delegate bool SelectLambdaDelegate(DataColumn col);

    /// <summary>
    /// SelectColumns abstraction is supposed to support select columns operation with both Query flow and DataTable flow.
    /// When wiring SelectColumns in a ITableDataFlow chain, it will first check with source until end point to get operating mode.
    ///
    ///Input ports:
    /// 1. ITableDataFlow: interface between abstractions implemented ITableDataFlow
    ///
    /// Output ports:
    /// 1. ITableDataFlow sourceDataFlow: used for source ITableDataFlow wire
    /// 2. ITableDataFlow destinationDataFlow: used for destination ITableDataFlow wire
    /// 
    /// Query mode: Only used in source direction, GetHeader and GetPage
    /// 1. new SelectColumns() => select all columns.
    /// 2. new SelectColumns("Column0", "Column1", "Column2") => Add "SELECT"-"Columns"-["Column0", "Column1", "Column2"] information in JObject.
    /// 3. new SelectColumns(new string[] {"Column0", "Column1", "Column2"}, new string[] {"NewName1", "NewName2"})
    ///     => Add "SELECT"-"Columns"-["Column0 AS NewName1", "Column1 AS NewName1", "Column2"] information in JObject.
    /// 4. new SelectColumns(new string[] {"Column0", "Column1"}, new string[] {"NewName1", "NewName2", "NewName3"})
    ///     => Add "SELECT"-"Columns"-["Column0 AS NewName1", "Column1 AS NewName1", "NULL AS NewName3"] information in JObject.
    /// 5. The updated JObject will be passed through the chain until end point to compile and get DataTable return.
    ///
    /// DataTable mode: can be used in both directions
    /// 1. new SelectColumns() => select all columns.
    /// 2. new SelectColumns("Column0", "Column1", "Column2") => select columns with column names "Column0", "Column1", "Column2" from DataTable.
    /// 3. new SelectColumns(){ SelectDelegate = (DataColumn col) => return new List<string>() { "Column0", "Column1", "Column2" }.Contains(col.ColumnName); }
    ///     => select columns with column names "Column0", "Column1", "Column2" from DataTable.
    /// 4. In some cases, for example, process 3000-format session file, the actual header information is at the 4th row of the session file,
    /// we want to remove empty-value columns and duplicate-header-value columns, e.g.:
    /// new SelectColumns(){ SelectDelegate =
    /// (DataColumn col) =>
    /// {
    /// return col.Table.Columns.Cast<DataColumn>()
    /// .GroupBy(c => col.Table.Rows[4][c])
    /// .Select(g => g.ToList().First())
    ///     .Contains(col) // columns for distinct values in a row
    ///     && !string.IsNullOrWhiteSpace(col.Table.Rows[4][col].ToString());
    /// };}
    /// 
    /// Description for the Linq above:
    /// 1) Group row[4] of the Table by cell values
    /// 2) Select the first column of each group(Non duplicate values group) and generate a list of DataColumn as Non-duplicate column list
    /// 3) Every time receives a column, will check whether this column exists in the Non-duplicate column list or not.
    /// 4) return true if exist, false if not.
    /// 
    /// A simple ref for Linq GroupBy, https://stackoverflow.com/questions/8939516/how-to-find-duplicate-record-using-linq-from-datatable
    /// </summary>
    public class SelectColumns : ITableDataFlow, IDataFlow<DataTable>
    {
        // properties
        public string InstanceName = "Default";
        public SelectLambdaDelegate SelectDelegate;

        // ports
        // ---- |SelectColumns| ---- |Other instance| ---- |Destination instance (return whether support query or not)|
        private ITableDataFlow sourceDataFlow;
        private ITableDataFlow destinationDataFlow;
        private IDataFlow<DataTable> dataTableResult;

        // fields
        private DataTable dataTable = new DataTable();
        private int currentIndex = 0;
        private bool queryMode = false;
        private JObject selectContainer = new JObject();
        private List<string> selectColumns = new List<string>();
        private bool selectAdded = false;
        private Tuple<int, int> sourceTuple;

        // ctor
        public SelectColumns() { }

        public SelectColumns(params string[] columns)
        {
            foreach (var column in columns) selectColumns.Add(column);
            SelectDelegate = (DataColumn col) => selectColumns.Contains(col.ColumnName);
        }

        public SelectColumns(string[] columns, string[] queryAlias)
        {
            // used in query mode, when need change display column names or select extra information like a NULL column.
            if (columns.Length >= queryAlias.Length)
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    if (string.IsNullOrEmpty(columns[i])) columns[i] = "NULL";

                    selectColumns.Add(i < queryAlias.Length
                        ? $"{columns[i]} AS \"{queryAlias[i]}\""
                        : $"{columns[i]}");
                }
            }
            else
            {
                for (var j = 0; j < queryAlias.Length; j++)
                {
                    if (string.IsNullOrEmpty(columns[j])) columns[j] = "NULL";

                    selectColumns.Add(j < columns.Length
                        ? $"{columns[j]} AS \"{queryAlias[j]}\""
                        : $"NULL AS \"{queryAlias[j]}\"");
                }
            }
        }

        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // request operating mode with end point.
            if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

            if (queryMode)
            {
                // check whether received JObject from other instances
                if (queryOperation != null) selectContainer = queryOperation as JObject;
                // initialise "SELECT"-"Columns" property
                InitialiseSelectColumns(selectContainer);
                // add columns information into JObject
                if (!selectAdded)
                {
                    AddSelectColumns(selectContainer, selectColumns);
                    selectAdded = true;
                }
                // pass JObject through ITableDataFlow chain
                if (sourceDataFlow != null) await sourceDataFlow.GetHeadersFromSourceAsync(selectContainer);
                dataTable = sourceDataFlow?.DataTable;
            }
            else
            {
                // get DataTable from source
                await sourceDataFlow.GetHeadersFromSourceAsync();

                // In case the lambda expression use table rows
                // sourceTuple = await sourceDataFlow.GetPageFromSourceAsync();

                // initialise local data table
                dataTable.Rows.Clear();
                dataTable.Columns.Clear();
                dataTable.TableName = sourceDataFlow.DataTable.TableName;

                // add column names match lambda
                foreach (DataColumn col in sourceDataFlow.DataTable.Columns)
                {
                    var selectCondition = SelectDelegate?.Invoke(col) ?? true;
                    if (!selectCondition) continue;

                    dataTable.Columns.Add(new DataColumn(col.ColumnName, col.DataType) { Prefix = col.Prefix });
                }
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            // var tuple = await sourceDataFlow.GetPageFromSourceAsync();

            if (queryMode)
            {
                sourceTuple = await sourceDataFlow.GetPageFromSourceAsync();
                dataTable = sourceDataFlow?.DataTable;
                if (dataTableResult != null) dataTableResult.Data = dataTable;
                return sourceTuple;
            }

            sourceTuple = await sourceDataFlow.GetPageFromSourceAsync();

            for (int i = dataTable.Rows.Count; i < sourceDataFlow.DataTable.Rows.Count; i++)
            {
                dataTable.ImportRow(sourceDataFlow.DataTable.Rows[i]);
            }

            return sourceTuple;
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            // only DataTable flow is supported in Put direction
            if (destinationDataFlow != null) await destinationDataFlow.PutHeaderToDestinationAsync();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (destinationDataFlow == null) return;
            // clear destination
            destinationDataFlow.DataTable.Rows.Clear();
            destinationDataFlow.DataTable.Columns.Clear();
            destinationDataFlow.DataTable.TableName = dataTable.TableName;
            // select columns match requirements.
            // reason to put this part here instead of in PutHeader is some Lambda may need to use rows in the DataTable.
            foreach (DataColumn col in dataTable.Columns)
            {
                var selectCondition = SelectDelegate?.Invoke(col) ?? true;
                if (!selectCondition) continue;
                destinationDataFlow.DataTable.Columns.Add(new DataColumn(col.ColumnName, col.DataType)
                    {Prefix = col.Prefix});
            }
            // after select columns, import rows
            for (var i = firstRowIndex; i < lastRowIndex; i++)
            {
                destinationDataFlow.DataTable.ImportRow(dataTable.Rows[i]);
            }
            // put selected DataTable to next ITableDataFlow instance
            await destinationDataFlow.PutPageToDestinationAsync(firstRowIndex, lastRowIndex, getNextPage);

        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() { return sourceDataFlow?.RequestQuerySupport() ?? true; }

        // private methods
        /// <summary>
        /// Initialise JObject query container with "SELECT"-"Columns"
        /// </summary>
        /// <param name="container">Query operation container</param>
        private void InitialiseSelectColumns(JObject container)
        {
            if (container.ContainsKey("SELECT"))
            {
                JObject selectObj = (JObject)container["SELECT"];

                if (!selectObj.ContainsKey("Columns"))
                {
                    selectObj.Add(new JProperty("Columns", new JArray())
                    );
                }
            }
            else
            {
                container.Add(new JProperty("SELECT", new JObject(new JProperty("Columns", new JArray()))));
            }
        }
        /// <summary>
        /// Add select columns information into JObject container
        /// </summary>
        /// <param name="container">Query operation container</param>
        /// <param name="columns">Columns selected</param>
        private void AddSelectColumns(JObject container, List<string> columns)
        {
            var columnArray = (JArray) container["SELECT"]["Columns"];
            columnArray.Add(columns);
        }

        // Two columns contains column names and aliases
        DataTable IDataFlow<DataTable>.Data
        {
            set
            {
                foreach (DataRow dataRow in value.AsEnumerable())
                {
                    var selectAlias = $"{dataRow[0]} AS \"{dataRow[1]}\"";
                    if (!selectColumns.Contains(selectAlias)) selectColumns.Add(selectAlias);
                }
            }
        }
    }
}
