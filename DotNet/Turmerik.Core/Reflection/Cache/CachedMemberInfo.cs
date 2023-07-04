using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Synchronized;
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
        Lazy<ReadOnlyCollection<Attribute>> OwnAttributes { get; }
        Lazy<ReadOnlyCollection<Attribute>> AllAttributes { get; }
        Lazy<ReadOnlyCollection<ICachedCustomAttributeData>> AttributesData { get; }

        Lazy<ReadOnlyCollection<Attribute>> GetAttributes(bool includeInherited);
    }

    public abstract class CachedMemberInfoBase<TMemberInfo, TFlags> : CachedItemBase<TMemberInfo, TFlags>, ICachedMemberInfo<TMemberInfo, TFlags>
        where TMemberInfo : MemberInfo
    {
        public CachedMemberInfoBase(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            TMemberInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
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

            OwnAttributes = LazyH.Lazy(
                () => Data.GetCustomAttributes(false).Cast<Attribute>().RdnlC());

            AllAttributes = LazyH.Lazy(
                () => Data.GetCustomAttributes(true).Cast<Attribute>().RdnlC());

            AttributesData = LazyH.Lazy(
                () => Data.GetCustomAttributesData().Select(
                    data => ItemsFactory.AttributeData(data)).RdnlC());
        }

        public string Name { get; }
        public MemberTypes MemberType { get; }

        public Lazy<ICachedTypeInfo> DeclaringType { get; }
        public Lazy<ICachedTypeInfo> ReflectedType { get; }

        public Lazy<ReadOnlyCollection<Attribute>> OwnAttributes { get; }
        public Lazy<ReadOnlyCollection<Attribute>> AllAttributes { get; }
        public Lazy<ReadOnlyCollection<ICachedCustomAttributeData>> AttributesData { get; }

        public Lazy<ReadOnlyCollection<Attribute>> GetAttributes(
            bool includeInherited) => includeInherited ? AllAttributes : OwnAttributes;
    }
}
