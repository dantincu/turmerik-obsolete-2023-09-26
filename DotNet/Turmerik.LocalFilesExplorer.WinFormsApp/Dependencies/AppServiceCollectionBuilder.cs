using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalFilesExplorer.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.LocalFilesExplorer.WinFormsApp.Dependencies
{
    public static class AppServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool includeNetCoreAppEnv = false,
            bool registerFsExplorerServiceEngineAsDefault = false,
            bool useAppProcessIdnfForAppLoggersByDefault = true)
        {
            WinFormsServiceCollectionBuilder.RegisterAll(
                services,
                includeNetCoreAppEnv,
                registerFsExplorerServiceEngineAsDefault,
                useAppProcessIdnfForAppLoggersByDefault);

            DriveExplorerCore.WinFormsUCLib.Dependencies.ServiceCollectionBuilder.RegisterAll(services);

            services.AddTransient<IMainFormVM, MainFormVM>();
        }
    }
}
