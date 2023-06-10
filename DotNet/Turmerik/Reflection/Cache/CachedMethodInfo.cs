using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;

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
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            MethodInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
        }

        protected override ICachedMemberFlags GetFlags() => CachedMemberFlags.Create(this);
    }
}
