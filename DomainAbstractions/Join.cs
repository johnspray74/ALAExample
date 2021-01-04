using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainAbstractions;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    public class Join : ITableDataFlow
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private ITableDataFlow sourceDataFlow;
        private ITableDataFlow destinationDataFlow;

        // fields
        private DataTable dataTable = new DataTable();
        private int currentIndex = 0;
        private bool queryMode;
        private JObject joinContainer = new JObject();
        private Tuple<int, int> sourceTuple;
        private string joinType = "LEFTJOIN";
        private bool joinAdded;
        private string joinTable;
        private string joinCondition;

        // ctor
        public Join(string joinTable, string joinCondition, string joinType = "LEFT")
        {
            this.joinTable = joinTable;
            this.joinCondition = joinCondition;

            if (new string[] { "LEFT", "RIGHT", "OUTER", "INNER" }.Contains(joinType.ToUpper()))
            {
                this.joinType = joinType.ToUpper() + "JOIN"; // type would be: LEFTJOIN, RIGHTJOIN, OUTERJOIN or INNERJOIN
            }
        }

        // public Join(string leftTable, string rightTable, string joinColumn, string joinType = "LEFT")
        // {
        //     leftQueryTable = leftTable;
        //     rightQueryTable = rightTable;
        //     this.joinColumn = joinColumn;
        //
        //     if (new string[] {"LEFT", "RIGHT", "OUTER", "INNER"}.Contains(joinType.ToUpper()))
        //     {
        //         this.joinType = joinType.ToUpper() + "JOIN"; // type would be: LEFTJOIN, RIGHTJOIN, OUTERJOIN or INNERJOIN
        //     }
        // }


        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // request operating mode with end point.
            if (sourceDataFlow != null) queryMode = sourceDataFlow.RequestQuerySupport();

            if (queryMode)
            {
                // check whether received JObject from other instances
                if (queryOperation != null) joinContainer = queryOperation as JObject;

                // initialise "Join" property
                InitialiseJoin(joinContainer);
                
                // create join condition
                var joinObj = CreateJoinObject(joinTable, joinCondition);

                // add join condition
                if (!joinAdded)
                {
                    AddJoinCondition(joinContainer, joinObj);
                    joinAdded = true;
                }
                // pass JObject through ITableDataFlow chain
                if (sourceDataFlow != null) await sourceDataFlow.GetHeadersFromSourceAsync(joinContainer);
                dataTable = sourceDataFlow?.DataTable;
            }
            else
            {
                // TODO: Need implementation for DataTable
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

            foreach (DataRow r in sourceDataFlow.DataTable.Rows) dataTable.ImportRow(r);
            return sourceTuple;
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            return sourceDataFlow?.RequestQuerySupport() ?? true;
        }

        // private methods
        private void InitialiseJoin(JObject container)
        {
            if (container.ContainsKey("SELECT"))
            {
                JObject selectObj = (JObject) container["SELECT"];

                if (!selectObj.ContainsKey("Join"))
                {
                    selectObj.Add(
                        new JProperty("Join",
                            new JObject(
                                new JProperty("LEFTJOIN", new JArray()),
                                new JProperty("RIGHTJOIN", new JArray()),
                                new JProperty("OUTERJOIN", new JArray()),
                                new JProperty("INNERJOIN", new JArray())
                            )
                        )
                    );
                }
            }
            else
            {
                container.Add(
                    new JProperty("SELECT",
                        new JProperty("Join",
                            new JObject(
                                new JProperty("LEFTJOIN", new JArray()),
                                new JProperty("RIGHTJOIN", new JArray()),
                                new JProperty("OUTERJOIN", new JArray()),
                                new JProperty("INNERJOIN", new JArray())
                            )
                        )
                    )
                );
            }
        }

        private JObject CreateJoinObject(string jTable, string joinCon)
        {
            return new JObject(
                new JProperty("JoinTable", jTable),
                new JProperty("JoinCondition", joinCon)
                );
        }

        private void AddJoinCondition(JObject container, JObject joinCond)
        {
            var joinArray = (JArray) container["SELECT"]["Join"][joinType];

            joinArray.Add(joinCond);
        }
    }
}
