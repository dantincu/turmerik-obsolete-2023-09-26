using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Turmerik.Reflection;
using Turmerik.Synchronized;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppEnvJsonConfigComponent<TData>
    {
        TData Data { get; }
        event Action<TData> DataLoaded;
        TData Load();
    }

    public abstract class AppEnvJsonConfigComponentBase<TData> : IAppEnvJsonConfigComponent<TData>
        where TData : class
    {
        protected const string DEFAULT_CONFIG_JSON_FILE_NAME = "config.json";

        protected static readonly Type DataType = typeof(TData);

        private Action<TData> dataLoaded;

        protected AppEnvJsonConfigComponentBase(
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory,
            IAppEnv appEnv)
        {
            ConcurrentActionComponentFactory = concurrentActionComponentFactory ?? throw new ArgumentNullException(
                nameof(concurrentActionComponentFactory));

            AppEnv = appEnv ?? throw new ArgumentNullException(
                nameof(appEnv));

            JsonFileDirPath = GetJsonFileDirPath();
            JsonFileName = GetJsonFileName();

            JsonFilePath = Path.Combine(
                JsonFileDirPath,
                JsonFileName);

            ConcurrentActionComponent = ConcurrentActionComponentFactory.Create(
                JsonFilePath,
                createGlobalMutex: true);
        }

        public TData Data
        {
            get
            {
                ConcurrentActionComponent.Execute(() =>
                {
                    if (DataCore == null)
                    {
                        LoadCore();
                    }
                });

                return DataCore;
            }
        }

        protected IInterProcessConcurrentActionComponentFactory ConcurrentActionComponentFactory { get; }
        protected IInterProcessConcurrentActionComponent ConcurrentActionComponent { get; }
        protected TData DataCore { get; set; }

        protected IAppEnv AppEnv { get; }

        protected string JsonFileDirPath { get; }
        protected string JsonFileName { get; }
        protected string JsonFilePath { get; }

        protected abstract Type MtblType { get; }
        protected abstract Type ImmtblType { get; }

        public event Action<TData> DataLoaded
        {
            add => dataLoaded += value;
            remove => dataLoaded -= value;
        }

        public TData Load()
        {
            ConcurrentActionComponent.Execute(LoadCore);

            dataLoaded?.Invoke(DataCore);
            return DataCore;
        }

        protected virtual string GetJsonFileDirPath() => AppEnv.GetPath(AppEnvDir.Config, GetType());
        protected virtual string GetJsonFileName() => DEFAULT_CONFIG_JSON_FILE_NAME;
        protected virtual TData GetDefaultMtbl(Type mtblType) => Activator.CreateInstance(mtblType) as TData;

        private void LoadCore()
        {
            var mtblData = LoadMtblCore(MtblType);
            DataCore = mtblData.CreateInstance<TData>(ImmtblType);
        }

        private TData LoadMtblCore(Type mtblType)
        {
            TData mtblData;

            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                mtblData = JsonConvert.DeserializeObject(json, mtblType) as TData;
            }
            else
            {
                mtblData = GetDefaultMtbl(mtblType);
            }

            return mtblData;
        }
    }
}
