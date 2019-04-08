using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSync
{
    public class TypeConfiguration<U> : TypeConfiguration
    {
        public TypeConfiguration<U> SyncWith(ISyncProvider<U> provider)
        {
            TypeProvider.SyncProvider = provider;
            return this;
        }
    }

    public class TypeConfiguration
    {
        internal TypeProvider TypeProvider { get; set; } = new TypeProvider();
    }

    internal class TypeProvider
    {
        internal object SyncProvider { get; set; }

        internal async Task Pull<T>()
        {
            await Invoke<T>(nameof(ISyncProvider.PullAsync));
        }

        internal async Task Push<T>()
        {
            await Invoke<T>(nameof(ISyncProvider.PushAsync));
        }

        internal async Task Put<T>()
        {
            await Invoke<T>(nameof(ISyncProvider.PutAsync));
        }

        private Task Invoke<T>(string methodName)
        {
            var p = SyncProvider.GetType();
            var task = p
                .GetMethod(methodName)
                .MakeGenericMethod(typeof(T))
                .Invoke(SyncProvider, null) as Task;

            return task;
        }
    }
}