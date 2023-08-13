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
    public interface IObjectViewerVM : IViewModelCore<ObjectViewerState>
    {
    }

    public class ObjectViewerVM : ViewModelBase<ObjectViewerState>, IObjectViewerVM
    {
        public ObjectViewerVM(
            IAppLoggerCreator appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }
    }

    public class ObjectViewerState
    {
    }
}
