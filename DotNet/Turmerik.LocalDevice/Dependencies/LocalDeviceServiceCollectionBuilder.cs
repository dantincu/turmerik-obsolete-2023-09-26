using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Dependencies;
using Turmerik.LocalDevice.Logging;
using Turmerik.LocalDevice.Env;
using Turmerik.DriveExplorerCore;
using Turmerik.LocalDevice.FileExplorerCore;

namespace Turmerik.LocalDevice.Dependencies
{
    public static class LocalDeviceServiceCollectionBuilder
    {
        public static ILocalDeviceServiceCollection RegisterAll(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false,
            bool registerFsExplorerServiceEngineAsDefault = false)
        {
            LocalDeviceServiceCollectionMtbl mtbl = new();
            TrmrkCoreServiceCollectionBuilder.RegisterAll(services, mtbl);

            services.AddSingleton<IAppEnv, AppEnv>();
            services.AddSingleton<IAppLoggerFactory, AppLoggerFactory>();
            services.AddSingleton<IBufferedLoggerActionComponent, BufferedLoggerActionComponent>();

            if (includeNetCoreAppEnv)
            {
                services.AddSingleton<INetCoreAppEnv, NetCoreAppEnv>();
            }

            services.AddTransient<IFsEntriesRetriever, FsEntriesRetriever>();
            services.AddTransient<IFsExplorerServiceEngine, FsExplorerServiceEngine>();

            if (registerFsExplorerServiceEngineAsDefault)
            {
                services.AddTransient<IDriveItemsRetriever, FsEntriesRetriever>();
                services.AddTransient<IDriveExplorerServiceEngine, FsExplorerServiceEngine>();
            }

            return mtbl;
        }
    }
}
