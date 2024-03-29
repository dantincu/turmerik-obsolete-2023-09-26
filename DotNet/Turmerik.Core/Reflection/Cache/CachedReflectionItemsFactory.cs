﻿using System;
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
        ICachedCustomAttributeData AttributeData(CustomAttributeData attributeData);
        ICachedCustomAttributeTypedArgument AttributeTypedArgument(CustomAttributeTypedArgument argument);
        ICachedCustomAttributeNamedArgument AttributeNamedArgument(CustomAttributeNamedArgument argument);

        ICachedInterfaceMapping InterfaceMapping(
            ICachedTypeInfo type,
            ICachedTypeInfo interfaceType);

        ICachedInheritedPropertiesCollection InheritedProperties(
            ICachedTypeInfo type,
            bool isInstancePropsCollection,
            bool publicOnly = false,
            bool excludeInheritables = false);

        ICachedInheritedFieldsCollection InheritedFields(
            ICachedTypeInfo type,
            bool isInstanceFieldsCollection,
            bool excludeInheritables = false);

        ICachedInheritedMethodsCollection InheritedMethods(
            ICachedTypeInfo type,
            bool isInstanceMethodsCollection,
            bool publicOnly = false,
            bool excludeInheritables = false);

        ICachedInheritedEventsCollection InheritedEvents(
            ICachedTypeInfo type,
            bool publicOnly = false);

        ICachedInheritedConstructorsCollection InheritedConstructors(
            ICachedTypeInfo type,
            bool publicOnly = false);

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

        ICachedConstructorsCollection Constructors(
            ReadOnlyCollection<ICachedConstructorInfo> items,
            Func<ICachedConstructorInfo, MemberVisibility, bool> filterMatchPrediate,
            Func<MemberVisibility, MemberVisibility> filterReducer);
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

        public ICachedCustomAttributeData AttributeData(
            CustomAttributeData attributeData) => new CachedCustomAttributeData(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                attributeData);

        public ICachedCustomAttributeTypedArgument AttributeTypedArgument(
            CustomAttributeTypedArgument argument) => new CachedCustomAttributeTypedArgument(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                argument);

        public ICachedCustomAttributeNamedArgument AttributeNamedArgument(
            CustomAttributeNamedArgument argument) => new CachedCustomAttributeNamedArgument(
                cachedTypesMap,
                this,
                staticDataCacheFactory,
                argument);

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
            bool isInstancePropsCollection,
            bool publicOnly = false,
            bool excludeInheritables = false) => (isInstancePropsCollection ? MemberScope.Static : MemberScope.Instance).WithValue(
                substractedScope => new CachedInheritedPropertiesCollection(
                    cachedTypesMap.Value,
                    this,
                    staticDataCacheFactory,
                    type,
                    isInstancePropsCollection,
                    filter => filter.ReduceFilterIfReq(
                        isInstancePropsCollection,
                        false, false, publicOnly,
                        excludeInheritables),
                    filter => filter.ReduceFilterIfReq(
                        isInstancePropsCollection,
                        false, true, publicOnly,
                        excludeInheritables),
                    filter => filter.ReduceFilterIfReq(
                        isInstancePropsCollection,
                        true, true, publicOnly,
                        excludeInheritables)));

        public ICachedInheritedFieldsCollection InheritedFields(
            ICachedTypeInfo type,
            bool isInstanceFieldsCollection,
            bool excludeInheritables = false) => new CachedInheritedFieldsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                isInstanceFieldsCollection,
                filter => filter.ReduceFilterIfReq(false, false, excludeInheritables),
                filter => filter.ReduceFilterIfReq(false, true, excludeInheritables),
                filter => filter.ReduceFilterIfReq(true, true, excludeInheritables));

        public ICachedInheritedMethodsCollection InheritedMethods(
            ICachedTypeInfo type,
            bool isInstanceMethodsCollection,
            bool publicOnly = false,
            bool excludeInheritables = false) => new CachedInheritedMethodsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                isInstanceMethodsCollection,
                filter => filter.ReduceFilterIfReq(
                    isInstanceMethodsCollection,
                    false, false, publicOnly,
                    excludeInheritables),
                filter => filter.ReduceFilterIfReq(
                    isInstanceMethodsCollection,
                    false, true, publicOnly,
                    excludeInheritables),
                filter => filter.ReduceFilterIfReq(
                    isInstanceMethodsCollection,
                    true, true, publicOnly,
                    excludeInheritables));

        public ICachedInheritedEventsCollection InheritedEvents(
            ICachedTypeInfo type,
            bool publicOnly = false) => new CachedInheritedEventsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                filter => filter.ReduceFilterIfReq(false, false, publicOnly),
                filter => filter.ReduceFilterIfReq(false, true, publicOnly),
                filter => filter.ReduceFilterIfReq(true, true, publicOnly));

        public ICachedInheritedConstructorsCollection InheritedConstructors(
            ICachedTypeInfo type,
            bool publicOnly = false) => new CachedInheritedConstructorsCollection(
                cachedTypesMap.Value,
                this,
                staticDataCacheFactory,
                type,
                filter => filter.ReduceIfReq(false, false, publicOnly),
                filter => filter.ReduceIfReq(false, true, publicOnly),
                filter => filter.ReduceIfReq(true, true, publicOnly));

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

        public ICachedConstructorsCollection Constructors(
            ReadOnlyCollection<ICachedConstructorInfo> items,
            Func<ICachedConstructorInfo, MemberVisibility, bool> filterMatchPredicate,
            Func<MemberVisibility, MemberVisibility> filterReducer) => new CachedConstructorsCollection(
                staticDataCacheFactory,
                EqualityComparer<MemberVisibility>.Default,
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
