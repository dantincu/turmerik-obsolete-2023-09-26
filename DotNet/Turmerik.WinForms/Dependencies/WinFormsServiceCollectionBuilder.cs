using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.Dependencies
{
    public static class WinFormsServiceCollectionBuilder
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

            services.AddSingleton<IWinFormsActionComponentsManagerRetriever, WinFormsActionComponentsManagerRetriever>();
            services.AddSingleton<IWinFormsActionComponentFactory, WinFormsActionComponentFactory>();
        }
    }
}
