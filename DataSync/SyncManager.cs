﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yansoft.DataSync
{
    public class SyncManager
    {
        private Dictionary<Type, TypeProvider> _typeProviders;

        internal SyncManager(Dictionary<Type, TypeProvider> typeProviders)
        {
            _typeProviders = typeProviders;
        }

        public async Task SyncAsync<T>(SyncMode mode) where T : new()
        {
            var t = typeof(T);
            var key = _typeProviders.Keys
                .Where(c => c.IsAssignableFrom(t))
                .OrderBy(c => Depth(t, c))
                .FirstOrDefault() 
                ?? throw new InvalidOperationException($"No provider was found for type {typeof(T)}");

            var config = _typeProviders[key];
            
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


        private int Depth<U, V>(int depth = 0) =>
            Depth(typeof(U), typeof(V));

        private int Depth(Type u, Type v, int depth = 0)
        {
            if (u == v)
            {
                return depth;
            }
            return u.BaseType != null ? Depth(u.BaseType, v, depth + 1) : int.MaxValue;
        }
    }
}
