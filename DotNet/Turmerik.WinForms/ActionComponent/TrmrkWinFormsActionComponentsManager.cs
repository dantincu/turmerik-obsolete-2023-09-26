using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Text;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.Forms;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.ActionComponent
{
    public interface ITrmrkWinFormsActionComponentsManager : ITrmrkActionComponentsManager
    {
        TrmrkUIMessagesForm UIMessagesListForm { get; }
        ToolStripStatusLabel ToolStripStatusLabel { get; }

        Color StatusLabelDefaultForeColor { get; }
        Color StatusLabelErrorForeColor { get; }
        LogLevel MinLogLevel { get; set; }
    }

    public class TrmrkWinFormsActionComponentsManager : TrmrkActionComponentsManager, ITrmrkWinFormsActionComponentsManager
    {
        private readonly ITimeStampHelper timeStampHelper;

        public TrmrkWinFormsActionComponentsManager(
            ITimeStampHelper timeStampHelper,
            TrmrkWinFormsActionComponentsManagerOpts.IClnbl opts)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            UIMessagesListForm = opts.UIMessagesListForm ?? throw new ArgumentNullException(nameof(opts.UIMessagesListForm));
            ToolStripStatusLabel = opts.ToolStripStatusLabel ?? throw new ArgumentNullException(nameof(opts.ToolStripStatusLabel));
            StatusLabelDefaultForeColor = opts.StatusLabelDefaultForeColor;
            StatusLabelErrorForeColor = opts.StatusLabelErrorForeColor;
            MinLogLevel = opts.MinLogLevel;
        }

        public TrmrkUIMessagesForm UIMessagesListForm { get; }
        public ToolStripStatusLabel ToolStripStatusLabel { get; }

        public Color StatusLabelDefaultForeColor { get; }
        public Color StatusLabelErrorForeColor { get; }

        public LogLevel MinLogLevel { get; set; }

        public override void ShowUIMessage(
            ShowUIMessageArgs args,
            bool showUIMessage)
        {
            if (MinLogLevel <= args.LogLevel)
            {
                var msg = new UIMessageLogCoreDateTime.Mtbl
                {
                    Level = args.LogLevel,
                    Message = args.MsgTuple.Message,
                    RenderedMsg = ": ".JoinNotNullStr(
                        args.MsgTuple.Caption.Arr(
                            args.MsgTuple.Message)),
                    Exception = SerializableExcp.FromExcp(args.Exc),
                    TimeStamp = DateTime.Now,
                };

                UIMessagesListForm.InvokeIfReq(() =>
                {
                    UIMessagesListForm.AddMessage(msg);
                    ToolStripStatusLabel.Text = args.MsgTuple.Message;

                    if (args.ActionResult.IsSuccess)
                    {
                        ToolStripStatusLabel.ForeColor = StatusLabelDefaultForeColor;
                    }
                    else
                    {
                        ToolStripStatusLabel.ForeColor = StatusLabelErrorForeColor;
                    }

                    if (showUIMessage)
                    {
                        UIMessagesListForm.ShowDialog();
                    }
                });
            }
        }
    }
}
