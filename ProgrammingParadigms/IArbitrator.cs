using System.Threading.Tasks;

namespace ProgrammingParadigms
{
    /// <summary>
    /// This defines the management mutiple asynchronized resource requests.
    /// The instance which requests the resource will occupy it until it calls the Release() method,
    /// during this period any request will be paused and stored in an execution queue. After the previous resource being released,
    /// the first request in the queue will be dequeued and executed and it occupies the resource again.
    /// example:
    ///    if in some domain abstraction, a port called "arbitrator" of type IArbitrator is wired to an instance of Arbitrator,
    ///    and a port called scpDevice is wired to some slow device which can only handle one request at a time:
    ///    await arbitrator.Request("ID");
    ///    response = await scpDevice.SendRequest("{ZA1}");
    ///    arbitrator.Release("ID");
    /// </summary>
public interface IArbitrator
    {
        // Requesting for occupying the source, might not return immediately so Task pattern is used.
        Task Request(string requestor);

        // The release requires an id parameter, which comes from the return of request method.
        void Release(string requestor);
    }
}
