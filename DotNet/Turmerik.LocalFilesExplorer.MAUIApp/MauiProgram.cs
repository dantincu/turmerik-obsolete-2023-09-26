using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Turmerik.LocalDevice.Dependencies;
using Turmerik.MauiCore.Dependencies;

namespace Turmerik.LocalFilesExplorer.MAUIApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            RegisterAllServices();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterAllServices()
        {
            var services = new ServiceCollection();
            var servicesAgg = LocalDeviceServiceCollectionBuilder.RegisterAll(services);

            MauiCoreServiceCollectionBuilder.RegisterAll(
                services,
                servicesAgg);

            ServiceProviderContainer.Instance.Value.RegisterServices(services);
        }
    }
}