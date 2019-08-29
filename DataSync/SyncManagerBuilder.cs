using System;
using System.Collections.Generic;
using System.Linq;

namespace Yansoft.DataSync
{
    public class SyncManagerBuilder
    {
        private Dictionary<Type, TypeConfiguration> typeConfigurations = new Dictionary<Type, TypeConfiguration>();

        public TypeConfiguration<T> Sync<T>()
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
            var invalidItems = typeConfigurations.Where(t => t.Value.TypeProvider.SyncProvider == null);
            if (invalidItems.Any())
            {
                var item = invalidItems.First();
                var message = $"There isn't any {nameof(ISyncProvider)} registered for type {item.Key}. Register an {nameof(ISyncProvider)} by chaining a {nameof(TypeConfiguration<object>.With)} method call with the {nameof(Sync)} method call.";
                throw new InvalidOperationException(message);
            }
            var providers = typeConfigurations.ToDictionary(c => c.Key, c => c.Value.TypeProvider);
            return new SyncManager(providers);
        }
    }
}