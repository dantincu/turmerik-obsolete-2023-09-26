using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turmerik.Collections;
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

        protected string CallMethod(
            string methodName,
            bool useCamelCase = true,
            params object[] args) => Component.CallMethod(
                ModulePropName.Arr(
                    methodName),
                useCamelCase,
                args);

        protected TResult CallMethod<TResult>(
            string methodName,
            bool useCamelCase = true,
            params object[] args) => Component.CallMethod<TResult>(
                ModulePropName.Arr(
                    methodName),
                useCamelCase,
                args);

        protected string CallMethod(
            string[] methodPath,
            bool useCamelCase = true,
            params object[] args) => Component.CallMethod(
                ModulePropName.Arr().Concat(
                    methodPath).ToArray(),
                useCamelCase,
                args);

        protected TResult CallMethod<TResult>(
            string[] methodPath,
            bool useCamelCase = true,
            params object[] args) => Component.CallMethod<TResult>(
                ModulePropName.Arr().Concat(
                    methodPath).ToArray(),
                useCamelCase,
                args);
    }
}
