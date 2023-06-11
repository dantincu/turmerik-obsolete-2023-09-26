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

    public interface INonSynchronizedTypesMapFactory : ICachedTypesMapFactory
    {
    }

    public interface IThreadSafeTypesMapFactory : ICachedTypesMapFactory
    {
    }

    public class CachedTypesMapFactory<TDataCacheFactory> : ICachedTypesMapFactory
        where TDataCacheFactory : class, IDataCacheFactory
    {
        private readonly TDataCacheFactory dataCacheFactory;
        private readonly Lazy<ICachedReflectionItemsFactory> cachedTypeInfoFactory;

        public CachedTypesMapFactory(
            TDataCacheFactory dataCacheFactory,
            Lazy<ICachedReflectionItemsFactory> cachedTypeInfoFactory)
        {
            this.dataCacheFactory = dataCacheFactory ?? throw new ArgumentNullException(nameof(dataCacheFactory));
            this.cachedTypeInfoFactory = cachedTypeInfoFactory ?? throw new ArgumentNullException(nameof(cachedTypeInfoFactory));
        }

        public ICachedTypesMap Create() => new CachedTypesMap(
            dataCacheFactory.Create<Type, ICachedTypeInfo>(),
            cachedTypeInfoFactory.Value);
    }

    public class NonSynchronizedCachedTypesMapFactory : CachedTypesMapFactory<INonSynchronizedDataCacheFactory>
    {
        public NonSynchronizedCachedTypesMapFactory(
            INonSynchronizedDataCacheFactory dataCacheFactory,
            Lazy<ICachedReflectionItemsFactory> cachedTypeInfoFactory) : base(
                dataCacheFactory,
                cachedTypeInfoFactory)
        {
        }
    }

    public class ThreadSafeCachedTypesMapFactory : CachedTypesMapFactory<IThreadSafeDataCacheFactory>
    {
        public ThreadSafeCachedTypesMapFactory(
            IThreadSafeDataCacheFactory dataCacheFactory,
            Lazy<ICachedReflectionItemsFactory> cachedTypeInfoFactory) : base(
                dataCacheFactory,
                cachedTypeInfoFactory)
        {
        }
    }
}
