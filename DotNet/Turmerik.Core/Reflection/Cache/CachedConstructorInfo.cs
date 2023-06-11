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
    public interface ICachedConstructorInfo : ICachedMethodCore<ConstructorInfo, ICachedMemberFlagsCore>
    {
    }

    public class CachedConstructorInfo : CachedMethodBase<ConstructorInfo, ICachedMemberFlagsCore>, ICachedConstructorInfo
    {
        public CachedConstructorInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            ConstructorInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
        }

        protected override ICachedMemberFlagsCore GetFlags() => CachedMemberFlagsCore.Create(this);
    }
}
