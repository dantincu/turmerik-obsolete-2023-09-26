using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Text;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.Forms;

namespace Turmerik.WinForms.ActionComponent
{
    public interface IWinFormsActionComponentsManagerRetriever
    {
        IWinFormsActionComponentsManager Retrieve();
    }

    public class WinFormsActionComponentsManagerRetriever : IWinFormsActionComponentsManagerRetriever
    {
        private readonly Lazy<IWinFormsActionComponentsManager> actionComponentsManager;
        private readonly ITimeStampHelper timeStampHelper;

        public WinFormsActionComponentsManagerRetriever(
            ITimeStampHelper timeStampHelper)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));

            actionComponentsManager = new Lazy<IWinFormsActionComponentsManager>(
                () => new WinFormsActionComponentsManager(
                    timeStampHelper,
                    new UIMessageForm(
                        actionComponentsManager)),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IWinFormsActionComponentsManager Retrieve() => actionComponentsManager.Value;
    }
}
