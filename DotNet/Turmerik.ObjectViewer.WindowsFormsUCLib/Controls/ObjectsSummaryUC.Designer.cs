namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    partial class ObjectsSummaryUC
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
            this.splitContainerSummary = new System.Windows.Forms.SplitContainer();
            this.groupBoxObjectsList = new System.Windows.Forms.GroupBox();
            this.groupBoxExecutedAssemblies = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSummary)).BeginInit();
            this.splitContainerSummary.Panel1.SuspendLayout();
            this.splitContainerSummary.Panel2.SuspendLayout();
            this.splitContainerSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerSummary
            // 
            this.splitContainerSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSummary.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSummary.Name = "splitContainerSummary";
            // 
            // splitContainerSummary.Panel1
            // 
            this.splitContainerSummary.Panel1.Controls.Add(this.groupBoxObjectsList);
            // 
            // splitContainerSummary.Panel2
            // 
            this.splitContainerSummary.Panel2.Controls.Add(this.groupBoxExecutedAssemblies);
            this.splitContainerSummary.Size = new System.Drawing.Size(1792, 874);
            this.splitContainerSummary.SplitterDistance = 896;
            this.splitContainerSummary.SplitterWidth = 1;
            this.splitContainerSummary.TabIndex = 2;
            // 
            // groupBoxObjectsList
            // 
            this.groupBoxObjectsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxObjectsList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxObjectsList.Name = "groupBoxObjectsList";
            this.groupBoxObjectsList.Size = new System.Drawing.Size(896, 874);
            this.groupBoxObjectsList.TabIndex = 0;
            this.groupBoxObjectsList.TabStop = false;
            this.groupBoxObjectsList.Text = "Objects";
            // 
            // groupBoxExecutedAssemblies
            // 
            this.groupBoxExecutedAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxExecutedAssemblies.Location = new System.Drawing.Point(0, 0);
            this.groupBoxExecutedAssemblies.Name = "groupBoxExecutedAssemblies";
            this.groupBoxExecutedAssemblies.Size = new System.Drawing.Size(895, 874);
            this.groupBoxExecutedAssemblies.TabIndex = 1;
            this.groupBoxExecutedAssemblies.TabStop = false;
            this.groupBoxExecutedAssemblies.Text = "Executed Assemblies";
            // 
            // ObjectsSummaryUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerSummary);
            this.Name = "ObjectsSummaryUC";
            this.Size = new System.Drawing.Size(1792, 874);
            this.splitContainerSummary.Panel1.ResumeLayout(false);
            this.splitContainerSummary.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSummary)).EndInit();
            this.splitContainerSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerSummary;
        private System.Windows.Forms.GroupBox groupBoxObjectsList;
        private System.Windows.Forms.GroupBox groupBoxExecutedAssemblies;
    }
}
