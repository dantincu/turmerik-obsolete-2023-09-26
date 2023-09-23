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
    public interface IWinFormsActionComponentsManager : ITrmrkActionComponentsManager
    {
    }

    public class WinFormsActionComponentsManager : TrmrkActionComponentsManager, IWinFormsActionComponentsManager
    {
        private readonly ITimeStampHelper timeStampHelper;

        public WinFormsActionComponentsManager(
            ITimeStampHelper timeStampHelper,
            UIMessageForm uIMessageForm)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            UIMessageForm = uIMessageForm ?? throw new ArgumentNullException(nameof(uIMessageForm));
        }

        protected UIMessageForm UIMessageForm { get; }

        public override void ShowUIMessage(
            ShowUIMessageArgs args)
        {
            UIMessageForm.InvokeIfReq(() =>
            {
                var logLevelStr = args.LogLevel.ToString();
                UIMessageForm.Text = args.MsgTuple.UIMessage;

                UIMessageForm.TimeStampTextBox.Text = timeStampHelper.TmStmp(
                    DateTime.Now, true, TimeStamp.Ticks,
                    true, false, false, null);

                UIMessageForm.LogLevelTextBox.Text = logLevelStr;
                UIMessageForm.MessageTextBox.Text = args.MsgTuple.UIMessage ?? string.Empty;

                UIMessageForm.Show();
            });
        }
    }
}
