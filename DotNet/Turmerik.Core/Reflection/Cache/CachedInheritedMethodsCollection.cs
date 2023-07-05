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
    public interface ICachedInheritedMethodsCollection : ICachedInheritedItemsCollection<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter, ICachedMethodsCollection>
    {
        bool IsInstanceMethodsCollection { get; }
    }

    public class CachedInheritedMethodsCollection : CachedInheritedItemsCollectionBase<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter, ICachedMethodsCollection>, ICachedInheritedMethodsCollection
    {
        public CachedInheritedMethodsCollection(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            bool isInstanceMethodsCollection,
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
            IsInstanceMethodsCollection = isInstanceMethodsCollection;
        }

        public bool IsInstanceMethodsCollection { get; }

        protected override ICachedMethodsCollection CreateCollection(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> filterReducer) => ItemsFactory.Methods(
                items, FilterMatchPredicate, filterReducer);

        protected override ICachedMethodsCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => this.IsInstanceMethodsCollection.IfTrue(
                () => baseType.InstanceMethods.Value.ExtAsmVisible.Value,
                () => baseType.StaticMethods.Value.ExtAsmVisible.Value);

        protected override ICachedMethodsCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => this.IsInstanceMethodsCollection.IfTrue(
                () => baseType.InstanceMethods.Value.ExtAsmVisible.Value,
                () => baseType.StaticMethods.Value.ExtAsmVisible.Value);

        protected override ICachedMethodsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => this.IsInstanceMethodsCollection.IfTrue(
                () => baseType.InstanceMethods.Value.Own.Value,
                () => baseType.StaticMethods.Value.Own.Value);

        protected override ICachedMethodInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetMethods(
                ReflC.Filter.BindingFlag.DeclaredOnly).Where(
                method => !method.IsSpecialName).Select(
                method => ItemsFactory.MethodInfo(method)).ToArray();
    }
}
