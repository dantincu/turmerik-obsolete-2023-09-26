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
    public interface ITrmrkWinFormsActionComponentFactory : ITrmrkActionComponentFactory
    {
        ITrmrkWinFormsActionComponent Create(IAppLogger logger);
    }

    public class TrmrkWinFormsActionComponentFactory : ITrmrkWinFormsActionComponentFactory
    {
        private readonly ITrmrkWinFormsActionComponentsManager manager;

        public TrmrkWinFormsActionComponentFactory(
            ITrmrkWinFormsActionComponentsManagerRetriever actionComponentManagerRetriever)
        {
            this.manager = actionComponentManagerRetriever.Retrieve();
        }

        public ITrmrkWinFormsActionComponent Create(
            IAppLogger logger) => new TrmrkWinFormsActionComponent(manager, logger);

        public ITrmrkActionComponent CreateCore(
            IAppLogger logger) => Create(logger);
    }
}
