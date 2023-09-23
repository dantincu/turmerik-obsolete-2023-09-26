using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Avalonia.ActionComponent;
using Turmerik.Dependencies;
using Turmerik.TrmrkAction;

namespace Turmerik.Avalonia.Dependencies
{
    public static class AvaloniaServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool addCurrentProcessInfo = false)
        {
            TrmrkCoreServiceCollectionBuilder.RegisterAll(
                services,
                addCurrentProcessInfo);

            services.AddSingleton<ITrmrkAvlnActionComponentsManagerRetriever, TrmrkAvlnActionComponentsManagerRetriever>();
            services.AddSingleton<ITrmrkAvlnActionComponentFactory, TrmrkAvlnActionComponentFactory>();
            services.AddSingleton<ITrmrkActionComponentFactory, TrmrkAvlnActionComponentFactory>();
        }
    }
}
