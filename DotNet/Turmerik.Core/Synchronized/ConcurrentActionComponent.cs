using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Utils;

namespace Turmerik.Core.Synchronized
{
    public interface IConcurrentActionComponent : IDisposable
    {
        void Execute(Action action);

        T Execute<T>(Func<T> action);
    }

    public interface IThreadSafeActionComponent : IConcurrentActionComponent
    {
    }

    public interface IInterProcessConcurrentActionComponent : IConcurrentActionComponent
    {
    }

    public interface IInterProcessConcurrentActionComponentFactory
    {
        IInterProcessConcurrentActionComponent Create(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false);
    }

    public abstract class ConcurrentActionComponentBase<TSynchronizer> : IConcurrentActionComponent
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
