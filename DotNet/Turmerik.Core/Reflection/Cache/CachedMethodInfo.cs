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
    public interface ICachedMethodInfo : ICachedMethodCore<MethodInfo, CachedMemberFlags.IClnbl>
    {
    }

    public class CachedMethodInfo : CachedMethodBase<MethodInfo, CachedMemberFlags.IClnbl>, ICachedMethodInfo
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

        protected override CachedMemberFlags.IClnbl GetFlags() => CachedMemberFlags.Create(this);
    }
}
