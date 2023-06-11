using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Synchronized;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedReflectionItemsFactory
    {
        ICachedTypeInfo TypeInfo(Type type);
        ICachedParameterInfo ParameterInfo(ParameterInfo @param);
        ICachedFieldInfo FieldInfo(FieldInfo field);
        ICachedPropertyInfo PropertyInfo(PropertyInfo property);
        ICachedEventInfo EventInfo(EventInfo @event);
        ICachedMethodInfo MethodInfo(MethodInfo method);
        ICachedConstructorInfo ConstructorInfo(ConstructorInfo constructor);
        ICachedAssemblyInfo AssemblyInfo(Assembly assembly);

        ICachedInterfaceMapping InterfaceMapping(
            ICachedTypeInfo type,
            ICachedTypeInfo interfaceType);

        ICachedInheritedPropertiesCollection InheritedProperties(
            ICachedTypeInfo type,
            bool isInstancePropsCollection);

        ICachedInheritedFieldsCollection InheritedFields(
            ICachedTypeInfo type);

        ICachedInheritedMethodsCollection InheritedMethods(
            ICachedTypeInfo type);

        ICachedInheritedEventsCollection InheritedEvents(
            ICachedTypeInfo type);

        ICachedPropertiesCollection Properties(
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> filterReducer);

        ICachedFieldsCollection Fields(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> filterReducer);

        ICachedMethodsCollection Methods(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> filterReducer);

        ICachedEventsCollection Events(
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> filterReducer);
    }

    public class CachedReflectionItemsFactory<TStaticDataCacheFactory> : ICachedReflectionItemsFactory
        where TStaticDataCacheFactory : class, IStaticDataCacheFactory
    {
        private readonly ICachedTypesMapFactory cachedTypesMapFactory;
        private readonly TStaticDataCacheFactory staticDataCacheFactory;
        private readonly IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory;

        private readonly Lazy<ICachedTypesMap> cachedTypesMap;

        public CachedReflectionItemsFactory(
            ICachedTypesMapFactory cachedTypesMapFactory,
            TStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory)
        {
            this.cachedTypesMapFactory = cachedTypesMapFactory ?? throw new ArgumentNullException(nameof(cachedTypesMapFactory));

            this.cachedTypesMap = new Lazy<ICachedTypesMap>(
                () => this.cachedTypesMapFactory.Create());

            this.staticDataCacheFactory = staticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(staticDataCacheFactory));

            this.memberAccessibiliyFilterEqualityComparerFactory = memberAccessibiliyFilterEqualityComparerFactory ?? throw new ArgumentNullException(
                nameof(memberAccessibiliyFilterEqualityComparerFactory));
        }

        public ICachedTypeInfo TypeInfo(
            Type type) => new CachedTypeInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                type);

        public ICachedParameterInfo ParameterInfo(
            ParameterInfo param) => new CachedParameterInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                param);

        public ICachedFieldInfo FieldInfo(
            FieldInfo field) => new CachedFieldInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                field);

        public ICachedPropertyInfo PropertyInfo(
            PropertyInfo property) => new CachedPropertyInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                property);

        public ICachedEventInfo EventInfo(
            EventInfo @event) => new CachedEventInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                @event);

        public ICachedMethodInfo MethodInfo(
            MethodInfo method) => new CachedMethodInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                method);

        public ICachedConstructorInfo ConstructorInfo(
            ConstructorInfo constructor) => new CachedConstructorInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                constructor);

        public ICachedAssemblyInfo AssemblyInfo(
            Assembly assembly) => new CachedAssemblyInfo(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                assembly);

        public ICachedInterfaceMapping InterfaceMapping(
            ICachedTypeInfo type,
            ICachedTypeInfo interfaceType) => new CachedInterfaceMapping(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                type,
                interfaceType);

        public ICachedInheritedPropertiesCollection InheritedProperties(
            ICachedTypeInfo type,
            bool isInstancePropsCollection) => (isInstancePropsCollection ? MemberScope.Static : MemberScope.Instance).WithValue(
                substractedScope => new CachedInheritedPropertiesCollection(
                    cachedTypesMap.Value,
                    this,
                    staticDataCacheFactory,
                    type,
                    isInstancePropsCollection,
                    filter => filter.ReduceFilterIfReq(
                        isInstancePropsCollection),
                    filter => filter.ReduceFilterIfReq(
                        isInstancePropsCollection,
                        false, true),
                    filter => filter.ReduceFilterIfReq(
                        isInstancePropsCollection,
                        true, true)));

        public ICachedInheritedFieldsCollection InheritedFields(
            ICachedTypeInfo type) => new CachedInheritedFieldsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                filter => filter.ReduceFilterIfReq(),
                filter => filter.ReduceFilterIfReq(false, true),
                filter => filter.ReduceFilterIfReq(true, true));

        public ICachedInheritedMethodsCollection InheritedMethods(
            ICachedTypeInfo type) => new CachedInheritedMethodsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                filter => filter.ReduceFilterIfReq(),
                filter => filter.ReduceFilterIfReq(false, true),
                filter => filter.ReduceFilterIfReq(true, true));

        public ICachedInheritedEventsCollection InheritedEvents(
            ICachedTypeInfo type) => new CachedInheritedEventsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                filter => filter.ReduceFilterIfReq(),
                filter => filter.ReduceFilterIfReq(false, true),
                filter => filter.ReduceFilterIfReq(true, true));

        public ICachedPropertiesCollection Properties(
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> filterReducer) => new CachedPropertiesCollection(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory.Property(),
                items,
                filterMatchPredicate,
                filterReducer);

        public ICachedFieldsCollection Fields(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> filterReducer) => new CachedFieldsCollection(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory.Field(),
                items,
                filterMatchPredicate,
                filterReducer);

        public ICachedMethodsCollection Methods(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> filterReducer) => new CachedMethodsCollection(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory.Method(),
                items,
                filterMatchPredicate,
                filterReducer);

        public ICachedEventsCollection Events(
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> filterReducer) => new CachedEventsCollection(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory.Event(),
                items,
                filterMatchPredicate,
                filterReducer);
    }

    public class NonSynchronizedCachedReflectionItemsFactory : CachedReflectionItemsFactory<INonSynchronizedStaticDataCacheFactory>
    {
        public NonSynchronizedCachedReflectionItemsFactory(
            ICachedTypesMapFactory cachedTypesMapFactory,
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory) : base(
                cachedTypesMapFactory,
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory)
        {
        }
    }

    public class ThreadSafeCachedReflectionItemsFactory : CachedReflectionItemsFactory<IThreadSafeStaticDataCacheFactory>
    {
        public ThreadSafeCachedReflectionItemsFactory(
            ICachedTypesMapFactory cachedTypesMapFactory,
            IThreadSafeStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory) : base(
                cachedTypesMapFactory,
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory)
        {
        }
    }
}
