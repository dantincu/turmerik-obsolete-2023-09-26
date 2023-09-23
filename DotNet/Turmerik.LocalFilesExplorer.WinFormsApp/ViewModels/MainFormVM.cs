using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Ux.MvvmH;
using Turmerik.WinForms.ViewModels;

namespace Turmerik.LocalFilesExplorer.WinFormsApp.ViewModels
{
    public interface IMainFormVM : IViewModelCore<MainFormState>
    {
    }

    public class MainFormVM : TrmrkViewModelBase<MainFormState>, IMainFormVM
    {
        public MainFormVM(
            IAppLoggerCreator appLoggerFactory/* ,
            ITrmrkWinFormsActionComponentFactory actionComponentFactory*/) : base(
                appLoggerFactory/* ,
                actionComponentFactory*/)
        {
        }
    }

    public class MainFormState
    {
    }
}
