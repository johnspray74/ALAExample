using System;
using System.Data;
using System.Threading.Tasks;
using Libraries;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Deletes all alerts off of an XRS2 device through SCP.
    /// Process is started by triggering the IEvent. The alert session list
    /// can be updated through the ITableDataFlow alertList and outputTableFlow ports.
    /// ------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: Starts the deletion process.
    /// 2. IDataFlow<DataTable> alertList: Table of sessions to delete. 
    /// 3. IArbitrator arbitrator: Arbitrator for the request response data flow commands.
    /// 4. IRequestResponseDataFlow<string, string> requestResponseDataFlow: Sends the SCP commands to the device.
    /// 5. IEvent success: Fired when the sessions are successfully deleted. 
    /// 6. IEvent error: Fired when there is an error deleting the sessions.
    /// </summary>
    class AlertDeleteSCP : IEvent, ITableDataFlow // start, alertList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IArbitrator arbitrator;
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IEvent success;
        private IEvent error;

        // private fields
        private DataTable alertTable = new DataTable();

        /// <summary>
        /// Deletes all alerts off of an XRS2 device through SCP.
        /// Process is started by triggering the IEvent. The alert session list
        /// can be updated through the ITableDataFlow alertList and outputTableFlow ports.
        /// </summary>
        public AlertDeleteSCP() { }

        /// <summary>
        /// Deletes the alert data off the device.
        /// </summary>
        private async void Delete()
        {
            try
            {
                await arbitrator.Request(InstanceName);
                
                await requestResponseDataFlow.SendRequest("{FGLD}"); // operate on life data
                await requestResponseDataFlow.SendRequest("{CRA}"); // clear all alerts
                alertTable.Rows.Clear(); // clear alert table

                arbitrator.Release(InstanceName);
            }
            catch (Exception e)
            {
                Logging.WriteText($"AlertDeleteSCP {InstanceName} FAILED: {e.Message}");
                error?.Execute();
                return;
            }

            success?.Execute();
        }

        // IEvent implementation
        void IEvent.Execute() => Delete();

        // ITableDataFlow implementation
        private bool calledGetHeaderMethod = false;
        DataTable ITableDataFlow.DataTable => alertTable;
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
                return Task.FromResult(new Tuple<int, int>(0, alertTable.Rows.Count));
            }
            else
            {
                return Task.FromResult(new Tuple<int, int>(alertTable.Rows.Count, alertTable.Rows.Count));
            }
        }
        

        Task ITableDataFlow.PutHeaderToDestinationAsync() => Task.CompletedTask;

        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) => Task.CompletedTask;

        bool ITableDataFlow.RequestQuerySupport() => false;
    }
}