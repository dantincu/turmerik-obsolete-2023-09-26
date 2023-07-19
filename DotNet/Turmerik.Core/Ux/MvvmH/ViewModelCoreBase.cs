using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Logging;
using Turmerik.TrmrkAction;

namespace Turmerik.Ux.MvvmH
{
    public interface IViewModelCore
    {
        object GetState();
    }

    public interface IViewModelCore<TState> : IViewModelCore
    {
        TState State { get; }

        void SetState(TState state);
    }

    public interface IViewModelControlCore<TViewModel>
        where TViewModel : IViewModelCore
    {
        TViewModel ViewModel { get; }
    }

    public interface IViewModelControlCore<TViewModel, TState> : IViewModelControlCore<TViewModel>
        where TViewModel : IViewModelCore<TState>
    {
        void SetViewModelState(TState state);
    }

    public abstract class ViewModelCoreBase
    {
        protected ViewModelCoreBase(
            IAppLoggerCreator appLoggerFactory,
            ITrmrkActionComponentFactory actionComponentFactory)
        {
            Logger = appLoggerFactory.GetSharedAppLogger(GetType());
            ActionComponentCore = actionComponentFactory.CreateCore(Logger);
        }

        protected IAppLogger Logger { get; }
        protected ITrmrkActionComponent ActionComponentCore { get; }
    }

    public abstract class ViewModelCoreBase<TState> : ViewModelCoreBase, IViewModelCore<TState>
    {
        protected ViewModelCoreBase(
            IAppLoggerCreator appLoggerFactory,
            ITrmrkActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }

        public virtual TState State { get; protected set; }

        public object GetState() => State;

        public virtual void SetState(TState state)
        {
            State = state;
        }
    }
}
