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
    /// TBD
    ///
    /// This class
    /// 
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow "NEEDNAME":
    /// 2. IIterator<Tuple<string, string, string, string>> iterator: 
    /// </summary>
    public class ConvertTableToIterator : ITableDataFlow
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";

        // ports -----------------------------------------------------------------
        private IIterator<Tuple<string, string, string, string>> iterator;

        /// <summary>
        /// 
        /// </summary>
        public ConvertTableToIterator()
        {
            // add table headers, prefix "hide" means the column will not show          
            dataTable.Columns.Add("checkbox");
            dataTable.Columns.Add("description");
            dataTable.Columns.Add(new DataColumn("date") { Prefix = "hide" });
            dataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
            dataTable.Columns.Add(new DataColumn("name") { Prefix = "hide" });
            dataTable.Columns.Add(new DataColumn("count") { Prefix = "hide" });
        }

        // ITableDataFlow implementation ------------------------------------------------------
        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation) { }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (iterator.Finished)
                return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);

            Tuple<string, string, string, string> current = await iterator.Next();
            DataRow r = dataTable.NewRow();
            r["checkbox"] = false;
            r["index"] = current.Item1;
            r["name"] = current.Item2;
            r["date"] = current.Item3;
            r["count"] = current.Item4;
            r["description"] = current.Item2 + "\n" + current.Item3 
                + " (" + current.Item4 + " records) " + current.Item1;
            dataTable.Rows.Add(r);

            return new Tuple<int, int>(dataTable.Rows.Count - 1, dataTable.Rows.Count);
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }
    }
}
