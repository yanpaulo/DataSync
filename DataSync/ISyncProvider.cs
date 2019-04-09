using System.Threading.Tasks;

namespace Yansoft.DataSync
{
    internal interface ISyncProvider : ISyncProvider<object> { }

    public interface ISyncProvider<U> : IPullProvider<U>, IPushProvider<U>, IPutProvider<U> { }

    public interface IPullProvider<U>
    {
        Task PullAsync<T>() where T : U, new();
    }

    public interface IPushProvider<U>
    {
        Task PushAsync<T>() where T : U, new();
    }

    public interface IPutProvider<U>
    {
        Task PutAsync<T>() where T : U, new();
    }
}