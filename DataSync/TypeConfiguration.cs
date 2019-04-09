using System;
using System.Collections.Generic;

namespace Yansoft.DataSync
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
}