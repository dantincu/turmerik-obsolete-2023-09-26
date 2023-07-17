﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.TrmrkAction;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.Forms
{
    public partial class UIMessageForm : Form
    {
        private readonly Lazy<IWinFormsActionComponentsManager> actionComponentsManager;

        public UIMessageForm()
        {
            InitializeComponent();
        }

        public UIMessageForm(
            Lazy<IWinFormsActionComponentsManager> actionComponentsManager)
        {
            this.actionComponentsManager = actionComponentsManager ?? throw new ArgumentNullException(
                nameof(actionComponentsManager));
        }

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
            actionComponentsManager.Value.EnableUIBlockingMessagePopups = checkBoxUseBlockingAlerts.Checked;
        }

        #endregion UI Event Handlers
    }
}