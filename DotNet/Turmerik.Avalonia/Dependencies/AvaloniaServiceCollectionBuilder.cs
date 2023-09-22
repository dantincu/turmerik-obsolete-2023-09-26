using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Dependencies;

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
        }
    }
}
