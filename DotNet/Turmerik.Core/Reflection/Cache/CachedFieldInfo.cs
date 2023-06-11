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
    public interface ICachedFieldInfo : ICachedMemberInfo<FieldInfo, CachedFieldFlags.IClnbl>
    {
    }

    public class CachedFieldInfo : CachedMemberInfoBase<FieldInfo, CachedFieldFlags.IClnbl>, ICachedFieldInfo
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

        protected override CachedFieldFlags.IClnbl GetFlags() => CachedFieldFlags.Create(this);
    }
}
