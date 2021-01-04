using ProgrammingParadigms;
using System.Collections;
using System.Data;

namespace DomainAbstractions
{
    /// <summary>
    /// Converts any kind of collective data structure eg.DataTable, Array, List, Dictionary to IDataFlow
    /// as the number of elements in the collection.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IDataFlow<T> "NEEDNAME" : input
    /// ITableDataFlow countOutput: output of row count
    /// </summary>
    /// <typeparam name="T">The Generic Type of the Collection</typeparam>
    public class Count<T> : IDataFlow<T>
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // outputs ---------------------------------------------------------------
        private IDataFlow<string> countOutput;

        /// <summary>
        /// Converts any kind of collective data structure eg.DataTable, Array, List, Dictionary to IDataFlow
        /// as the number of elements in the collection.
        /// </summary>
        public Count() { }

        // IDataFlow<T> implmentation -------------------------------------------------------
        T IDataFlow<T>.Data
        {
            set
            {
                if (value is DataTable)
                {
                    DataTable dt = value as DataTable;
                    countOutput.Data = dt.Rows.Count.ToString();
                }

                if (value is ICollection)
                {
                    ICollection c = value as ICollection;
                    countOutput.Data = c.Count.ToString();
                }
            }
        }
    }
}
