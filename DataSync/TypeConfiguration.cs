using System;
using System.Collections.Generic;

namespace Yansoft.DataSync
{
    public class TypeConfiguration<U> : TypeConfiguration
    {
        public TypeConfiguration<U> With(ISyncProvider<U> provider)
        {
            TypeProvider.SyncProvider = provider;
            return this;
        }

        public TypeConfiguration<U> With<T>() where T : ISyncProvider<U>, new()
        {
            TypeProvider.SyncProvider = Activator.CreateInstance<T>();
            return this;
        }
    }

    public class TypeConfiguration
    {
        internal TypeProvider TypeProvider { get; set; } = new TypeProvider();
    }
}