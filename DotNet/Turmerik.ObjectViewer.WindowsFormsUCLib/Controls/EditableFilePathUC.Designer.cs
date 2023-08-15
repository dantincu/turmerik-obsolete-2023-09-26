using Turmerik.WinForms.Controls;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    partial class EditableFilePathUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.iconLabelBrowseFilePath = new Turmerik.WinForms.Controls.IconLabel();
            this.iconLabelExecuteFile = new Turmerik.WinForms.Controls.IconLabel();
            this.iconLabelRefreshFile = new Turmerik.WinForms.Controls.IconLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(3);
            this.labelTitle.Size = new System.Drawing.Size(54, 19);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "File Path";
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilePath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxFilePath.Location = new System.Drawing.Point(54, 0);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(141, 22);
            this.textBoxFilePath.TabIndex = 1;
            // 
            // iconLabelBrowseFilePath
            // 
            this.iconLabelBrowseFilePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelBrowseFilePath.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelBrowseFilePath.FontFamily = null;
            this.iconLabelBrowseFilePath.Location = new System.Drawing.Point(195, 0);
            this.iconLabelBrowseFilePath.Name = "iconLabelBrowseFilePath";
            this.iconLabelBrowseFilePath.Size = new System.Drawing.Size(35, 21);
            this.iconLabelBrowseFilePath.TabIndex = 2;
            this.iconLabelBrowseFilePath.Text = "B";
            // 
            // iconLabelExecuteFile
            // 
            this.iconLabelExecuteFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelExecuteFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelExecuteFile.FontFamily = null;
            this.iconLabelExecuteFile.Location = new System.Drawing.Point(265, 0);
            this.iconLabelExecuteFile.Name = "iconLabelExecuteFile";
            this.iconLabelExecuteFile.Size = new System.Drawing.Size(35, 21);
            this.iconLabelExecuteFile.TabIndex = 3;
            this.iconLabelExecuteFile.Text = "E";
            // 
            // iconLabelRefreshFile
            // 
            this.iconLabelRefreshFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelRefreshFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelRefreshFile.FontFamily = null;
            this.iconLabelRefreshFile.Location = new System.Drawing.Point(230, 0);
            this.iconLabelRefreshFile.Name = "iconLabelRefreshFile";
            this.iconLabelRefreshFile.Size = new System.Drawing.Size(35, 21);
            this.iconLabelRefreshFile.TabIndex = 8;
            this.iconLabelRefreshFile.Text = "R";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // EditableFilePathUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.iconLabelBrowseFilePath);
            this.Controls.Add(this.iconLabelRefreshFile);
            this.Controls.Add(this.iconLabelExecuteFile);
            this.Controls.Add(this.labelTitle);
            this.Name = "EditableFilePathUC";
            this.Size = new System.Drawing.Size(300, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private IconLabel iconLabelBrowseFilePath;
        private IconLabel iconLabelExecuteFile;
        private IconLabel iconLabelRefreshFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
