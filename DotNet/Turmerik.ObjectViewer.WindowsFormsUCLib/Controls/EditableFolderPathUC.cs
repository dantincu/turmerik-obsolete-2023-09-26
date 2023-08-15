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
using Turmerik.Utils;
using Turmerik.WinForms.Controls;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    public partial class EditableFolderPathUC : UserControl
    {
        private Action<MutableValueWrapper<string>> folderPathChosen;

        public EditableFolderPathUC()
        {
            InitializeComponent();

            iconLabelBrowseFolderPath.Text = Unicodes.FolderOpen;
            iconLabelRefreshFolder.Text = Unicodes.Refresh;
            iconLabelExecuteFolder.Text = Unicodes.PlayCircleFilled;

            iconLabelBrowseFolderPath.Click += IconLabelBrowseFolderPath_Click;
        }

        public Label LabelTitle => labelTitle;
        public TextBox TextBoxFolderPath => textBoxFolderPath;
        public IconLabel ButtonBrowseFolderPath => iconLabelBrowseFolderPath;
        public IconLabel ButtonRefreshFolder => iconLabelRefreshFolder;
        public IconLabel ButtonExecuteFolder => iconLabelExecuteFolder;

        public FolderBrowserDialog FolderBrowserDialog => folderBrowserDialog;

        public event Action<MutableValueWrapper<string>> FolderPathChosen
        {
            add => folderPathChosen += value;
            remove => folderPathChosen -= value;
        }

        #region Event Handlers

        private void IconLabelBrowseFolderPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var mtbl = new MutableValueWrapper<string>
                {
                    Value = folderBrowserDialog.SelectedPath
                };

                folderPathChosen?.Invoke(mtbl);
                textBoxFolderPath.Text = mtbl.Value;
            }
        }

        #endregion Event Handlers
    }
}
