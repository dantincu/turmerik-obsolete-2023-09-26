namespace Turmerik.WinForms.Forms
{
    partial class TrmrkUIMessagesForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.dataGridViewMessages = new System.Windows.Forms.DataGridView();
            this.dgvMessagesColTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMessagesColLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMessagesColContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelMainActionButtons = new System.Windows.Forms.Panel();
            this.buttonClearAllMessages = new System.Windows.Forms.Button();
            this.buttonViewLogFiles = new System.Windows.Forms.Button();
            this.splitContainerMessageDetails = new System.Windows.Forms.SplitContainer();
            this.richTextBoxMessageDetailsContent = new System.Windows.Forms.RichTextBox();
            this.richTextBoxMessageDetailsException = new System.Windows.Forms.RichTextBox();
            this.panelMessageDetailsTopPanel = new System.Windows.Forms.Panel();
            this.textBoxMessageDetailsLevel = new System.Windows.Forms.TextBox();
            this.textBoxMessageDetailsTimeStamp = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessages)).BeginInit();
            this.panelMainActionButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMessageDetails)).BeginInit();
            this.splitContainerMessageDetails.Panel1.SuspendLayout();
            this.splitContainerMessageDetails.Panel2.SuspendLayout();
            this.splitContainerMessageDetails.SuspendLayout();
            this.panelMessageDetailsTopPanel.SuspendLayout();
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
            this.splitContainerMain.Panel1.Controls.Add(this.dataGridViewMessages);
            this.splitContainerMain.Panel1.Controls.Add(this.panelMainActionButtons);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerMessageDetails);
            this.splitContainerMain.Panel2.Controls.Add(this.panelMessageDetailsTopPanel);
            this.splitContainerMain.Size = new System.Drawing.Size(1800, 900);
            this.splitContainerMain.SplitterDistance = 600;
            this.splitContainerMain.TabIndex = 0;
            // 
            // dataGridViewMessages
            // 
            this.dataGridViewMessages.AllowUserToAddRows = false;
            this.dataGridViewMessages.AllowUserToDeleteRows = false;
            this.dataGridViewMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvMessagesColTimeStamp,
            this.dgvMessagesColLevel,
            this.dgvMessagesColContent});
            this.dataGridViewMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMessages.Location = new System.Drawing.Point(0, 23);
            this.dataGridViewMessages.Name = "dataGridViewMessages";
            this.dataGridViewMessages.RowHeadersVisible = false;
            this.dataGridViewMessages.Size = new System.Drawing.Size(1800, 577);
            this.dataGridViewMessages.TabIndex = 0;
            this.dataGridViewMessages.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewMessages_CellClick);
            // 
            // dgvMessagesColTimeStamp
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dgvMessagesColTimeStamp.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMessagesColTimeStamp.HeaderText = "Time Stamp";
            this.dgvMessagesColTimeStamp.Name = "dgvMessagesColTimeStamp";
            this.dgvMessagesColTimeStamp.ReadOnly = true;
            this.dgvMessagesColTimeStamp.Width = 150;
            // 
            // dgvMessagesColLevel
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dgvMessagesColLevel.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMessagesColLevel.HeaderText = "Level";
            this.dgvMessagesColLevel.Name = "dgvMessagesColLevel";
            this.dgvMessagesColLevel.ReadOnly = true;
            // 
            // dgvMessagesColContent
            // 
            this.dgvMessagesColContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvMessagesColContent.HeaderText = "Content";
            this.dgvMessagesColContent.Name = "dgvMessagesColContent";
            this.dgvMessagesColContent.ReadOnly = true;
            // 
            // panelMainActionButtons
            // 
            this.panelMainActionButtons.Controls.Add(this.buttonClearAllMessages);
            this.panelMainActionButtons.Controls.Add(this.buttonViewLogFiles);
            this.panelMainActionButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMainActionButtons.Location = new System.Drawing.Point(0, 0);
            this.panelMainActionButtons.Name = "panelMainActionButtons";
            this.panelMainActionButtons.Size = new System.Drawing.Size(1800, 23);
            this.panelMainActionButtons.TabIndex = 1;
            // 
            // buttonClearAllMessages
            // 
            this.buttonClearAllMessages.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonClearAllMessages.Location = new System.Drawing.Point(86, 0);
            this.buttonClearAllMessages.Name = "buttonClearAllMessages";
            this.buttonClearAllMessages.Size = new System.Drawing.Size(106, 23);
            this.buttonClearAllMessages.TabIndex = 0;
            this.buttonClearAllMessages.Text = "Clear All Messages";
            this.buttonClearAllMessages.UseVisualStyleBackColor = true;
            this.buttonClearAllMessages.Click += new System.EventHandler(this.ButtonClearAllMessages_Click);
            // 
            // buttonViewLogFiles
            // 
            this.buttonViewLogFiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonViewLogFiles.Location = new System.Drawing.Point(0, 0);
            this.buttonViewLogFiles.Name = "buttonViewLogFiles";
            this.buttonViewLogFiles.Size = new System.Drawing.Size(86, 23);
            this.buttonViewLogFiles.TabIndex = 1;
            this.buttonViewLogFiles.Text = "View Log Files";
            this.buttonViewLogFiles.UseVisualStyleBackColor = true;
            this.buttonViewLogFiles.Click += new System.EventHandler(this.ButtonViewLogFiles_Click);
            // 
            // splitContainerMessageDetails
            // 
            this.splitContainerMessageDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMessageDetails.Location = new System.Drawing.Point(0, 23);
            this.splitContainerMessageDetails.Name = "splitContainerMessageDetails";
            // 
            // splitContainerMessageDetails.Panel1
            // 
            this.splitContainerMessageDetails.Panel1.Controls.Add(this.richTextBoxMessageDetailsContent);
            // 
            // splitContainerMessageDetails.Panel2
            // 
            this.splitContainerMessageDetails.Panel2.Controls.Add(this.richTextBoxMessageDetailsException);
            this.splitContainerMessageDetails.Size = new System.Drawing.Size(1800, 273);
            this.splitContainerMessageDetails.SplitterDistance = 900;
            this.splitContainerMessageDetails.TabIndex = 1;
            // 
            // richTextBoxMessageDetailsContent
            // 
            this.richTextBoxMessageDetailsContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessageDetailsContent.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMessageDetailsContent.Name = "richTextBoxMessageDetailsContent";
            this.richTextBoxMessageDetailsContent.ReadOnly = true;
            this.richTextBoxMessageDetailsContent.Size = new System.Drawing.Size(900, 273);
            this.richTextBoxMessageDetailsContent.TabIndex = 0;
            this.richTextBoxMessageDetailsContent.Text = "";
            // 
            // richTextBoxMessageDetailsException
            // 
            this.richTextBoxMessageDetailsException.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessageDetailsException.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMessageDetailsException.Name = "richTextBoxMessageDetailsException";
            this.richTextBoxMessageDetailsException.ReadOnly = true;
            this.richTextBoxMessageDetailsException.Size = new System.Drawing.Size(896, 273);
            this.richTextBoxMessageDetailsException.TabIndex = 1;
            this.richTextBoxMessageDetailsException.Text = "";
            // 
            // panelMessageDetailsTopPanel
            // 
            this.panelMessageDetailsTopPanel.Controls.Add(this.textBoxMessageDetailsLevel);
            this.panelMessageDetailsTopPanel.Controls.Add(this.textBoxMessageDetailsTimeStamp);
            this.panelMessageDetailsTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMessageDetailsTopPanel.Location = new System.Drawing.Point(0, 0);
            this.panelMessageDetailsTopPanel.Name = "panelMessageDetailsTopPanel";
            this.panelMessageDetailsTopPanel.Padding = new System.Windows.Forms.Padding(3);
            this.panelMessageDetailsTopPanel.Size = new System.Drawing.Size(1800, 23);
            this.panelMessageDetailsTopPanel.TabIndex = 0;
            // 
            // textBoxMessageDetailsLevel
            // 
            this.textBoxMessageDetailsLevel.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxMessageDetailsLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxMessageDetailsLevel.Location = new System.Drawing.Point(203, 3);
            this.textBoxMessageDetailsLevel.Name = "textBoxMessageDetailsLevel";
            this.textBoxMessageDetailsLevel.ReadOnly = true;
            this.textBoxMessageDetailsLevel.Size = new System.Drawing.Size(200, 20);
            this.textBoxMessageDetailsLevel.TabIndex = 1;
            // 
            // textBoxMessageDetailsTimeStamp
            // 
            this.textBoxMessageDetailsTimeStamp.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxMessageDetailsTimeStamp.Location = new System.Drawing.Point(3, 3);
            this.textBoxMessageDetailsTimeStamp.Name = "textBoxMessageDetailsTimeStamp";
            this.textBoxMessageDetailsTimeStamp.ReadOnly = true;
            this.textBoxMessageDetailsTimeStamp.Size = new System.Drawing.Size(200, 20);
            this.textBoxMessageDetailsTimeStamp.TabIndex = 0;
            // 
            // TrmrkUIMessagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1800, 900);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "TrmrkUIMessagesForm";
            this.Text = "UIMessagesListForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrmrkUIMessagesForm_FormClosing);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessages)).EndInit();
            this.panelMainActionButtons.ResumeLayout(false);
            this.splitContainerMessageDetails.Panel1.ResumeLayout(false);
            this.splitContainerMessageDetails.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMessageDetails)).EndInit();
            this.splitContainerMessageDetails.ResumeLayout(false);
            this.panelMessageDetailsTopPanel.ResumeLayout(false);
            this.panelMessageDetailsTopPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelMessageDetailsTopPanel;
        private System.Windows.Forms.TextBox textBoxMessageDetailsTimeStamp;
        private System.Windows.Forms.TextBox textBoxMessageDetailsLevel;
        private System.Windows.Forms.DataGridView dataGridViewMessages;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvMessagesColTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvMessagesColLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvMessagesColContent;
        private System.Windows.Forms.Panel panelMainActionButtons;
        private System.Windows.Forms.Button buttonClearAllMessages;
        private System.Windows.Forms.Button buttonViewLogFiles;
        private System.Windows.Forms.SplitContainer splitContainerMessageDetails;
        private System.Windows.Forms.RichTextBox richTextBoxMessageDetailsContent;
        private System.Windows.Forms.RichTextBox richTextBoxMessageDetailsException;
    }
}