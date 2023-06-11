using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using static Turmerik.Reflection.ReflC;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedItemsCollection<T, TItem, TFilter>
        where TItem : ICachedItem<T>
    {
        ReadOnlyCollection<TItem> Items { get; }
        IKeyReducerStaticDataCache<TFilter, ReadOnlyCollection<TItem>> Filtered { get; }
    }

    public interface ICachedFieldsCollection : ICachedItemsCollection<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter>
    {
    }

    public interface ICachedPropertiesCollection : ICachedItemsCollection<PropertyInfo, ICachedPropertyInfo, PropertyAccessibilityFilter>
    {
    }

    public interface ICachedEventsCollection : ICachedItemsCollection<EventInfo, ICachedEventInfo, EventAccessibilityFilter>
    {
    }

    public interface ICachedMethodsCollection : ICachedItemsCollection<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter>
    {
    }

    public interface ICachedConstructorsCollection : ICachedItemsCollection<ConstructorInfo, ICachedConstructorInfo, MemberVisibility>
    {
    }

    public class CachedItemsCollection<T, TItem, TFilter> : ICachedItemsCollection<T, TItem, TFilter>
        where TItem : ICachedItem<T>
    {
        public CachedItemsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IEqualityComparer<TFilter> filterEqCompr,
            ReadOnlyCollection<TItem> items,
            Func<TItem, TFilter, bool> filterMatchPredicate,
            Func<TFilter, TFilter> filterReducer)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));

            StaticDataCacheFactory = staticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(staticDataCacheFactory));

            FilterEqCompr = filterEqCompr ?? throw new ArgumentNullException(
                nameof(filterEqCompr));

            FilterMatchPredicate = filterMatchPredicate ?? throw new ArgumentNullException(
                nameof(filterMatchPredicate));

            Filtered = StaticDataCacheFactory.CreateKeyReducer(
                filter => Items.Where(item => FilterMatchPredicate(item, filter)).RdnlC(),
                FilterEqCompr, filterReducer, filter => filter);
        }

        public ReadOnlyCollection<TItem> Items { get; }
        public IKeyReducerStaticDataCache<TFilter, ReadOnlyCollection<TItem>> Filtered { get; }

        protected INonSynchronizedStaticDataCacheFactory StaticDataCacheFactory { get; }
        protected IEqualityComparer<TFilter> FilterEqCompr { get; }
        private Func<TItem, TFilter, bool> FilterMatchPredicate { get; }
    }

    public class CachedFieldsCollection : CachedItemsCollection<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter>, ICachedFieldsCollection
    {
        public CachedFieldsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IEqualityComparer<FieldAccessibilityFilter> filterEqCompr,
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate,
            Func<FieldAccessibilityFilter, FieldAccessibilityFilter> filterReducer) : base(
                staticDataCacheFactory,
                filterEqCompr,
                items,
                filterMatchPredicate,
                filterReducer)
        {
        }
    }

    public class CachedPropertiesCollection : CachedItemsCollection<PropertyInfo, ICachedPropertyInfo, PropertyAccessibilityFilter>, ICachedPropertiesCollection
    {
        public CachedPropertiesCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IEqualityComparer<PropertyAccessibilityFilter> filterEqCompr,
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate,
            Func<PropertyAccessibilityFilter, PropertyAccessibilityFilter> filterReducer) : base(
                staticDataCacheFactory,
                filterEqCompr,
                items,
                filterMatchPredicate,
                filterReducer)
        {
        }
    }

    public class CachedEventsCollection : CachedItemsCollection<EventInfo, ICachedEventInfo, EventAccessibilityFilter>, ICachedEventsCollection
    {
        public CachedEventsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IEqualityComparer<EventAccessibilityFilter> filterEqCompr,
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> filterReducer) : base(
                staticDataCacheFactory,
                filterEqCompr,
                items,
                filterMatchPredicate,
                filterReducer)
        {
        }
    }

    public class CachedMethodsCollection : CachedItemsCollection<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter>, ICachedMethodsCollection
    {
        public CachedMethodsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IEqualityComparer<MethodAccessibilityFilter> filterEqCompr,
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate,
            Func<MethodAccessibilityFilter, MethodAccessibilityFilter> filterReducer) : base(
                staticDataCacheFactory,
                filterEqCompr,
                items,
                filterMatchPredicate,
                filterReducer)
        {
        }
    }

    public class CachedConstructorsCollection : CachedItemsCollection<ConstructorInfo, ICachedConstructorInfo, MemberVisibility>, ICachedConstructorsCollection
    {
        public CachedConstructorsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IEqualityComparer<MemberVisibility> filterEqCompr,
            ReadOnlyCollection<ICachedConstructorInfo> items,
            Func<ICachedConstructorInfo, MemberVisibility, bool> filterMatchPredicate,
            Func<MemberVisibility, MemberVisibility> filterReducer) : base(
                staticDataCacheFactory,
                filterEqCompr,
                items,
                filterMatchPredicate,
                filterReducer)
        {
        }
    }
}
