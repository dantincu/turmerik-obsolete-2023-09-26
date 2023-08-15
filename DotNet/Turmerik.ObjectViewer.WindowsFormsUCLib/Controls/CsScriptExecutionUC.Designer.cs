namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    partial class CsScriptExecutionUC
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
            this.splitContainerScript = new System.Windows.Forms.SplitContainer();
            this.groupBoxScript = new System.Windows.Forms.GroupBox();
            this.richTextBoxScript = new System.Windows.Forms.RichTextBox();
            this.panelTitleSampleScript = new System.Windows.Forms.Panel();
            this.iconLabelExecuteScript = new Turmerik.WinForms.Controls.IconLabel();
            this.clickToggleIconLabelSampleScript = new Turmerik.WinForms.Controls.ClickToggleIconLabel();
            this.labelTitleSampleScript = new System.Windows.Forms.Label();
            this.richTextBoxSampleScript = new System.Windows.Forms.RichTextBox();
            this.groupBoxScriptAssemblies = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScript)).BeginInit();
            this.splitContainerScript.Panel1.SuspendLayout();
            this.splitContainerScript.Panel2.SuspendLayout();
            this.splitContainerScript.SuspendLayout();
            this.groupBoxScript.SuspendLayout();
            this.panelTitleSampleScript.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerScript
            // 
            this.splitContainerScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerScript.Location = new System.Drawing.Point(0, 0);
            this.splitContainerScript.Name = "splitContainerScript";
            // 
            // splitContainerScript.Panel1
            // 
            this.splitContainerScript.Panel1.Controls.Add(this.groupBoxScript);
            // 
            // splitContainerScript.Panel2
            // 
            this.splitContainerScript.Panel2.Controls.Add(this.groupBoxScriptAssemblies);
            this.splitContainerScript.Size = new System.Drawing.Size(1792, 874);
            this.splitContainerScript.SplitterDistance = 896;
            this.splitContainerScript.SplitterWidth = 1;
            this.splitContainerScript.TabIndex = 2;
            // 
            // groupBoxScript
            // 
            this.groupBoxScript.Controls.Add(this.richTextBoxScript);
            this.groupBoxScript.Controls.Add(this.panelTitleSampleScript);
            this.groupBoxScript.Controls.Add(this.richTextBoxSampleScript);
            this.groupBoxScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxScript.Location = new System.Drawing.Point(0, 0);
            this.groupBoxScript.Name = "groupBoxScript";
            this.groupBoxScript.Size = new System.Drawing.Size(896, 874);
            this.groupBoxScript.TabIndex = 0;
            this.groupBoxScript.TabStop = false;
            this.groupBoxScript.Text = "Script";
            // 
            // richTextBoxScript
            // 
            this.richTextBoxScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxScript.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBoxScript.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxScript.Name = "richTextBoxScript";
            this.richTextBoxScript.Size = new System.Drawing.Size(890, 642);
            this.richTextBoxScript.TabIndex = 1;
            this.richTextBoxScript.Text = "";
            // 
            // panelTitleSampleScript
            // 
            this.panelTitleSampleScript.Controls.Add(this.iconLabelExecuteScript);
            this.panelTitleSampleScript.Controls.Add(this.clickToggleIconLabelSampleScript);
            this.panelTitleSampleScript.Controls.Add(this.labelTitleSampleScript);
            this.panelTitleSampleScript.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTitleSampleScript.Location = new System.Drawing.Point(3, 658);
            this.panelTitleSampleScript.Name = "panelTitleSampleScript";
            this.panelTitleSampleScript.Size = new System.Drawing.Size(890, 21);
            this.panelTitleSampleScript.TabIndex = 4;
            // 
            // iconLabelExecuteScript
            // 
            this.iconLabelExecuteScript.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconLabelExecuteScript.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconLabelExecuteScript.FontFamily = null;
            this.iconLabelExecuteScript.Location = new System.Drawing.Point(92, 0);
            this.iconLabelExecuteScript.Name = "iconLabelExecuteScript";
            this.iconLabelExecuteScript.Size = new System.Drawing.Size(35, 21);
            this.iconLabelExecuteScript.TabIndex = 13;
            this.iconLabelExecuteScript.Text = "E";
            // 
            // clickToggleIconLabelSampleScript
            // 
            this.clickToggleIconLabelSampleScript.AutoSize = true;
            this.clickToggleIconLabelSampleScript.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clickToggleIconLabelSampleScript.Dock = System.Windows.Forms.DockStyle.Left;
            this.clickToggleIconLabelSampleScript.FontFamily = null;
            this.clickToggleIconLabelSampleScript.Location = new System.Drawing.Point(78, 0);
            this.clickToggleIconLabelSampleScript.Name = "clickToggleIconLabelSampleScript";
            this.clickToggleIconLabelSampleScript.Size = new System.Drawing.Size(14, 13);
            this.clickToggleIconLabelSampleScript.TabIndex = 3;
            this.clickToggleIconLabelSampleScript.Text = "T";
            this.clickToggleIconLabelSampleScript.ToggledFalseText = null;
            this.clickToggleIconLabelSampleScript.ToggledTrueText = null;
            this.clickToggleIconLabelSampleScript.ToggledValue = false;
            this.clickToggleIconLabelSampleScript.ClickToggled += new System.Action<bool>(this.ClickToggleIconLabelSampleScript_ClickToggled);
            // 
            // labelTitleSampleScript
            // 
            this.labelTitleSampleScript.AutoSize = true;
            this.labelTitleSampleScript.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTitleSampleScript.Location = new System.Drawing.Point(0, 0);
            this.labelTitleSampleScript.Name = "labelTitleSampleScript";
            this.labelTitleSampleScript.Padding = new System.Windows.Forms.Padding(3);
            this.labelTitleSampleScript.Size = new System.Drawing.Size(78, 19);
            this.labelTitleSampleScript.TabIndex = 2;
            this.labelTitleSampleScript.Text = "Sample Script";
            // 
            // richTextBoxSampleScript
            // 
            this.richTextBoxSampleScript.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxSampleScript.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBoxSampleScript.Location = new System.Drawing.Point(3, 679);
            this.richTextBoxSampleScript.Name = "richTextBoxSampleScript";
            this.richTextBoxSampleScript.ReadOnly = true;
            this.richTextBoxSampleScript.Size = new System.Drawing.Size(890, 192);
            this.richTextBoxSampleScript.TabIndex = 0;
            this.richTextBoxSampleScript.Text = "";
            this.richTextBoxSampleScript.Visible = false;
            // 
            // groupBoxScriptAssemblies
            // 
            this.groupBoxScriptAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxScriptAssemblies.Location = new System.Drawing.Point(0, 0);
            this.groupBoxScriptAssemblies.Name = "groupBoxScriptAssemblies";
            this.groupBoxScriptAssemblies.Size = new System.Drawing.Size(895, 874);
            this.groupBoxScriptAssemblies.TabIndex = 1;
            this.groupBoxScriptAssemblies.TabStop = false;
            this.groupBoxScriptAssemblies.Text = "Script Assemblies";
            // 
            // CsScriptExecutionUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerScript);
            this.Name = "CsScriptExecutionUC";
            this.Size = new System.Drawing.Size(1792, 874);
            this.splitContainerScript.Panel1.ResumeLayout(false);
            this.splitContainerScript.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScript)).EndInit();
            this.splitContainerScript.ResumeLayout(false);
            this.groupBoxScript.ResumeLayout(false);
            this.panelTitleSampleScript.ResumeLayout(false);
            this.panelTitleSampleScript.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerScript;
        private System.Windows.Forms.GroupBox groupBoxScript;
        private System.Windows.Forms.RichTextBox richTextBoxScript;
        private System.Windows.Forms.Label labelTitleSampleScript;
        private System.Windows.Forms.RichTextBox richTextBoxSampleScript;
        private System.Windows.Forms.GroupBox groupBoxScriptAssemblies;
        private WinForms.Controls.ClickToggleIconLabel clickToggleIconLabelSampleScript;
        private System.Windows.Forms.Panel panelTitleSampleScript;
        private WinForms.Controls.IconLabel iconLabelExecuteScript;
    }
}
