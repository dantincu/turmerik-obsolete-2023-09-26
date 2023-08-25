using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Synchronized;

namespace Turmerik.ObjectViewer.Lib.Components
{
    public interface IAppSettings : IAppSettingsCore<AppSettingsData.Immtbl, AppSettingsData.Mtbl>
    {
    }

    public class AppSettings : AppSettingsCoreBase<AppSettingsData.Immtbl, AppSettingsData.Mtbl>, IAppSettings
    {
        public AppSettings(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory) : base(
                appEnv,
                concurrentActionComponentFactory)
        {
        }

        protected override AppSettingsData.Mtbl GetDefaultConfigCore(
            ) => new AppSettingsData.Mtbl();

        protected override AppSettingsData.Immtbl NormalizeConfig(
            AppSettingsData.Mtbl config) => config.AsImmtbl();

        protected override AppSettingsData.Mtbl SerializeConfig(
            AppSettingsData.Immtbl config) => config.AsMtbl();
    }
}
