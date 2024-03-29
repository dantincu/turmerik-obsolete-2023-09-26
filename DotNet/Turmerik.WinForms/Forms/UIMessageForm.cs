﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turmerik.WinForms.Forms
{
    public partial class UIMessageForm : Form
    {
        // private readonly Lazy<IWinFormsActionComponentsManager> actionComponentsManager;

        public UIMessageForm()
        {
            InitializeComponent();
        }

        /* public UIMessageForm(
            Lazy<IWinFormsActionComponentsManager> actionComponentsManager)
        {
            this.actionComponentsManager = actionComponentsManager ?? throw new ArgumentNullException(
                nameof(actionComponentsManager)); 

            InitializeComponent();
        }*/

        public TextBox TimeStampTextBox => this.textBoxTimeStamp;
        public TextBox LogLevelTextBox => this.textBoxLogLevel;
        public TextBox MessageTextBox => this.textBoxMessage;

        #region UI Event Handlers

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void UIMessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void CheckBoxUseBlockingAlerts_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void UIMessageForm_Load(object sender, EventArgs e)
        {
        }

        #endregion UI Event Handlers
    }
}
