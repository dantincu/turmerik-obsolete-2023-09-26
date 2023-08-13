using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.Ux.MvvmH;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.ViewModels;

namespace Turmerik.ObjectViewer.WinFormsApp.ViewModels
{
    public interface IMainFormVM : IViewModelCore<MainFormState>
    {
    }

    public class MainFormVM : ViewModelBase<MainFormState>, IMainFormVM
    {
        public MainFormVM(
            IAppLoggerCreator appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }
    }

    public class MainFormState
    {
    }
}
