using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.Synchronized
{
    public interface IActionComponent
    {
        void Execute(Action action);
        T Execute<T>(Func<T> action);

        ITrmrkActionResult TryExecute(Action action);
        ITrmrkActionResult<TData> TryExecute<TData>(Func<TData> action);
    }

    public interface INonSynchronizedActionComponent : IActionComponent
    {
    }

    public interface IConcurrentActionComponent : IActionComponent, IDisposable
    {
    }

    public interface IThreadSafeActionComponent : IConcurrentActionComponent
    {
    }

    public interface IInterProcessConcurrentActionComponent : IConcurrentActionComponent
    {
    }

    public interface IActionComponentFactory<TActionComponent>
        where TActionComponent : IActionComponent
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

    public abstract class ActionComponentBase
    {
        protected ITrmrkActionResult TryExecuteCore(Action action)
        {
            ITrmrkActionResult result;

            try
            {
                action();
                result = new TrmrkActionResult(true, null);
            }
            catch (Exception ex)
            {
                result = new TrmrkActionResult(
                    false,
                    new TrmrkActionError(
                        null, ex));
            }

            return result;
        }

        protected ITrmrkActionResult<TData> TryExecuteCore<TData>(Func<TData> action)
        {
            ITrmrkActionResult<TData> result;

            try
            {
                var data = action();
                result = new TrmrkActionResult<TData>(true, data, null);
            }
            catch (Exception ex)
            {
                result = new TrmrkActionResult<TData>(
                    false,
                    default,
                    new TrmrkActionError(
                        null, ex));
            }

            return result;
        }
    }

    public abstract class ConcurrentActionComponentBase<TSynchronizer> : ActionComponentBase, IConcurrentActionComponent
        where TSynchronizer : class, IDisposable
    {
        private readonly IDisposableComponent<TSynchronizer> synchronizerComponent;

        protected ConcurrentActionComponentBase(
            IDisposableComponentFactory disposableComponentFactory)
        {
            synchronizerComponent = disposableComponentFactory.Create(
                CreateSynchronizer());

            Synchronizer = synchronizerComponent.Component;
        }

        protected TSynchronizer Synchronizer { get; }

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

        public void Dispose() => synchronizerComponent.TryDispose();

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
    public class NonSynchronizedActionComponent : ActionComponentBase, INonSynchronizedActionComponent
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
