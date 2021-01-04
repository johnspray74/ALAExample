using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    // TODO: to be aborted, replaced with SessionListQuery
    /// <summary>
    /// QueryGeneratorSessionList abstraction is used to get session data from query devices such as XR5000, ID5000.
    /// Input ports:
    /// 1. ITableDataFlow: Usually wired from Transact as data source to get session data from device.
    /// Output ports:
    /// 1. IRequestResponseDataFlow<string, DataTable> sessionListDbRequest: session list related database request.
    /// 2. IRequestResponseDataFlow<Dictionary<string, object>, string> sessionListQueryRequest: SQLite query compile request.
    /// 3. IDataFlow<string> sessionCount: count of all valid sessions.
    /// </summary>
    public class QueryGeneratorSessionList : ITableDataFlow
    {
        // public properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, DataTable> sessionListDbRequest;
        private IRequestResponseDataFlow_B<Dictionary<string, object>, string> sessionListQueryRequest;
        private IDataFlow<string> sessionCount;

        // private filed
        // Dictionary<string, object> used to store raw requests to database, pass to query builder to validate and final generation of query command.
        // private readonly Dictionary<string, object> sqlSyntaxDict;
        private readonly int limitNumber;
        private DataTable sessionListDataTable; // used to store session list data
        private int currentIndex = 0;

        // private List<string> sessionIndexList;  // used to store actual session id in database

        public QueryGeneratorSessionList(int batchNumber = 100)
        {
            limitNumber = batchNumber;
        }

        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => sessionListDataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // sessionIndexList = new List<string>();
            currentIndex = 0;
            sessionListDataTable = new DataTable();

            sessionListDataTable.Columns.Add("checkbox");
            sessionListDataTable.Columns.Add("description");
            sessionListDataTable.Columns.Add(new DataColumn("date") { Prefix = "hide" });
            sessionListDataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
            sessionListDataTable.Columns.Add(new DataColumn("name") { Prefix = "hide" });
            sessionListDataTable.Columns.Add(new DataColumn("count") { Prefix = "hide" });

            var sessionIndexCountComm = "SELECT COUNT(*) FROM SessionTable WHERE SessionTable.sessioncount > 0";

            var sessionIndexCount = await sessionListDbRequest.SendRequest(sessionIndexCountComm);

            // get data from database
            var sessionListQuerySyntax = CreateSqlSelectSyntax();

            SelectQuery(sessionListQuerySyntax, new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("'false'", "checkbox"),
                new Tuple<string, string>("SessionTable.sessionid", "index"),
                new Tuple<string, string>("SessionTable.sessionname", "name"),
                new Tuple<string, string>("SessionTable.sessioncount", "count"),
                new Tuple<string, string>("SessionTable.sessionstartdate", "date")
            });

            var fromTable = sessionListQuerySyntax["FROM"] as List<string>;
            fromTable?.Add("SessionTable");
            sessionListQuerySyntax["FROM"] = fromTable;

            FilterQuery(sessionListQuerySyntax,
                new Tuple<string, string, string>("SessionTable.sessioncount", ">", "0"));

            SortQuery(sessionListQuerySyntax,
                new Tuple<string, string>("SessionTable.sessionid", "DESC"));

            var sessionListQueryComm = await sessionListQueryRequest.SendRequest(sessionListQuerySyntax);

            var sessionListQuery = await sessionListDbRequest.SendRequest(sessionListQueryComm);

            // process with data table and fill sessionListDataTable
            foreach (DataRow dataRow in sessionListQuery.Rows)
            {
                DataRow newSessionRow = sessionListDataTable.NewRow();
                newSessionRow["checkbox"] = dataRow["checkbox"];
                newSessionRow["name"] = dataRow["name"].ToString();
                newSessionRow["index"] = dataRow["index"].ToString();
                newSessionRow["count"] = dataRow["count"].ToString();
                newSessionRow["date"] = dataRow["date"].ToString();
                newSessionRow["description"] = $"{dataRow["name"]}\n{dataRow["date"]} ({dataRow["count"]} records)";
                sessionListDataTable.Rows.Add(newSessionRow);
            }

            sessionCount.Data = sessionListDataTable.Rows.Count.ToString();
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            var newIndexOfTransact = (currentIndex + limitNumber) > sessionListDataTable.Rows.Count
                ? sessionListDataTable.Rows.Count
                : currentIndex + limitNumber;
            
            var transactIndex = new Tuple<int, int>(currentIndex, newIndexOfTransact);
            
            currentIndex = newIndexOfTransact;
            
            return transactIndex;
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() => throw new NotImplementedException();

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) => throw new NotImplementedException();

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        #region sql select region

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

        /// <summary>
        /// Add Filter operation into Select syntax.
        /// </summary>
        /// <param name="sqlFilter"></param>
        /// <param name="filterTuple"></param>
        private void FilterQuery(Dictionary<string, object> sqlFilter,
            Tuple<string, string, string> filterTuple)
        {
            var validFilterOperator = new[] { "=", "!=", ">", "<", ">=", "<=", "IN", "NOT IN", "IS", "IS NOT" };

            if (string.IsNullOrEmpty(filterTuple.Item3)
                || string.IsNullOrEmpty(filterTuple.Item3.Trim())
                || string.Equals(filterTuple.Item3.Trim().ToLower(), "null")) // in case the input is "null", "NULL" or "Null" etc.
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, "is", "null");
            }
            else if (string.Equals(filterTuple.Item2.ToLower(), "not null"))
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, "is", "not null");
            }
            else
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, filterTuple.Item2, $"'{filterTuple.Item3}'");
            }

            filterTuple = validFilterOperator.Contains(filterTuple.Item2.ToUpper())
                ? filterTuple
                : new Tuple<string, string, string>(filterTuple.Item1, "=", filterTuple.Item3);

            var filterValue = sqlFilter["FILTER"] as List<Tuple<string, string, string>>;

            filterValue?.Add(filterTuple);

            sqlFilter["FILTER"] = filterValue;
        }

        /// <summary>
        /// Add Sort operation into Select syntax.
        /// </summary>
        /// <param name="sqlSort"></param>
        /// <param name="sortTuple"></param>
        private void SortQuery(Dictionary<string, object> sqlSort,
            Tuple<string, string> sortTuple)
        {
            var validSortOperator = new[] { "ASC", "DESC" };
            sortTuple = validSortOperator.Contains(sortTuple.Item2)
                ? sortTuple
                : new Tuple<string, string>(sortTuple.Item1, "ASC");

            var sortValue = sqlSort["SORT"] as List<Tuple<string, string>>;

            sortValue?.Add(sortTuple);

            sqlSort["SORT"] = sortValue;
        }
        #endregion
    }
}
