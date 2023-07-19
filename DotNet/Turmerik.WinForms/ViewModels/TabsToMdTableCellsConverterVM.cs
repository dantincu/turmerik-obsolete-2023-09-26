using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.Text.MdH;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public interface ITabsToMdTableCellsConverterVM
    {
        ITrmrkActionResult<string> LinesToMdTable(
            string linesStr,
            bool firstLineIsHeader = false);
    }

    public class TabsToMdTableCellsConverterVM : ViewModelBase, ITabsToMdTableCellsConverterVM
    {
        private readonly ITabsToMdTableConverter tabsToMdTable;

        public TabsToMdTableCellsConverterVM(
            IAppLoggerCreator appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory,
            ITabsToMdTableConverter tabsToMdTable) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
            this.tabsToMdTable = tabsToMdTable ?? throw new ArgumentNullException(nameof(tabsToMdTable));
        }

        public ITrmrkActionResult<string> LinesToMdTable(
            string linesStr,
            bool firstLineIsHeader = false) => ActionComponent.Execute(new TrmrkActionComponentOpts<string>
            {
                Action = () => new TrmrkActionResult<string>
                {
                    Data = tabsToMdTable.LinesToMdTable(
                        linesStr,
                        firstLineIsHeader)
                },
                ActionName = nameof(LinesToMdTable),
            });
    }
}
