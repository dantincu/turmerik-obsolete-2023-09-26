using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.MsVSTextTemplating.Components;

namespace Turmerik.MsVSTextTemplating
{
    public static class MsVsTextTemplatingServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false,
            bool registerFsExplorerServiceEngineAsDefault = false,
            bool useAppProcessIdnfForAppLoggersByDefault = true)
        {
            LocalDeviceServiceCollectionBuilder.RegisterAll(
                services,
                includeNetCoreAppEnv,
                registerFsExplorerServiceEngineAsDefault,
                useAppProcessIdnfForAppLoggersByDefault);

            RegisterAllCore(services);
        }

        public static void RegisterAllCore(
            IServiceCollection services)
        {
            services.AddSingleton<IAppConfig, AppConfig>();
            services.AddSingleton<IClnblTypesCodeParser, ClnblTypesCsCodeParser>();
            services.AddSingleton<IClnblTypesCodeGenerator, ClnblTypesCodeGenerator>();
        }
    }
}
