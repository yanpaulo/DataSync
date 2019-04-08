using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSync
{
    public class SyncManagerBuilder
    {
        private Dictionary<Type, TypeConfiguration> typeConfigurations = new Dictionary<Type, TypeConfiguration>();

        public TypeConfiguration<T> Type<T>() where T : new()
        {
            var t = typeof(T);
            TypeConfiguration<T> config;
            if (typeConfigurations.TryGetValue(t, out TypeConfiguration cfg))
            {
                config = cfg as TypeConfiguration<T>;
            }
            else
            {
                typeConfigurations.Add(t, config = new TypeConfiguration<T>());
            }
            return config;
        }

        public SyncManager Build()
        {
            var providers = typeConfigurations.ToDictionary(c => c.Key, c => c.Value.TypeProvider);
            return new SyncManager(providers);
        }
    }
}