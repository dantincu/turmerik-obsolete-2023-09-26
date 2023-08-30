using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        ITrmrkWinFormsActionComponentsManager Retrieve();
    }

    public class TrmrkWinFormsActionComponentsManagerRetriever : ITrmrkWinFormsActionComponentsManagerRetriever
    {
        private readonly Lazy<ITrmrkWinFormsActionComponentsManager> actionComponentsManager;
        private readonly ITimeStampHelper timeStampHelper;
        private readonly IThreadSafeActionComponent threadSafeActionComponent;,
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
                    new TrmrkUIMessagesForm(
                        actionComponentsManager,
                        threadSafeActionComponent,
                        timeStampHelper,
                        appLoggerCreator,
                        appEnv)),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public ITrmrkWinFormsActionComponentsManager Retrieve() => actionComponentsManager.Value;
    }
}
