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
    public interface ICachedEventInfo : ICachedMemberInfo<EventInfo, CachedEventFlags.IClnbl>
    {
        Lazy<ICachedMethodInfo> Adder { get; }
        Lazy<ICachedMethodInfo> Remover { get; }
        Lazy<ICachedMethodInfo> Invoker { get; }
    }

    public class CachedEventInfo : CachedMemberInfoBase<EventInfo, CachedEventFlags.IClnbl>, ICachedEventInfo
    {
        public CachedEventInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            EventInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
        }

        protected override CachedEventFlags.IClnbl GetFlags() => CachedEventFlags.Create(this);

        public Lazy<ICachedMethodInfo> Adder { get; }
        public Lazy<ICachedMethodInfo> Remover { get; }
        public Lazy<ICachedMethodInfo> Invoker { get; }
    }
}
