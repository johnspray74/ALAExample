using Libraries;
using ProgrammingParadigms;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Deletes a selection or all sessions off an SCP device. The sessions to be deleted (if not all)
    /// can be given through the IDataFlow<DataTable> port. The session list is updated through the ITableDataFlow
    /// port. The process is started by calling the abstraction as an IEvent.
    /// ------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: Starts the deletion process.
    /// 2. IDataFlow<DataTable> sessions: Table of sessions to delete. 
    /// 3. ITableDataFlow sessionList: The full table of sessions.
    /// 4. IArbitrator arbitrator: Arbitrator for the request response data flow commands.
    /// 5. IRequestResponseDataFlow<string, string> requestResponseDataFlow: Sends the SCP commands to the device.
    /// 6. IEvent success: Fired when the sessions are successfully deleted. 
    /// 7. IEvent error: Fired when there is an error deleting the sessions.
    /// </summary>
    class SessionDeleteSCP : IEvent, IDataFlow<DataTable>, ITableDataFlow // start, sessions, sessionList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IArbitrator arbitrator;
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IEvent success;
        private IEvent error;

        // private fields
        private bool deleteAll;
        private DataTable dataTable = new DataTable();
        private DataTable outputTable = new DataTable();

        /// <summary>
        /// Deletes a selection or all sessions off an SCP device. The sessions to be deleted (if not all)
        /// can be given through the IDataFlow<DataTable> port. The session list is updated through the ITableDataFlow
        /// port. The process is started by calling the abstraction as an IEvent.
        /// </summary>
        /// <param name="deleteAll">Whether all sessions should be deleted.</param>
        public SessionDeleteSCP(bool deleteAll = false)
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
                await arbitrator.Request(InstanceName);

                if (deleteAll) await DeleteAll();
                else await DeleteSelection();

                arbitrator.Release(InstanceName);
            }
            catch (Exception e)
            {
                Logging.WriteText($"SessionDeleteSCP {InstanceName} FAILED: {e.Message}");
                error?.Execute();
                return;
            }

            success?.Execute();
        }

        /// <summary>
        /// Deletes all the sessions AND cross references off the device and creates a new default session.
        /// </summary>
        private async Task DeleteAll()
        {
            await requestResponseDataFlow.SendRequest("{FGDD}"); // select session data
            await requestResponseDataFlow.SendRequest("{CL}"); // clear session data
            await requestResponseDataFlow.SendRequest("{FF0}"); // create and go to new empty session

            outputTable.Clear(); // clear datatable

            // add new row for the recently created empty session
            string name = default, date = default, count = default;
            
            // retrieve name, date and count of new session
            // even though we technically know both date and count
            try
            {
                name = await requestResponseDataFlow.SendRequest("{FPNA0}");
                date = await requestResponseDataFlow.SendRequest("{FPDA0}");
                count = await requestResponseDataFlow.SendRequest("{FPNR0}");
            }
            catch (TaskCanceledException)
            {}

            DataRow row = outputTable.NewRow();

            row["name"] = name;
            row["checkbox"] = false;
            row["index"] = 0;
            row["count"] = count;
            row["date"] = date;
            row["description"] = name + "\n" + date + " (" + count + " records)";

            outputTable.Rows.Add(row);
        }

        /// <summary>
        /// Deletes a selection of sessions that are given through the IDataFlow<DataTable> port.
        /// </summary>
        private async Task DeleteSelection()
        {
            await requestResponseDataFlow.SendRequest("{FGDD}"); // operate on session data

            foreach (DataRow row in dataTable.Rows)
            {
                await requestResponseDataFlow.SendRequest("{FF" + row["index"] + "}"); // select session
                await requestResponseDataFlow.SendRequest("{FC}"); // clear currently selected session
                await requestResponseDataFlow.SendRequest("{FPNA" + row["index"] + ", }"); // remove session by setting name to null

                // remove session from output datatable
                foreach (DataRow session in outputTable.AsEnumerable().ToList())
                {
                    if (session["index"] == row["index"])
                    {
                        outputTable.Rows.Remove(session);
                    }
                }
            }
        }

        // IEvent implementation
        void IEvent.Execute() => Delete();
        
        // IDataFlow<DataTable> implementation
        DataTable IDataFlow<DataTable>.Data { set => dataTable = value; }

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
