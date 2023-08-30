using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Text;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.Forms;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.ActionComponent
{
    public interface ITrmrkWinFormsActionComponentsManager : ITrmrkActionComponentsManager
    {
    }

    public class TrmrkWinFormsActionComponentsManager : TrmrkActionComponentsManager, ITrmrkWinFormsActionComponentsManager
    {
        private readonly ITimeStampHelper timeStampHelper;

        public TrmrkWinFormsActionComponentsManager(
            ITimeStampHelper timeStampHelper,
            TrmrkUIMessagesForm uIMessagesListForm)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            UIMessagesListForm = uIMessagesListForm ?? throw new ArgumentNullException(nameof(uIMessagesListForm));
        }

        protected TrmrkUIMessagesForm UIMessagesListForm { get; }

        public override void ShowUIMessageAlert(
            ShowUIMessageAlertArgs args,
            bool useUIBlockingMessagePopup)
        {
            var logLevelStr = args.LogLevel.ToString();
            UIMessagesListForm.Text = args.MsgTuple.Caption ?? logLevelStr;

            /* UIMessagesListForm.TimeStampTextBox.Text = timeStampHelper.TmStmp(
                DateTime.Now, true, TimeStamp.Ticks,
                true, false, false, null);

            UIMessagesListForm.LogLevelTextBox.Text = logLevelStr;
            UIMessagesListForm.MessageTextBox.Text = args.MsgTuple.Message ?? string.Empty;

            UIMessagesListForm.InvokeIfReq(() =>
            {
                ShowUIMessageAlertCore(args, useUIBlockingMessagePopup);
            }); */
        }

        protected virtual void ShowUIMessageAlertCore(
            ShowUIMessageAlertArgs args,
            bool useUIBlockingMessagePopup)
        {
            if (useUIBlockingMessagePopup)
            {
                UIMessagesListForm.ShowDialog();
            }
            else
            {
                UIMessagesListForm.Show();
            }
        }
    }
}
