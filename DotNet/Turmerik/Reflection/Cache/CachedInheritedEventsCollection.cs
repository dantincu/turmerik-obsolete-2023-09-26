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
            INonSynchronizedStaticDataCacheFactory staticDataCacheFactory,
            IMemberAccessibiliyFilterEqualityComparerFactory memberAccessibiliyFilterEqualityComparerFactory,
            ICachedTypeInfo type) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                memberAccessibiliyFilterEqualityComparerFactory,
                type,
                (arg, filter, isSameAssebly) => arg.Matches(
                    filter,
                    !isSameAssebly,
                    true))
        {
        }

        protected override ICachedEventsCollection CreateCollection(
            ReadOnlyCollection<ICachedEventInfo> items,
            Func<ICachedEventInfo, EventAccessibilityFilter, bool> filterMatchPredicate) => ItemsFactory.Events(
                items, filterMatchPredicate);

        protected override ICachedEventsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => baseType.Events.Value.All.Value;

        protected override ICachedEventInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetEvents(
                ReflC.Filter.AllDeclaredOnlyBindingFlags).Select(
                @event => ItemsFactory.EventInfo(@event)).ToArray();
    }
}
