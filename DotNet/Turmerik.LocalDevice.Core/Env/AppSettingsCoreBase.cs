﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppSettingsCore<TImmtbl, TMtblSrlzbl> : IAppConfigCore<TImmtbl, TMtblSrlzbl>
        where TImmtbl : class
    {
        string DefaultJsonFilePath { get; }

        event Action<TImmtbl> DataSaved;

        TImmtbl Update(RefAction<TMtblSrlzbl> updateAction);
        TImmtbl ResetToDefault();
    }

    public abstract class AppSettingsCoreBase<TImmtbl, TMtblSrlzbl> : AppConfigCoreBase<TImmtbl, TMtblSrlzbl>, IAppSettingsCore<TImmtbl, TMtblSrlzbl>
        where TImmtbl : class
    {
        private Action<TImmtbl> dataSaved;

        protected AppSettingsCoreBase(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory) : base(
                appEnv,
                concurrentActionComponentFactory)
        {
            DefaultJsonFilePath = GetDefaultJsonFilePath();
        }

        public string DefaultJsonFilePath { get; }

        public event Action<TImmtbl> DataSaved
        {
            add => dataSaved += value;
            remove => dataSaved -= value;
        }

        public TImmtbl Update(
            RefAction<TMtblSrlzbl> updateAction) => ConcurrentActionComponent.Execute(() =>
            {
                var srlzblData = SerializeConfig(DataCore);
                updateAction(ref srlzblData);

                var data = NormalizeConfig(srlzblData);

                SaveJsonCore(
                    srlzblData,
                    JsonFilePath);

                DataCore = data;
                OnDataSaved(data);

                return data;
            });

        public TImmtbl ResetToDefault() => Update((ref TMtblSrlzbl data) =>
        {
            data = GetDefaultConfig();
        });

        protected abstract TMtblSrlzbl GetDefaultConfigCore();
        protected abstract TMtblSrlzbl SerializeConfig(TImmtbl config);

        protected override string GetJsonDirPath() => AppEnv.GetTypePath(
            AppEnvDir.Data,
            GetType());

        protected override string GetJsonFilePath() => AppEnv.GetTypePath(
            AppEnvDir.Data,
            GetType(),
            JSON_FILE_NAME);

        protected override TMtblSrlzbl GetDefaultConfig() => LoadJsonCore(
            DefaultJsonFilePath,
            GetDefaultConfigCore);

        protected virtual string GetDefaultJsonFilePath() => base.GetJsonFilePath();

        protected void OnDataSaved(TImmtbl data) => dataSaved?.Invoke(data);
    }
}
