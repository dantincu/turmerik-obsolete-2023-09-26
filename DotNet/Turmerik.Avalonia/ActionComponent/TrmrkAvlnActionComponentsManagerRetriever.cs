using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.TrmrkAction;
using Turmerik.Utils;

namespace Turmerik.Avalonia.ActionComponent
{
    public interface ITrmrkAvlnActionComponentsManagerRetriever
    {
        IBrush MsgTextBoxDefaultForeground { get; set; }
        IBrush MsgTextBoxSuccessForeground { get; set; }
        IBrush MsgTextBoxErrorForeground { get; set; }
        LogLevel MinLogLevel { get; set; }
        Func<string> MsgTextBoxContentGetter { get; set; }
        Action<string> MsgTextBoxContentSetter { get; set; }
        Func<IBrush> MsgTextBoxForegroundGetter { get; set; }
        Action<IBrush> MsgTextBoxForegroundSetter { get; set; }

        ITrmrkAvlnActionComponentsManager Retrieve();
    }

    public class TrmrkAvlnActionComponentsManagerRetriever : ITrmrkAvlnActionComponentsManagerRetriever
    {
        private readonly Lazy<ITrmrkAvlnActionComponentsManager> actionComponentsManager;

        public TrmrkAvlnActionComponentsManagerRetriever()
        {
            actionComponentsManager = new Lazy<ITrmrkAvlnActionComponentsManager>(
                () => new TrmrkAvlnActionComponentsManager(
                    new TrmrkAvlnActionComponentsManagerOpts.Mtbl
                    {
                        MsgTextBoxContentGetter = MsgTextBoxContentGetter ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxContentGetter)),
                        MsgTextBoxContentSetter = MsgTextBoxContentSetter ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxContentSetter)),
                        MsgTextBoxForegroundGetter = MsgTextBoxForegroundGetter ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxForegroundGetter)),
                        MsgTextBoxForegroundSetter = MsgTextBoxForegroundSetter ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxForegroundSetter)),
                        MsgTextBoxDefaultForeground = MsgTextBoxDefaultForeground ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxDefaultForeground)),
                        MsgTextBoxSuccessForeground = MsgTextBoxSuccessForeground ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxSuccessForeground)),
                        MsgTextBoxErrorForeground = MsgTextBoxErrorForeground ?? throw new ArgumentNullException(
                            nameof(MsgTextBoxErrorForeground)),
                        MinLogLevel = MinLogLevel
                    }),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IBrush MsgTextBoxDefaultForeground { get; set; }
        public IBrush MsgTextBoxSuccessForeground { get; set; }
        public IBrush MsgTextBoxErrorForeground { get; set; }

        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;

        public Func<string> MsgTextBoxContentGetter { get; set; }
        public Action<string> MsgTextBoxContentSetter { get; set; }
        public Func<IBrush> MsgTextBoxForegroundGetter { get; set; }
        public Action<IBrush> MsgTextBoxForegroundSetter { get; set; }

        public ITrmrkAvlnActionComponentsManager Retrieve() => actionComponentsManager.Value;
    }
}
