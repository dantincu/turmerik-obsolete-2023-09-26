using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedCustomAttributeData : ICachedItem<CustomAttributeData>
    {
        Lazy<ICachedTypeInfo> AttributeType { get; }
        Lazy<ICachedConstructorInfo> Constructor { get; }
        Lazy<ReadOnlyCollection<ICachedCustomAttributeNamedArgument>> NamedArguments { get; }
    }

    public class CachedCustomAttributeData : CachedItemBase<CustomAttributeData>, ICachedCustomAttributeData
    {
        public CachedCustomAttributeData(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            CustomAttributeData value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            AttributeType = LazyH.Lazy(
                () => ItemsFactory.TypeInfo(
                    Data.AttributeType));

            Constructor = LazyH.Lazy(
                () => ItemsFactory.ConstructorInfo(
                    Data.Constructor));

            NamedArguments = LazyH.Lazy(
                () => Data.NamedArguments.Select(
                    arg => ItemsFactory.AttributeNamedArgument(arg)).RdnlC());
        }

        public Lazy<ICachedTypeInfo> AttributeType { get; }
        public Lazy<ICachedConstructorInfo> Constructor { get; }
        public Lazy<ReadOnlyCollection<ICachedCustomAttributeNamedArgument>> NamedArguments { get; }
    }
}
