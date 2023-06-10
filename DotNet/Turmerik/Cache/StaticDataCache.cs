using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Synchronized;

namespace Turmerik.Cache
{
    public interface IStaticDataCache<TKey, TValue> : IDataCacheCore<TKey, TValue>
    {
        TValue Get(TKey key);
        bool TryRemove(TKey key);
        bool TryRemove(TKey key, out TValue removed);
    }

    public interface IStaticDataCacheFactory<TConcurrentActionComponent, TDataCacheFactory>
        where TConcurrentActionComponent : IActionComponent
        where TDataCacheFactory : IDataCacheFactory<TConcurrentActionComponent>
    {
        IStaticDataCache<TKey, TValue> Create<TKey, TValue>(
            Func<TKey, TValue> factory,
            IEqualityComparer<TKey> keyEqCompr = null);
    }

    public interface IThreadSafeStaticDataCacheFactory : IStaticDataCacheFactory<IThreadSafeActionComponent, IThreadSafeDataCacheFactory>
    {
    }

    public interface INonSynchronizedStaticDataCacheFactory : IStaticDataCacheFactory<INonSynchronizedActionComponent, INonSynchronizedDataCacheFactory>
    {
    }

    public interface IInterProcessConcurrentStaticDataCacheFactory
    {
        IStaticDataCache<TKey, TValue> CreateDataCache<TKey, TValue>(
            Func<TKey, TValue> factory,
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false,
            IEqualityComparer<TKey> keyEqCompr = null);
    }

    public class StaticDataCache<TKey, TValue> : IStaticDataCache<TKey, TValue>
    {
        private readonly IDataCache<TKey, TValue> innerCache;
        private readonly Func<TKey, TValue> factory;

        public StaticDataCache(
            IDataCache<TKey, TValue> innerCache,
            Func<TKey, TValue> factory)
        {
            this.innerCache = innerCache ?? throw new ArgumentNullException(nameof(innerCache));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public TValue Get(TKey key) => innerCache.GetOrCreate(key, factory);
        public bool TryRemove(TKey key) => innerCache.TryRemove(key);

        public bool TryRemove(
            TKey key,
            out TValue removed) => innerCache.TryRemove(
                key,
                out removed);

        public void Clear()
        {
            innerCache.Clear();
        }
    }

    public class StaticDataCacheFactory<TConcurrentActionComponent, TDataCacheFactory> : IStaticDataCacheFactory<TConcurrentActionComponent, TDataCacheFactory>
        where TConcurrentActionComponent : IActionComponent
        where TDataCacheFactory : IDataCacheFactory<TConcurrentActionComponent>
    {
        private readonly TDataCacheFactory dataCacheFactory;

        public StaticDataCacheFactory(TDataCacheFactory dataCacheFactory)
        {
            this.dataCacheFactory = dataCacheFactory ?? throw new ArgumentNullException(nameof(dataCacheFactory));
        }

        public IStaticDataCache<TKey, TValue> Create<TKey, TValue>(
            Func<TKey, TValue> factory,
            IEqualityComparer<TKey> keyEqCompr = null)
        {
            var dataCache = dataCacheFactory.Create<TKey, TValue>(
                keyEqCompr);

            var staticDataCache = new StaticDataCache<TKey, TValue>(
                dataCache,
                factory);

            return staticDataCache;
        }
    }

    public class ThreadSafeStaticDataCacheFactory : StaticDataCacheFactory<IThreadSafeActionComponent, IThreadSafeDataCacheFactory>, IThreadSafeStaticDataCacheFactory
    {
        public ThreadSafeStaticDataCacheFactory(
            IThreadSafeDataCacheFactory dataCacheFactory) : base(dataCacheFactory)
        {
        }
    }

    public class NonSynchronizedStaticDataCacheFactory : StaticDataCacheFactory<INonSynchronizedActionComponent, INonSynchronizedDataCacheFactory>, INonSynchronizedStaticDataCacheFactory
    {
        public NonSynchronizedStaticDataCacheFactory(
            INonSynchronizedDataCacheFactory dataCacheFactory) : base(dataCacheFactory)
        {
        }
    }

    public class InterProcessConcurrentStaticDataCacheFactory : IInterProcessConcurrentStaticDataCacheFactory
    {
        private readonly IInterProcessConcurrentDataCacheFactory dataCacheFactory;

        public InterProcessConcurrentStaticDataCacheFactory(
            IInterProcessConcurrentDataCacheFactory dataCacheFactory)
        {
            this.dataCacheFactory = dataCacheFactory ?? throw new ArgumentNullException(
                nameof(dataCacheFactory));
        }

        public IStaticDataCache<TKey, TValue> CreateDataCache<TKey, TValue>(
            Func<TKey, TValue> factory,
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false,
            IEqualityComparer<TKey> keyEqCompr = null)
        {
            var dataCache = dataCacheFactory.CreateDataCache<TKey, TValue>(
                mutexName,
                initiallyOwned,
                createGlobalMutex,
                keyEqCompr);

            var staticDataCache = new StaticDataCache<TKey, TValue>(
                dataCache,
                factory);

            return staticDataCache;
        }
    }
}
