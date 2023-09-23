using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Logging;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.Forms;

namespace Turmerik.WinForms.ActionComponent
{
    public interface ITrmrkWinFormsActionComponentsManagerRetriever
    {
        Color StatusLabelDefaultForeColor { get; }
        Color StatusLabelErrorForeColor { get; }
        LogLevel MinLogLevel { get; set; }
        ToolStripStatusLabel ToolStripStatusLabel { get; set; }

        ITrmrkWinFormsActionComponentsManager Retrieve();
    }

    public class TrmrkWinFormsActionComponentsManagerRetriever : ITrmrkWinFormsActionComponentsManagerRetriever
    {
        private readonly Lazy<ITrmrkWinFormsActionComponentsManager> actionComponentsManager;
        private readonly ITimeStampHelper timeStampHelper;
        private readonly IThreadSafeActionComponent threadSafeActionComponent;
        private readonly IAppLoggerCreator appLoggerCreator;
        private readonly IAppEnv appEnv;

        public TrmrkWinFormsActionComponentsManagerRetriever(
            ITimeStampHelper timeStampHelper,
            IThreadSafeActionComponent threadSafeActionComponent,
            IAppLoggerCreator appLoggerCreator,
            IAppEnv appEnv)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));

            this.threadSafeActionComponent = threadSafeActionComponent ?? throw new ArgumentNullException(
                nameof(threadSafeActionComponent));

            this.appLoggerCreator = appLoggerCreator ?? throw new ArgumentNullException(
                nameof(appLoggerCreator));

            this.appEnv = appEnv ?? throw new ArgumentNullException(
                nameof(appEnv));

            actionComponentsManager = new Lazy<ITrmrkWinFormsActionComponentsManager>(
                () => new TrmrkWinFormsActionComponentsManager(
                    timeStampHelper,
                    new TrmrkWinFormsActionComponentsManagerOpts.Mtbl
                    {
                        UIMessagesListForm = new TrmrkUIMessagesForm(
                            actionComponentsManager,
                            threadSafeActionComponent,
                            timeStampHelper,
                            appLoggerCreator,
                            appEnv),
                        ToolStripStatusLabel = ToolStripStatusLabel,
                        StatusLabelDefaultForeColor = StatusLabelDefaultForeColor,
                        StatusLabelErrorForeColor = StatusLabelErrorForeColor,
                        MinLogLevel = MinLogLevel
                    }),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public Color StatusLabelDefaultForeColor { get; set; } = Color.Black;
        public Color StatusLabelErrorForeColor { get; set; } = Color.Red;
        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;

        public ToolStripStatusLabel ToolStripStatusLabel { get; set; }

        public ITrmrkWinFormsActionComponentsManager Retrieve() => actionComponentsManager.Value;
    }
}
