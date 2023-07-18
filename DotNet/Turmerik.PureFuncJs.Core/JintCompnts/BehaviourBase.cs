using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turmerik.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public abstract class BehaviourBase
    {
        protected BehaviourBase(
            string moduleName,
            IJintComponent component)
        {
            ModuleName = moduleName ?? throw new ArgumentNullException(nameof(moduleName));
            Component = component ?? throw new ArgumentNullException(nameof(component));

            ModulePropName = string.Join(
                ".",
                JintComponent.TrmrkExpObjPath,
                ModuleName);
        }

        public string ModuleName { get; }
        protected IJintComponent Component { get; }
        protected string ModulePropName { get; }

        protected TResult Call<TResult>(
            string methodName,
            bool useCamelCase = true,
            params object[] args)
        {
            string methodCallScript = $"{ModulePropName}.{methodName}();";

            var result = Component.Call<TResult>(
                methodCallScript,
                useCamelCase,
                args);

            return result;
        }
    }
}
