using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.ObjectViewer.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.ObjectViewer.WinFormsApp.Dependencies
{
    public static class AppServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services)
        {
            WinFormsServiceCollectionBuilder.RegisterAll(services);

            services.AddTransient<IMainFormVM, MainFormVM>();
            services.AddTransient<IObjectViewerVM, ObjectViewerVM>();
            services.AddTransient<IFieldViewerVM, FieldViewerVM>();
        }
    }
}
