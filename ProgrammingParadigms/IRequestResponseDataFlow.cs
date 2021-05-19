using System.Threading.Tasks;

namespace ProgrammingParadigms
{
    /// <summary>
    /// An asynchronous data flow interface.
    /// The data sent and recieved is generic.
    /// Task pattern is used to enable the code runs synchronouly by using async/await key words.
    /// </summary>
    /// <typeparam name="TRequest">type of request</typeparam>
    /// <typeparam name="TResponse">type of response</typeparam>
    public interface IRequestResponseDataFlow<TRequest, TResponse>
    {
        Task<TResponse> SendRequest(TRequest data);
    }
}
