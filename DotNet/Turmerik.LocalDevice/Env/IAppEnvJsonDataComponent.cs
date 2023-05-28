using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Turmerik.Reflection;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.Env
{
    public interface IAppEnvJsonDataComponent<TData> : IAppEnvJsonConfigComponent<TData>
    {
        event Action<TData> DataSaved;
        void Save(TData data);
        void Save<TMtbl>(Action<TMtbl> callback) where TMtbl : TData;
    }

    public abstract class AppEnvJsonDataComponentBase<TData> : AppEnvJsonConfigComponentBase<TData>, IAppEnvJsonDataComponent<TData>
        where TData : class
    {
        protected const string DEFAULT_DATA_JSON_FILE_NAME = "data.json";

        private Action<TData> dataSaved;

        protected AppEnvJsonDataComponentBase(
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory,
            IAppEnv appEnv) : base(
                concurrentActionComponentFactory,
                appEnv)
        {
        }

        public event Action<TData> DataSaved
        {
            add => dataSaved += value;
            remove => dataSaved -= value;
        }

        public void Save(TData data)
        {
            TData immtbl = data.CreateInstance<TData>(ImmtblType);
            Directory.CreateDirectory(JsonFileDirPath);

            string json = JsonH.ToJson(immtbl);

            ConcurrentActionComponent.Execute(
            () =>
            {
                File.WriteAllText(JsonFilePath, json);
                DataCore = immtbl;
            });

            dataSaved?.Invoke(data);
        }

        public void Save<TMtbl>(Action<TMtbl> callback)
            where TMtbl : TData
        {
            TMtbl mtbl = Data.CreateInstance<TMtbl>();
            callback(mtbl);

            Save(mtbl);
        }

        protected override string GetJsonFileDirPath() => AppEnv.GetPath(AppEnvDir.Data, GetType());
        protected override string GetJsonFileName() => DEFAULT_DATA_JSON_FILE_NAME;
    }
}
