using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.Synchronized;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppDefaultBehaviourCore
    {
        string JsFilePath { get; }
        IJintComponent Behaviour { get; }

        event Action<IJintComponent> BehaviourLoaded;

        IJintComponent LoadBehaviour();
    }

    public abstract class AppDefaultBehaviourCoreBase
    {
        public const string JS_FILE_NAME = "behaviour.js";

        private Action<IJintComponent> behaviourLoaded;

        protected AppDefaultBehaviourCoreBase(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory,
            IJintComponentFactory componentFactory)
        {
            AppEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));
            JsFilePath = GetJsFilePath();

            ConcurrentActionComponent = concurrentActionComponentFactory.Create(
                JsFilePath, false, true);

            ComponentFactory = componentFactory ?? throw new ArgumentNullException(nameof(componentFactory));
        }

        public string JsFilePath { get; }

        public IJintComponent Behaviour => ConcurrentActionComponent.Execute(
            () => BehaviourCore ?? LoadDataNotSync());

        protected IAppEnv AppEnv { get; }
        protected IInterProcessConcurrentActionComponent ConcurrentActionComponent { get; }
        protected IJintComponentFactory ComponentFactory { get; }

        protected IJintComponent BehaviourCore { get; set; }

        public event Action<IJintComponent> BehaviourLoaded
        {
            add => behaviourLoaded += value;
            remove => behaviourLoaded -= value;
        }

        public IJintComponent LoadBehaviour() => ConcurrentActionComponent.Execute(
            () => LoadDataNotSync());

        public void Dispose()
        {
            ConcurrentActionComponent.Dispose();
        }

        protected abstract string GetDefaultBehaviourJsCode();

        protected virtual string GetJsFilePath() => AppEnv.GetPath(
            AppEnvDir.Config,
            GetType(),
            JS_FILE_NAME);

        protected virtual IJintComponent LoadDataNotSyncCore()
        {
            string behaviourJsCode = LoadJsCore(
                JsFilePath,
                GetDefaultBehaviourJsCode);

            var behaviour = ComponentFactory.Create(
                behaviourJsCode);

            return behaviour;
        }

        protected void OnBehaviourLoaded(
            IJintComponent behaviour) => behaviourLoaded?.Invoke(behaviour);

        protected IJintComponent LoadDataNotSync()
        {
            var data = LoadDataNotSyncCore();

            BehaviourCore = data;
            OnBehaviourLoaded(data);

            return data;
        }

        protected string LoadJsCore(
            string jsFilePath,
            Func<string> defaultCodeFactory)
        {
            string jsCode;

            if (File.Exists(jsFilePath))
            {
                jsCode = File.ReadAllText(jsFilePath);
            }
            else
            {
                jsCode = defaultCodeFactory();
            }

            return jsCode;
        }

        protected IJintComponent SaveJsCore(
            string jsCode,
            string jsFilePath)
        {
            var behaviour = ComponentFactory.Create(jsCode);
            File.WriteAllText(jsFilePath, jsCode);

            return behaviour;
        }
    }
}
