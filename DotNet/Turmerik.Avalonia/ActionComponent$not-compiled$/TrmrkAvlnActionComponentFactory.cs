using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.TrmrkAction;

namespace Turmerik.Avalonia.ActionComponent
{
    public interface ITrmrkAvlnActionComponentFactory : ITrmrkActionComponentFactory
    {
        ITrmrkAvlnActionComponent Create(IAppLogger logger);
    }

    public class TrmrkAvlnActionComponentFactory : ITrmrkAvlnActionComponentFactory
    {
        public TrmrkAvlnActionComponentFactory(
            ITrmrkAvlnActionComponentsManagerRetriever actionComponentManagerRetriever)
        {
            ActionComponentManagerRetriever = actionComponentManagerRetriever ?? throw new ArgumentNullException(
                nameof(actionComponentManagerRetriever));
        }

        protected ITrmrkAvlnActionComponentsManagerRetriever ActionComponentManagerRetriever { get; }

        public ITrmrkAvlnActionComponent Create(
            IAppLogger logger) => new TrmrkAvlnActionComponent(
                ActionComponentManagerRetriever.Retrieve(),
                logger);

        public ITrmrkActionComponent CreateCore(
            IAppLogger logger) => Create(logger);
    }
}
