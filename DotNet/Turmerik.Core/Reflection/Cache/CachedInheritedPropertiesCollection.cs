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
    public interface ICachedInheritedPropertiesCollection : ICachedInheritedItemsCollection<PropertyInfo, ICachedPropertyInfo, PropertyAccessibilityFilter, ICachedPropertiesCollection>
    {
        bool IsInstancePropsCollection { get; }
    }

    public class CachedInheritedPropertiesCollection : CachedInheritedItemsCollectionBase<PropertyInfo, ICachedPropertyInfo, PropertyAccessibilityFilter, ICachedPropertiesCollection>, ICachedInheritedPropertiesCollection
    {
        public CachedInheritedPropertiesCollection(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            bool isInstancePropsCollection,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> ownFilterReducer,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> allVisibleFilterReducer,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> asmVisibleFilterReducer) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                type,
                (arg, filter) => filter.Matches(arg),
                ownFilterReducer,
                allVisibleFilterReducer,
                asmVisibleFilterReducer)
        {
            this.IsInstancePropsCollection = isInstancePropsCollection;
        }

        public bool IsInstancePropsCollection { get; }

        protected override ICachedPropertiesCollection CreateCollection(
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> filterReducer) => ItemsFactory.Properties(
                items, FilterMatchPredicate, filterReducer);

        protected override ICachedPropertiesCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => this.IsInstancePropsCollection.IfTrue(
                () => baseType.InstanceProps.Value.AsmVisible.Value,
                () => baseType.StaticProps.Value.AsmVisible.Value);

        protected override ICachedPropertiesCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => this.IsInstancePropsCollection.IfTrue(
                () => baseType.InstanceProps.Value.AllVisible.Value,
                () => baseType.StaticProps.Value.AllVisible.Value);

        protected override ICachedPropertyInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetProperties(
                ReflC.Filter.AllDeclaredOnlyBindingFlags).Select(
                property => ItemsFactory.PropertyInfo(property)).ToArray();
    }
}
