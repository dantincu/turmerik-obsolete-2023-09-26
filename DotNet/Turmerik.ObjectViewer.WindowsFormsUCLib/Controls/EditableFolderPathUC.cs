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
using Turmerik.WinForms.Components;
using Turmerik.WinForms.Controls;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    public partial class EditableFolderPathUC : UserControl
    {
        private readonly MultiStateControlStyle<TextBox, EditingState> textBoxFolderPathMultiStateStyle;

        private Action<MutableValueWrapper<string>> folderPathChosen;

        public EditableFolderPathUC()
        {
            InitializeComponent();

            textBoxFolderPathMultiStateStyle = GetTextBoxFolderPathMultiStateStyle();

            iconLabelBrowseFolderPath.Text = Unicodes.FolderOpen;
            iconLabelRefreshFolder.Text = Unicodes.Refresh;
            iconLabelExecuteFolder.Text = Unicodes.PlayCircleFilled;

            iconLabelBrowseFolderPath.Click += IconLabelBrowseFolderPath_Click;
            iconLabelEdit.Click += IconLabelEdit_Click;
            iconLabelSubmitChanges.Click += IconLabelSubmitChanges_Click;
            iconLabelUndoChanges.Click += IconLabelUndoChanges_Click;
        }

        public Label LabelTitle => labelTitle;
        public IconLabel ButtonBrowseFolderPath => iconLabelBrowseFolderPath;
        public IconLabel ButtonRefreshFolder => iconLabelRefreshFolder;
        public IconLabel ButtonExecuteFolder => iconLabelExecuteFolder;

        public FolderBrowserDialog FolderBrowserDialog => folderBrowserDialog;

        public string FolderPath { get; private set; }

        public event Action<MutableValueWrapper<string>> FolderPathChosen
        {
            add => folderPathChosen += value;
            remove => folderPathChosen -= value;
        }

        private void ToggleEditMode(bool edit)
        {
            textBoxFolderPath.ReadOnly = !edit;
            iconLabelUndoChanges.Visible = edit;

            var newState = edit.IfTrue(
                () => EditingState.HasChanges,
                () => EditingState.None);

            textBoxFolderPathMultiStateStyle.SetState(newState);
        }

        private MultiStateControlStyle<TextBox, EditingState> GetTextBoxFolderPathMultiStateStyle(
            ) => new MultiStateControlStyle<TextBox, EditingState>(
                textBoxFolderPath,
                ControlStyle.Mtbl.FromControl(
                    textBoxFolderPath).WithValue(initialStyle =>
                        new Dictionary<EditingState, ControlStyle.Immtbl>
                        {
                            {
                                EditingState.HasNoChanges,
                                initialStyle.ToMtbl().ActWithValue(style =>
                                {
                                    style.FontStyle = FlagsH.SubstractFlagIfReq(
                                        style.FontStyle,
                                        FontStyle.Italic);
                                }).ToImmtbl()
                            },
                            {
                                EditingState.HasChanges,
                                initialStyle.ToMtbl().ActWithValue(style =>
                                {
                                    style.FontStyle = style.FontStyle | FontStyle.Italic;
                                }).ToImmtbl()
                            }
                        }.WithValue(map => map.ToDictionary<KeyValuePair<EditingState, ControlStyle.Immtbl>, EditingState, Func<TextBox, EditingState, ControlStyle.Immtbl>>(
                            kvp => kvp.Key,
                            kvp => (control, state) => kvp.Value))));

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

        private void IconLabelEdit_Click(object sender, EventArgs e)
        {
            ToggleEditMode(true);
        }

        private void IconLabelSubmitChanges_Click(object sender, EventArgs e)
        {
            ToggleEditMode(false);
        }

        private void IconLabelUndoChanges_Click(object sender, EventArgs e)
        {
            ToggleEditMode(false);
        }

        private void TextBoxFolderPath_TextChanged(object sender, EventArgs e)
        {
            bool hasChanges = textBoxFolderPath.Text == FolderPath;
            HasChanges = hasChanges;

            var newState = hasChanges.IfTrue(
                () => EditingState.HasChanges,
                () => EditingState.HasNoChanges);

            textBoxFolderPathMultiStateStyle.SetState(newState);
        }

        #endregion Event Handlers
    }
}
