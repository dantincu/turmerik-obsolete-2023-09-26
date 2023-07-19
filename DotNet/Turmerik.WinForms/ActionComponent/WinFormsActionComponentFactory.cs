using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Logging;
using Turmerik.TrmrkAction;

namespace Turmerik.WinForms.ActionComponent
{
    public interface IWinFormsActionComponentFactory : ITrmrkActionComponentFactory
    {
        IWinFormsActionComponent Create(IAppLogger logger);
    }

    public class WinFormsActionComponentFactory : IWinFormsActionComponentFactory
    {
        private readonly IWinFormsActionComponentsManager manager;

        public WinFormsActionComponentFactory(
            IWinFormsActionComponentsManagerRetriever actionComponentManagerRetriever)
        {
            this.manager = actionComponentManagerRetriever.Retrieve();
        }

        public IWinFormsActionComponent Create(
            IAppLogger logger) => new WinFormsActionComponent(manager, logger);

        public ITrmrkActionComponent CreateCore(
            IAppLogger logger) => Create(logger);
    }
}
