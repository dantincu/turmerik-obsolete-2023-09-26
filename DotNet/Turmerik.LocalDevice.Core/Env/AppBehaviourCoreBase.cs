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

        event Action<IJintComponent<TBehaviour>> BehaviourSaved;

        IJintComponent<TBehaviour> Update(string newBehaviourJsCode);
        IJintComponent<TBehaviour> ResetToDefault();
    }

    public abstract class AppBehaviourCoreBase<TBehaviour> : AppDefaultBehaviourCoreBase<TBehaviour>, IAppBehaviourCore<TBehaviour>
    {
        private Action<IJintComponent<TBehaviour>> behaviourSaved;

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

        public event Action<IJintComponent<TBehaviour>> BehaviourSaved
        {
            add => behaviourSaved += value;
            remove => behaviourSaved -= value;
        }

        public IJintComponent<TBehaviour> Update(
            string newBehaviourJsCode) => ConcurrentActionComponent.Execute(() =>
            {
                var newBehaviour = SaveJsCore(
                    newBehaviourJsCode,
                    JsFilePath);

                BehaviourCore = newBehaviour;
                OnDataSaved(newBehaviour);

                return newBehaviour;
            });

        public IJintComponent<TBehaviour> ResetToDefault() => Update(
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

        protected void OnDataSaved(IJintComponent<TBehaviour> newBehaviour) => behaviourSaved?.Invoke(newBehaviour);
    }
}
