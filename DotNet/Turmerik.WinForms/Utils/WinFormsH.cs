using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turmerik.WinForms.Utils
{
    public static class WinFormsH
    {
        public static void InvokeIfReq(
            this Control control,
            Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
        }
    }
}
