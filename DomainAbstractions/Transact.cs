using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// Decorator of ITableDataFlow that does a data transfer, either left or right. From source to destination copying the data in the process.
    /// Matches the column names and copies the data in the header and rows.
    /// If a column does not exist on the destination, then it attempts to create that column.
    /// If columns have different compatible types (e.g. string with any other type) then it tries to convert the data on a row by row basis.
    /// If columns have incompatible types (e.g.date and number) then it does not transfer data, and generates an error.
    /// 
    /// TODO: Matches rows by ID's. TBD What are the rules?
    /// Error messages are output as rows on an Error Port which is a ITableData.
    /// Error messages can be for a whole columns, a whole row, or a specific cell.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: start transacting data from the source DataTable to the destination DataTable
    /// 2. ITableDataFlow tableDataFlowSource: the source data to be transferred
    /// 3. ITableDataFlow tableDataFlowDestination: destination for the DataTable to be transferred to
    /// 4. List<IDataFlow<string>> dataFlowsIndex: fan-out list 
    /// 5. IEvent eventCompleteNoErrors: transaction complete event
    /// 6. IDataFlow<bool> transactCompleteFlag: transaction complete boolean value
    /// 7. IDataFlow<bool> dataFlowTransacting: boolean output of whether the transact is still processing
    /// 8. IEventB cancel: if Transact has started, it listens for another Event from this input port to cancel the transact
    /// </summary>

    public class Transact : IEvent // start
    {
        // properties 
        public string InstanceName = "Default";
        public bool ClearDestination = false;
        public bool AutoLoadNextBatch = false;

        // outputs
        private ITableDataFlow tableDataFlowSource;
        private ITableDataFlow tableDataFlowDestination;
        private List<IDataFlow<string>> dataFlowsIndex = new List<IDataFlow<string>>();
        private IEvent eventCompleteNoErrors;
        private IDataFlow<bool> transactCompleteFlag;
        private IDataFlow<bool> dataFlowTransacting;
        private IEventB cancel;

        // private fields
        private bool transactionInProgress;
        private bool newTransactionPending;
        private bool continueTransact = true;

        /// <summary>
        /// Transact data from source to destination copying the data in the process.
        /// </summary>
        public Transact() { }

        private void PostWiringInitialize()
        {
           if (cancel != null) cancel.EventHappened += Cancel_EventHappened;
        }

        // -----------------------------------------------------------------------------------------
        // IEvent implementation
        void IEvent.Execute()
        {
            if (transactionInProgress)
            {
                newTransactionPending = true;
            }
            else
            {
                var _fireAndForgot = TransactStartTask(tableDataFlowSource, tableDataFlowDestination);
            }
        }


        // private methods --------------------------------------------------------------------------
        private async Task TransactStartTask(ITableDataFlow source, ITableDataFlow destination)
        {
            transactionInProgress = true;
            
            if (ClearDestination)
            {
                destination.DataTable.Rows.Clear();
                destination.DataTable.Columns.Clear();
                //await destination.PutHeaderToDestinationAsync();
            }

            await source.GetHeadersFromSourceAsync();

            // transact column headers and meta datas
            destination.DataTable.TableName = source.DataTable.TableName;
            destination.CurrentRow = source.CurrentRow;
            foreach (DataColumn c in source.DataTable.Columns)
            {
                if (destination.DataTable.Columns.Contains(c.ColumnName))
                {
                    destination.DataTable.Columns.Remove(c.ColumnName);
                }
                else
                {
                    destination.DataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }
            }

            await destination.PutHeaderToDestinationAsync();

            if (destination.DataTable.Columns.Count > 0)
            {
                continueTransact = true; // Reset after any previous cancel event was sent and resolved
                await TransferOnePageTask(source, destination);
            }

            transactionInProgress = false;
        }


        // continuation task below
        private async Task TransferOnePageTask(ITableDataFlow source, ITableDataFlow destination)
        {
            // This function will move all the data if the instance configuration, AutoLoadNextBatch is true, other wise only one batch


            transactionInProgress = true;
            try
            {
                do
                {
                    Tuple<int, int> tuple = await source.GetPageFromSourceAsync();

                    if (newTransactionPending)
                    {
                        newTransactionPending = false;
                        await TransactStartTask(tableDataFlowSource, tableDataFlowDestination);
                    }
                    else
                    {
                        if (tuple.Item1 < tuple.Item2)  // we actually got some data
                        {
                            TransactOnePage(source, destination, tuple.Item1, tuple.Item2);

                            if (AutoLoadNextBatch)
                            {
                                //if (tuple.Item1 >= tuple.Item2) break;
                                // {
                                // await TransferOnePageTask(source, destination);
                                // }
                            }
                            else
                            {
                                // notify transaction
                                await destination.PutPageToDestinationAsync(tuple.Item1, tuple.Item2, async () =>
                                {
                                    if (tuple.Item1 < tuple.Item2)
                                    {
                                        // KL: bug of unplugging device while loading session data
                                        await TransferOnePageTask(source, destination);
                                    }
                                });
                            }
                        }
                        else
                        {
                            transactionInProgress = false;
                            await destination.PutPageToDestinationAsync(AutoLoadNextBatch ? 0 : tuple.Item1, tuple.Item2, null);

                            if (transactCompleteFlag != null) transactCompleteFlag.Data = true;

                            // source.DataTable.LogDataChange($"{(InstanceName != "Default" ? InstanceName : "(No instance name) transact")} source DataTable");
                            destination.DataTable.LogDataChange($"{(InstanceName != "Default" ? InstanceName : "(No instance name) transact")} destination DataTable");
                            eventCompleteNoErrors?.Execute();
                            if (dataFlowTransacting != null) dataFlowTransacting.Data = false;
                            break;
                        }
                    }

                    transactionInProgress = false;
                }
                while (AutoLoadNextBatch && continueTransact);
            }
            catch (TaskCanceledException tsc) // KL: Added this to hopefully catch the Task Cancelled from within e.g. PutPageToDestinationAsync using SCP commands HOWEVER needs further testing as may not work
            {
                System.Diagnostics.Debug.WriteLine($"Transact {InstanceName} caught TaskCancelled. Stopping transact.");
                Cancel_EventHappened();
            }
        }

        private void Cancel_EventHappened()
        {
            continueTransact = false;
            tableDataFlowDestination.DataTable.Clear(); // This will always clear on e.g. a button press, but what if we just want to stop an append?
        }


        // start to transact the data from source to destination 
        // firstly trasact the headers, then the columns
        // the destination will be filled with the data that it does not have in the source
        private void TransactOnePage(ITableDataFlow source, ITableDataFlow destination, int firstRowIndex, int lastRowIndex)
        {
            var dtSource = source.DataTable;
            var dtDestination = destination.DataTable;

            // --------------------------------------------
            // transact rows
            for (int i = firstRowIndex; i < lastRowIndex; i++)
            {
                dtDestination.ImportRow(dtSource.Rows[i]);
                // the reason of using i+1 is the index of a table starts from 0, the user should see 1 when it's 0.
                foreach (var d in dataFlowsIndex) d.Data = (i+1).ToString();
            }
        }
    }
}
