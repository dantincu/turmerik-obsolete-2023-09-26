using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.Ux.MvvmH;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public abstract class ViewModelBase : ViewModelCoreBase
    {
        protected ViewModelBase(
            IAppLoggerCreator appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(appLoggerFactory, actionComponentFactory)
        {
            ActionComponent = (IWinFormsActionComponent)ActionComponentCore;
        }

        protected IWinFormsActionComponent ActionComponent { get; }
    }

    public abstract class ViewModelBase<TState> : ViewModelCoreBase<TState>
    {
        protected ViewModelBase(
            IAppLoggerCreator appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(appLoggerFactory, actionComponentFactory)
        {
            ActionComponent = (IWinFormsActionComponent)ActionComponentCore;
        }

        protected IWinFormsActionComponent ActionComponent { get; }
    }
}
