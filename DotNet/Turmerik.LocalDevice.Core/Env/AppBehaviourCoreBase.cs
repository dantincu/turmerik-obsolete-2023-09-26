using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppBehaviourCore : IAppDefaultBehaviourCore
    {
        string DefaultJsFilePath { get; }

        event Action<IJintComponent> BehaviourSaved;

        IJintComponent Update(string newBehaviourJsCode);
        IJintComponent ResetToDefault();
    }

    public abstract class AppBehaviourCoreBase : AppDefaultBehaviourCoreBase, IAppBehaviourCore
    {
        private Action<IJintComponent> behaviourSaved;

        protected AppBehaviourCoreBase(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory,
            IJintComponentFactory componentFactory) : base(
                appEnv,
                concurrentActionComponentFactory,
                componentFactory)
        {
            DefaultJsFilePath = GetDefaultJsFilePath();
        }

        public string DefaultJsFilePath { get; }

        public event Action<IJintComponent> BehaviourSaved
        {
            add => behaviourSaved += value;
            remove => behaviourSaved -= value;
        }

        public IJintComponent Update(
            string newBehaviourJsCode) => ConcurrentActionComponent.Execute(() =>
            {
                var newBehaviour = SaveJsCore(
                    newBehaviourJsCode,
                    JsFilePath);

                BehaviourCore = newBehaviour;
                OnDataSaved(newBehaviour);

                return newBehaviour;
            });

        public IJintComponent ResetToDefault() => Update(
            GetDefaultBehaviourJsCode());

        protected abstract string GetDefaultBehaviourJsCodeCore();

        protected override string GetJsFilePath() => AppEnv.GetPath(
            AppEnvDir.Data,
            GetType(),
            JS_FILE_NAME);

        protected override string GetDefaultBehaviourJsCode() => LoadJsCore(
            DefaultJsFilePath,
            GetDefaultBehaviourJsCodeCore);

        protected virtual string GetDefaultJsFilePath() => base.GetJsFilePath();

        protected void OnDataSaved(IJintComponent newBehaviour) => behaviourSaved?.Invoke(newBehaviour);
    }
}
