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
                Component.ExportedMembersRootObjVarName,
                ModuleName);
        }

        public string ModuleName { get; }
        protected IJintComponent Component { get; }
        protected string ModulePropName { get; }

        protected TResult ExecuteJs<TResult>(
            string methodName,
            params object[] args)
        {
            string jsMethodName = string.Join(
                ".",
                ModulePropName,
                methodName);

            string argsStr = string.Join(", ", args);
            argsStr = $"({argsStr});";

            string methodCallScript = string.Concat(
                jsMethodName,
                argsStr);

            var result = Component.Execute<TResult>(
                methodCallScript);

            return result;
        }
    }
}
