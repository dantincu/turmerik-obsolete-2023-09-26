using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Logging;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionComponentFactory
    {
        ITrmrkActionComponent Create(IAppLogger logger);
    }

    public class TrmrkActionComponentFactory : ITrmrkActionComponentFactory
    {
        public TrmrkActionComponentFactory(
            ITrmrkActionComponentsManagerFactoryCore managerFactory)
        {
            Manager = managerFactory.Create();
        }

        protected ITrmrkActionComponentsManagerCore Manager { get; }

        public ITrmrkActionComponent Create(
            IAppLogger logger) => new TrmrkActionComponent(Manager, logger);
    }
}
