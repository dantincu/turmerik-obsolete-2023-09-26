using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Turmerik.Cache;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedCustomAttributeTypedArgument : ICachedItem<CustomAttributeTypedArgument>
    {
        Lazy<ICachedTypeInfo> ArgumentType { get; }
        object Value { get; }
    }

    public class CachedCustomAttributeTypedArgument : CachedItemBase<CustomAttributeTypedArgument>, ICachedCustomAttributeTypedArgument
    {
        public CachedCustomAttributeTypedArgument(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            CustomAttributeTypedArgument value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            ArgumentType = LazyH.Lazy(
                () => TypesMap.Value.Get(
                    Data.ArgumentType));

            Value = Data.Value;
        }

        public Lazy<ICachedTypeInfo> ArgumentType { get; }
        public object Value { get; }
    }
}
