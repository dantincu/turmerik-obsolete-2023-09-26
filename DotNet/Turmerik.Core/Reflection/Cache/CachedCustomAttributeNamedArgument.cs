using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Turmerik.Cache;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedCustomAttributeNamedArgument : ICachedItem<CustomAttributeNamedArgument>
    {
        bool IsField { get; }
        string MemberName { get; }
        Lazy<ICachedCustomAttributeTypedArgument> TypedValue { get; }
    }

    public class CachedCustomAttributeNamedArgument : CachedItemBase<CustomAttributeNamedArgument>, ICachedCustomAttributeNamedArgument
    {
        public CachedCustomAttributeNamedArgument(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            CustomAttributeNamedArgument value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            IsField = Data.IsField;
            MemberName = Data.MemberName;

            TypedValue = LazyH.Lazy(
                () => ItemsFactory.AttributeTypedArgument(
                    Data.TypedValue));
        }

        public bool IsField { get; }
        public string MemberName { get; }
        public Lazy<ICachedCustomAttributeTypedArgument> TypedValue { get; }
    }
}
