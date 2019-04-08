using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class SyncManager
    {
        internal Dictionary<Type, TypeProvider> TypeProviders;

        internal SyncManager(Dictionary<Type, TypeProvider> typeProviders)
        {
            TypeProviders = typeProviders;
        }

        public async Task SyncAsync<T>(SyncMode mode)
        {
            var t = typeof(T);
            var key = TypeProviders.Keys
                .Where(c => c.IsAssignableFrom(t))
                .OrderBy(c => Depth(t, c))
                .First();
            var config = TypeProviders[key];
            
            switch (mode)
            {
                case SyncMode.Pull:
                    await config.Pull<T>();
                    break;
                case SyncMode.Push:
                    await config.Put<T>();
                    await config.Push<T>();
                    break;
                case SyncMode.RemoteWins:
                    await config.Pull<T>();
                    await config.Put<T>();
                    await config.Push<T>();
                    break;
                case SyncMode.LocalWins:
                    await config.Put<T>();
                    await config.Pull<T>();
                    await config.Push<T>();
                    break;
                default:
                    break;
            }
        }


        private int Depth<T, U>(int depth = 0) =>
            Depth(typeof(T), typeof(U));

        private int Depth(Type t, Type u, int depth = 0)
        {
            if (t == u)
            {
                return depth;
            }
            return depth + Depth(t.BaseType, u);
        }
    }
}
