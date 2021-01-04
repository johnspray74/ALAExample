using System;
using System.Data;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Basically, it is an ITableDataFlow decorator that sort rows by a giving Column and Order type, Ascending or Descending.
    /// This would happen on a whole table which based on all the rows has been transacted.
    /// Otherwise, after the sorting the firtRowIndex and lastRowIndex will not accurately
    /// indicate the right rows which has been transacted.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports
    /// 1. ITableDataFlow "NEEDNAME":
    /// 2. ITableDataFlow "sourceDataFlow":
    /// </summary>
    public class Sort : ITableDataFlow
    {
        // properties
        public string InstanceName = "Default";
        public string Column;
        public bool IsDescending;

        // ports
        private ITableDataFlow sourceDataFlow;
        private ITableDataFlow destinationDataFlow;

        // fields
        private DataTable dataTable = new DataTable();
        private int currentIndex = 0;
        private bool queryMode = false;
        private JObject sortContainer = new JObject();
        private Tuple<int, int> sourceTuple;
        private bool sortAdded;

        /// <summary>
        /// Sort rows by a giving Column and Order type, Ascending or Descending
        /// </summary>
        public Sort() { }

        // ITableDataFlow implementation -------------------------------------------------------
        DataTable ITableDataFlow.DataTable => dataTable;

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            return sourceDataFlow?.RequestQuerySupport() ?? true;
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // request operating mode with end point.
            if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

            if (queryMode)
            {
                // check whether received JObject from other instances
                if (queryOperation != null) sortContainer = queryOperation as JObject;

                // initialise "Sort" property
                InitialiseSort(sortContainer);

                // create and add sort condition
                if (!sortAdded)
                {
                    AddSortCondition(sortContainer, Column);
                    sortAdded = true;
                }

                // pass JObject through ITableDataFlow chain
                if (sourceDataFlow != null) await sourceDataFlow.GetHeadersFromSourceAsync(sortContainer);
                dataTable = sourceDataFlow?.DataTable;
            }
            else
            {
                await sourceDataFlow.GetHeadersFromSourceAsync();

                dataTable.Rows.Clear();
                dataTable.Columns.Clear();
                dataTable.TableName = sourceDataFlow.DataTable.TableName;

                foreach (DataColumn c in sourceDataFlow.DataTable.Columns)
                {
                    dataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
                }
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (queryMode)
            {
                sourceTuple = await sourceDataFlow.GetPageFromSourceAsync();
                dataTable = sourceDataFlow?.DataTable;
                return sourceTuple;
            }
            else
            {
                Tuple<int, int> tuple = await sourceDataFlow.GetPageFromSourceAsync();

                DataTable tempTable = IsDescending ?
                    (from row in dataTable.AsEnumerable()
                        let date = Convert.ToDateTime(row.Field<string>(Column))
                        orderby date descending
                        select row).CopyToDataTable() :
                    (from row in dataTable.AsEnumerable()
                        let date = Convert.ToDateTime(row.Field<string>(Column))
                        orderby date ascending
                        select row).CopyToDataTable();

                foreach (DataRow r in tempTable.Rows) dataTable.ImportRow(r);

                return tuple;
            }
        }        

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            if (sourceDataFlow == null) return;

            sourceDataFlow.DataTable.Rows.Clear();
            sourceDataFlow.DataTable.Columns.Clear();
            sourceDataFlow.DataTable.TableName = dataTable.TableName;

            foreach (DataColumn c in dataTable.Columns)
            {
                sourceDataFlow.DataTable.Columns.Add(new DataColumn(c.ColumnName) { Prefix = c.Prefix });
            }

            await sourceDataFlow.PutHeaderToDestinationAsync();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate callBack)
        {
            if (sourceDataFlow == null)
            {
                callBack?.Invoke();
                return;
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow r in dataTable.Rows) sourceDataFlow.DataTable.ImportRow(r);
            }

            await sourceDataFlow.PutPageToDestinationAsync(firstRowIndex, lastRowIndex, callBack);
        }

        // private methods
        private void InitialiseSort(JObject container)
        {
            if (container.ContainsKey("SELECT"))
            {
                JObject selectObj = (JObject) container["SELECT"];

                if (!selectObj.ContainsKey("Sort"))
                {
                    selectObj.Add(
                        new JProperty("Sort",
                            new JObject(
                                new JProperty("ASC", new JValue(string.Empty)),
                                new JProperty("DESC", new JValue(string.Empty))
                            )
                        )
                    );
                }
            }
            else
            {
                container.Add(
                    new JProperty("SELECT",
                        new JProperty("Sort",
                            new JObject(
                                new JProperty("ASC", new JValue(string.Empty)),
                                new JProperty("DESC", new JValue(string.Empty))
                            )
                        )
                    )
                );
            }
        }

        private void AddSortCondition(JObject container, string col)
        {
            var sortLabel = IsDescending ? "DESC" : "ASC";
            JValue sortValue = (JValue)container["SELECT"]["Sort"][sortLabel];

            sortValue.Value = col;
        }

    }
}
