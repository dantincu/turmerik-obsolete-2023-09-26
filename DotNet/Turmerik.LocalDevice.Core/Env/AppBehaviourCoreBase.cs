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
    public interface IAppBehaviourCore<TBehaviour> : IAppDefaultBehaviourCore<TBehaviour>
    {
        string DefaultJsFilePath { get; }

        event Action<TBehaviour> BehaviourSaved;

        TBehaviour Update(string newBehaviourJsCode);
        TBehaviour ResetToDefault();
    }

    public abstract class AppBehaviourCoreBase<TBehaviour> : AppDefaultBehaviourCoreBase<TBehaviour>, IAppBehaviourCore<TBehaviour>
    {
        private Action<TBehaviour> behaviourSaved;

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

        public event Action<TBehaviour> BehaviourSaved
        {
            add => behaviourSaved += value;
            remove => behaviourSaved -= value;
        }

        public TBehaviour Update(
            string newBehaviourJsCode) => ConcurrentActionComponent.Execute(() =>
            {
                var component = SaveJsCore(
                    newBehaviourJsCode,
                    JsFilePath);

                Component = component;
                OnDataSaved(component);

                return component.Config;
            });

        public TBehaviour ResetToDefault() => Update(
            GetDefaultBehaviourJsCode());

        protected abstract string GetDefaultBehaviourJsCodeCore();

        protected override string GetJsFilePath() => AppEnv.GetTypePath(
            AppEnvDir.Data,
            GetType(),
            JS_FILE_NAME);

        protected override string GetDefaultBehaviourJsCode() => LoadJsCore(
            DefaultJsFilePath,
            GetDefaultBehaviourJsCodeCore);

        protected virtual string GetDefaultJsFilePath() => base.GetJsFilePath();

        protected void OnDataSaved(
            IJintComponent<TBehaviour> component) => behaviourSaved?.Invoke(
                component.Config);
    }
}
