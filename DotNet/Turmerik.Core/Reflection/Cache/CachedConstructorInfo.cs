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
    public interface ICachedConstructorInfo : ICachedMethodCore<ConstructorInfo, CachedMemberFlagsCore.IClnbl>
    {
    }

    public class CachedConstructorInfo : CachedMethodBase<ConstructorInfo, CachedMemberFlagsCore.IClnbl>, ICachedConstructorInfo
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

        protected override CachedMemberFlagsCore.IClnbl GetFlags() => CachedMemberFlagsCore.Create(this);
    }
}
