using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Dependencies
{
    public static class ServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services)
        {
            Lib.Dependencies.ServiceCollectionBuilder.RegisterAll(services);
        }
    }
}
