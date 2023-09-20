using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.TrmrkAction;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Components;
using Turmerik.WinForms.ViewModels;

namespace Turmerik.WinForms.Dependencies
{
    public static class WinFormsServiceCollectionBuilder
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
            services.AddSingleton<IWinFormsActionComponentsManagerRetriever, WinFormsActionComponentsManagerRetriever>();
            services.AddSingleton<IWinFormsActionComponentFactory, WinFormsActionComponentFactory>();
            services.AddSingleton<ITrmrkActionComponentFactory, WinFormsActionComponentFactory>();

            services.AddSingleton<ITrmrkWinFormsActionComponentsManagerRetriever, TrmrkWinFormsActionComponentsManagerRetriever>();
            services.AddSingleton<ITrmrkWinFormsActionComponentFactory, TrmrkWinFormsActionComponentFactory>();

            services.AddSingleton<IToolStripItemFactory, ToolStripItemFactory>();
            services.AddSingleton<IContextMenuStripFactory, ContextMenuStripFactory>();
            services.AddSingleton<IImageListDecoratorFactory, ImageListDecoratorFactory>();
            services.AddSingleton<ITreeViewDataAdapterFactory, TreeViewDataAdapterFactory>();
        }
    }
}
