using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class SyncManager
    {
        private readonly ISyncProvider _provider;

        public SyncManager(ISyncProvider provider)
        {
            _provider = provider;
        }

        public async Task SyncAsync<T, U>(SyncMode mode) where T : U, new()
        {
            var provider = (ISyncProvider<U>)_provider;

            switch (mode)
            {
                case SyncMode.Pull:
                    await provider.PullAsync<T>();
                    break;
                case SyncMode.Push:
                    await provider.PutAsync<T>();
                    await provider.PushAsync<T>();
                    break;
                case SyncMode.RemoteWins:
                    await provider.PullAsync<T>();
                    await provider.PutAsync<T>();
                    await provider.PushAsync<T>();
                    break;
                case SyncMode.LocalWins:
                    await provider.PutAsync<T>();
                    await provider.PullAsync<T>();
                    await provider.PushAsync<T>();
                    break;
                default:
                    break;
            }
        }
    }
}
