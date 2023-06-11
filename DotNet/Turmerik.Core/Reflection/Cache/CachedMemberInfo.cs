using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedMemberInfo<TMemberInfo, TFlags> : ICachedItem<TMemberInfo, TFlags>
        where TMemberInfo : MemberInfo
    {
        string Name { get; }
        MemberTypes MemberType { get; }
        Lazy<ICachedTypeInfo> DeclaringType { get; }
        Lazy<ICachedTypeInfo> ReflectedType { get; }
        Lazy<ReadOnlyCollection<Attribute>> CustomAttributes { get; }
    }

    public abstract class CachedMemberInfoBase<TMemberInfo, TFlags> : CachedItemBase<TMemberInfo, TFlags>, ICachedMemberInfo<TMemberInfo, TFlags>
        where TMemberInfo : MemberInfo
    {
        public CachedMemberInfoBase(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            TMemberInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
            Name = value.Name;
            MemberType = value.MemberType;

            DeclaringType = LazyH.Lazy(
                () => TypesMap.Value.Get(
                    Data.DeclaringType));

            ReflectedType = LazyH.Lazy(
                () => TypesMap.Value.Get(
                    Data.ReflectedType));

            CustomAttributes = new Lazy<ReadOnlyCollection<Attribute>>(
                () => Data.GetCustomAttributes().RdnlC());
        }

        public string Name { get; }
        public MemberTypes MemberType { get; }

        public Lazy<ICachedTypeInfo> DeclaringType { get; }
        public Lazy<ICachedTypeInfo> ReflectedType { get; }

        public Lazy<ReadOnlyCollection<Attribute>> CustomAttributes { get; }
    }
}
