using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedFieldInfo : ICachedMemberInfo<FieldInfo, ICachedFieldFlags>
    {
    }

    public class CachedFieldInfo : CachedMemberInfoBase<FieldInfo, ICachedFieldFlags>, ICachedFieldInfo
    {
        public CachedFieldInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            FieldInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
        }

        protected override ICachedFieldFlags GetFlags() => CachedFieldFlags.Create(this);
    }
}
