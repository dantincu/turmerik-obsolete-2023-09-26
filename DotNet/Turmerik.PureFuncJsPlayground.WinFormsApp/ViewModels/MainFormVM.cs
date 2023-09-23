using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.WinForms.ViewModels;

namespace Turmerik.PureFuncJsPlayground.WinFormsApp.ViewModels
{
    public interface IMainFormVM
    {
    }

    public class MainFormVM : ViewModelBase, IMainFormVM
    {
        public MainFormVM(
            IAppLoggerCreator appLoggerFactory/* ,
            IWinFormsActionComponentFactory actionComponentFactory*/) : base(
                appLoggerFactory/*,
                actionComponentFactory*/)
        {
        }
    }
}
