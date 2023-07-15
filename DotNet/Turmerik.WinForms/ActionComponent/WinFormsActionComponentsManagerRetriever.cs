using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public WinFormsActionComponentsManagerRetriever()
        {
            actionComponentsManager = new Lazy<IWinFormsActionComponentsManager>(
                () => new WinFormsActionComponentsManager(
                    new UIMessageForm(
                        actionComponentsManager)),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IWinFormsActionComponentsManager Retrieve() => actionComponentsManager.Value;
    }
}
