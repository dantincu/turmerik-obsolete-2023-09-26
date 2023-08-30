using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Logging;
using Turmerik.TrmrkAction;
using Turmerik.Utils;

namespace Turmerik.WinForms.ActionComponent
{
    public interface ITrmrkWinFormsActionComponent : ITrmrkActionComponent
    {
    }

    public class TrmrkWinFormsActionComponent : TrmrkActionComponent<ITrmrkWinFormsActionComponentsManager>, ITrmrkWinFormsActionComponent
    {
        public TrmrkWinFormsActionComponent(
            ITrmrkWinFormsActionComponentsManager manager,
            IAppLogger logger) : base(
                manager,
                logger)
        {
        }
    }
}
