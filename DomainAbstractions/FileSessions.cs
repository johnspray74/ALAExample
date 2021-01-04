using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Stores session data from csv files. Works in a similar way as SessionDataSCP, caches the session data records
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. ITableDataFlow "NEEDNAME": incoming ITableDataFlow
    /// 2. IDataFlow<DataTable> "NEEDNAME": cache data table
    /// 3. IDataFlow<string> "NEEDNAME": incoming string input of the session id
    /// 4. IDataFlowB<string> dataFlowBDeviceId: input of device id (PCB serial number)
    /// </summary>
    public class FileSessions : ITableDataFlow, IDataFlow<DataTable>, IDataFlow<string>
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // outputs ---------------------------------------------------------------
        private IDataFlowB<string> dataFlowBDeviceId;

        // private fields ---------------------------------------------------------------
        private Dictionary<string, DataTable> sessionCache = new Dictionary<string, DataTable>();
        private int index = 0;
        private string sessionId;

        /// <summary>
        /// Stores session data from csv files.Works in a similar way as SessionDataSCP, caches the session data records
        /// </summary>
        public FileSessions() { }

        // IDataFlow<string> implementation ---------------------------------------------------------
        string IDataFlow<string>.Data
        {
            set
            {
                sessionId = value;
                index = 0;
            }
        }

        // IDataFlow<DataTable> implementation ---------------------------------------------------------
        DataTable IDataFlow<DataTable>.Data { set => sessionCache[dataFlowBDeviceId.Data + "-" + sessionId] = value.Copy(); }


        // ITableDataFlow implementation ---------------------------------------------------------
        DataTable ITableDataFlow.DataTable
        {
            get
            {
                if (sessionCache.ContainsKey(dataFlowBDeviceId.Data + "-" + sessionId))
                    return sessionCache[dataFlowBDeviceId.Data + "-" + sessionId].Copy();
                return new DataTable();
            }
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation) {}

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (index != 0)
                return new Tuple<int, int>(index, index);

            int count = sessionCache[dataFlowBDeviceId.Data + "-" + sessionId].Rows.Count;
            index = count;
            return new Tuple<int, int>(0, count);
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
