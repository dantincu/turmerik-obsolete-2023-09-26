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
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate);

        ICachedFieldsCollection Fields(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate);

        ICachedMethodsCollection Methods(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate);

        ICachedEventsCollection Events(
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate);
    }

    public class CachedReflectionItemsFactory : ICachedReflectionItemsFactory
    {
        private readonly ICachedTypesMapFactory cachedTypesMapFactory;
        private readonly INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory;
        private readonly IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory;

        private readonly Lazy<ICachedTypesMap> cachedTypesMap;

        public CachedReflectionItemsFactory(
            ICachedTypesMapFactory cachedTypesMapFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory)
        {
            this.cachedTypesMapFactory = cachedTypesMapFactory ?? throw new ArgumentNullException(nameof(cachedTypesMapFactory));

            this.cachedTypesMap = new Lazy<ICachedTypesMap>(
                () => this.cachedTypesMapFactory.Create());

            this.nonSynchronizedStaticDataCacheFactory = nonSynchronizedStaticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(nonSynchronizedStaticDataCacheFactory));

            this.memberAccessibiliyFilterEqualityComparerFactory = memberAccessibiliyFilterEqualityComparerFactory ?? throw new ArgumentNullException(
                nameof(memberAccessibiliyFilterEqualityComparerFactory));
        }

        public ICachedTypeInfo TypeInfo(
            Type type) => new CachedTypeInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                type);

        public ICachedParameterInfo ParameterInfo(
            ParameterInfo param) => new CachedParameterInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                param);

        public ICachedFieldInfo FieldInfo(
            FieldInfo field) => new CachedFieldInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                field);

        public ICachedPropertyInfo PropertyInfo(
            PropertyInfo property) => new CachedPropertyInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                property);

        public ICachedEventInfo EventInfo(
            EventInfo @event) => new CachedEventInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                @event);

        public ICachedMethodInfo MethodInfo(
            MethodInfo method) => new CachedMethodInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                method);

        public ICachedConstructorInfo ConstructorInfo(
            ConstructorInfo constructor) => new CachedConstructorInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                constructor);

        public ICachedAssemblyInfo AssemblyInfo(
            Assembly assembly) => new CachedAssemblyInfo(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                assembly);

        public ICachedInterfaceMapping InterfaceMapping(
            ICachedTypeInfo type,
            ICachedTypeInfo interfaceType) => new CachedInterfaceMapping(
                cachedTypesMap,
                this,
                nonSynchronizedStaticDataCacheFactory,
                type,
                interfaceType);

        public ICachedInheritedPropertiesCollection InheritedProperties(
            ICachedTypeInfo type,
            bool isInstancePropsCollection) => new CachedInheritedPropertiesCollection(
                cachedTypesMap.Value,
                this,
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                type,
                isInstancePropsCollection);

        public ICachedInheritedFieldsCollection InheritedFields(
            ICachedTypeInfo type) => new CachedInheritedFieldsCollection(
                cachedTypesMap.Value,
                this,
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                type);

        public ICachedInheritedMethodsCollection InheritedMethods(
            ICachedTypeInfo type) => new CachedInheritedMethodsCollection(
                cachedTypesMap.Value,
                this,
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                type);

        public ICachedInheritedEventsCollection InheritedEvents(
            ICachedTypeInfo type) => new CachedInheritedEventsCollection(
                cachedTypesMap.Value,
                this,
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                type);

        public ICachedPropertiesCollection Properties(
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate) => new CachedPropertiesCollection(
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate);

        public ICachedFieldsCollection Fields(
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate) => new CachedFieldsCollection(
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate);

        public ICachedMethodsCollection Methods(
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate) => new CachedMethodsCollection(
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate);

        public ICachedEventsCollection Events(
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate) => new CachedEventsCollection(
                nonSynchronizedStaticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate);
    }
}
