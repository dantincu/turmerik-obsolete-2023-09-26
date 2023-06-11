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
    public interface ICachedEventInfo : ICachedMemberInfo<EventInfo, ICachedEventFlags>
    {
        Lazy<ICachedMethodInfo> Adder { get; }
        Lazy<ICachedMethodInfo> Remover { get; }
        Lazy<ICachedMethodInfo> Invoker { get; }
    }

    public class CachedEventInfo : CachedMemberInfoBase<EventInfo, ICachedEventFlags>, ICachedEventInfo
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

        protected override ICachedEventFlags GetFlags() => CachedEventFlags.Create(this);

        public Lazy<ICachedMethodInfo> Adder { get; }
        public Lazy<ICachedMethodInfo> Remover { get; }
        public Lazy<ICachedMethodInfo> Invoker { get; }
    }
}
