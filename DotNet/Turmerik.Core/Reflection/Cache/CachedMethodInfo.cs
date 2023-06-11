using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Synchronized;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedMethodInfo : ICachedMethodCore<MethodInfo, ICachedMemberFlags>
    {
    }

    public class CachedMethodInfo : CachedMethodBase<MethodInfo, ICachedMemberFlags>, ICachedMethodInfo
    {
        public CachedMethodInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            MethodInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
        }

        protected override ICachedMemberFlags GetFlags() => CachedMemberFlags.Create(this);
    }
}
