using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Collections;
using Turmerik.Synchronized;
using Turmerik.Utils;

namespace Turmerik.Cache
{
    public interface IStaticDataCache<TKey, TValue> : IDataCacheCore<TKey, TValue>
    {
        TValue Get(TKey key);
    }

    public interface IKeyReducerStaticDataCache<TKey, TValue> : IStaticDataCache<TKey, TValue>
    {
        bool ContainsKey(TKey key);
    }

    public interface IStaticDataCacheFactory
    {
        IStaticDataCache<TKey, TValue> Create<TKey, TValue>(
            Func<TKey, TValue> factory,
            IEqualityComparer<TKey> keyEqCompr = null);

        IKeyReducerStaticDataCache<TKey, TValue> CreateKeyReducer<TKey, TValue>(
            Func<TKey, TValue> factory,
            IEqualityComparer<TKey> keyEqCompr = null,
            Func<TKey, TKey> createKeyReducer = null,
            Func<TKey, TKey> removeKeyReducer = null,
            Func<TKey, TKey> hasKeyReducer = null);
    }

    public interface IThreadSafeStaticDataCacheFactory : IStaticDataCacheFactory
    {
    }

    public interface INonSynchronizedStaticDataCacheFactory : IStaticDataCacheFactory
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

        public virtual TValue Get(TKey key) => innerCache.GetOrCreate(key, factory);
        public virtual bool TryRemove(TKey key) => innerCache.TryRemove(key);

        public virtual bool TryRemove(
            TKey key,
            out TValue removed) => innerCache.TryRemove(
                key,
                out removed);

        public virtual bool HasKey(TKey key) => innerCache.HasKey(key);

        public virtual TKey[] GetKeys() => innerCache.GetKeys();

        public void Clear()
        {
            innerCache.Clear();
        }
    }

    public class KeyReducerStaticDataCache<TKey, TValue> : StaticDataCache<TKey, TValue>, IKeyReducerStaticDataCache<TKey, TValue>
    {
        private readonly Func<TKey, TKey> createKeyReducer;
        private readonly Func<TKey, TKey> removeKeyReducer;
        private readonly Func<TKey, TKey> hasKeyReducer;

        public KeyReducerStaticDataCache(
            IDataCache<TKey, TValue> innerCache,
            Func<TKey, TValue> factory,
            Func<TKey, TKey> createKeyReducer = null,
            Func<TKey, TKey> removeKeyReducer = null,
            Func<TKey, TKey> hasKeyReducer = null) : base(innerCache, factory)
        {
            this.createKeyReducer = createKeyReducer.FirstNotNull(
                key => key);

            this.removeKeyReducer = removeKeyReducer.FirstNotNull(
                key => key);

            this.hasKeyReducer = hasKeyReducer.FirstNotNull(
                this.createKeyReducer);
        }

        public override TValue Get(TKey key)
        {
            key = createKeyReducer(key);
            var value = base.Get(key);

            return value;
        }

        public override bool TryRemove(TKey key)
        {
            key = removeKeyReducer(key);
            var value = base.TryRemove(key);

            return value;
        }

        public override bool TryRemove(
            TKey key,
            out TValue removedValue)
        {
            key = removeKeyReducer(key);
            var value = base.TryRemove(key, out removedValue);

            return value;
        }

        public override bool HasKey(TKey key)
        {
            key = hasKeyReducer(key);
            var value = base.HasKey(key);

            return value;
        }

        public virtual bool ContainsKey(TKey key)
        {
            var value = base.HasKey(key);
            return value;
        }
    }

    public class StaticDataCacheFactory<TConcurrentActionComponent, TDataCacheFactory> : IStaticDataCacheFactory
        where TConcurrentActionComponent : IConcurrentActionComponentCore
        where TDataCacheFactory : class, IDataCacheFactory
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

        public IKeyReducerStaticDataCache<TKey, TValue> CreateKeyReducer<TKey, TValue>(
            Func<TKey, TValue> factory,
            IEqualityComparer<TKey> keyEqCompr = null,
            Func<TKey, TKey> createKeyReducer = null,
            Func<TKey, TKey> removeKeyReducer = null,
            Func<TKey, TKey> hasKeyReducer = null)
        {
            var dataCache = dataCacheFactory.Create<TKey, TValue>(
                keyEqCompr);

            var staticDataCache = new KeyReducerStaticDataCache<TKey, TValue>(
                dataCache,
                factory,
                createKeyReducer,
                removeKeyReducer,
                hasKeyReducer);

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
