using System.Threading.Tasks;

namespace ProgrammingParadigms
{
    /// <summary>
    /// This defines the management mutiple asynchronized resource requests.
    /// The instance which requests the resource will occupy it until it calls the Release() method,
    /// during this period any request will be paused and stored in an execution queue. After the previous resource being released,
    /// the first request in the queue will be dequeued and executed and it occupies the resource again.
    /// example:
    ///    await arbitrator.Request(InstanceName);
    ///    response = await requestResponseDataFlow.SendRequest("{ZA1}");
    ///    arbitrator.Release(InstanceName);
    /// </summary>
public interface IArbitrator
    {
        // Requesting for occupying the source, might not return immediately so Task pattern is used.
        Task Request(string requestor);

        // The release requires an id parameter, which comes from the return of request method.
        void Release(string requestor);
    }
}
