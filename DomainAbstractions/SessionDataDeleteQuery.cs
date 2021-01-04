using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Deletes session data from a table of sessions. The process is started by triggering the
    /// IEvent start. The session table can be updated through the ITableDataFlow sessionList port.
    /// -------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: Starts the deletion of the session data.
    /// 2. IDataFlow<DataTable> sessions: The list of sessions to delete animal information from.
    /// 3. ITableDataFlow sessionList: The session list to update.
    /// 4. IRequestResponseDataFlow<string, DataTable> sqlDataFlow: Sends the SQL queries to the device.
    /// 5. IEvent success: Fired when the animal information is successfully deleted.
    /// 6. IEvent error: Fired when there is an error deleting the animal information.
    /// </summary>
    class SessionDataDeleteQuery : IEvent, IDataFlow<DataTable>, ITableDataFlow // start, sessions, sessionList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, DataTable> sqlDataFlow;
        private IEvent success;
        private IEvent error;

        // private fields
        private DataTable sessionSelection = new DataTable();
        private DataTable outputTable = new DataTable();

        /// <summary>
        /// Deletes session data from a table of sessions. The process is started by triggering the
        /// IEvent start. The session table can be updated through the ITableDataFlow sessionList port.
        /// </summary>
        public SessionDataDeleteQuery() { }

        /// <summary>
        /// Deletes the animal information from the sessions.
        /// </summary>
        private async void Delete()
        {
            try
            {
                foreach (DataRow row in sessionSelection.Rows)
                {
                    await sqlDataFlow.SendRequest($"DELETE FROM `WeightTable` WHERE `sessionid` = " + row["index"]);

                    foreach (DataRow oldRow in outputTable.AsEnumerable().ToList())
                    {
                        if (oldRow["index"] == row["index"])
                        {
                            oldRow["count"] = 0;
                            oldRow["description"] = String.Format("{0}\n{1} ({2} records)", oldRow["name"], oldRow["date"], oldRow["count"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SessionDataDeleteQuery {InstanceName} ERROR: {e.Message}");
                error?.Execute();
                return;
            }

            success?.Execute();
        }

        // IEvent implementation
        void IEvent.Execute() => Delete();

        // IDataFlow<DataTable> implementation
        DataTable IDataFlow<DataTable>.Data { set => sessionSelection = value; }

        // ITableDataFlow implementation
        private bool calledGetHeaderMethod = false;
        DataTable ITableDataFlow.DataTable => outputTable;
        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }

        Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            calledGetHeaderMethod = true; 
            return Task.CompletedTask;
        }

        Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (calledGetHeaderMethod)
            {
                calledGetHeaderMethod = false;
                return Task.FromResult(new Tuple<int, int>(0, outputTable.Rows.Count));
            }
            else
            {
                return Task.FromResult(new Tuple<int, int>(outputTable.Rows.Count, outputTable.Rows.Count));
            }
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync() => Task.CompletedTask;

        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) => Task.CompletedTask;

        bool ITableDataFlow.RequestQuerySupport() => false;
    }
}