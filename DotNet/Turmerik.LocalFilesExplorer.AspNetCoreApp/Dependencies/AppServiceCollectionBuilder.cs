using Microsoft.AspNetCore.Authentication;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Services;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Dependencies
{
    public static class AppServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            IConfiguration configuration,
            out IClientAppSettingsService clientAppSettingsService,
            out IAppSettingsService appSettingsService)
        {
            LocalDeviceServiceCollectionBuilder.RegisterAll(
                services, true, true);

            var clientAppSettingsSvc = new ClientAppSettingsService();
            var appSettingsSvc = new AppSettingsService(configuration);

            services.AddSingleton<IClientAppSettingsService>(svcProv => clientAppSettingsSvc);
            services.AddSingleton<IAppSettingsService>(svcProv => appSettingsSvc);

            services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

            clientAppSettingsService = clientAppSettingsSvc;
            appSettingsService = appSettingsSvc;
        }
    }
}
