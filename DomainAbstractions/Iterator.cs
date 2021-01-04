using ProgrammingParadigms;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Takes a ITableDataFlow as input and when a transaction occurs on this port, it starts iterating through the rows by outputting trigger events
    /// on Transact and receives event to do next after the previous transaction.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow tableToBeIterated: input table that is wanting to be iterated through
    /// 2. IEvent startOrToContinue: event to start or continue the iteration
    /// 3. IDataFlow<DataTable>: currentRowOutput: output data table
    /// 4. IDataFlow<bool>: iteratorRunningOutput: boolean output of whether the iterator has finished iterating
    /// 5. IEvent eventCompleteOutput: input called Next that causes it to go to the next row of the input table.
    /// 6. IDataFlow<string>: indexOutput: string output of the current row index
    /// 
    /// TBD 7. IEventB eventBStopInput: input to stop the iterator before it finishes all the rows. 
    /// </summary>
    public class Iterator : ITableDataFlow, IEvent // tableToBeIterated, startOrToContinue
    {
        // properties -------------------------------------------------------
        public string InstanceName = "Default";

        // outputs -------------------------------------------------------
        
        private IDataFlow<DataTable> currentRowOutput;
        private IDataFlow<bool> iteratorRunningOutput;
        private IEvent eventCompleteOutput;
        private IDataFlow<string> indexOutput;

        // Stop - input (reversal output)
        private IEventB eventBStopInput; //TBD

        /// <summary>
        /// When a ITableDataFlow transaction occurs on this port, it starts iterating through the rows by outputting trigger events
        /// on Transact and receives event to do next after the previous transaction.
        /// </summary>
        public Iterator() { }

        //TBD
        private void PostWiringInitialize()
        {
            //eventBStopInput.EventHappened += () =>
            //{
            //    // stop iterating
            //};
        }

        // IEvent implmentation ------------------------------------------
        // Next - input
        void IEvent.Execute()
        {
            currentRowIndex += 1;

            if (currentRowIndex >= dataTable.Rows.Count)
            {
                // finish iterating, stop and output signals
                currentRowIndex = 0;
                eventCompleteOutput?.Execute();
                if (iteratorRunningOutput != null) iteratorRunningOutput.Data = false;
            }
            else
            {
                Iterating(currentRowIndex);
            }
        }        

        // ITableDataFlow implmentation ------------------------------------------------
        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;
        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation) { throw new NotImplementedException(); }
        Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() { }

        async Task ITableDataFlow.PutPageToDestinationAsync(
            int firstRowIndex, 
            int lastRowIndex, 
            GetNextPageDelegate getNextPage)
        {
            Iterating(currentRowIndex);
        }

        private int currentRowIndex = 0;
        private void Iterating(int rowIndex)
        {
            if (rowIndex >= dataTable.Rows.Count) return;

            if (indexOutput != null) indexOutput.Data = (rowIndex + 1).ToString();
            if (iteratorRunningOutput != null) iteratorRunningOutput.Data = true;

            var dt = dataTable.Copy();
            dt.Rows.Clear();
            dt.ImportRow(dataTable.Rows[rowIndex]);
            if (currentRowOutput != null) currentRowOutput.Data = dt;
        }
    }
}
