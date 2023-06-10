using System;
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
        IStaticDataCache<TFilter, ReadOnlyCollection<TItem>> Filtered { get; }
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
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ReadOnlyCollection<TItem> items,
            Func<TItem, TFilter, bool> filterMatchPredicate)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));

            StaticDataCacheFactory = staticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(staticDataCacheFactory));

            MemberAccessibiliyFilterEqualityComparerFactory = memberAccessibiliyFilterEqualityComparerFactory ?? throw new ArgumentNullException(
                nameof(memberAccessibiliyFilterEqualityComparerFactory));

            FilterMatchPredicate = filterMatchPredicate ?? throw new ArgumentNullException(
                nameof(filterMatchPredicate));

            Filtered = StaticDataCacheFactory.Create<TFilter, ReadOnlyCollection<TItem>>(
                filter => Items.Where(item => FilterMatchPredicate(item, filter)).RdnlC());
        }

        public ReadOnlyCollection<TItem> Items { get; }
        public IStaticDataCache<TFilter, ReadOnlyCollection<TItem>> Filtered { get; }

        protected INonSynchronizedStaticDataCacheFactory StaticDataCacheFactory { get; }
        protected IMemberAccessibiliyFilterEqualityComparerFactory MemberAccessibiliyFilterEqualityComparerFactory { get; }
        private Func<TItem, TFilter, bool> FilterMatchPredicate { get; }
    }

    public class CachedFieldsCollection : CachedItemsCollection<FieldInfo, ICachedFieldInfo, FieldAccessibilityFilter>, ICachedFieldsCollection
    {
        public CachedFieldsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ReadOnlyCollection<ICachedFieldInfo> items,
            Func<ICachedFieldInfo, FieldAccessibilityFilter, bool> filterMatchPredicate) : base(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate)
        {
        }
    }

    public class CachedPropertiesCollection : CachedItemsCollection<PropertyInfo, ICachedPropertyInfo, PropertyAccessibilityFilter>, ICachedPropertiesCollection
    {
        public CachedPropertiesCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ReadOnlyCollection<ICachedPropertyInfo> items,
            Func<ICachedPropertyInfo, PropertyAccessibilityFilter, bool> filterMatchPredicate) : base(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate)
        {
        }
    }

    public class CachedEventsCollection : CachedItemsCollection<EventInfo, ICachedEventInfo, EventAccessibilityFilter>, ICachedEventsCollection
    {
        public CachedEventsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate) : base(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate)
        {
        }
    }

    public class CachedMethodsCollection : CachedItemsCollection<MethodInfo, ICachedMethodInfo, MethodAccessibilityFilter>, ICachedMethodsCollection
    {
        public CachedMethodsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ReadOnlyCollection<ICachedMethodInfo> items,
            Func<ICachedMethodInfo, MethodAccessibilityFilter, bool> filterMatchPredicate) : base(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate)
        {
        }
    }

    public class CachedConstructorsCollection : CachedItemsCollection<ConstructorInfo, ICachedConstructorInfo, MemberVisibility>, ICachedConstructorsCollection
    {
        public CachedConstructorsCollection(
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ReadOnlyCollection<ICachedConstructorInfo> items,
            Func<ICachedConstructorInfo, MemberVisibility, bool> filterMatchPredicate) : base(
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                items,
                filterMatchPredicate)
        {
        }
    }
}
