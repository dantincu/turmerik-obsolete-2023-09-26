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
    public interface ICachedFieldInfo : ICachedMemberInfo<FieldInfo, ICachedFieldFlags>
    {
    }

    public class CachedFieldInfo : CachedMemberInfoBase<FieldInfo, ICachedFieldFlags>, ICachedFieldInfo
    {
        public CachedFieldInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            FieldInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
        }

        protected override ICachedFieldFlags GetFlags() => CachedFieldFlags.Create(this);
    }
}
