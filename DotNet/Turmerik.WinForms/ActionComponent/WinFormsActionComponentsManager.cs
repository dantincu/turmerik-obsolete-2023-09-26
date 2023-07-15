using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.Forms;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.ActionComponent
{
    public interface IWinFormsActionComponentsManager : ITrmrkActionComponentsManager
    {
    }

    public class WinFormsActionComponentsManager : TrmrkActionComponentsManager, IWinFormsActionComponentsManager
    {
        public WinFormsActionComponentsManager(
            UIMessageForm uIMessageForm)
        {
            UIMessageForm = uIMessageForm ?? throw new ArgumentNullException(nameof(uIMessageForm));
        }

        protected UIMessageForm UIMessageForm { get; }

        public override void ShowUIMessageAlert(
            ShowUIMessageAlertArgs args,
            bool useUIBlockingMessagePopup)
        {
            UIMessageForm.Text = args.MsgTuple.Caption ?? args.LogLevel.ToString();
            UIMessageForm.MessageTextBox.Text = args.MsgTuple.Message ?? string.Empty;

            UIMessageForm.InvokeIfReq(() =>
            {
                ShowUIMessageAlertCore(args, useUIBlockingMessagePopup);
            });
        }

        protected virtual void ShowUIMessageAlertCore(
            ShowUIMessageAlertArgs args,
            bool useUIBlockingMessagePopup)
        {
            if (useUIBlockingMessagePopup)
            {
                UIMessageForm.ShowDialog();
            }
            else
            {
                UIMessageForm.Show();
            }
        }
    }
}
