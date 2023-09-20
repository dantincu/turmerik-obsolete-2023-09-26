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
    public abstract class TrmrkViewModelBase : ViewModelCoreBase
    {
        protected TrmrkViewModelBase(
            IAppLoggerCreator appLoggerFactory,
            ITrmrkWinFormsActionComponentFactory actionComponentFactory) : base(appLoggerFactory, actionComponentFactory)
        {
            ActionComponent = (ITrmrkWinFormsActionComponent)ActionComponentCore;
        }

        protected ITrmrkWinFormsActionComponent ActionComponent { get; }
    }

    public abstract class TrmrkViewModelBase<TState> : ViewModelCoreBase<TState>
    {
        protected TrmrkViewModelBase(
            IAppLoggerCreator appLoggerFactory,
            ITrmrkWinFormsActionComponentFactory actionComponentFactory) : base(appLoggerFactory, actionComponentFactory)
        {
            ActionComponent = (ITrmrkWinFormsActionComponent)ActionComponentCore;
        }

        protected ITrmrkWinFormsActionComponent ActionComponent { get; }
    }
}
