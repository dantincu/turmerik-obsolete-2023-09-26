using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public interface ITextLinesIndenterVM
    {
    }

    public class TextLinesIndenterVM : ViewModelBase, ITextLinesIndenterVM
    {
        public TextLinesIndenterVM(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }
    }
}
