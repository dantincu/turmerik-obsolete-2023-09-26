using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Turmerik.Synchronized;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppConfigCore<TImmtbl, TMtblSrlzbl> : IDisposable
        where TImmtbl : class
    {
        string JsonFilePath { get; }
        TImmtbl Data { get; }

        event Action<TImmtbl> DataLoaded;

        TImmtbl LoadData();
    }

    public abstract class AppConfigCoreBase<TImmtbl, TMtblSrlzbl> : IAppConfigCore<TImmtbl, TMtblSrlzbl>
        where TImmtbl : class
    {
        public const string JSON_FILE_NAME = "data.json";

        private Action<TImmtbl> dataLoaded;

        protected AppConfigCoreBase(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory)
        {
            AppEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));
            JsonFilePath = GetJsonFilePath();

            ConcurrentActionComponent = concurrentActionComponentFactory.Create(
                JsonFilePath);
        }

        public string JsonFilePath { get; }

        public TImmtbl Data => ConcurrentActionComponent.Execute(
            () => DataCore ?? LoadDataNotSync());

        protected IAppEnv AppEnv { get; }
        protected IInterProcessConcurrentActionComponent ConcurrentActionComponent { get; }

        protected TImmtbl DataCore { get; set; }

        public event Action<TImmtbl> DataLoaded
        {
            add => dataLoaded += value;
            remove => dataLoaded -= value;
        }

        public TImmtbl LoadData() => ConcurrentActionComponent.Execute(
            () => LoadDataNotSync());

        public void Dispose()
        {
            ConcurrentActionComponent.Dispose();
        }

        protected abstract TMtblSrlzbl GetDefaultConfig();
        protected abstract TImmtbl NormalizeConfig(TMtblSrlzbl config);

        protected virtual string GetJsonFilePath() => AppEnv.GetPath(
            AppEnvDir.Config,
            GetType(),
            JSON_FILE_NAME);

        protected virtual TImmtbl LoadDataNotSyncCore()
        {
            TMtblSrlzbl dataMtblSrlzbl = LoadJsonCore(
                JsonFilePath,
                GetDefaultConfig);

            var data = NormalizeConfig(dataMtblSrlzbl);
            return data;
        }

        protected void OnDataLoaded(TImmtbl data) => dataLoaded?.Invoke(data);

        protected TImmtbl LoadDataNotSync()
        {
            var data = LoadDataNotSyncCore();

            DataCore = data;
            OnDataLoaded(data);

            return data;
        }

        protected SrlzblData LoadJsonCore<SrlzblData>(
            string jsonFilePath,
            Func<SrlzblData> defaultValueFactory)
        {
            SrlzblData srlzblData;

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);

                srlzblData = JsonH.FromJson<SrlzblData>(
                    json, false);
            }
            else
            {
                srlzblData = defaultValueFactory();
            }

            return srlzblData;
        }

        protected void SaveJsonCore(
            object obj,
            string jsonFilePath)
        {
            string json = JsonH.ToJson(
                obj, false);

            File.WriteAllText(jsonFilePath, json);
        }
    }
}
