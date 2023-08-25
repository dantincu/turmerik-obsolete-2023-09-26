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

        private Action<MutableValueWrapper<string>> folderPathChanged;

        public EditableFolderPathUC()
        {
            InitializeComponent();

            textBoxFolderPathMultiStateStyle = GetTextBoxFolderPathMultiStateStyle();

            iconLabelBrowseFolderPath.Text = Unicodes.FolderOpen;
            iconLabelRefreshFolder.Text = Unicodes.Refresh;
            iconLabelExecuteFolder.Text = Unicodes.PlayCircleFilled;
            iconLabelEdit.Text = Unicodes.Edit;
            iconLabelSubmitChanges.Text = Unicodes.Done;
            iconLabelUndoChanges.Text = Unicodes.Cancel;

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

        public bool HasChanges { get; private set; }

        public event Action<MutableValueWrapper<string>> FolderPathChanged
        {
            add => folderPathChanged += value;
            remove => folderPathChanged -= value;
        }

        public void SetFolderPath(string folderPath)
        {
            textBoxFolderPath.Text = folderPath;
            FolderPath = folderPath;

            OnFolderPathChanged(folderPath);
        }

        private void OnFolderPathChanged(
            string newFolderPath)
        {
            var mtbl = new MutableValueWrapper<string>
            {
                Value = newFolderPath,
            };

            folderPathChanged?.Invoke(mtbl);

            if (mtbl.Value != FolderPath)
            {
                FolderPath = mtbl.Value;
                textBoxFolderPath.Text = mtbl.Value;
            }
        }

        private void ToggleEditMode(bool edit)
        {
            bool @readonly = !edit;
            textBoxFolderPath.ReadOnly = @readonly;
            textBoxFolderPath.BackColor = edit ? Color.White : SystemColors.Control;

            iconLabelBrowseFolderPath.Visible = @readonly;
            iconLabelRefreshFolder.Visible = @readonly;
            iconLabelExecuteFolder.Visible = @readonly;
            iconLabelEdit.Visible = @readonly;

            iconLabelSubmitChanges.Visible = edit;
            iconLabelUndoChanges.Visible = edit;

            var newState = edit.IfTrue(
                () => EditingState.HasNoChanges,
                () => EditingState.None);

            textBoxFolderPathMultiStateStyle.SetState(newState);
        }

        private void StartChanges()
        {
            ToggleEditMode(true);
        }

        private void SubmitChanges()
        {
            FolderPath = textBoxFolderPath.Text;
            ToggleEditMode(false);

            OnFolderPathChanged(FolderPath);
        }

        private void RevertChanges()
        {
            textBoxFolderPath.Text = FolderPath;
            ToggleEditMode(false);
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

                                    style.BackColor = Color.White;
                                }).ToImmtbl()
                            },
                            {
                                EditingState.HasChanges,
                                initialStyle.ToMtbl().ActWithValue(style =>
                                {
                                    style.FontStyle = style.FontStyle | FontStyle.Italic;
                                    style.BackColor = Color.White;
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
                SetFolderPath(folderBrowserDialog.SelectedPath);
            }
        }

        private void IconLabelEdit_Click(object sender, EventArgs e)
        {
            StartChanges();
        }

        private void IconLabelSubmitChanges_Click(object sender, EventArgs e)
        {
            SubmitChanges();
        }

        private void IconLabelUndoChanges_Click(object sender, EventArgs e)
        {
            RevertChanges();
        }

        private void TextBoxFolderPath_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFolderPathMultiStateStyle.State != EditingState.None)
            {
                bool hasChanges = textBoxFolderPath.Text != FolderPath;
                HasChanges = hasChanges;

                var newState = hasChanges.IfTrue(
                    () => EditingState.HasChanges,
                    () => EditingState.HasNoChanges);

                textBoxFolderPathMultiStateStyle.SetState(newState);
            }
        }

        private void TextBoxFolderPath_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void TextBoxFolderPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                SubmitChanges();
            }
            else if (e.KeyData == Keys.Escape)
            {
                RevertChanges();
            }
        }

        #endregion Event Handlers
    }
}
