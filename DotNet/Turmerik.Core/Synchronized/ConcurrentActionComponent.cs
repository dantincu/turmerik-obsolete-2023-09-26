﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.Synchronized
{
    public interface IConcurrentActionComponentCore
    {
        void Execute(Action action);
        T Execute<T>(Func<T> action);

        ITrmrkActionResult TryExecute(Action action);
        ITrmrkActionResult<TData> TryExecute<TData>(Func<TData> action);
    }

    public interface INonSynchronizedActionComponent : IConcurrentActionComponentCore
    {
    }

    public interface IConcurrentActionComponent : IConcurrentActionComponentCore, IDisposable
    {
    }

    public interface IThreadSafeActionComponent : IConcurrentActionComponent
    {
    }

    public interface IInterProcessConcurrentActionComponent : IConcurrentActionComponent
    {
    }

    public interface IActionComponentFactory<TActionComponent>
        where TActionComponent : IConcurrentActionComponentCore
    {
        TActionComponent Create();
    }

    public interface IThreadSafeActionComponentFactory : IActionComponentFactory<IThreadSafeActionComponent>
    {
    }

    public interface INonSynchronizedActionComponentFactory : IActionComponentFactory<INonSynchronizedActionComponent>
    {
    }

    public interface IInterProcessConcurrentActionComponentFactory
    {
        IInterProcessConcurrentActionComponent Create(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false);
    }

    public abstract class ConcurrentActionComponentBase
    {
        protected ITrmrkActionResult TryExecuteCore(Action action)
        {
            ITrmrkActionResult result;

            try
            {
                action();
                result = new TrmrkActionResult();
            }
            catch (Exception ex)
            {
                result = new TrmrkActionResult
                {
                    Exception = ex
                };
            }

            return result;
        }

        protected ITrmrkActionResult<TData> TryExecuteCore<TData>(Func<TData> action)
        {
            ITrmrkActionResult<TData> result;

            try
            {
                var data = action();

                result = new TrmrkActionResult<TData>
                {
                    Data = data
                };
            }
            catch (Exception ex)
            {
                result = new TrmrkActionResult<TData>
                {
                    Exception = ex
                };
            }

            return result;
        }
    }

    public abstract class ConcurrentActionComponentBase<TSynchronizer> : ConcurrentActionComponentBase, IConcurrentActionComponent
        where TSynchronizer : class, IDisposable
    {
        private readonly Lazy<IDisposableComponent<TSynchronizer>> synchronizerComponent;

        protected ConcurrentActionComponentBase(
            IDisposableComponentFactory disposableComponentFactory)
        {
            synchronizerComponent = new Lazy<IDisposableComponent<TSynchronizer>>(
                () => disposableComponentFactory.Create(CreateSynchronizer()),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        protected TSynchronizer Synchronizer => synchronizerComponent.Value.Component;

        public void Execute(Action action)
        {
            WaitOne();

            try
            {
                action();
            }
            finally
            {
                Release();
            }
        }

        public T Execute<T>(Func<T> action)
        {
            WaitOne();
            T val;

            try
            {
                val = action();
            }
            finally
            {
                Release();
            }

            return val;
        }

        public ITrmrkActionResult TryExecute(
            Action action) => Execute(
                () => TryExecuteCore(action));

        public ITrmrkActionResult<TData> TryExecute<TData>(
            Func<TData> action) => Execute(
                () => TryExecuteCore(action));

        public void Dispose() => synchronizerComponent.Value.TryDispose();

        protected abstract void WaitOne();
        protected abstract void Release();
        protected abstract TSynchronizer CreateSynchronizer();
    }

    public class ThreadSafeActionComponent : ConcurrentActionComponentBase<Semaphore>, IThreadSafeActionComponent
    {
        public ThreadSafeActionComponent(
            IDisposableComponentFactory disposableComponentFactory) : base(disposableComponentFactory)
        {
        }

        protected override Semaphore CreateSynchronizer() => new Semaphore(1, 1);

        protected override void Release()
        {
            Synchronizer?.Release();
        }

        protected override void WaitOne()
        {
            Synchronizer?.WaitOne();
        }
    }

    public class InterProcessConcurrentActionComponent : ConcurrentActionComponentBase<Mutex>, IInterProcessConcurrentActionComponent
    {
        private readonly Mutex mutex;

        public InterProcessConcurrentActionComponent(
            IDisposableComponentFactory disposableComponentFactory,
            Mutex mutex) : base(disposableComponentFactory)
        {
            this.mutex = mutex ?? throw new ArgumentNullException(nameof(mutex));
        }

        protected override Mutex CreateSynchronizer() => mutex;

        protected override void Release()
        {
            mutex?.ReleaseMutex();
        }

        protected override void WaitOne()
        {
            mutex?.WaitOne();
        }
    }

    /// <summary>
    /// Used for easily switching from synchronized to non-synchronized component implementation.
    /// </summary>
    public class NonSynchronizedActionComponent : ConcurrentActionComponentBase, INonSynchronizedActionComponent
    {
        public void Execute(Action action) => action();
        public T Execute<T>(Func<T> action) => action();

        public ITrmrkActionResult TryExecute(
            Action action) => TryExecuteCore(action);

        public ITrmrkActionResult<TData> TryExecute<TData>(
            Func<TData> action) => TryExecuteCore(action);
    }

    public class ThreadSafeActionComponentFactory : IThreadSafeActionComponentFactory
    {
        private readonly IDisposableComponentFactory disposableComponentFactory;

        public ThreadSafeActionComponentFactory(
            IDisposableComponentFactory disposableComponentFactory)
        {
            this.disposableComponentFactory = disposableComponentFactory ?? throw new ArgumentNullException(nameof(disposableComponentFactory));
        }

        public IThreadSafeActionComponent Create() => new ThreadSafeActionComponent(
            disposableComponentFactory);
    }

    public class NonSynchronizedActionComponentFactory : INonSynchronizedActionComponentFactory
    {
        public INonSynchronizedActionComponent Create() => new NonSynchronizedActionComponent();
    }

    public class InterProcessConcurrentActionComponentFactory : IInterProcessConcurrentActionComponentFactory
    {
        private readonly IDisposableComponentFactory disposableComponentFactory;
        private readonly IMutexCreator mutexCreator;

        public InterProcessConcurrentActionComponentFactory(
            IDisposableComponentFactory disposableComponentFactory,
            IMutexCreator mutexCreator)
        {
            this.disposableComponentFactory = disposableComponentFactory ?? throw new ArgumentNullException(
                nameof(disposableComponentFactory));

            this.mutexCreator = mutexCreator ?? throw new ArgumentNullException(nameof(mutexCreator));
        }

        public IInterProcessConcurrentActionComponent Create(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false)
        {
            var mutex = mutexCreator.Create(
                mutexName,
                initiallyOwned,
                createGlobalMutex);

            var component = new InterProcessConcurrentActionComponent(
                disposableComponentFactory,
                mutex);

            return component;
        }
    }
}
