using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedInheritedFieldsCollection : ICachedInheritedItemsCollection<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter, ICachedFieldsCollection>
    {
        bool IsInstanceFieldsCollection { get; }
    }

    public class CachedInheritedFieldsCollection : CachedInheritedItemsCollectionBase<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter, ICachedFieldsCollection>, ICachedInheritedFieldsCollection
    {
        public CachedInheritedFieldsCollection(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            bool isInstanceFieldsCollection,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> ownFilterReducer,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> allVisibleFilterReducer,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> asmVisibleFilterReducer) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                type,
                (arg, filter) => arg.Matches(filter),
                ownFilterReducer,
                allVisibleFilterReducer,
                asmVisibleFilterReducer)
        {
            IsInstanceFieldsCollection = isInstanceFieldsCollection;
        }

        public bool IsInstanceFieldsCollection { get; }

        protected override ICachedFieldsCollection CreateCollection(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> filterReducer) => ItemsFactory.Fields(
                items, FilterMatchPredicate, filterReducer);

        protected override ICachedFieldsCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => this.IsInstanceFieldsCollection.IfTrue(
                () => baseType.InstanceFields.Value.ExtAsmVisible.Value,
                () => baseType.StaticFields.Value.ExtAsmVisible.Value);

        protected override ICachedFieldsCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => this.IsInstanceFieldsCollection.IfTrue(
                () => baseType.InstanceFields.Value.ExtAsmVisible.Value,
                () => baseType.StaticFields.Value.ExtAsmVisible.Value);

        protected override ICachedFieldsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => this.IsInstanceFieldsCollection.IfTrue(
                () => baseType.InstanceFields.Value.ExtAsmVisible.Value,
                () => baseType.StaticFields.Value.ExtAsmVisible.Value);

        protected override ICachedFieldInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetFields(
                ReflC.Filter.BindingFlag.DeclaredOnly).Select(
                field => ItemsFactory.FieldInfo(field)).ToArray();
    }
}
