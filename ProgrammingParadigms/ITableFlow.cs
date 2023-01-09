using System;
using System.Data;
using System.Threading.Tasks;

namespace ProgrammingParadigms
{
    /// <summary>
    /// A data flow connection that carries rows and self-describing columns that have names and types.
    /// Sometimes one directional and sometimes bi-directional depending on the operators(some are bidirectional).
    /// Data only moves when an operator called a transact is fired. (Called transact because it transfers the whole table at once so it remains self-consistent).
    /// Transactions can be triggered in either direction.
    /// Transact can be near the source, or the destination, or anywhere in-between.
    /// Thus if the transaction is initiated at the source it is analogous to RX (Reactive extensions) with a hot observable.
    /// If the transaction is at the destination, it is analogous to a query.
    /// Some abstractions are sources (SCPSessionList), some are destinations (readonly grid) and many are both(FileReaderWriter, DatabaseTable, SCPSessionData).
    /// Some abstractions transform data and are not considered a source or destination, so they send the data through, transformed, when a transaction occurs anywhere in their stream.
    /// The ITableData interface also has a 'current row'.
    /// At the logical level, all rows are transmitted on a transaction.
    /// For example, several of these interfaces can be connected directly to a single destination and the destination will show the last transaction sent from any of the sources. 
    /// In practice only the rows that are actually needed are transferred, so a grid actually requests the data it needs for the display even if the transact itself is at the source.
    /// This will cause less data to be brought off the device or from the database.
    /// If the sink is say a file or a website, then all data must be queried. When this happens for a slow device, the data is queried in chunks that can be adjusted for efficiency.
    /// Many decorators can be implemented such as the equivalent of Select (Map), Where (Filter), Aggregate (Reduce). 
    /// When the inputs change, a new transaction will be sent from that point.
    /// For example when a filter input port changes, a new transaction is sent logically consisting of the new set of rows.
    /// Three or more ITableDataFlows can be connected to a single point.
    /// Transacts or inactive Gates will block them.
    /// If there is more than one active destination, the data flows to all destinations.
    /// When there are multiple active sources it will cause an error.
    /// Rule: When you have more than two ITableDataFlows connected to a single point, all but one must have a Transact so it is clear which two the data is flowing through on a transaction.
    /// </summary>
    public interface ITableFlow
    {
        /// <summary>
        /// Data source that stores the headers (columns) and content (rows) of a table
        /// </summary>
        DataTable DataTable { get; }


        /// <summary>
        /// This tells the source to load the table header into the source DataTable. The headers could be from the device, or a file in the cloud.
        /// </summary>
        /// <returns></returns>
        Task GetHeadersFromSourceAsync(object queryOperation = null);


        /// <summary>
        /// This tells the source to load the data into the source DataTable.
        /// This method is called on the UI thread so it may load it all, but more commonly it will load just one batch of rows
        /// plus a suitable size to optimize performance. Returns two indices in the table: the first row the last row + 1.
        /// (Sources should never spend more than 100ms loading, because this will hold up the UI, but should probably
        /// do more than one record at a time to get reasonable efficiency).
        /// The Task pattern is used because we run on the UI thread, and this tells us when the batch is loaded.
        /// </summary>
        Task<Tuple<int, int>> GetPageFromSourceAsync();


        /// <summary>
        /// This notifies the destination that the table meta-data, including the headers (columns), date, tablename has been transferred.
        /// This is an asynchronous process as the destination might upload the headers to device, cloud, etc.
        /// </summary>
        /// <returns></returns>
        Task PutHeaderToDestinationAsync();


        /// <summary>
        /// This notifies the destination that the page data (a batch of rows) has been transferred, so the destination can implement its logic to handle the data.
        /// The firstRowIndex and lastRowIndex indicate the cursors where the rows were transerred.
        /// For the getNextPage callback function, it enables the destination to trigger a new transaction to fetch the next batch of rows.
        /// </summary>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="getNextPage"></param>
        /// <returns></returns>
        Task PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex);


        /// <summary>
        /// The ITableDataFlow interface also has a concept of a current row. Either the source or the destination can change the current row. The current row can, for example, select a session in a device or it can be the row that gets deleted by a delete command given to the source or the destination.
        /// </summary>
        DataRow CurrentRow { get; set; }

        bool SupportQuery { get; }

        bool RequestQuerySupport();
    }
}
