using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Avalonia;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Text;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Avalonia.Media;
using Avalonia.Controls;

namespace Turmerik.Avalonia.ActionComponent
{
    public interface ITrmrkAvlnActionComponentsManager : ITrmrkActionComponentsManager
    {
        Func<string> MsgTextBoxContentGetter { get; }
        Action<string> MsgTextBoxContentSetter { get; }
        Func<IBrush> MsgTextBoxForegroundGetter { get; }
        Action<IBrush> MsgTextBoxForegroundSetter { get; }

        IBrush MsgTextBoxDefaultForeground { get; }
        IBrush MsgTextBoxErrorForeground { get; }
        LogLevel MinLogLevel { get; set; }
    }

    public class TrmrkAvlnActionComponentsManager : TrmrkActionComponentsManager, ITrmrkAvlnActionComponentsManager
    {
        public TrmrkAvlnActionComponentsManager(
            TrmrkAvlnActionComponentsManagerOpts.IClnbl opts)
        {
            MsgTextBoxContentGetter = opts.MsgTextBoxContentGetter ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxContentGetter));

            MsgTextBoxContentSetter = opts.MsgTextBoxContentSetter ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxContentSetter));

            MsgTextBoxForegroundGetter = opts.MsgTextBoxForegroundGetter ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxForegroundGetter));

            MsgTextBoxForegroundSetter = opts.MsgTextBoxForegroundSetter ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxForegroundSetter));

            MsgTextBoxDefaultForeground = opts.MsgTextBoxDefaultForeground ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxDefaultForeground));

            MsgTextBoxSuccessForeground = opts.MsgTextBoxSuccessForeground ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxSuccessForeground));

            MsgTextBoxErrorForeground = opts.MsgTextBoxErrorForeground ?? throw new ArgumentNullException(
                nameof(opts.MsgTextBoxErrorForeground));

            MinLogLevel = opts.MinLogLevel;
        }

        public Func<string> MsgTextBoxContentGetter { get; }
        public Action<string> MsgTextBoxContentSetter { get; }
        public Func<IBrush> MsgTextBoxForegroundGetter { get; }
        public Action<IBrush> MsgTextBoxForegroundSetter { get; }

        public IBrush MsgTextBoxDefaultForeground { get; }
        public IBrush MsgTextBoxSuccessForeground { get; }
        public IBrush MsgTextBoxErrorForeground { get; }

        public LogLevel MinLogLevel { get; set; }

        public override void ShowUIMessage(ShowUIMessageArgs args)
        {
            if (MinLogLevel <= args.LogLevel)
            {
                MsgTextBoxContentSetter(args.MsgTuple.UIMessage);

                if (args.ActionResult != null)
                {
                    if (args.ActionResult.IsSuccess)
                    {
                        MsgTextBoxForegroundSetter(MsgTextBoxSuccessForeground);
                    }
                    else
                    {
                        MsgTextBoxForegroundSetter(MsgTextBoxErrorForeground);
                    }
                }
                else
                {
                    if (args.LogLevel >= LogLevel.Error)
                    {
                        MsgTextBoxForegroundSetter(MsgTextBoxErrorForeground);
                    }
                    else
                    {
                        MsgTextBoxForegroundSetter(MsgTextBoxDefaultForeground);
                    }
                }
            }
        }
    }
}
