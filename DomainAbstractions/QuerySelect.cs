using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    // TODO: aborted
    public class QuerySelect : ITableDataFlow
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private ITableDataFlow sourceDirection;
        private ITableDataFlow destinationDirection;

        // private fields
        private bool distinct = false;
        private List<List<Tuple<string, string>>> selectColumns = new List<List<Tuple<string, string>>>();
        // private List<string> columnAlias = new List<string>();
        private List<string> table = new List<string>();
        private List<string> limit = new List<string>();
        private List<string> offset = new List<string>();

        private Dictionary<string, object>  selectOperation;
        private DataTable dataTable= new DataTable();
        private bool queryMode = false;

        public QuerySelect()
        {
        }

        public QuerySelect(string table)
        {
            From(table);
        }


        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation = null)
        {
            // step 1: request operating mode(Query or not) from source
            // This class will be switched into query mode when queryMode == True;
            // This class will keep normal mode when queryMode == False;
            queryMode = sourceDirection.RequestQuerySupport();

            if (queryMode)
            {
                // Query get headers
                // Create and add query select operation
                if (queryOperation != null) selectOperation = queryOperation as Dictionary<string, object>;

                // Add temp information into dictionary
                if (selectOperation == null) selectOperation = CreateSqlSelectSyntax();

                foreach (var columns in selectColumns)
                {
                    SelectQuery(selectOperation, columns);
                }

                selectOperation["DISTINCT"] = distinct;

                var fromTable = selectOperation["FROM"] as List<string>;
                foreach (var tableName in table)
                {
                    fromTable?.Add(tableName);
                }
                selectOperation["FROM"] = fromTable;

                var limitNum = selectOperation["LIMIT"] as List<string>;
                foreach (var value in limit)
                {
                    limitNum?.Add(value);
                }
                selectOperation["LIMIT"] = limitNum;

                var offsetNum = selectOperation["OFFSET"] as List<string>;
                foreach (var value in offset)
                {
                    offsetNum?.Add(value);
                }
                selectOperation["OFFSET"] = offsetNum;



                await sourceDirection.GetHeadersFromSourceAsync(selectOperation);

                dataTable = sourceDirection.DataTable;

            }
            else
            {
                // Normal get headers

            }


        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) { throw new NotImplementedException(); }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }

        bool ITableDataFlow.RequestQuerySupport()
        {
            return sourceDirection.RequestQuerySupport();
        }

        // private methods
        public QuerySelect Select()
        {
            return this;
        }

        public QuerySelect Select(params string[] columns)
        {
            var tmpSelect = new List<Tuple<string, string>>();

            foreach (var column in columns) tmpSelect.Add(new Tuple<string, string>($"{column}", ""));

            selectColumns.Add(tmpSelect);

            // SelectQuery(selectOperation, tmpSelect);

            return this;
        }

        public QuerySelect Select(string[] columns, string[] alias)
        {
            var tmpSelect = new List<Tuple<string, string>>();

            if (columns.Length >= alias.Length)
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    tmpSelect.Add(i < alias.Length 
                        ? new Tuple<string, string>($"{columns[i]}", $"{alias[i]}")
                        : new Tuple<string, string>($"{columns[i]}", ""));

                }
            }
            else
            {
                for (var j = 0; j < alias.Length; j++)
                {
                    tmpSelect.Add(j < columns.Length
                        ? new Tuple<string, string>($"{columns[j]}", $"{alias[j]}") 
                        : new Tuple<string, string>("", $"{alias[j]}"));
                }
            }

            selectColumns.Add(tmpSelect);

            // SelectQuery(selectOperation, tmpSelect);

            return this;
        }

        public QuerySelect Distinct()
        {
            distinct = true;

            // selectOperation["DISTINCT"] = true;

            return this;
        }

        public QuerySelect From(string tableName)
        {
            // var fromTable = selectOperation["FROM"] as List<string>;
            // fromTable?.Add(tableName);
            // selectOperation["FROM"] = fromTable;

            table.Add(tableName);

            return this;
        }

        public QuerySelect Limit(int value)
        {
            // var limitNum = selectOperation["LIMIT"] as List<string>;
            // limitNum?.Add(value.ToString());
            // if (value > 0) selectOperation["LIMIT"] = limitNum;

            if (value > 0) limit.Add(value.ToString());

            return this;
        }

        public QuerySelect Offset(int value)
        {
            // var offsetNum = selectOperation["OFFSET"] as List<string>;
            // offsetNum?.Add(value.ToString());
            // if (value > 0) selectOperation["OFFSET"] = offsetNum;

            if (value > 0) offset.Add(value.ToString());

            return this;
        }

        /// <summary>
        /// Create SQL SELECT syntax
        /// </summary>
        /// <param name="operation"> SELECT, INSERT ALTER or UPDATE</param>
        /// <returns></returns>
        private Dictionary<string, object> CreateSqlSelectSyntax(string operation = "SELECT")
        {
            return new Dictionary<string, object>()
            {
                {
                    "OPERATION", operation
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "SELECT", new List<List<Tuple<string, string>>>() { }
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "DISTINCT", false
                }, // stores whether return distinct result or not
                {
                    "FROM", new List<string>() { }
                }, // stores a list of table names, use the first one as the main data source table
                {
                    "LEFT JOIN", new List<string>() { }
                }, // stores a list join tables need to be joined with main table
                {
                    "ON", new List<string>() { }
                }, // stores a list of join conditions match with join tables
                {
                    "FILTER", new List<Tuple<string, string, string>>() { }
                }, // stores a list of filter conditions in tuple format: <"SessionTable.sessionid", "=", "{primary key}">
                {
                    "GROUP BY", new List<string>() { }
                }, // stores a list of group conditions
                {
                    "HAVING", new List<string>() { }
                }, // stores a list of having conditions, should use with GROUP BY
                {
                    "SORT", new List<Tuple<string, string>>() { }
                }, // stores a list of sort conditions in tuple format, but only the last one will be used, <"sort key", "ASC/DESC">
                {
                    "LIMIT", new List<string>() { }
                }, // stores a list of limit numbers of selection
                {
                    "OFFSET", new List<string>() { }
                } // stores a list of index, use with LIMIT
            };
        }

        /// <summary>
        /// Add Select operation into Select syntax.
        /// </summary>
        /// <param name="sqlSelect"></param>
        /// <param name="selectTupleList"></param>
        private void SelectQuery(Dictionary<string, object> sqlSelect,
            List<Tuple<string, string>> selectTupleList)
        {
            var selectValue = sqlSelect["SELECT"] as List<List<Tuple<string, string>>>; // get value from dictionary and assign the type

            selectValue?.Add(selectTupleList);

            sqlSelect["SELECT"] = selectValue;
        }

    }
}
