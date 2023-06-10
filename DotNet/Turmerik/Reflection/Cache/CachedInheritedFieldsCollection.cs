using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedInheritedFieldsCollection : ICachedInheritedItemsCollection<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter, ICachedFieldsCollection>
    {
    }

    public class CachedInheritedFieldsCollection : CachedInheritedItemsCollectionBase<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter, ICachedFieldsCollection>, ICachedInheritedFieldsCollection
    {
        public CachedInheritedFieldsCollection(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ICachedTypeInfo type) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                type,
                (arg, filter, isSameAssebly) => arg.Matches(
                    filter,
                    !isSameAssebly,
                    true))
        {
        }

        protected override ICachedFieldsCollection CreateCollection(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate) => ItemsFactory.Fields(
                items, filterMatchPredicate);

        protected override ICachedFieldsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => baseType.Fields.Value.All.Value;

        protected override ICachedFieldInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetFields(
                ReflC.Filter.AllDeclaredOnlyBindingFlags).Select(
                field => ItemsFactory.FieldInfo(field)).ToArray();
    }
}
