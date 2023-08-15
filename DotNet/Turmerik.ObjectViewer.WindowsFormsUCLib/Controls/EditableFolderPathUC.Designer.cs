﻿namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    partial class EditableFolderPathUC
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
            this.textBoxFolderPath = new System.Windows.Forms.TextBox();
            this.iconLabelBrowseFolderPath = new Turmerik.WinForms.Controls.IconLabel();
            this.iconLabelRefreshFolder = new Turmerik.WinForms.Controls.IconLabel();
            this.iconLabelExecuteFolder = new Turmerik.WinForms.Controls.IconLabel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // textBoxFolderPath
            // 
            this.textBoxFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFolderPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxFolderPath.Location = new System.Drawing.Point(67, 0);
            this.textBoxFolderPath.Name = "textBoxFolderPath";
            this.textBoxFolderPath.Size = new System.Drawing.Size(128, 22);
            this.textBoxFolderPath.TabIndex = 10;
            // 
            // iconLabelBrowseFolderPath
            // 
            this.iconLabelBrowseFolderPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelBrowseFolderPath.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelBrowseFolderPath.FontFamily = null;
            this.iconLabelBrowseFolderPath.Location = new System.Drawing.Point(195, 0);
            this.iconLabelBrowseFolderPath.Name = "iconLabelBrowseFolderPath";
            this.iconLabelBrowseFolderPath.Size = new System.Drawing.Size(35, 21);
            this.iconLabelBrowseFolderPath.TabIndex = 11;
            this.iconLabelBrowseFolderPath.Text = "B";
            // 
            // iconLabelRefreshFolder
            // 
            this.iconLabelRefreshFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelRefreshFolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelRefreshFolder.FontFamily = null;
            this.iconLabelRefreshFolder.Location = new System.Drawing.Point(230, 0);
            this.iconLabelRefreshFolder.Name = "iconLabelRefreshFolder";
            this.iconLabelRefreshFolder.Size = new System.Drawing.Size(35, 21);
            this.iconLabelRefreshFolder.TabIndex = 13;
            this.iconLabelRefreshFolder.Text = "R";
            // 
            // iconLabelExecuteFolder
            // 
            this.iconLabelExecuteFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelExecuteFolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelExecuteFolder.FontFamily = null;
            this.iconLabelExecuteFolder.Location = new System.Drawing.Point(265, 0);
            this.iconLabelExecuteFolder.Name = "iconLabelExecuteFolder";
            this.iconLabelExecuteFolder.Size = new System.Drawing.Size(35, 21);
            this.iconLabelExecuteFolder.TabIndex = 12;
            this.iconLabelExecuteFolder.Text = "E";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(3);
            this.labelTitle.Size = new System.Drawing.Size(67, 19);
            this.labelTitle.TabIndex = 9;
            this.labelTitle.Text = "Folder Path";
            // 
            // EditableFolderPathUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxFolderPath);
            this.Controls.Add(this.iconLabelBrowseFolderPath);
            this.Controls.Add(this.iconLabelRefreshFolder);
            this.Controls.Add(this.iconLabelExecuteFolder);
            this.Controls.Add(this.labelTitle);
            this.Name = "EditableFolderPathUC";
            this.Size = new System.Drawing.Size(300, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFolderPath;
        private WinForms.Controls.IconLabel iconLabelBrowseFolderPath;
        private WinForms.Controls.IconLabel iconLabelRefreshFolder;
        private WinForms.Controls.IconLabel iconLabelExecuteFolder;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
