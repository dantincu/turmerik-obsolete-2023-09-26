using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Dependencies;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.LocalDevice.Core.Logging;

namespace Turmerik.LocalDevice.Core.Dependencies
{
    public static class LocalDeviceServiceCollectionBuilder
    {
        public static ILocalDeviceServiceCollection RegisterAll(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false)
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

            return mtbl;
        }
    }
}
