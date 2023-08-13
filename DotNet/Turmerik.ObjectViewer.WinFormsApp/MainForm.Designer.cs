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
            this.fieldViewerUC1 = new Turmerik.ObjectViewer.WinFormsApp.Controls.FieldViewerUC();
            this.SuspendLayout();
            // 
            // fieldViewerUC1
            // 
            this.fieldViewerUC1.Location = new System.Drawing.Point(36, 27);
            this.fieldViewerUC1.Name = "fieldViewerUC1";
            this.fieldViewerUC1.Size = new System.Drawing.Size(800, 24);
            this.fieldViewerUC1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1800, 900);
            this.Controls.Add(this.fieldViewerUC1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.FieldViewerUC fieldViewerUC1;
    }
}

