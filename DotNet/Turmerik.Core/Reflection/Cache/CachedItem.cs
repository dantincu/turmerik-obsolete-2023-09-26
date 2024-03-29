﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Synchronized;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedItem<T>
    {
        T Data { get; }
    }

    public interface ICachedItem<T, TFlags> : ICachedItem<T>
    {
        Lazy<TFlags> Flags { get; }
    }

    public class CachedItemBase<T> : ICachedItem<T>
    {
        public CachedItemBase(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            T value)
        {
            this.TypesMap = cachedTypesMap ?? throw new ArgumentNullException(
                nameof(cachedTypesMap));

            this.ItemsFactory = cachedReflectionItemsFactory ?? throw new ArgumentNullException(
                nameof(cachedReflectionItemsFactory));

            this.StaticDataCacheFactory = staticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(staticDataCacheFactory));

            Data = value;
        }

        public T Data { get; }

        protected Lazy<ICachedTypesMap> TypesMap { get; }
        protected ICachedReflectionItemsFactory ItemsFactory { get; }
        protected IStaticDataCacheFactory StaticDataCacheFactory { get; }
    }

    public abstract class CachedItemBase<T, TFlags> : CachedItemBase<T>, ICachedItem<T, TFlags>
    {
        public CachedItemBase(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            T value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            Flags = new Lazy<TFlags>(GetFlags);
        }

        public Lazy<TFlags> Flags { get; }

        protected abstract TFlags GetFlags();
    }
}
