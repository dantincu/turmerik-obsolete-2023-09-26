using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.ObjectViewer.Lib.Components;

namespace Turmerik.ObjectViewer.Lib.Dependencies
{
    public static class ServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services)
        {
            services.AddSingleton<IObjectsContainer, ObjectsContainer>();
            services.AddSingleton<IAppSettings, AppSettings>();
        }
    }
}
