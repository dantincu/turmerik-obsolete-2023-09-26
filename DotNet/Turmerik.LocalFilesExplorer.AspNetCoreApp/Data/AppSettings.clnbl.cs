using Turmerik.AspNetCore.Infrastucture;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public interface IAppSettings : IAppSettingsCore
    {
    }

    public static class AppSettings
    {
    }

    public class AppSettingsImmtbl : AppSettingsCoreImmtbl, IAppSettings
    {
        public AppSettingsImmtbl(IAppSettingsCore src) : base(src)
        {
        }
    }

    public class AppSettingsMtbl : AppSettingsCoreMtbl, IAppSettings
    {
        public AppSettingsMtbl()
        {
        }

        public AppSettingsMtbl(IAppSettingsCore src) : base(src)
        {
        }
    }
}
