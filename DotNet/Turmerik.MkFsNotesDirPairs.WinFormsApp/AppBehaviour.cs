using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.Synchronized;

namespace Turmerik.MkFsNotesDirPairs.WinFormsApp
{
    public interface IAppBehaviour : IAppBehaviourCore
    {

    }

    public class AppBehaviour : AppBehaviourCoreBase, IAppBehaviour
    {
        public AppBehaviour(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory,
            IJintComponentFactory componentFactory) : base(
                appEnv,
                concurrentActionComponentFactory,
                componentFactory)
        {
        }

        protected override string GetDefaultBehaviourJsCodeCore(
            ) => throw new NotSupportedException("No default js code availlable");
    }
}
