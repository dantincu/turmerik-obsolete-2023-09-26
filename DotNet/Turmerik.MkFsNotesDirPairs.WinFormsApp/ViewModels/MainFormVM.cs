using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.ViewModels;

namespace Turmerik.MkFsNotesDirPairs.WinFormsApp.ViewModels
{
    public interface IMainFormVM
    {
    }

    public class MainFormVM : ViewModelBase, IMainFormVM
    {
        public MainFormVM(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }
    }
}
