using Jint;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public abstract class BehavioursAggBase
    {
        protected BehavioursAggBase(Engine jsEngine)
        {
            JsEngine = jsEngine ?? throw new ArgumentNullException(nameof(jsEngine));
        }

        protected Engine JsEngine { get; }

        protected TBehaviour CreateBehaviour<TBehaviour>(
            string moduleName)
            where TBehaviour : BehaviourBase => moduleName.CreateInstance<TBehaviour>(
                null, moduleName, JsEngine);
    }
}
