using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turmerik.Synchronized;

namespace Turmerik.Cache
{
    public interface IDataCacheCore<TKey, TValue>
    {
        void Clear();
        bool TryRemove(TKey key);
        bool TryRemove(TKey key, out TValue removed);
        bool ContainsKey(TKey key);
        TKey[] GetKeys();
    }

    public interface IDataCache<TKey, TValue> : IDataCacheCore<TKey, TValue>
    {
        TValue GetOrCreate(TKey key, Func<TKey, TValue> factory);
    }

    public interface IDataCacheFactory
    {
        IDataCache<TKey, TValue> Create<TKey, TValue>(
            IEqualityComparer<TKey> keyEqCompr = null);
    }

    public interface IThreadSafeDataCacheFactory : IDataCacheFactory
    {
    }

    public interface INonSynchronizedDataCacheFactory : IDataCacheFactory
    {
    }

    public interface IInterProcessConcurrentDataCacheFactory
    {
        IDataCache<TKey, TValue> CreateDataCache<TKey, TValue>(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false,
            IEqualityComparer<TKey> keyEqCompr = null);
    }

    public class DataCache<TKey, TValue> : IDataCache<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dictnr;

        public DataCache(
            IActionComponent threadSafeActionComponent,
            IEqualityComparer<TKey> keyEqCompr = null)
        {
            ConcurrentActionComponent = threadSafeActionComponent ?? throw new ArgumentNullException(
                nameof(threadSafeActionComponent));

            dictnr = new Dictionary<TKey, TValue>(keyEqCompr ?? EqualityComparer<TKey>.Default);
        }

        protected IActionComponent ConcurrentActionComponent { get; }

        public TValue GetOrCreate(TKey key, Func<TKey, TValue> factory)
        {
            TValue value = ConcurrentActionComponent.Execute(
                () => GetOrCreateCore(key, factory));

            return value;
        }

        public bool TryRemove(TKey key) => ConcurrentActionComponent.Execute(
            () => TryRemoveCore(key));

        public bool TryRemove(TKey key, out TValue removed)
        {
            TValue removedVal = default;

            bool retVal = ConcurrentActionComponent.Execute(
                () => TryRemoveCore(key, out removedVal));

            removed = removedVal;
            return retVal;
        }

        public bool ContainsKey(TKey key) => ConcurrentActionComponent.Execute(
            () => dictnr.ContainsKey(key));

        public TKey[] GetKeys() => ConcurrentActionComponent.Execute(
            () => dictnr.Keys.ToArray());

        public void Clear()
        {
            ConcurrentActionComponent.Execute(
                () => dictnr.Clear());
        }

        private TValue GetOrCreateCore(TKey key, Func<TKey, TValue> factory)
        {
            TValue value;

            if (!dictnr.TryGetValue(key, out value))
            {
                value = factory(key);
            }

            return value;
        }

        private bool TryRemoveCore(TKey key)
        {
            bool retVal = dictnr.TryGetValue(key, out _);

            if (retVal)
            {
                dictnr.Remove(key);
            }

            return retVal;
        }

        private bool TryRemoveCore(TKey key, out TValue removed)
        {
            bool retVal = dictnr.TryGetValue(key, out removed);

            if (retVal)
            {
                dictnr.Remove(key);
            }

            return retVal;
        }
    }

    public class InterProcessConcurrentDataCacheFactory : IInterProcessConcurrentDataCacheFactory
    {
        private readonly IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory;

        public InterProcessConcurrentDataCacheFactory(
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory)
        {
            this.concurrentActionComponentFactory = concurrentActionComponentFactory ?? throw new ArgumentNullException(
                nameof(concurrentActionComponentFactory));
        }

        public IDataCache<TKey, TValue> CreateDataCache<TKey, TValue>(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false,
            IEqualityComparer<TKey> keyEqCompr = null)
        {
            var actionComponent = this.concurrentActionComponentFactory.Create(
                mutexName,
                initiallyOwned,
                createGlobalMutex);

            var dataCache = new DataCache<TKey, TValue>(
                actionComponent,
                keyEqCompr);

            return dataCache;
        }
    }

    public class DataCacheFactory<TConcurrentActionComponent> : IDataCacheFactory
        where TConcurrentActionComponent : class, IActionComponent
    {
        private readonly TConcurrentActionComponent threadSafeActionComponent;

        public DataCacheFactory(TConcurrentActionComponent threadSafeActionComponent)
        {
            this.threadSafeActionComponent = threadSafeActionComponent ?? throw new ArgumentNullException(nameof(threadSafeActionComponent));
        }

        public IDataCache<TKey, TValue> Create<TKey, TValue>(
            IEqualityComparer<TKey> keyEqCompr = null)
        {
            var dataCache = new DataCache<TKey, TValue>(
                threadSafeActionComponent,
                keyEqCompr);

            return dataCache;
        }
    }

    public class ThreadSafeDataCacheFactory : DataCacheFactory<IThreadSafeActionComponent>, IThreadSafeDataCacheFactory
    {
        public ThreadSafeDataCacheFactory(
            IThreadSafeActionComponent threadSafeActionComponent) : base(threadSafeActionComponent)
        {
        }
    }

    public class NonSynchronizedDataCacheFactory : DataCacheFactory<INonSynchronizedActionComponent>, INonSynchronizedDataCacheFactory
    {
        public NonSynchronizedDataCacheFactory(
            INonSynchronizedActionComponent threadSafeActionComponent) : base(threadSafeActionComponent)
        {
        }
    }
}
