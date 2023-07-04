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
            IStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> ownFilterReducer,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> allVisibleFilterReducer,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> asmVisibleFilterReducer) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                type,
                (arg, filter) => arg.Matches(filter),
                ownFilterReducer,
                allVisibleFilterReducer,
                asmVisibleFilterReducer)
        {
        }

        protected override ICachedMethodsCollection CreateCollection(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> filterReducer) => ItemsFactory.Methods(
                items, FilterMatchPredicate, filterReducer);

        protected override ICachedMethodsCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => baseType.InstanceMethods.Value.AsmVisible.Value;

        protected override ICachedMethodsCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => baseType.InstanceMethods.Value.AllVisible.Value;

        protected override ICachedMethodInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetMethods(
                ReflC.Filter.BindingFlag.DeclaredOnly).Select(
                method => ItemsFactory.MethodInfo(method)).ToArray();
    }
}
