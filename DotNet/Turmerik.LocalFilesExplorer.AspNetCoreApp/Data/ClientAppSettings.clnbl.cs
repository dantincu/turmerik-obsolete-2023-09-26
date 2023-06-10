using Turmerik.AspNetCore.Infrastucture;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public interface IClientAppSettings : IClientAppSettingsCore
    {
    }

    public static class ClientAppSettings
    {
    }

    public class ClientAppSettingsImmtbl : ClientAppSettingsCoreImmtbl, IClientAppSettings
    {
        public ClientAppSettingsImmtbl(IClientAppSettingsCore src) : base(src)
        {
        }
    }

    public class ClientAppSettingsMtbl : ClientAppSettingsCoreMtbl, IClientAppSettings
    {
        public ClientAppSettingsMtbl()
        {
        }

        public ClientAppSettingsMtbl(IClientAppSettingsCore src) : base(src)
        {
        }
    }
}
