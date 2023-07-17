using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public interface ITabsToMdTableCellsConverterVM
    {
    }

    public class TabsToMdTableCellsConverterVM : ViewModelBase, ITabsToMdTableCellsConverterVM
    {
        public TabsToMdTableCellsConverterVM(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }
    }
}
