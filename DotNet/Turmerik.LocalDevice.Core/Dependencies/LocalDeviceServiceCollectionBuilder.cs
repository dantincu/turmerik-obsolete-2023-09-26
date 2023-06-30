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

namespace Turmerik.LocalDevice.Core.Dependencies
{
    public static class LocalDeviceServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false,
            bool registerFsExplorerServiceEngineAsDefault = false)
        {
            TrmrkCoreServiceCollectionBuilder.RegisterAll(services);

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
        }
    }
}
