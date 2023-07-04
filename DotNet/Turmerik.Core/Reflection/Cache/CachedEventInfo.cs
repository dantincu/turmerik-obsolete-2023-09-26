using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Synchronized;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedEventInfo : ICachedMemberInfo<EventInfo, CachedEventFlags.IClnbl>
    {
        Lazy<ICachedMethodInfo> Adder { get; }
        Lazy<ICachedMethodInfo> Remover { get; }
        Lazy<ICachedMethodInfo> Raiser { get; }
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
            Adder = LazyH.Lazy(() => Data.AddMethod?.WithValue(
                data => ItemsFactory.MethodInfo(data)));

            Remover = LazyH.Lazy(() => Data.RemoveMethod?.WithValue(
                data => ItemsFactory.MethodInfo(data)));

            Raiser = LazyH.Lazy(() => Data.RaiseMethod?.WithValue(
                data => ItemsFactory.MethodInfo(data)));
        }

        protected override CachedEventFlags.IClnbl GetFlags() => CachedEventFlags.Create(this);

        public Lazy<ICachedMethodInfo> Adder { get; }
        public Lazy<ICachedMethodInfo> Remover { get; }
        public Lazy<ICachedMethodInfo> Raiser { get; }
    }
}
