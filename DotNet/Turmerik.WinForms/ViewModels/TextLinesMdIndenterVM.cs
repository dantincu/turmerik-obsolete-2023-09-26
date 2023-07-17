using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public interface ITextLinesMdIndenterVM
    {
    }

    public class TextLinesMdIndenterVM : ViewModelBase, ITextLinesMdIndenterVM
    {
        public TextLinesMdIndenterVM(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }
    }
}
