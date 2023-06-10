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
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ICachedTypeInfo type,
            bool isInstancePropsCollection) : base(
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
            this.IsInstancePropsCollection = isInstancePropsCollection;
        }

        public bool IsInstancePropsCollection { get; }

        protected override ICachedPropertiesCollection CreateCollection(
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate) => ItemsFactory.Properties(
                items, filterMatchPredicate);

        protected override ICachedPropertiesCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => this.IsInstancePropsCollection.IfTrue(
                () => baseType.InstanceProps.Value.All.Value,
                () => baseType.StaticProps.Value.All.Value);

        protected override ICachedPropertyInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetProperties(
                ReflC.Filter.AllDeclaredOnlyBindingFlags).Select(
                property => ItemsFactory.PropertyInfo(property)).ToArray();
    }
}
