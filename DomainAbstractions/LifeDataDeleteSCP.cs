using System;
using System.Data;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Deletes all animal life data off of a SCP device. The process can be started
    /// by firing the IEvent.
    /// ----------------------------------------------------------------------------
    /// Ports:
    /// IEvent start: Starts the proces of deleting the life data.
    /// ITableDataFlow lifeDataList: The full table of life data.
    /// IArbitrator arbitrator: The request arbitrator.
    /// IRequestResponseDataFlow<string, string> requestResponseDataFlow: Used to send commands to the device.
    /// IEvent success: Fired when the life data is successfully deleted.
    /// IEvent error: Fired when an error occurs while deleting the life data. 
    /// </summary>
    class LifeDataDeleteSCP : IEvent, ITableDataFlow // start, lifeDataList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IArbitrator arbitrator;
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IEvent success;
        private IEvent error;

        // private fields
        private DataTable outputTable = new DataTable();

        /// <summary>
        /// Deletes all animal life data off of a SCP device. The process can be started
        /// by firing the IEvent.
        /// </summary>
        public LifeDataDeleteSCP() { }

        /// <summary>
        /// Deletes the life data off the device.
        /// </summary>
        private async void Delete()
        {
            try
            {
                await arbitrator.Request(InstanceName);

                await requestResponseDataFlow.SendRequest("{FGLD}"); // operate on life data
                await requestResponseDataFlow.SendRequest("{CRV}"); // clear all life data

                arbitrator.Release(InstanceName);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"LifeDataDeleteSCP {InstanceName} ERROR: {e.Message}");
                error?.Execute();
                return;
            }

            success?.Execute();
        }

        // IEvent implementation
        void IEvent.Execute() => Delete();

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