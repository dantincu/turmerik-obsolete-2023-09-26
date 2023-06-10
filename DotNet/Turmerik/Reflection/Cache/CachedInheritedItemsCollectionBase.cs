using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using static Turmerik.Reflection.ReflC;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedInheritedItemsCollection<T, TItem, TFilter, TCollection>
        where TItem : ICachedItem<T>
        where TCollection : ICachedItemsCollection<T, TItem, TFilter>
    {
        Lazy<TCollection> Own { get; }
        Lazy<TCollection> All { get; }
    }

    public abstract class CachedInheritedItemsCollectionBase<T, TItem, TFilter, TCollection> : ICachedInheritedItemsCollection<T, TItem, TFilter, TCollection>
        where TItem : ICachedItem<T>
        where TCollection : ICachedItemsCollection<T, TItem, TFilter>
    {
        protected CachedInheritedItemsCollectionBase(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ICachedTypeInfo type,
            Func<TItem, TFilter, bool, bool> filterMatchPredicate)
        {
            TypesMap = typesMap ?? throw new ArgumentNullException(nameof(typesMap));
            ItemsFactory = itemsFactory ?? throw new ArgumentNullException(nameof(itemsFactory));

            StaticDataCacheFactory = staticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(staticDataCacheFactory));

            MemberAccessibiliyFilterEqualityComparerFactory = memberAccessibiliyFilterEqualityComparerFactory ?? throw new ArgumentNullException(
                nameof(memberAccessibiliyFilterEqualityComparerFactory));

            FilterMatchPredicate = (item, filter) => filterMatchPredicate(
                item, filter, Type.Assembly == BaseType.Value.Assembly);

            Type = type;

            BaseType = new Lazy<ICachedTypeInfo>(
                () => Type.BaseType.Value);

            Own = new Lazy<TCollection>(
                () => CreateCollection(
                    GetOwnItems(Type).RdnlC(),
                    FilterMatchPredicate));

            All = new Lazy<TCollection>(
                () => CreateCollection(
                    GetAllItems(Own.Value.Items),
                    FilterMatchPredicate));
        }

        public Lazy<TCollection> Own { get; }
        public Lazy<TCollection> All { get; }

        protected ICachedTypesMap TypesMap { get; }
        protected ICachedReflectionItemsFactory ItemsFactory { get; }
        protected INonSynchronizedStaticDataCacheFactory StaticDataCacheFactory { get; }
        protected IMemberAccessibiliyFilterEqualityComparerFactory MemberAccessibiliyFilterEqualityComparerFactory { get; }
        protected Func<TItem, TFilter, bool> FilterMatchPredicate { get; }

        protected ICachedTypeInfo Type { get; }
        protected Lazy<ICachedTypeInfo> BaseType { get; }

        protected abstract TItem[] GetOwnItems(ICachedTypeInfo type);

        protected abstract TCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType);

        protected abstract TCollection CreateCollection(
            ReadOnlyCollection<TItem> items,
            Func<TItem, TFilter, bool> filterMatchPredicate);

        private ReadOnlyCollection<TItem> GetAllItems(
            ReadOnlyCollection<TItem> ownItems)
        {
            var allItems = ownItems;
            var baseType = BaseType.Value;

            if (baseType != null)
            {
                var baseItems = GetBaseTypeOwnItems(baseType);
                var allBaseItems = baseItems.Items;

                allItems = allItems.Concat(allBaseItems).RdnlC();
            }

            return allItems;
        }
    }
}
