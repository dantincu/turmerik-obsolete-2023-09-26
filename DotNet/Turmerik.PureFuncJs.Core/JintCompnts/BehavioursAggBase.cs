using Jint;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public abstract class BehavioursAggBase
    {
        protected BehavioursAggBase(
            IJintComponent component)
        {
            Component = component ?? throw new ArgumentNullException(nameof(component));
        }

        protected IJintComponent Component { get; }

        protected TBehaviour CreateBehaviour<TBehaviour>(
            string moduleName)
            where TBehaviour : BehaviourBase => moduleName.CreateInstance<TBehaviour>(
                null, moduleName, Component);
    }
}
