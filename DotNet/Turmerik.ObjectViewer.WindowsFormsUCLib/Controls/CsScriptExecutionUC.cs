using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Text;
using Turmerik.WinForms.Utils;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    public partial class CsScriptExecutionUC : UserControl
    {
        public CsScriptExecutionUC()
        {
            InitializeComponent();

            clickToggleIconLabelSampleScript.SetExpandMoreLess();
            iconLabelExecuteScript.Text = Unicodes.PlayCircleFilled;
        }

        #region UI Event Handlers

        private void ClickToggleIconLabelSampleScript_ClickToggled(bool obj)
        {
            richTextBoxSampleScript.Visible = obj;
        }

        #endregion UI Event Handlers
    }
}
