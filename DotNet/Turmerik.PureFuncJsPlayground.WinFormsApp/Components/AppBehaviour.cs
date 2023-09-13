using Jint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.Synchronized;

namespace Turmerik.PureFuncJsPlayground.WinFormsApp.Components
{
    public interface IAppBehaviour : IAppBehaviourCore<IBehavioursAgg>
    {
    }

    public class AppBehaviour : AppBehaviourCoreBase<IBehavioursAgg>, IAppBehaviour
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
