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
    public interface ICachedPropertyInfo : ICachedMemberInfo<PropertyInfo, ICachedPropertyFlags>
    {
        bool CanRead { get; }
        bool CanWrite { get; }
        Lazy<ICachedMethodInfo> Getter { get; }
        Lazy<ICachedMethodInfo> Setter { get; }
    }

    public class CachedPropertyInfo : CachedMemberInfoBase<PropertyInfo, ICachedPropertyFlags>, ICachedPropertyInfo
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
            CanRead = value.CanRead;
            CanWrite = value.CanWrite;

            Getter = new Lazy<ICachedMethodInfo>(
                () => Data.GetGetMethod()?.WithValue(
                    mth => this.ItemsFactory.MethodInfo(mth)));

            Setter = new Lazy<ICachedMethodInfo>(
                () => Data.GetSetMethod()?.WithValue(
                    mth => this.ItemsFactory.MethodInfo(mth)));
        }

        public bool CanRead { get; }
        public bool CanWrite { get; }
        public Lazy<ICachedMethodInfo> Getter { get; }
        public Lazy<ICachedMethodInfo> Setter { get; }

        protected override ICachedPropertyFlags GetFlags() => CachedPropertyFlags.Create(this);
    }
}
