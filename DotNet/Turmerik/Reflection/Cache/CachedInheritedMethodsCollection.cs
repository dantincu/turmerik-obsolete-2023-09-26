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
    public interface ICachedInheritedMethodsCollection : ICachedInheritedItemsCollection<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter, ICachedMethodsCollection>
    {
    }

    public class CachedInheritedMethodsCollection : CachedInheritedItemsCollectionBase<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter, ICachedMethodsCollection>, ICachedInheritedMethodsCollection
    {
        public CachedInheritedMethodsCollection(
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

        protected override ICachedMethodsCollection CreateCollection(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate) => ItemsFactory.Methods(
                items, filterMatchPredicate);

        protected override ICachedMethodsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => baseType.Methods.Value.All.Value;

        protected override ICachedMethodInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetMethods(
                ReflC.Filter.AllDeclaredOnlyBindingFlags).Select(
                method => ItemsFactory.MethodInfo(method)).ToArray();
    }
}
