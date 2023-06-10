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
        Lazy<TCollection> AllVisible { get; }
        Lazy<TCollection> AsmVisible { get; }
    }

    public abstract class CachedInheritedItemsCollectionBase<T, TItem, TFilter, TCollection> : ICachedInheritedItemsCollection<T, TItem, TFilter, TCollection>
        where TItem : ICachedItem<T>
        where TCollection : ICachedItemsCollection<T, TItem, TFilter>
    {
        protected CachedInheritedItemsCollectionBase(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            Func<TItem, TFilter, bool> filterMatchPredicate,
            Func<TFilter, TFilter> ownFilterReducer,
            Func<TFilter, TFilter> allVisibleFilterReducer,
            Func<TFilter, TFilter> asmVisibleFilterReducer)
        {
            TypesMap = typesMap ?? throw new ArgumentNullException(nameof(typesMap));
            ItemsFactory = itemsFactory ?? throw new ArgumentNullException(nameof(itemsFactory));

            StaticDataCacheFactory = staticDataCacheFactory ?? throw new ArgumentNullException(
                nameof(staticDataCacheFactory));

            FilterMatchPredicate = filterMatchPredicate ?? throw new ArgumentNullException(
                nameof(filterMatchPredicate));

            OwnFilterReducer = ownFilterReducer ?? throw new ArgumentNullException(
                nameof(ownFilterReducer));

            AllVisibleFilterReducer = allVisibleFilterReducer ?? throw new ArgumentNullException(
                nameof(allVisibleFilterReducer));

            AsmVisibleFilterReducer = asmVisibleFilterReducer ?? throw new ArgumentNullException(
                nameof(asmVisibleFilterReducer));

            Type = type;

            BaseType = new Lazy<ICachedTypeInfo>(
                () => Type.BaseType.Value);

            Own = new Lazy<TCollection>(
                () => CreateCollection(
                    GetOwnItems(Type).RdnlC(),
                    OwnFilterReducer));

            AllVisible = new Lazy<TCollection>(
                () => CreateCollection(
                    GetAllVisibleItems(Own.Value.Items),
                    AllVisibleFilterReducer));

            AsmVisible = new Lazy<TCollection>(
                () => CreateCollection(
                    GetAsmVisibleItems(Own.Value.Items),
                    AsmVisibleFilterReducer));
        }

        public Lazy<TCollection> Own { get; }
        public Lazy<TCollection> AllVisible { get; }
        public Lazy<TCollection> AsmVisible { get; }

        protected ICachedTypesMap TypesMap { get; }
        protected ICachedReflectionItemsFactory ItemsFactory { get; }
        protected INonSynchronizedStaticDataCacheFactory StaticDataCacheFactory { get; }

        protected Func<TItem, TFilter, bool> FilterMatchPredicate { get; }
        protected Func<TFilter, TFilter> OwnFilterReducer { get; }
        protected Func<TFilter, TFilter> AllVisibleFilterReducer { get; }
        protected Func<TFilter, TFilter> AsmVisibleFilterReducer { get; }

        protected ICachedTypeInfo Type { get; }
        protected Lazy<ICachedTypeInfo> BaseType { get; }

        protected abstract TItem[] GetOwnItems(ICachedTypeInfo type);

        protected abstract TCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType);

        protected abstract TCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType);

        protected abstract TCollection CreateCollection(
            ReadOnlyCollection<TItem> items,
            Func<TFilter, TFilter> filterReducer);

        private ReadOnlyCollection<TItem> GetAllItems(
            ReadOnlyCollection<TItem> ownItems,
            Func<ICachedTypeInfo, TCollection> baseItemsCollectionFactory)
        {
            var allItems = ownItems;
            var baseType = BaseType.Value;

            if (baseType != null)
            {
                var baseItems = baseItemsCollectionFactory(baseType);
                var allBaseItems = baseItems.Items;

                allItems = allItems.Concat(allBaseItems).RdnlC();
            }

            return allItems;
        }

        private ReadOnlyCollection<TItem> GetAsmVisibleItems(
            ReadOnlyCollection<TItem> ownItems) => GetAllItems(
                ownItems, GetBaseTypeAsmVisibleItems);

        private ReadOnlyCollection<TItem> GetAllVisibleItems(
            ReadOnlyCollection<TItem> ownItems) => GetAllItems(
                ownItems, GetBaseTypeAllVisibleItems);
    }
}
