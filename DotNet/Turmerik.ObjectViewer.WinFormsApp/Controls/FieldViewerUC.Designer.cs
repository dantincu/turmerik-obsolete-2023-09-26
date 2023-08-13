using Turmerik.WinForms.Controls;

namespace Turmerik.ObjectViewer.WinFormsApp.Controls
{
    partial class FieldViewerUC
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
            this.splitContainerAccessors = new System.Windows.Forms.SplitContainer();
            this.textBoxAccessors = new System.Windows.Forms.TextBox();
            this.clickToggleIconLabelExpandCollapse = new Turmerik.WinForms.Controls.ClickToggleIconLabel();
            this.splitContainerTypeName = new System.Windows.Forms.SplitContainer();
            this.textBoxTypeName = new System.Windows.Forms.TextBox();
            this.textBoxMemberName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAccessors)).BeginInit();
            this.splitContainerAccessors.Panel1.SuspendLayout();
            this.splitContainerAccessors.Panel2.SuspendLayout();
            this.splitContainerAccessors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTypeName)).BeginInit();
            this.splitContainerTypeName.Panel1.SuspendLayout();
            this.splitContainerTypeName.Panel2.SuspendLayout();
            this.splitContainerTypeName.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerAccessors);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 24);
            this.splitContainerMain.SplitterDistance = 25;
            this.splitContainerMain.SplitterWidth = 1;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerAccessors
            // 
            this.splitContainerAccessors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAccessors.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAccessors.Name = "splitContainerAccessors";
            // 
            // splitContainerAccessors.Panel1
            // 
            this.splitContainerAccessors.Panel1.Controls.Add(this.textBoxAccessors);
            this.splitContainerAccessors.Panel1.Controls.Add(this.clickToggleIconLabelExpandCollapse);
            // 
            // splitContainerAccessors.Panel2
            // 
            this.splitContainerAccessors.Panel2.Controls.Add(this.splitContainerTypeName);
            this.splitContainerAccessors.Size = new System.Drawing.Size(800, 25);
            this.splitContainerAccessors.SplitterDistance = 100;
            this.splitContainerAccessors.SplitterWidth = 1;
            this.splitContainerAccessors.TabIndex = 0;
            // 
            // textBoxAccessors
            // 
            this.textBoxAccessors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAccessors.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxAccessors.Location = new System.Drawing.Point(0, 0);
            this.textBoxAccessors.Name = "textBoxAccessors";
            this.textBoxAccessors.ReadOnly = true;
            this.textBoxAccessors.Size = new System.Drawing.Size(100, 22);
            this.textBoxAccessors.TabIndex = 0;
            // 
            // clickToggleIconLabelExpandCollapse
            // 
            this.clickToggleIconLabelExpandCollapse.AutoSize = true;
            this.clickToggleIconLabelExpandCollapse.BackColor = System.Drawing.Color.White;
            this.clickToggleIconLabelExpandCollapse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clickToggleIconLabelExpandCollapse.Dock = System.Windows.Forms.DockStyle.Left;
            this.clickToggleIconLabelExpandCollapse.FontFamily = null;
            this.clickToggleIconLabelExpandCollapse.Location = new System.Drawing.Point(0, 0);
            this.clickToggleIconLabelExpandCollapse.Margin = new System.Windows.Forms.Padding(0);
            this.clickToggleIconLabelExpandCollapse.Name = "clickToggleIconLabelExpandCollapse";
            this.clickToggleIconLabelExpandCollapse.Size = new System.Drawing.Size(0, 13);
            this.clickToggleIconLabelExpandCollapse.TabIndex = 1;
            this.clickToggleIconLabelExpandCollapse.ToggledFalseText = "";
            this.clickToggleIconLabelExpandCollapse.ToggledTrueText = "";
            this.clickToggleIconLabelExpandCollapse.ToggledValue = false;
            // 
            // splitContainerTypeName
            // 
            this.splitContainerTypeName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTypeName.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTypeName.Name = "splitContainerTypeName";
            // 
            // splitContainerTypeName.Panel1
            // 
            this.splitContainerTypeName.Panel1.Controls.Add(this.textBoxTypeName);
            // 
            // splitContainerTypeName.Panel2
            // 
            this.splitContainerTypeName.Panel2.Controls.Add(this.textBoxMemberName);
            this.splitContainerTypeName.Size = new System.Drawing.Size(699, 25);
            this.splitContainerTypeName.SplitterDistance = 450;
            this.splitContainerTypeName.SplitterWidth = 1;
            this.splitContainerTypeName.TabIndex = 1;
            // 
            // textBoxTypeName
            // 
            this.textBoxTypeName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTypeName.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxTypeName.Location = new System.Drawing.Point(0, 0);
            this.textBoxTypeName.Name = "textBoxTypeName";
            this.textBoxTypeName.ReadOnly = true;
            this.textBoxTypeName.Size = new System.Drawing.Size(450, 22);
            this.textBoxTypeName.TabIndex = 1;
            // 
            // textBoxMemberName
            // 
            this.textBoxMemberName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMemberName.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxMemberName.Location = new System.Drawing.Point(0, 0);
            this.textBoxMemberName.Name = "textBoxMemberName";
            this.textBoxMemberName.ReadOnly = true;
            this.textBoxMemberName.Size = new System.Drawing.Size(248, 22);
            this.textBoxMemberName.TabIndex = 1;
            // 
            // FieldViewerUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "FieldViewerUC";
            this.Size = new System.Drawing.Size(800, 24);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerAccessors.Panel1.ResumeLayout(false);
            this.splitContainerAccessors.Panel1.PerformLayout();
            this.splitContainerAccessors.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAccessors)).EndInit();
            this.splitContainerAccessors.ResumeLayout(false);
            this.splitContainerTypeName.Panel1.ResumeLayout(false);
            this.splitContainerTypeName.Panel1.PerformLayout();
            this.splitContainerTypeName.Panel2.ResumeLayout(false);
            this.splitContainerTypeName.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTypeName)).EndInit();
            this.splitContainerTypeName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerAccessors;
        private System.Windows.Forms.SplitContainer splitContainerTypeName;
        private System.Windows.Forms.TextBox textBoxAccessors;
        private System.Windows.Forms.TextBox textBoxTypeName;
        private System.Windows.Forms.TextBox textBoxMemberName;
        private ClickToggleIconLabel clickToggleIconLabelExpandCollapse;
    }
}
