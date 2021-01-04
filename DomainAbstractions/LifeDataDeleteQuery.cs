using System;
using System.Data;
using System.Threading.Tasks;
using Libraries;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Deletes all of the animal life data off a SQL based device. The output table can be updated
    /// through the ITableDataFlow lifeDataList port. The process is started with the IEvent start event.
    /// --------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: Starts the process of deleting the life data.
    /// 2. ITableDataFlow lifeDataList: The full table of life data. 
    /// 3. IRequestResponseDataFlow<string, DataTable> sqlDataFlow: Sends the SQL queries to the device.
    /// 4. IEvent success: Fired when the life data is successfully deleted.
    /// 5. IEvent error: Fired when there is an error deleting the life data.
    /// </summary>
    class LifeDataDeleteQuery : IEvent, ITableDataFlow // start, lifeDataList
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, DataTable> sqlDataFlow;
        private IEvent success;
        private IEvent error;

        // private fields
        private DataTable outputTable = new DataTable();

        /// <summary>
        /// Deletes all of the animal life data off a SQL based device. The output table can be updated
        /// through the ITableDataFlow lifeDataList port. The process is started with the IEvent start event.
        /// </summary>
        public LifeDataDeleteQuery() { }

        /// <summary>
        /// Deletes the life data.
        /// </summary>
        private async void Delete()
        {
            try
            {
                await sqlDataFlow.SendRequest("DELETE FROM `AnimalTable` WHERE 1"); // clear animal table
                outputTable.Rows.Clear(); // clear life data grid
            }
            catch (Exception e)
            {
                Logging.WriteText($"LifeDataDeleteQuery {InstanceName} FAILED: {e.Message}");
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