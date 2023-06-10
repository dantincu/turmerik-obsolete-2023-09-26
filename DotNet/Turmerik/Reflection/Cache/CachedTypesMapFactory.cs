using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Synchronized;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedTypesMapFactory
    {
        ICachedTypesMap Create();
    }

    public class CachedTypesMapFactory : ICachedTypesMapFactory
    {
        private readonly INonSynchronizedDataCacheFactory dataCacheFactory;
        private readonly Lazy<ICachedReflectionItemsFactory> cachedTypeInfoFactory;

        public CachedTypesMapFactory(
            INonSynchronizedDataCacheFactory dataCacheFactory,
            Lazy<ICachedReflectionItemsFactory> cachedTypeInfoFactory)
        {
            this.dataCacheFactory = dataCacheFactory ?? throw new ArgumentNullException(nameof(dataCacheFactory));
            this.cachedTypeInfoFactory = cachedTypeInfoFactory ?? throw new ArgumentNullException(nameof(cachedTypeInfoFactory));
        }

        public ICachedTypesMap Create() => new CachedTypesMap(
            dataCacheFactory.Create<Type, ICachedTypeInfo>(),
            cachedTypeInfoFactory.Value);
    }
}
