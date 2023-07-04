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
    public interface ICachedInheritedEventsCollection : ICachedInheritedItemsCollection<EventInfo, ICachedEventInfo, EventAccessibilityFilter, ICachedEventsCollection>
    {
    }

    public class CachedInheritedEventsCollection : CachedInheritedItemsCollectionBase<EventInfo, ICachedEventInfo, EventAccessibilityFilter, ICachedEventsCollection>, ICachedInheritedEventsCollection
    {
        public CachedInheritedEventsCollection(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> ownFilterReducer,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> allVisibleFilterReducer,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> asmVisibleFilterReducer) : base(
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

        protected override ICachedEventsCollection CreateCollection(
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<EventAccessibilityFilter, EventAccessibilityFilter> filterReducer) => ItemsFactory.Events(
                items, FilterMatchPredicate, filterReducer);

        protected override ICachedEventsCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => baseType.Events.Value.AsmVisible.Value;

        protected override ICachedEventsCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => baseType.Events.Value.AllVisible.Value;

        protected override ICachedEventsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => baseType.Events.Value.Own.Value;

        protected override ICachedEventInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetEvents(
                ReflC.Filter.BindingFlag.DeclaredOnly).Select(
                @event => ItemsFactory.EventInfo(@event)).ToArray();
    }
}
