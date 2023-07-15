using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turmerik.WinForms.ActionComponent
{
    public interface IWinFormsActionComponent
    {

    }

    public class WinFormsActionComponent : IWinFormsActionComponent
    {
        public WinFormsActionComponent(Control control)
        {
            Control = control ?? throw new ArgumentNullException(nameof(control));
        }

        protected Control Control { get; }


    }
}
