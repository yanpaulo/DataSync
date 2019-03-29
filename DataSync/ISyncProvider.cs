using System.Threading.Tasks;

namespace DataSync
{
    public interface ISyncProvider<U> : ISyncProvider
    {
        Task PullAsync<T>() where T : U, new();
        Task PushAsync<T>() where T : U, new();
        Task PutAsync<T>() where T : U, new();
    }

    public interface ISyncProvider
    {
    }
}