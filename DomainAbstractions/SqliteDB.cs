using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DomainAbstractions;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// SqliteDB is used to work together with QueryBuilder, receive string format query command, execute query command at database side, return DataTable result, or affected rows.
    /// Currently, SqliteDB can work in QueryDatabase mode or ExecuteNonQueryDatabase mode.
    /// In order to accept JSON format query operations and combine with QueryBuilder, SqliteDbVer2 is under creating.
    /// 
    /// TODO: further need implementation to support Rollback
    /// ------------------------------------------------------------------------------------------------------------------
    /// Input ports:
    /// 1. IDataFlow<string> dbFilePath: database file path, used to builder SQLiteConnection with sqlite database.
    /// 2. IRequestResponseDataFlow<string, DataTable> queryReturnRequest: query request, return DataTable
    /// 3. IRequestResponseDataFlow<string, int> : ExecuteNonQuery request, return affected numbers.
    /// 4. IRequestResponseDataFlow<string, SQLiteTransaction> dbTransactRequest: Database transact request.
    /// 5. IRequestResponseDataFlow<Dictionary<string, object>, string> queryBuilderRequest: request repsonse for building the query command to SQLite
    /// </summary>
    public class SqliteDB : IDataFlow<string>, ITableDataFlow, 
        IRequestResponseDataFlow_B<string, DataTable>, 
        IRequestResponseDataFlow_B<string, int>, 
        IRequestResponseDataFlow_B<string, SQLiteTransaction> // dbFilePath, queryReturnRequest, , dbTransactRequest
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<Dictionary<string, object>, string> queryBuilderRequest;

        // private fields
        private string sqliteConnection;
        private SQLiteConnection conn;
        private SQLiteCommand command;
        private SQLiteDataAdapter adapter;
        private DataTable dataTable;

        // constructor
        /// <summary>
        ///  Build connection to a SQLite database, operate with the database and return result as a DataTable.
        /// </summary>
        public SqliteDB()
        {
        }

        // implementation of IDataFlow<string>
        string IDataFlow<string>.Data
        {
            set
            {
                if (!String.IsNullOrEmpty(value)) sqliteConnection = $"Data Source={value}";

                // else throw new ArgumentException("Database path is null or empty.");
            }
        }

        // implementation of IRequestResponseDataFlow<string, DataTable>
        async Task<DataTable> IRequestResponseDataFlow_B<string, DataTable>.SendRequest(string queryComm)
        {
            DataTable queryDataTable = QueryDatabase(sqliteConnection, queryComm);
            return queryDataTable;
        }

        // implementation of IRequestResponseDataFlow<string, int>
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

        // Build connection to database and query data.
        private DataTable QueryDatabase(string connString, string query)
        {
            DataTable resultDataTable = new DataTable();

            if (!string.IsNullOrEmpty(connString) && !string.IsNullOrEmpty(query))
            {
                conn = new SQLiteConnection(connString);
                command = new SQLiteCommand(query, conn);
                adapter = new SQLiteDataAdapter(command);
                System.Diagnostics.Debug.WriteLine("Query in SqliteDB using: \n **********QUERY COMM********** \n" + query + "******************************");


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
                        System.Diagnostics.Debug.WriteLine("INSERT in SqliteDB using: \n **********QUERY COMM********** \n" + executeQuery + "******************************");

                        adapter.InsertCommand = command;
                        affectedRows = adapter.InsertCommand.ExecuteNonQuery();
                    }
                    else if (executeQuery.StartsWith("UPDATE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine("UPDATE in SqliteDB using: \n **********QUERY COMM********** \n" + executeQuery + "******************************");
                        adapter.UpdateCommand = command;
                        affectedRows = adapter.UpdateCommand.ExecuteNonQuery();
                    }
                    else if(executeQuery.StartsWith("DELETE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine("DELETE in SqliteDB using: \n **********QUERY COMM********** \n" + executeQuery + "******************************");
                        adapter.DeleteCommand = command;
                        affectedRows = adapter.DeleteCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("EXECUTE NON QUERY in SqliteDB using: \n **********QUERY COMM********** \n" + executeQuery + "******************************");

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

        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            var tmpOperation = queryOperation as Dictionary<string, object>;
            
            var queryComm = await queryBuilderRequest.SendRequest(tmpOperation);
            
            dataTable = QueryDatabase(sqliteConnection, queryComm);

            // var queryContainer = (JObject) queryOperation;
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            return new Tuple<int, int>(0, dataTable.Rows.Count);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) { throw new NotImplementedException(); }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }

        bool ITableDataFlow.RequestQuerySupport()
        {
            // the end point of query support
            return true;
        }
    }
}
