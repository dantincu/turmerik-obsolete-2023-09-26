using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Synchronized
{
    public interface IDisposableComponent<TComponent>
    {
        TComponent Component { get; }
        bool HasBeenDisposed { get; }

        bool TryDispose();
    }

    public interface IDisposableComponentFactory
    {
        IDisposableComponent<TComponent> Create<TComponent>(
            TComponent component)
            where TComponent : class, IDisposable;
    }

    public class DisposableComponent<TComponent> : IDisposableComponent<TComponent>
        where TComponent : class, IDisposable
    {
        private readonly IOnceExecutedAction onceExecutedAction;
        private readonly TComponent component;

        public DisposableComponent(
            IOnceExecutedActionFactory onceExecutedActionFactory,
            TComponent component)
        {
            this.onceExecutedAction = onceExecutedActionFactory.Create(DisposeCore);
            this.component = component ?? throw new ArgumentNullException();
        }

        public TComponent Component => component;
        public bool HasBeenDisposed => onceExecutedAction.HasBeenExecuted;

        public bool TryDispose() => onceExecutedAction.ExecuteIfFirstTime();

        private void DisposeCore()
        {
            component?.Dispose();
        }
    }

    public class DisposableComponentFactory : IDisposableComponentFactory
    {
        private readonly IOnceExecutedActionFactory onceExecutedActionFactory;

        public DisposableComponentFactory(IOnceExecutedActionFactory onceExecutedActionFactory)
        {
            this.onceExecutedActionFactory = onceExecutedActionFactory ?? throw new ArgumentNullException(nameof(onceExecutedActionFactory));
        }

        public IDisposableComponent<TComponent> Create<TComponent>(
            TComponent component)
            where TComponent : class, IDisposable => new DisposableComponent<TComponent>(
                onceExecutedActionFactory,
                component);
    }
}
