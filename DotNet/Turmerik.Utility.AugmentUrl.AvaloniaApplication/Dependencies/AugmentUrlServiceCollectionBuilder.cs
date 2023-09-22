using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Avalonia.Dependencies;
using Turmerik.Dependencies;
using Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.Dependencies
{
    public static class AugmentUrlServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services)
        {
            AvaloniaServiceCollectionBuilder.RegisterAll(services);

            services.AddTransient<IMainViewModel, MainViewModel>();
        }
    }
}
