using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turmerik.WinForms.ActionComponent
{
    public interface IWinFormsActionComponentFactory
    {
        IWinFormsActionComponent Create(Control control);
    }

    public class WinFormsActionComponentFactory : IWinFormsActionComponentFactory
    {
        public IWinFormsActionComponent Create(Control control) => new WinFormsActionComponent(control);
    }
}
