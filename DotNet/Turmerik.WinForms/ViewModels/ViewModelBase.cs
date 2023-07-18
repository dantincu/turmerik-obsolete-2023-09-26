using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.Logging;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public abstract class ViewModelBase
    {
        protected ViewModelBase(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory)
        {
            Logger = appLoggerFactory.GetSharedAppLogger(GetType());
            ActionComponent = actionComponentFactory.Create(Logger);
        }

        protected IAppLogger Logger { get; }
        protected IWinFormsActionComponent ActionComponent { get; }
    }
}
