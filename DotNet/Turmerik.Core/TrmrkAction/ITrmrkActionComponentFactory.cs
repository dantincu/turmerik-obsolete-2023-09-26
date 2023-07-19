using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Logging;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionComponentFactory
    {
        ITrmrkActionComponent CreateCore(IAppLogger logger);
    }
}
