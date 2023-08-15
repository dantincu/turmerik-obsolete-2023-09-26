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
using Turmerik.WinForms.Controls;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    public partial class FilePathUC : UserControl
    {
        public FilePathUC()
        {
            InitializeComponent();

            iconLabelRefreshFile.Text = Unicodes.Refresh;
        }

        public Label LabelTitle => labelTitle;
        public TextBox TextBoxFilePath => textBoxFilePath;
        public IconLabel ButtonRefreshFile => iconLabelRefreshFile;
    }
}
