using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public interface IPureFuncJsPlaygroundVM
    {
        IJintComponent Component { get; }

        void SetComponent(IJintComponent component);

        ITrmrkActionResult<string> CallJs(
            string jsCode,
            string[] jsonArgsArr,
            bool useCamelCase = true);
    }

    public class PureFuncJsPlaygroundVM : ViewModelBase, IPureFuncJsPlaygroundVM
    {
        public PureFuncJsPlaygroundVM(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }

        public IJintComponent Component { get; private set; }

        public void SetComponent(IJintComponent component)
        {
            Component = component;
        }

        public ITrmrkActionResult<string> CallJs(
            string jsCode,
            string[] jsonArgsArr,
            bool useCamelCase = true) => ActionComponent.Execute(
                new TrmrkActionComponentOpts<string>
                {
                    Action = () =>
                    {
                        jsCode = JintH.CreateScript(
                            jsCode,
                            jsonArgsArr);

                        string output = Component.Call(jsCode, useCamelCase);

                        return new TrmrkActionResult<string>
                        {
                            Data = output
                        }.AsIntf();
                    },
                    ActionName = nameof(CallJs)
                });
    }
}
