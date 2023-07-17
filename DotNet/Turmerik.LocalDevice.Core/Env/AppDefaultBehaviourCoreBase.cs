using Jint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Turmerik.PureFuncJs.Core.JintCompnts;
using Turmerik.Synchronized;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppDefaultBehaviourCore<TBehaviour>
    {
        string JsFilePath { get; }
        TBehaviour Behaviour { get; }

        event Action<TBehaviour> BehaviourLoaded;

        TBehaviour LoadBehaviour();
    }

    public abstract class AppDefaultBehaviourCoreBase<TBehaviour> : IAppDefaultBehaviourCore<TBehaviour>
    {
        public const string JS_FILE_NAME = "behaviour.js";

        private Action<TBehaviour> behaviourLoaded;

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

        public TBehaviour Behaviour => ConcurrentActionComponent.Execute(
            () => (Component ?? LoadBehaviourNotSync()).Behaviour);

        protected IAppEnv AppEnv { get; }
        protected IInterProcessConcurrentActionComponent ConcurrentActionComponent { get; }
        protected IJintComponentFactory ComponentFactory { get; }
        protected IJintComponent<TBehaviour> Component { get; set; }

        public event Action<TBehaviour> BehaviourLoaded
        {
            add => behaviourLoaded += value;
            remove => behaviourLoaded -= value;
        }

        public TBehaviour LoadBehaviour() => ConcurrentActionComponent.Execute(
            () => LoadBehaviourNotSync().Behaviour);

        public void Dispose()
        {
            ConcurrentActionComponent.Dispose();
        }

        protected abstract string GetDefaultBehaviourJsCode();

        protected abstract TBehaviour CreateBehaviour(
            IJintComponent<TBehaviour> component,
            ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> exportedMemberNames);

        protected virtual string GetJsFilePath() => AppEnv.GetPath(
            AppEnvDir.Config,
            GetType(),
            JS_FILE_NAME);

        protected virtual IJintComponent<TBehaviour> LoadDataNotSyncCore()
        {
            string behaviourJsCode = LoadJsCore(
                JsFilePath,
                GetDefaultBehaviourJsCode);

            var behaviour = ComponentFactory.Create<TBehaviour>(
                behaviourJsCode,
                CreateBehaviour);

            return behaviour;
        }

        protected void OnBehaviourLoaded(
            IJintComponent<TBehaviour> component) => behaviourLoaded?.Invoke(
                component.Behaviour);

        protected IJintComponent<TBehaviour> LoadBehaviourNotSync()
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

        protected IJintComponent<TBehaviour> SaveJsCore(
            string jsCode,
            string jsFilePath)
        {
            var behaviour = ComponentFactory.Create<TBehaviour>(
                jsCode,
                CreateBehaviour);

            File.WriteAllText(jsFilePath, jsCode);

            return behaviour;
        }
    }
}
