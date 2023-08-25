namespace Turmerik.WinForms.Forms
{
    partial class UIMessageForm
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.textBoxLogLevel = new System.Windows.Forms.TextBox();
            this.textBoxTimeStamp = new System.Windows.Forms.TextBox();
            this.checkBoxUseBlockingAlerts = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxMessage);
            this.splitContainerMain.Panel1.Controls.Add(this.panelTop);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.checkBoxUseBlockingAlerts);
            this.splitContainerMain.Panel2.Controls.Add(this.buttonOk);
            this.splitContainerMain.Size = new System.Drawing.Size(484, 261);
            this.splitContainerMain.SplitterDistance = 235;
            this.splitContainerMain.SplitterWidth = 1;
            this.splitContainerMain.TabIndex = 0;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.Location = new System.Drawing.Point(0, 21);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = true;
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessage.Size = new System.Drawing.Size(484, 214);
            this.textBoxMessage.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.textBoxLogLevel);
            this.panelTop.Controls.Add(this.textBoxTimeStamp);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(484, 21);
            this.panelTop.TabIndex = 1;
            // 
            // textBoxLogLevel
            // 
            this.textBoxLogLevel.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxLogLevel.Location = new System.Drawing.Point(384, 0);
            this.textBoxLogLevel.Name = "textBoxLogLevel";
            this.textBoxLogLevel.ReadOnly = true;
            this.textBoxLogLevel.Size = new System.Drawing.Size(100, 20);
            this.textBoxLogLevel.TabIndex = 1;
            // 
            // textBoxTimeStamp
            // 
            this.textBoxTimeStamp.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxTimeStamp.Location = new System.Drawing.Point(0, 0);
            this.textBoxTimeStamp.Name = "textBoxTimeStamp";
            this.textBoxTimeStamp.ReadOnly = true;
            this.textBoxTimeStamp.Size = new System.Drawing.Size(200, 20);
            this.textBoxTimeStamp.TabIndex = 0;
            // 
            // checkBoxUseBlockingAlerts
            // 
            this.checkBoxUseBlockingAlerts.AutoSize = true;
            this.checkBoxUseBlockingAlerts.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxUseBlockingAlerts.Location = new System.Drawing.Point(0, 0);
            this.checkBoxUseBlockingAlerts.Name = "checkBoxUseBlockingAlerts";
            this.checkBoxUseBlockingAlerts.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.checkBoxUseBlockingAlerts.Size = new System.Drawing.Size(128, 25);
            this.checkBoxUseBlockingAlerts.TabIndex = 1;
            this.checkBoxUseBlockingAlerts.Text = "Use Blocking Alerts";
            this.checkBoxUseBlockingAlerts.UseVisualStyleBackColor = true;
            this.checkBoxUseBlockingAlerts.CheckedChanged += new System.EventHandler(this.CheckBoxUseBlockingAlerts_CheckedChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOk.Location = new System.Drawing.Point(409, 0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 25);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // UIMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "UIMessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UIMessageForm_FormClosing);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.CheckBox checkBoxUseBlockingAlerts;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox textBoxTimeStamp;
        private System.Windows.Forms.TextBox textBoxLogLevel;
    }
}