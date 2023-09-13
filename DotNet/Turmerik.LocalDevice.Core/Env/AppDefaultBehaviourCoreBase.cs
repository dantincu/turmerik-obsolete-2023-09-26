using Jint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.Synchronized;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppDefaultBehaviourCore<TCfg> : IDisposable
    {
        string JsFilePath { get; }
        TCfg Config { get; }

        event Action<TCfg> BehaviourLoaded;

        TCfg LoadBehaviour();
    }

    public abstract class AppDefaultBehaviourCoreBase<TCfg> : IAppDefaultBehaviourCore<TCfg>
    {
        public const string JS_FILE_NAME = "behaviour.js";
        public const string CFG_OBJ_NAME = "bhvCfg";

        private Action<TCfg> behaviourLoaded;

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

        public TCfg Config => ConcurrentActionComponent.Execute(
            () => (Component ?? LoadBehaviourNotSync()).Config);

        protected IAppEnv AppEnv { get; }
        protected IInterProcessConcurrentActionComponent ConcurrentActionComponent { get; }
        protected IJintComponentFactory ComponentFactory { get; }
        protected IJintComponent<TCfg> Component { get; set; }

        public event Action<TCfg> BehaviourLoaded
        {
            add => behaviourLoaded += value;
            remove => behaviourLoaded -= value;
        }

        public TCfg LoadBehaviour() => ConcurrentActionComponent.Execute(
            () => LoadBehaviourNotSync().Config);

        public void Dispose()
        {
            Component?.Dispose();
            ConcurrentActionComponent.Dispose();
        }

        protected abstract string GetDefaultBehaviourJsCode();

        protected virtual string GetJsFilePath() => AppEnv.GetTypePath(
            AppEnvDir.Config,
            GetType(),
            JS_FILE_NAME);

        protected virtual IJintComponent<TCfg> LoadDataNotSyncCore()
        {
            string behaviourJsCode = LoadJsCore(
                JsFilePath,
                GetDefaultBehaviourJsCode);

            var behaviour = ComponentFactory.Create<TCfg>(
                behaviourJsCode,
                CFG_OBJ_NAME);

            return behaviour;
        }

        protected void OnBehaviourLoaded(
            IJintComponent<TCfg> component) => behaviourLoaded?.Invoke(
                component.Config);

        protected IJintComponent<TCfg> LoadBehaviourNotSync()
        {
            var data = LoadDataNotSyncCore();

            Component = data;
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

        protected IJintComponent<TCfg> SaveJsCore(
            string jsCode,
            string jsFilePath)
        {
            var behaviour = ComponentFactory.Create<TCfg>(
                jsCode,
                CFG_OBJ_NAME);

            File.WriteAllText(
                jsFilePath,
                jsCode);

            return behaviour;
        }
    }
}
