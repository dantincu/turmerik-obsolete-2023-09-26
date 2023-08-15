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
    public partial class EditableFilePathUC : UserControl
    {
        private Action<MutableValueWrapper<string>> filePathChosen;

        public EditableFilePathUC()
        {
            InitializeComponent();

            iconLabelBrowseFilePath.Text = Unicodes.FolderOpen;
            iconLabelRefreshFile.Text = Unicodes.Refresh;
            iconLabelExecuteFile.Text = Unicodes.PlayCircleFilled;

            iconLabelBrowseFilePath.Click += IconLabelBrowseFilePath_Click;
        }

        public Label LabelTitle => labelTitle;
        public TextBox TextBoxFilePath => textBoxFilePath;
        public IconLabel ButtonBrowseFilePath => iconLabelBrowseFilePath;
        public IconLabel ButtonRefreshFile => iconLabelRefreshFile;
        public IconLabel ButtonExecuteFile => iconLabelExecuteFile;

        public OpenFileDialog OpenFileDialog => openFileDialog;

        public event Action<MutableValueWrapper<string>> FilePathChosen
        {
            add => filePathChosen += value;
            remove => filePathChosen -= value;
        }

        #region Event Handlers

        private void IconLabelBrowseFilePath_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var mtbl = new MutableValueWrapper<string>
                {
                    Value = openFileDialog.FileName
                };

                filePathChosen?.Invoke(mtbl);
                textBoxFilePath.Text = mtbl.Value;
            }
        }

        #endregion Event Handlers
    }
}
