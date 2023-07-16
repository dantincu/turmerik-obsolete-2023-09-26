using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.PureFuncJs.Core.JintCompnts;

namespace Turmerik.PureFuncJs.Core.Dependencies
{
    public static class PureFuncJsServiceCollectionBuilder
    {
        public static void RegisterAllCore(
            IServiceCollection services)
        {
            services.AddSingleton<IJintComponentFactory, JintComponentFactory>();
        }
    }
}
