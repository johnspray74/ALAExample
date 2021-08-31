using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Foundation;
using System.Threading;

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
    /// 2. IDataFlow<bool> clearDestination: whether to clear destination before transferring
    /// 2. ITableDataFlow tableDataFlowSource: the source data to be transferred
    /// 3. ITableDataFlow tableDataFlowDestination: destination for the DataTable to be transferred to
    /// 4. List<IDataFlow<string>> dataFlowsIndex: fan-out list 
    /// 5. IEvent eventCompleteNoErrors: transaction complete event
    /// 6. IDataFlow<bool> transactCompleteFlag: transaction complete boolean value
    /// 7. IDataFlow<bool> dataFlowTransacting: boolean output of whether the transact is still processing
    /// 8. IEventB cancel: if Transact has started, it listens for another Event from this input port to cancel the transact
    /// </summary>
    public class Transfer : IEvent, IDataFlow<bool> // start, clearDestination
    {
        // properties 
        public string InstanceName = "Default";
        public bool ClearDestination = false;
        public bool AutoLoadNextBatch = false;
        public string MergeKey = null;

        // ports
        private ITableDataFlow tableDataFlowSource;
        private ITableDataFlow tableDataFlowDestination;
        private List<IDataFlow<string>> dataFlowsIndex = new List<IDataFlow<string>>();
        private IEvent eventCompleteNoErrors;
        private IEvent eventFailed;
        private IDataFlow<bool> transactCompleteFlag;
        private IDataFlow<bool> dataFlowTransacting;
        private IDataFlow<string> errorString;
        private IEventB cancel;

        // private fields
        private bool transactionInProgress;
        private bool newTransactionPending;
        private CancellationTokenSource cancelSource;

        /// <summary>
        /// Transact data from source to destination copying the data in the process.
        /// </summary>
        public Transfer() { }


        // This method is called by WireTo after it wires something to the port "cancel".
        private void cancelInitialize()
        {
            cancel.EventHappened += Cancel_EventHappened;
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

            await source.GetHeadersFromSourceAsync();

            // transact column headers and meta datas
            destination.DataTable.TableName = source.DataTable.TableName;
            destination.CurrentRow = source.CurrentRow;

            // clear destination - remove all rows and columns, reaad
            if (ClearDestination)
            {
                destination.DataTable.Rows.Clear();
                destination.DataTable.Columns.Clear();

                foreach (DataColumn c in source.DataTable.Columns)
                {
                    destination.DataTable.Columns.Add(new DataColumn(c.ColumnName)
                    {
                        Prefix = c.Prefix,
                        DataType = c.DataType
                    });
                }
            }
            // only readd columns that don't already exist
            else
            {
                foreach (DataColumn c in source.DataTable.Columns)
                {
                    if (! destination.DataTable.Columns.Contains(c.ColumnName))
                    {
                        destination.DataTable.Columns.Add(new DataColumn(c.ColumnName)
                        {
                            Prefix = c.Prefix,
                            DataType = c.DataType
                        });
                    }
                }
            }

            await destination.PutHeaderToDestinationAsync();

            if (destination.DataTable.Columns.Count > 0)
            {
                cancelSource = new CancellationTokenSource();
                CancellationToken token = cancelSource.Token;

                try
                {
                    await TransferOnePageTask(source, destination, token);
                }
                catch (Exception e)
                {
                    HandleTransactException(e);
                }
            }

            transactionInProgress = false;
        }

        private void HandleTransactException(Exception e)
        {
            // ensure we call this on the main thread
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (!(e is OperationCanceledException || e is ObjectDisposedException))
                {
                    diagnosticOutput?.Invoke($"Exception caught during transact: {e.Message}");
                    
                    if (eventFailed != null)
                    {
                        eventFailed.Execute();
                    }
                    else
                    {
                        throw e;
                    }

                    if (errorString != null) errorString.Data = e.Message;
                    Cancel_EventHappened();
                }
            });
        }


        // continuation task below
        private async Task TransferOnePageTask(ITableDataFlow source, ITableDataFlow destination, CancellationToken cancellationToken)
        {
            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                Tuple<int, int> tuple = await source.GetPageFromSourceAsync();

                // restart transaction
                if (newTransactionPending)
                {
                    newTransactionPending = false;
                    await TransactStartTask(source, destination);
                }
                else
                {
                    if (tuple.Item1 < tuple.Item2)  // we actually got some data
                    {
                        TransactOnePage(source, destination, tuple.Item1, tuple.Item2, cancellationToken);

                        if (!AutoLoadNextBatch)
                        {
                            // notify transaction
                            await destination.PutPageToDestinationAsync(tuple.Item1, tuple.Item2, () =>
                            {
                                if (tuple.Item1 < tuple.Item2)
                                {
                                    // KL: bug of unplugging device while loading session data
                                    var _ = TransferOnePageTask(source, destination, cancellationToken);
                                    _.ContinueWith((t) => HandleTransactException(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                                }
                            });
                        }
                    }
                    else
                    {
                        transactionInProgress = false;
                        await destination.PutPageToDestinationAsync(AutoLoadNextBatch ? 0 : tuple.Item1, tuple.Item2, null);

                        eventCompleteNoErrors?.Execute();
                        if (transactCompleteFlag != null) transactCompleteFlag.Data = true;
                        if (dataFlowTransacting != null) dataFlowTransacting.Data = false;
                        break;
                    }
                }

                transactionInProgress = false;
            }
            while (AutoLoadNextBatch);
        }

        private void Cancel_EventHappened()
        {
            if (cancelSource == null) return;
            cancelSource.Cancel();
        }


        // start to transact the data from source to destination 
        // firstly trasact the headers, then the columns
        // the destination will be filled with the data that it does not have in the source
        private void TransactOnePage(ITableDataFlow source, ITableDataFlow destination, int firstRowIndex, int lastRowIndex, CancellationToken token)
        {
            var dtSource = source.DataTable;
            var dtDestination = destination.DataTable;

            // --------------------------------------------
            // transact rows
            for (int i = firstRowIndex; i < lastRowIndex; i++)
            {
                token.ThrowIfCancellationRequested();
                dtDestination.ImportRow(dtSource.Rows[i]);

                // the reason of using i+1 is the index of a table starts from 0, the user should see 1 when it's 0.
                foreach (var d in dataFlowsIndex) d.Data = (i+1).ToString();
            }

            // if merge key is set - remove any duplicates, prioritizing rows with a higher index
            // the "MergeKey" will be checked in each row and compared - if they are equal, remove
            if (!String.IsNullOrEmpty(MergeKey))
            {
                for (int i = 0; i < dtDestination.Rows.Count; i++)
                {
                    for (int j = i + 1; j < dtDestination.Rows.Count; j++)
                    {
                        if (dtDestination.Rows[i][MergeKey].Equals(dtDestination.Rows[j][MergeKey]))
                        {
                            dtDestination.Rows.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
        
        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => ClearDestination = value; }





        public delegate void DiagnosticOutputDelegate(string output);
        private static DiagnosticOutputDelegate diagnosticOutput;
        public static DiagnosticOutputDelegate DiagnosticOutput { get => diagnosticOutput; set => diagnosticOutput = value; }
    }
}
