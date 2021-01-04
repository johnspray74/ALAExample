using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// SqliteDbVer2 is under creating, for the purpose of accept JSON format query operations, combine old SqliteDB and QueryBuilder together.
    /// SqliteDbVer2 is supposed to accept JSON format query operations, work with SelectColumns, From, FilterRows, Join, Sort abstractions.
    /// Structure for JSON format query container, please refer to Xmind diagram in .\Datamars_AMDA\AnimalManagementDesktopApp\DataLink_ALA\Application\Diagrams\JSON Format Query Container Structure.xmind
    /// JSON query container will be modified and passed by GetHeaderFromSource(queryContainer) one by one until sqlite  database, get datatable result and return one by one until Transact.
    /// TODO: UPDATE, INSERT and ALTER TBD
    /// </summary>
    public class SqliteDbVer2 : IDataFlow<string>, ITableDataFlow, 
        IRequestResponseDataFlow_B<string, DataTable>,
        IRequestResponseDataFlow_B<string, int>,
        IRequestResponseDataFlow_B<string, SQLiteTransaction>
    {
        // properties
        public string InstanceName = "Default";

        // ports


        // private fields
        private DataTable dataTable;
        private int currentIndex = 0;

        private string sqliteConnection;
        private SQLiteConnection conn;
        private SQLiteCommand command;
        private SQLiteDataAdapter adapter;

        private JObject queryContainer = new JObject();
        private string selectQuery;
        private string insertQuery;
        private string updateQuery;
        private string alterQuery;

        // ctor
        public SqliteDbVer2() { }

        public SqliteDbVer2(string dbPath)
        {
            if (!string.IsNullOrEmpty(dbPath)) sqliteConnection = $"Data Source={dbPath}";
        }
        
        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                if (!String.IsNullOrEmpty(value)) sqliteConnection = $"Data Source={value}";
            }
        }
        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            if (queryOperation != null) queryContainer = queryOperation as JObject;

            if (queryContainer.ContainsKey("SELECT"))
            {
                var rawQuery = SelectQueryBuilder(queryContainer);
                selectQuery = Regex.Replace(rawQuery, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                System.Diagnostics.Debug.WriteLine("Query in SqliteDbVer2 using: \n **********QUERY COMM********** \n" + selectQuery + "******************************");
                dataTable = QueryDatabase(sqliteConnection, selectQuery);
                currentIndex = 0;
            }

            if (queryContainer.ContainsKey("INSERT")) insertQuery = InsertQueryBuilder(queryContainer);

            if (queryContainer.ContainsKey("UPDATE")) updateQuery = UpdateQueryBuilder(queryContainer);

            if (queryContainer.ContainsKey("ALTER")) alterQuery = AlterQueryBuilder(queryContainer);

        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            var tmpIndex = currentIndex;

            if (currentIndex < dataTable.Rows.Count)
            {
                currentIndex = dataTable.Rows.Count;
            }
            
            return new Tuple<int, int>(tmpIndex, dataTable.Rows.Count);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() => true;

        //IRequestResponseDataFlow<string, DataTable> implementation
        async Task<DataTable> IRequestResponseDataFlow_B<string, DataTable>.SendRequest(string queryComm)
        {
            DataTable queryDataTable = QueryDatabase(sqliteConnection, queryComm);
            return queryDataTable;
        }
        // IRequestResponseDataFlow<string, int> implementation
        async Task<int> IRequestResponseDataFlow_B<string, int>.SendRequest(string executeComm)
        {
            int affectedRows = ExecuteNonQueryDatabase(sqliteConnection, executeComm);
            return affectedRows;
        }

        // implementation of IRequestResponseDataFlow<string, SQLiteTransaction>
        async Task<SQLiteTransaction> IRequestResponseDataFlow_B<string, SQLiteTransaction>.SendRequest(string requestTransaction)
        {
            return CreateNewTransaction(sqliteConnection);
        }

        // private methods
        

        // Build connection to database and query data.
        private DataTable QueryDatabase(string connString, string query)
        {
            DataTable resultDataTable = new DataTable();

            if (!string.IsNullOrEmpty(connString) && !string.IsNullOrEmpty(query))
            {
                conn = new SQLiteConnection(connString);
                command = new SQLiteCommand(query, conn);
                adapter = new SQLiteDataAdapter(command);

                try
                {
                    using (conn)
                    {
                        using (command)
                        {
                            using (adapter)
                            {
                                adapter.Fill(resultDataTable);
                            }
                        }
                    }
                }
                catch (SQLiteException se)
                {
                    System.Diagnostics.Debug.WriteLine("Sqlite database query error: " + se.Message + "\n");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            return resultDataTable;

            #region Old db operation
            // if (!string.IsNullOrEmpty(connString))
            // {
            //     conn = new SQLiteConnection(connString);
            //     using (conn)
            //     {
            //         try
            //         {
            //             System.Diagnostics.Debug.WriteLine($"==========> Try to query from database using: \n{query}");
            //             using var command = new SQLiteCommand(query, conn);
            //             using var adapter = new SQLiteDataAdapter(command);
            //             // using SQLiteCommandBuilder command = new SQLiteCommandBuilder(adapter);
            //             adapter.Fill(resultDataTable);
            //         }
            //         catch (SQLiteException ex)
            //         {
            //             System.Diagnostics.Debug.WriteLine("Sqlite database query error: " + ex.Message + "\n");
            //         }
            //     }
            // }
            #endregion
        }

        // build connection to database and do ExecuteNonQuery
        private int ExecuteNonQueryDatabase(string connString, string executeQuery)
        {
            var affectedRows = 0;

            if (!string.IsNullOrEmpty(connString) && !string.IsNullOrEmpty(executeQuery))
            {
                conn = new SQLiteConnection(connString);
                command = new SQLiteCommand(executeQuery, conn);
                adapter = new SQLiteDataAdapter();

                conn.Open();

                try
                {

                    if (executeQuery.StartsWith("INSERT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        adapter.InsertCommand = command;
                        affectedRows = adapter.InsertCommand.ExecuteNonQuery();
                    }
                    else if (executeQuery.StartsWith("UPDATE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        adapter.UpdateCommand = command;
                        affectedRows = adapter.UpdateCommand.ExecuteNonQuery();
                    }
                    else if (executeQuery.StartsWith("DELETE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        adapter.DeleteCommand = command;
                        affectedRows = adapter.DeleteCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        adapter.SelectCommand = command;
                        affectedRows = adapter.SelectCommand.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    adapter.Dispose();
                    command.Dispose();
                    conn.Dispose();
                }
            }

            return affectedRows;
        }

        // create new transaction and return
        private SQLiteTransaction CreateNewTransaction(string connString)
        {
            SQLiteTransaction transaction = null;

            if (!string.IsNullOrEmpty(connString))
            {
                conn = new SQLiteConnection(connString);

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    transaction = conn.BeginTransaction();
                }
                catch (SQLiteException ex)
                {
                    if (ex.ErrorCode == 10)
                    {
                        System.Diagnostics.Debug.WriteLine("DB file is not present (" + ex.Message + ")");
                    }
                    else
                    {
                        throw (ex);
                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            return transaction;
        }

        private string SelectQueryBuilder(JObject container)
        {
            var selectBuilder = new StringBuilder();
            var columns = container["SELECT"]["Columns"]?.Values<string>().ToList();
            var isDistinct = container["SELECT"]["Distinct"]?.Value<bool>();
            var table = container["SELECT"]["From"]?.Value<string>();
            var filter = (JObject) container["SELECT"]["Filter"];
            var join = (JObject) container["SELECT"]["Join"];
            var sort = (JObject) container["SELECT"]["Sort"];
            // TODO: complete all syntax support when implement new abstractions.

            selectBuilder.AppendLine("SELECT");
            selectBuilder.AppendLine(isDistinct == null ? string.Empty : "DISTINCT");
            selectBuilder.AppendLine(columns.Any() ? string.Join(", ", columns) : "*");
            selectBuilder.AppendLine("FROM");
            selectBuilder.AppendLine(table ?? string.Empty);
            if (join != null) selectBuilder.AppendLine(CompileJoin(join));
            if (filter != null) selectBuilder.AppendLine(CompileFilter(filter));
            if (sort != null) selectBuilder.AppendLine(CompileSort(sort));

            return selectBuilder.ToString();
        }

        private string InsertQueryBuilder(JObject container)
        {
            return "";
        }

        private string UpdateQueryBuilder(JObject container)
        {
            return "";
        }

        private string AlterQueryBuilder(JObject container)
        {
            return "";
        }

        private string CompileFilter(JObject filterContainer)
        {
            var filterBuilder = new StringBuilder();

            var andArray = (JArray) filterContainer["AND"];
            var orArray = (JArray) filterContainer["OR"];

            var andConditions = new List<string>();
            var orConditions = new List<string>();

            foreach (var jToken in andArray)
            {
                var andCond = (JObject) jToken;
                var col = andCond["Column"].Value<string>();
                var op = andCond["Operator"].Value<string>();
                string andValue;

                var valueToken = andCond["Value"];

                // TODO: this should accept nested query as well

                if (valueToken is JObject)
                {
                    andValue = SelectQueryBuilder((JObject) valueToken);
                }
                else
                {
                    andValue = andCond["Value"].Value<string>();
                }

                andConditions.Add($"{col} {op} {andValue}");
            }

            foreach (var jToken in orArray)
            {
                var orCond = (JObject) jToken;
                var col = orCond["Column"].Value<string>();
                var op = orCond["Operator"].Value<string>();
                string orValue;

                var valueToken = orCond["Value"];

                // TODO: this should accept nested query as well

                if (valueToken is JObject)
                {
                    orValue = $"({SelectQueryBuilder((JObject) valueToken)})";
                }
                else
                {
                    orValue = orCond["Value"].Value<string>();
                }

                orConditions.Add($"{col} {op} {orValue}");
            }

            if (andConditions.Any() && orConditions.Any())
            {
                filterBuilder.AppendLine("WHERE");
                filterBuilder.AppendLine(string.Join(" AND \r\n", andConditions));
                filterBuilder.AppendLine("AND");
                filterBuilder.AppendLine(string.Join(" OR \r\n", orConditions));
            }
            if (andConditions.Any() && !orConditions.Any())
            {
                filterBuilder.AppendLine("WHERE");
                filterBuilder.AppendLine(string.Join(" AND \r\n", andConditions));
            }
            if (!andConditions.Any() && orConditions.Any())
            {
                filterBuilder.AppendLine("WHERE");
                filterBuilder.AppendLine(string.Join(" OR \r\n", orConditions));
            }

            return filterBuilder.ToString();
        }

        private string CompileJoin(JObject joinContainer)
        {
            var joinBuilder = new StringBuilder();

            var leftJoin = (JArray) joinContainer["LEFTJOIN"];
            var rightJoin = (JArray) joinContainer["RIGHTJOIN"];
            var outerJoin = (JArray) joinContainer["OUTERJOIN"];
            var innerJoin = (JArray) joinContainer["INNERJOIN"];

            if (leftJoin.Any())
            {
                foreach (JToken jToken in leftJoin)
                {
                    var joinCond = (JObject) jToken;
                    joinBuilder.AppendLine("LEFT JOIN");
                    joinBuilder.AppendLine($"{joinCond["JoinTable"].Value<string>()} ON " +
                                           $"{joinCond["JoinCondition"].Value<string>()}");
                }
            }
            if (rightJoin.Any())
            {
                foreach (JToken jToken in rightJoin)
                {
                    var joinCond = (JObject)jToken;
                    joinBuilder.AppendLine("RIGHT JOIN");
                    joinBuilder.AppendLine($"{joinCond["JoinTable"].Value<string>()} ON " +
                                           $"{joinCond["JoinCondition"].Value<string>()}");
                }
            }
            if (outerJoin.Any())
            {
                foreach (JToken jToken in outerJoin)
                {
                    var joinCond = (JObject)jToken;
                    joinBuilder.AppendLine("OUTER JOIN");
                    joinBuilder.AppendLine($"{joinCond["JoinTable"].Value<string>()} ON " +
                                           $"{joinCond["JoinCondition"].Value<string>()}");
                }
            }
            if (innerJoin.Any())
            {
                foreach (JToken jToken in innerJoin)
                {
                    var joinCond = (JObject)jToken;
                    joinBuilder.AppendLine("INNER JOIN");
                    joinBuilder.AppendLine($"{joinCond["JoinTable"].Value<string>()} ON " +
                                           $"{joinCond["JoinCondition"].Value<string>()}");
                }
            }

            return joinBuilder.ToString();
        }

        private string CompileSort(JObject sortContainer)
        {
            var sortBuilder = new StringBuilder();
            var sortCond = new List<string>();

            var ascSort = ((JValue) sortContainer["ASC"]).Value<string>();
            var descSort = ((JValue) sortContainer["DESC"]).Value<string>();

            if (!string.IsNullOrEmpty(ascSort)) sortCond.Add(ascSort + " ASC");
            if (!string.IsNullOrEmpty(descSort)) sortCond.Add(descSort + " DESC");

            var orderCond = string.Join(", ", sortCond);

            if (!string.IsNullOrEmpty(orderCond))
            {
                sortBuilder.AppendLine("ORDER BY");
                sortBuilder.AppendLine(orderCond);
            }

            return sortBuilder.ToString();
        }
    }
}
