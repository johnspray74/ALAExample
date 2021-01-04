using System;
using System.Data;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Outputs the number of rows in a ITableDataFlow.
    /// Note that if the ITableDataFlow.DataTable changes object after wiring,
    /// the abstraction will not work.
    /// ----------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow table: Input table to count rows.
    /// 2. IDataFlow<int> output: Outputs the number of rows in the table.
    /// 3. IDataFlowB<int> outputB: Outputs the number of rows in the table.
    /// </summary>
    class CountRow : IDataFlowB<int> // outputB
    {
        // properties
        public string InstanceName = "Default";
        public int DebounceTime { set => debounceTime = value; }
        // ports
        private ITableDataFlow table;
        private IDataFlow<int> output;

        // private fields
        private int count = 0;
        private System.Timers.Timer debounceTimer;
        private int debounceTime = 100;

        public CountRow() { }

        /// <summary>
        /// Sets up the data table row changed listener.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (table == null) return;

            table.DataTable.RowChanged += HandleTableChanged;
        }
        
        /// <summary>
        /// Updates the debounce timer.
        /// </summary>
        private void CheckTimer()
        {
            if (debounceTime < 1)
            {
                UpdateDataFlow(null, null);
                return;
            } 

            if (debounceTimer != null)
            {
                debounceTimer.Dispose();
            }

            debounceTimer = new System.Timers.Timer(debounceTime);
            debounceTimer.Elapsed += UpdateDataFlow;
            debounceTimer.Enabled = true;
        }

        /// <summary>
        /// Handles the table changing by checking the timer and retrieving the count.
        /// </summary>
        private void HandleTableChanged(object sender, EventArgs e)
        {
            count = table.DataTable.Rows.Count;
            CheckTimer();
        }

        /// <summary>
        /// Updates the dataflow(s).
        /// </summary>
        private void UpdateDataFlow(object sender, EventArgs e)
        {
            output.Data = count;
            DataChanged?.Invoke();
            debounceTimer.Dispose();
            debounceTimer = null;
        }

        // IDataFlowB<int> implementation
        int IDataFlowB<int>.Data { get => count; }
        public event DataChangedDelegate DataChanged;
    }
}