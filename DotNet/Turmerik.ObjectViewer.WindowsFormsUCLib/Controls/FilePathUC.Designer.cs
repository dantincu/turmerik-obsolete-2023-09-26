namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    partial class FilePathUC
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
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.iconLabelRefreshFile = new Turmerik.WinForms.Controls.IconLabel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilePath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxFilePath.Location = new System.Drawing.Point(54, 0);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(211, 22);
            this.textBoxFilePath.TabIndex = 5;
            // 
            // iconLabelRefreshFile
            // 
            this.iconLabelRefreshFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelRefreshFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.iconLabelRefreshFile.FontFamily = null;
            this.iconLabelRefreshFile.Location = new System.Drawing.Point(265, 0);
            this.iconLabelRefreshFile.Name = "iconLabelRefreshFile";
            this.iconLabelRefreshFile.Size = new System.Drawing.Size(35, 21);
            this.iconLabelRefreshFile.TabIndex = 7;
            this.iconLabelRefreshFile.Text = "R";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(3);
            this.labelTitle.Size = new System.Drawing.Size(54, 19);
            this.labelTitle.TabIndex = 4;
            this.labelTitle.Text = "File Path";
            // 
            // FilePathUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.iconLabelRefreshFile);
            this.Controls.Add(this.labelTitle);
            this.Name = "FilePathUC";
            this.Size = new System.Drawing.Size(300, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilePath;
        private WinForms.Controls.IconLabel iconLabelRefreshFile;
        private System.Windows.Forms.Label labelTitle;
    }
}
