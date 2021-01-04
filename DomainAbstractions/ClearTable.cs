using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Clears a given ITableDataFlow when an IEvent is received.
    /// ---------------------------------------------------------
    /// Ports:
    /// IEvent start: Clears the table. 
    /// ITableDataFlow table: The table to clear.
    /// </summary>
    class ClearTable : IEvent // start
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private ITableDataFlow table;

        /// <summary>
        /// Clears a given ITableDataFlow when an IEvent is received.
        /// </summary>
        public ClearTable() { }

        // IEvent implementation
        void IEvent.Execute() => table.DataTable.Rows.Clear();
    }
}