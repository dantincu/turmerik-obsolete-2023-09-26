using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Dependencies;
using Turmerik.DriveExplorerCore;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.LocalDevice.Core.FileExplorerCore;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.PureFuncJs.Core.Dependencies;

namespace Turmerik.LocalDevice.Core.Dependencies
{
    public static class LocalDeviceServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false,
            bool registerFsExplorerServiceEngineAsDefault = false,
            Action<IServiceCollection> registerAppLoggerFactoryFunc = null)
        {
            TrmrkCoreServiceCollectionBuilder.RegisterAll(services);
            PureFuncJsServiceCollectionBuilder.RegisterAllCore(services);

            RegisterAllCore(
                services,
                includeNetCoreAppEnv,
                registerFsExplorerServiceEngineAsDefault,
                registerAppLoggerFactoryFunc);
        }

        public static void RegisterAllCore(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false,
            bool registerFsExplorerServiceEngineAsDefault = false,
            Action<IServiceCollection> registerAppLoggerFactoryFunc = null)
        {
            RegisterAppEnv(
                services,
                includeNetCoreAppEnv);

            RegisterAppLoggerFactory(
                services,
                registerAppLoggerFactoryFunc);

            RegisterFsExplorerServices(
                services,
                registerFsExplorerServiceEngineAsDefault);
        }

        public static void RegisterAppEnv(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false)
        {
            services.AddSingleton<IAppEnv, AppEnv>();

            if (includeNetCoreAppEnv)
            {
                services.AddSingleton<INetCoreAppEnv, NetCoreAppEnv>();
            }
        }

        public static void RegisterAppLoggerFactory(
            IServiceCollection services,
            Action<IServiceCollection> registerAppLoggerFactoryFunc = null)
        {
            services.AddSingleton<ITrmrkJsonFormatterFactory, TrmrkJsonFormatterFactory>();

            if (registerAppLoggerFactoryFunc != null)
            {
                registerAppLoggerFactoryFunc(services);
            }
            else
            {
                AppLoggerFactory.UseAppProcessIdnfByDefault = true;
                services.AddSingleton<IAppLoggerFactory, AppLoggerFactory>();
            }

            services.AddSingleton<IBufferedLoggerActionComponent, BufferedLoggerActionComponent>();
        }

        public static void RegisterFsExplorerServices(
            IServiceCollection services,
            bool registerFsExplorerServiceEngineAsDefault = false)
        {
            services.AddTransient<IFsEntriesRetriever, FsEntriesRetriever>();
            services.AddTransient<IFsExplorerServiceEngine, FsExplorerServiceEngine>();

            if (registerFsExplorerServiceEngineAsDefault)
            {
                services.AddTransient<IDriveItemsRetriever, FsEntriesRetriever>();
                services.AddTransient<IDriveExplorerServiceEngine, FsExplorerServiceEngine>();
            }
        }
    }
}
