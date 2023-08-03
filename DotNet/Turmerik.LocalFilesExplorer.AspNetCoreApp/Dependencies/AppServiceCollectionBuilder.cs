using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Services;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Dependencies
{
    public static class AppServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services)
        {
            LocalDeviceServiceCollectionBuilder.RegisterAll(
                services, true, true);

            services.AddSingleton<IClientAppSettingsService, ClientAppSettingsService>();
        }
    }
}
