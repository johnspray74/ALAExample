using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainAbstractions;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    // public delegate bool FilterLambdaDelegate(DataRow row);
    // public delegate bool FilterLambdaParamDelegate(DataRow row, string param);

    /// <summary>
    /// Filter abstraction is supposed to filter DataTable or add query filter information into query.
    /// By default, if the JObject query container is empty, will add information into "SELECT"-"Filter".
    /// If some of "SELECT", "INSERT", "UPDATE" property names exist in JObject, will add to all of them.
    /// </summary>
    public class FilterRows<T> : ITableDataFlow, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";
        public string Condition = "AND";
        public FilterLambdaDelegate FilterDelegate;
        public FilterLambdaParamDelegate FilterLambdaParamDelegate;

        // ports
        private ITableDataFlow sourceDataFlow;
        private ITableDataFlow destinationDataFlow;
        private IDataFlow<DataTable> dataFlowDataTableOutput;

        // fields
        private string lambdaParam;
        private DataTable dataTable = new DataTable();
        private int currentIndex = 0;
        private bool queryMode = false;
        private JObject filterContainer = new JObject();
        private string columnName;
        private string filterOpt;
        private string filterValue;
        private bool filterAdded = false;
        private List<DataRow> dataRows = new List<DataRow>();
        private DataRow currentRow;
        private Tuple<int, int> sourceTuple;

        // private T value;
        // private Tuple<double, double> valueRange;
        // private IEnumerable<T> valueCollection;

        // ctor
        public FilterRows() { }

        public FilterRows(string filterColumn, string filterOperator, T value)
        {
            // normal: column operator value
            columnName = filterColumn;

            filterOpt = ValidateFilterOperator(filterOperator, value);

            filterValue = value is null ? "NULL" : value.ToString();
        }
        
        public FilterRows(string filterColumn, double rangeStart, double rangeStop)
        {
            // between: column between rangeStart and rangeStop
            columnName = filterColumn;

            filterOpt = "BETWEEN";

            filterValue = $"{rangeStart} AND {rangeStop}";
        }

        public FilterRows(string filterColumn, IEnumerable<T> collectionValues)
        {
            // in: column in collectionValues
            columnName = filterColumn;

            filterOpt = "IN";

            var collection = string.Join(", ", collectionValues.Select(t => $"'{t}'"));

            filterValue = $"({collection})";
        }

        // IDataFlow implementation
        string IDataFlow<string>.Data { set => lambdaParam = value; }

        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation = null)
        {
            // request operating mode with end point.
            if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

            if (queryMode)
            {
                // check whether received JObject from other instances
                if (queryOperation != null) filterContainer = queryOperation as JObject;
                // initialise "Filter" property, by default add to SELECT property
                InitialiseFilter(filterContainer);
                // create filter condition
                var filterCondition = CreateFilterObjectFromValue(columnName, filterOpt, filterValue);
                // add filter condition into JObject
                if (!filterAdded)
                {
                    AddFilterCondition(filterContainer, filterCondition);
                    filterAdded = true;
                }
                // pass JObject through ITableDataFlow chain
                if (sourceDataFlow != null) await sourceDataFlow.GetHeadersFromSourceAsync(filterContainer);
                dataTable = sourceDataFlow?.DataTable;
            }
            else
            {
                // TODO: Need implementation for DataTable
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
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            if (queryMode)
            {
                // TODO: Need implementation for query mode
            }
            else
            {
                dataRows.Clear();

                if (destinationDataFlow == null) return;

                destinationDataFlow.DataTable.Rows.Clear();
                destinationDataFlow.DataTable.Columns.Clear();
                destinationDataFlow.DataTable.TableName = dataTable.TableName;

                foreach (DataColumn c in dataTable.Columns)
                {
                    destinationDataFlow.DataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }

                await destinationDataFlow.PutHeaderToDestinationAsync();
            }
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (queryMode)
            {
                // TODO: Need implementation for query mode
            }
            else
            {
                List<DataRow> rowList = new List<DataRow>();

            for (var i = firstRowIndex; i < lastRowIndex; i++)
            {
                DataRow r = dataTable.Rows[i];
                bool condition = FilterDelegate != null ?
                    FilterDelegate(r) : FilterLambdaParamDelegate(r, lambdaParam);

                if (condition) rowList.Add(r);
            }

            if (destinationDataFlow == null)
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
                int startIndex = destinationDataFlow.DataTable.Rows.Count;
                foreach (var r in rowList) destinationDataFlow.DataTable.ImportRow(r);

                if (destinationDataFlow.DataTable.Rows.Count == 0 && currentRow != null)
                {
                    destinationDataFlow.DataTable.ImportRow(currentRow);
                }

                if (dataFlowDataTableOutput != null)
                    dataFlowDataTableOutput.Data = destinationDataFlow.DataTable;

                await destinationDataFlow.PutPageToDestinationAsync(startIndex, destinationDataFlow.DataTable.Rows.Count, getNextPage);
            }
            }
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            if (sourceDataFlow == null && destinationDataFlow == null) return true;
            return sourceDataFlow?.RequestQuerySupport() ?? true;
        }

        // private methods
        private string ValidateFilterOperator(string filterOperator, T value)
        {
            var validOperator = filterOperator.ToUpper();

            if (value is int || value is short || value is long)
            {
                if (!new List<string>() { "=", ">", "<", ">=", "<=", "!=", "<>", "IS", "IS NOT" }
                        .Contains(validOperator) || validOperator.Equals("IS"))
                {
                    validOperator = "=";
                }

                if (validOperator.Equals("IS NOT"))
                {
                    validOperator = "<>";
                }
            }

            if (value is string)
            {
                if (!new List<string>() { "=", "!=", "<>", "LIKE", "NOT LIKE", "IS", "IS NOT", "IN" }
                        .Contains(validOperator) || validOperator.Equals("IS"))
                {
                    validOperator = "=";
                }

                if (validOperator.Equals("IS NOT") || validOperator.Equals("!="))
                {
                    validOperator = "<>";
                }
            }

            if (value is bool || value is null)
            {
                if (!new List<string>() { "=", "!=", "<>", "IS", "IS NOT" }
                        .Contains(validOperator) || validOperator.Equals("="))
                {
                    validOperator = "IS";
                }

                if (validOperator.Equals("!=") || validOperator.Equals("<>"))
                {
                    validOperator = "IS NOT";
                }
            }

            return validOperator;
        }

        private void InitialiseFilter(JObject container)
        {
            if (container.ContainsKey("SELECT"))
            {
                JObject selectObj = (JObject)container["SELECT"];

                if (!selectObj.ContainsKey("Filter"))
                {
                    selectObj.Add(new JProperty("Filter", 
                        new JObject(
                            new JProperty("AND", new JArray()),
                            new JProperty("OR", new JArray())
                            )
                        )
                    );
                }
            }
            else
            {
                container.Add(new JProperty("SELECT", 
                    new JObject(
                        new JProperty("Filter", 
                            new JObject(
                                new JProperty("AND", new JArray()),
                                new JProperty("OR", new JArray())
                                )
                            )
                        )
                    )
                );
            }
        }

        private JObject CreateFilterObjectFromValue(string col, string op, string value)
        {
            return new JObject(new JProperty("Column", new JValue(col)),
                new JProperty("Operator", new JValue(op)),
                new JProperty("Value", new JValue(value))
            );
        }

        private void AddFilterCondition(JObject container, JObject cond)
        {
            JArray filterArray;

            if (!new string[] {"AND", "OR"}.Contains(Condition.ToUpper()))
            {
                filterArray = (JArray) container["SELECT"]["Filter"]["AND"];
            }
            else
            {
                filterArray = (JArray) container["SELECT"]["Filter"][Condition.ToUpper()];
            }

            filterArray.Add(cond);

        }
    }
}
