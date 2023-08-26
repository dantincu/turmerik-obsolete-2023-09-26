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
    public interface ICachedPropertyInfo : ICachedMemberInfo<PropertyInfo, CachedPropertyFlags.IClnbl>
    {
        Lazy<ICachedMethodInfo> Getter { get; }
        Lazy<ICachedMethodInfo> Setter { get; }
    }

    public class CachedPropertyInfo : CachedMemberInfoBase<PropertyInfo, CachedPropertyFlags.IClnbl>, ICachedPropertyInfo
    {
        public CachedPropertyInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            PropertyInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            Getter = new Lazy<ICachedMethodInfo>(
                () => Data.GetMethod?.WithValue(
                    mth => this.ItemsFactory.MethodInfo(mth)));

            Setter = new Lazy<ICachedMethodInfo>(
                () => Data.SetMethod?.WithValue(
                    mth => this.ItemsFactory.MethodInfo(mth)));
        }

        public Lazy<ICachedMethodInfo> Getter { get; }
        public Lazy<ICachedMethodInfo> Setter { get; }

        protected override CachedPropertyFlags.IClnbl GetFlags() => CachedPropertyFlags.Create(this);
    }
}
