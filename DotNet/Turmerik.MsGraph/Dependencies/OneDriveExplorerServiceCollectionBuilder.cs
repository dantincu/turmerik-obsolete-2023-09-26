using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.DriveExplorerCore;
using Turmerik.MsGraph.OneDriveExplorerCore;

namespace Turmerik.MsGraph.Dependencies
{
    public static class OneDriveExplorerServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool registerAsDefaultServices = false)
        {
            services.AddTransient<IOneDriveExplorerServiceEngine, OneDriveExplorerServiceEngine>();
            services.AddTransient<IOneDriveItemsRetriever, OneDriveItemsRetriever>();

            if (registerAsDefaultServices)
            {
                services.AddTransient<IDriveExplorerServiceEngine, OneDriveExplorerServiceEngine>();
                services.AddTransient<IDriveItemsRetriever, OneDriveItemsRetriever>();
            }
        }
    }
}
