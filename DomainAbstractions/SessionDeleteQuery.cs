using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Libraries;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Deletes a selection or all sessions off a SQL based device. The sessions to be deleted (if not all)
    /// can be given through the IDataFlow<DataTable> port. The session list is updated through the ITableDataFlow
    /// port. The process is started by calling the abstraction as an IEvent.
    /// ------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: Starts the deletion process.
    /// 2. IDataFlow<DataTable> sessions: Table of sessions to delete. 
    /// 3. ITableDataFlow sessionList: The full table of sessions.
    /// 4. IRequestResponseDataFlow<string, int> sqlDataFlow: Sends the SQL queries to the device.
    /// 5. IEvent success: Fired when the sessions are successfully deleted. 
    /// 6. IEvent error: Fired when there is an error deleting the sessions.
    /// </summary>
    class SessionDeleteQuery : IEvent, IDataFlow<DataTable>, ITableDataFlow // start, sessions, sessionList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, DataTable> sqlDataFlow;
        private IEvent success;
        private IEvent error;

        // private fields
        private string[] tables = { "WeightTable", "TreatmentTable", "FieldLayoutTable", "SessionTable" };
        private bool deleteAll;
        private DataTable sessionSelection = new DataTable();
        private DataTable outputTable = new DataTable();

        public SessionDeleteQuery(bool deleteAll = false)
        {
            this.deleteAll = deleteAll;
        }

        /// <summary>
        /// Deletes a selection or all sessions off a device.
        /// </summary>
        private async void Delete()
        {
            try
            {
                if (deleteAll) await DeleteAll();
                else await DeleteSelection();
            }
            catch (Exception e)
            {
                Logging.WriteText($"SessionDeleteQuery {InstanceName} FAILED: {e.Message}");
                error?.Execute();
                return;
            }

            success?.Execute();
        }

        /// <summary>
        /// Deletes all the sessions off the device and creates a new default session.
        /// </summary>
        private async Task DeleteAll()
        {
            // clear tables
            foreach (string table in tables)
            {
                await sqlDataFlow.SendRequest($"DELETE FROM `{table}` WHERE 1");
            }

            // clear output datatable
            outputTable.Rows.Clear();
        }

        /// <summary>
        /// Deletes a selection of sessions that are given through the IDataFlow<DataTable> port.
        /// </summary>
        private async Task DeleteSelection()
        {
            foreach (DataRow row in sessionSelection.Rows)
            {
                // delete rows from each table that depends on the session
                // session table should ALWAYS be last as it does not depend on anything
                // being deleted
                foreach (string table in tables)
                {
                    await sqlDataFlow.SendRequest($"DELETE FROM `{table}` WHERE `sessionid` = " + row["index"]);
                }

                // remove the old row from the datatable 
                foreach (DataRow oldRow in outputTable.AsEnumerable().ToList())
                {
                    if (oldRow["index"] == row["index"])
                    {
                        outputTable.Rows.Remove(oldRow);
                        break;
                    }
                }
            }
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