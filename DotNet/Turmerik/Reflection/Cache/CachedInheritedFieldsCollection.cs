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
            ICachedTypeInfo type,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> ownFilterReducer,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> allVisibleFilterReducer,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> asmVisibleFilterReducer) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                type,
                (arg, filter) => filter.Matches(arg),
                ownFilterReducer,
                allVisibleFilterReducer,
                asmVisibleFilterReducer)
        {
        }

        protected override ICachedFieldsCollection CreateCollection(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> filterReducer) => ItemsFactory.Fields(
                items, FilterMatchPredicate, filterReducer);

        protected override ICachedFieldsCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => baseType.Fields.Value.AsmVisible.Value;

        protected override ICachedFieldsCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => baseType.Fields.Value.AllVisible.Value;

        protected override ICachedFieldInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetFields(
                ReflC.Filter.AllDeclaredOnlyBindingFlags).Select(
                field => ItemsFactory.FieldInfo(field)).ToArray();
    }
}
