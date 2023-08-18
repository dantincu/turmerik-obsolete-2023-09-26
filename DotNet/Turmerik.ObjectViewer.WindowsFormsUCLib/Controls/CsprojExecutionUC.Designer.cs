namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    partial class CsprojExecutionUC
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxProjectAsssemblies = new System.Windows.Forms.GroupBox();
            this.richTextBoxCodeFileContents = new System.Windows.Forms.RichTextBox();
            this.editableFolderPathUCCsprojFile = new Turmerik.ObjectViewer.WindowsFormsUCLib.Controls.EditableFolderPathUC();
            this.filePathUCCurrentCodeFile = new Turmerik.ObjectViewer.WindowsFormsUCLib.Controls.FilePathUC();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxProjectAsssemblies);
            this.splitContainerMain.Panel1.Controls.Add(this.editableFolderPathUCCsprojFile);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.richTextBoxCodeFileContents);
            this.splitContainerMain.Panel2.Controls.Add(this.filePathUCCurrentCodeFile);
            this.splitContainerMain.Size = new System.Drawing.Size(1792, 874);
            this.splitContainerMain.SplitterDistance = 896;
            this.splitContainerMain.SplitterWidth = 1;
            this.splitContainerMain.TabIndex = 0;
            // 
            // groupBoxProjectAsssemblies
            // 
            this.groupBoxProjectAsssemblies.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxProjectAsssemblies.Location = new System.Drawing.Point(0, 674);
            this.groupBoxProjectAsssemblies.Name = "groupBoxProjectAsssemblies";
            this.groupBoxProjectAsssemblies.Size = new System.Drawing.Size(896, 200);
            this.groupBoxProjectAsssemblies.TabIndex = 10;
            this.groupBoxProjectAsssemblies.TabStop = false;
            this.groupBoxProjectAsssemblies.Text = "Script Assemblies";
            // 
            // richTextBoxCodeFileContents
            // 
            this.richTextBoxCodeFileContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxCodeFileContents.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBoxCodeFileContents.Location = new System.Drawing.Point(0, 21);
            this.richTextBoxCodeFileContents.Name = "richTextBoxCodeFileContents";
            this.richTextBoxCodeFileContents.ReadOnly = true;
            this.richTextBoxCodeFileContents.Size = new System.Drawing.Size(895, 853);
            this.richTextBoxCodeFileContents.TabIndex = 2;
            this.richTextBoxCodeFileContents.Text = "";
            // 
            // editableFolderPathUCCsprojFile
            // 
            this.editableFolderPathUCCsprojFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.editableFolderPathUCCsprojFile.Location = new System.Drawing.Point(0, 0);
            this.editableFolderPathUCCsprojFile.Name = "editableFolderPathUCCsprojFile";
            this.editableFolderPathUCCsprojFile.Size = new System.Drawing.Size(896, 21);
            this.editableFolderPathUCCsprojFile.TabIndex = 0;
            // 
            // filePathUCCurrentCodeFile
            // 
            this.filePathUCCurrentCodeFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.filePathUCCurrentCodeFile.Location = new System.Drawing.Point(0, 0);
            this.filePathUCCurrentCodeFile.Name = "filePathUCCurrentCodeFile";
            this.filePathUCCurrentCodeFile.Size = new System.Drawing.Size(895, 21);
            this.filePathUCCurrentCodeFile.TabIndex = 0;
            // 
            // CsprojExecutionUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "CsprojExecutionUC";
            this.Size = new System.Drawing.Size(1792, 874);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private EditableFolderPathUC editableFolderPathUCCsprojFile;
        private System.Windows.Forms.GroupBox groupBoxProjectAsssemblies;
        private FilePathUC filePathUCCurrentCodeFile;
        private System.Windows.Forms.RichTextBox richTextBoxCodeFileContents;
    }
}
