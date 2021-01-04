using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Converts an IDataFlow to DataTable.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> "NEEDNAME": input data that wants to be changed to a DataTable
    /// 2. ITableDataFlow "NEEDNAME": output DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConvertDataFlowToTable<T> : IDataFlow<T>, ITableDataFlow
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";
        public string ColumnName;

        /// <summary>
        /// Converts an IDataFlow to DataTable.
        /// </summary>
        public ConvertDataFlowToTable() { }

        // IDataFlow<T> implementation -----------------------------------
        private T data;
        T IDataFlow<T>.Data { set => data = value; }

        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation = null)
        {
            if (!dataTable.Columns.Contains(ColumnName))
            {
                dataTable.Columns.Add(ColumnName);
            }
        }

        private int index = 0;
        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (index == 1)
                return new Tuple<int, int>(1, 1);

            dataTable.Rows.Add(new object[] { data });
            index = 1;
            return new Tuple<int, int>(0, 1);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }
    }
}
