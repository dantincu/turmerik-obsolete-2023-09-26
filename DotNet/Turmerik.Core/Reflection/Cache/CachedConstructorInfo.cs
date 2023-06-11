using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedConstructorInfo : ICachedMethodCore<ConstructorInfo, ICachedMemberFlagsCore>
    {
    }

    public class CachedConstructorInfo : CachedMethodBase<ConstructorInfo, ICachedMemberFlagsCore>, ICachedConstructorInfo
    {
        public CachedConstructorInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            ConstructorInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
        }

        protected override ICachedMemberFlagsCore GetFlags() => CachedMemberFlagsCore.Create(this);
    }
}
