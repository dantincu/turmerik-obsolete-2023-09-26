using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Synchronized
{
    public interface IAsyncDisposableComponent<TComponent>
    {
        TComponent Component { get; }
        bool HasBeenDisposed { get; }

        Task<bool> TryDisposeAsync();
    }

    public interface IAsyncDisposableComponentFactory
    {
        IAsyncDisposableComponent<TComponent> Create<TComponent>(
            TComponent component)
            where TComponent : class, IAsyncDisposable;
    }

    public class AsyncDisposableComponent<TComponent> : IAsyncDisposableComponent<TComponent>
        where TComponent : class, IAsyncDisposable
    {
        private readonly IOnceExecutedAsyncAction onceExecutedAction;
        private readonly TComponent component;

        public AsyncDisposableComponent(
            IOnceExecutedAsyncActionFactory onceExecutedActionFactory,
            TComponent component)
        {
            this.onceExecutedAction = onceExecutedActionFactory.Create(DisposeCore);
            this.component = component ?? throw new ArgumentNullException();
        }

        public TComponent Component => component;
        public bool HasBeenDisposed => onceExecutedAction.HasBeenExecuted;

        public Task<bool> TryDisposeAsync() => onceExecutedAction.ExecuteIfFirstTimeAsync();

        private Task DisposeCore() => component.DisposeAsync().AsTask();
    }

    public class AsyncDisposableComponentFactory : IAsyncDisposableComponentFactory
    {
        private readonly IOnceExecutedAsyncActionFactory onceExecutedActionFactory;

        public AsyncDisposableComponentFactory(IOnceExecutedAsyncActionFactory onceExecutedActionFactory)
        {
            this.onceExecutedActionFactory = onceExecutedActionFactory ?? throw new ArgumentNullException(nameof(onceExecutedActionFactory));
        }

        public IAsyncDisposableComponent<TComponent> Create<TComponent>(
            TComponent component)
            where TComponent : class, IAsyncDisposable => new AsyncDisposableComponent<TComponent>(
                onceExecutedActionFactory,
                component);
    }
}
