namespace Turmerik.ObjectViewer.WinFormsApp
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageObjects = new System.Windows.Forms.TabPage();
            this.objectsUC = new Turmerik.ObjectViewer.WindowsFormsUCLib.Controls.ObjectsUC();
            this.tabPageSummary = new System.Windows.Forms.TabPage();
            this.objectsSummaryUC = new Turmerik.ObjectViewer.WindowsFormsUCLib.Controls.ObjectsSummaryUC();
            this.tabPageScript = new System.Windows.Forms.TabPage();
            this.csScriptExecutionUC = new Turmerik.ObjectViewer.WindowsFormsUCLib.Controls.CsScriptExecutionUC();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.csprojExecutionUC = new Turmerik.ObjectViewer.WindowsFormsUCLib.Controls.CsprojExecutionUC();
            this.openFileDialogLoadScriptFromFile = new System.Windows.Forms.OpenFileDialog();
            this.tabControlMain.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.tabPageSummary.SuspendLayout();
            this.tabPageScript.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageObjects);
            this.tabControlMain.Controls.Add(this.tabPageSummary);
            this.tabControlMain.Controls.Add(this.tabPageScript);
            this.tabControlMain.Controls.Add(this.tabPageProject);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1800, 900);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageObjects
            // 
            this.tabPageObjects.Controls.Add(this.objectsUC);
            this.tabPageObjects.Location = new System.Drawing.Point(4, 22);
            this.tabPageObjects.Name = "tabPageObjects";
            this.tabPageObjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageObjects.Size = new System.Drawing.Size(1792, 874);
            this.tabPageObjects.TabIndex = 0;
            this.tabPageObjects.Text = "Objects";
            this.tabPageObjects.UseVisualStyleBackColor = true;
            // 
            // objectsUC
            // 
            this.objectsUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsUC.Location = new System.Drawing.Point(3, 3);
            this.objectsUC.Name = "objectsUC";
            this.objectsUC.Size = new System.Drawing.Size(1786, 868);
            this.objectsUC.TabIndex = 0;
            // 
            // tabPageSummary
            // 
            this.tabPageSummary.Controls.Add(this.objectsSummaryUC);
            this.tabPageSummary.Location = new System.Drawing.Point(4, 22);
            this.tabPageSummary.Name = "tabPageSummary";
            this.tabPageSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSummary.Size = new System.Drawing.Size(1792, 874);
            this.tabPageSummary.TabIndex = 1;
            this.tabPageSummary.Text = "Summary";
            this.tabPageSummary.UseVisualStyleBackColor = true;
            // 
            // objectsSummaryUC
            // 
            this.objectsSummaryUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsSummaryUC.Location = new System.Drawing.Point(3, 3);
            this.objectsSummaryUC.Name = "objectsSummaryUC";
            this.objectsSummaryUC.Size = new System.Drawing.Size(1786, 868);
            this.objectsSummaryUC.TabIndex = 0;
            // 
            // tabPageScript
            // 
            this.tabPageScript.Controls.Add(this.csScriptExecutionUC);
            this.tabPageScript.Location = new System.Drawing.Point(4, 22);
            this.tabPageScript.Name = "tabPageScript";
            this.tabPageScript.Size = new System.Drawing.Size(1792, 874);
            this.tabPageScript.TabIndex = 2;
            this.tabPageScript.Text = "Script";
            this.tabPageScript.UseVisualStyleBackColor = true;
            // 
            // csScriptExecutionUC
            // 
            this.csScriptExecutionUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csScriptExecutionUC.Location = new System.Drawing.Point(0, 0);
            this.csScriptExecutionUC.Name = "csScriptExecutionUC";
            this.csScriptExecutionUC.Size = new System.Drawing.Size(1792, 874);
            this.csScriptExecutionUC.TabIndex = 0;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.csprojExecutionUC);
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Size = new System.Drawing.Size(1792, 874);
            this.tabPageProject.TabIndex = 3;
            this.tabPageProject.Text = "Project";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // csprojExecutionUC
            // 
            this.csprojExecutionUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csprojExecutionUC.Location = new System.Drawing.Point(0, 0);
            this.csprojExecutionUC.Name = "csprojExecutionUC";
            this.csprojExecutionUC.Size = new System.Drawing.Size(1792, 874);
            this.csprojExecutionUC.TabIndex = 0;
            // 
            // openFileDialogLoadScriptFromFile
            // 
            this.openFileDialogLoadScriptFromFile.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1800, 900);
            this.Controls.Add(this.tabControlMain);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Viewer";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageObjects.ResumeLayout(false);
            this.tabPageSummary.ResumeLayout(false);
            this.tabPageScript.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageObjects;
        private System.Windows.Forms.TabPage tabPageSummary;
        private System.Windows.Forms.TabPage tabPageScript;
        private System.Windows.Forms.OpenFileDialog openFileDialogLoadScriptFromFile;
        private System.Windows.Forms.TabPage tabPageProject;
        private WindowsFormsUCLib.Controls.CsprojExecutionUC csprojExecutionUC;
        private WindowsFormsUCLib.Controls.CsScriptExecutionUC csScriptExecutionUC;
        private WindowsFormsUCLib.Controls.ObjectsUC objectsUC;
        private WindowsFormsUCLib.Controls.ObjectsSummaryUC objectsSummaryUC;
    }
}

