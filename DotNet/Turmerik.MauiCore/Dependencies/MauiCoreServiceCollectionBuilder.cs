using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Dependencies;

namespace Turmerik.MauiCore.Dependencies
{
    public static class MauiCoreServiceCollectionBuilder
    {
        public static TServiceCollection RegisterAll<TServiceCollection>(
            IServiceCollection services,
            TServiceCollection mtbl)
            where TServiceCollection : TrmrkCoreServiceCollectionMtbl
        {
            return mtbl;
        }
    }
}
