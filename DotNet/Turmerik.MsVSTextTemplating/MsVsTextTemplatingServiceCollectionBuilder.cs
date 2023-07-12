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
            Action<IServiceCollection> registerAppLoggerFactoryFunc = null)
        {
            LocalDeviceServiceCollectionBuilder.RegisterAll(
                services,
                includeNetCoreAppEnv,
                registerFsExplorerServiceEngineAsDefault,
                registerAppLoggerFactoryFunc);

            services.AddSingleton<IAppConfig, AppConfig>();
            services.AddSingleton<IClnblTypesCodeParser, ClnblTypesCodeParser>();
            services.AddSingleton<IClnblTypesCodeGenerator, ClnblTypesCodeGenerator>();
        }
    }
}
