using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.TrmrkAction;

namespace Turmerik.Avalonia.ActionComponent
{
    public interface ITrmrkAvlnActionComponent : ITrmrkActionComponent
    {
    }

    public class TrmrkAvlnActionComponent : TrmrkActionComponent<ITrmrkAvlnActionComponentsManager>, ITrmrkAvlnActionComponent
    {
        public TrmrkAvlnActionComponent(
            ITrmrkAvlnActionComponentsManager manager,
            IAppLogger logger) : base(
                manager,
                logger)
        {
        }
    }
}
