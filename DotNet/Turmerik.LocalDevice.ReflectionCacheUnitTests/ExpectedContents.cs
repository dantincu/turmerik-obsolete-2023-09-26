using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Reflection;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public readonly struct ExpectedContents<TContent>
    {
        public readonly TContent Included;
        public readonly TContent ReducedIncluded;

        public ExpectedContents(
            TContent included,
            TContent reducedIncluded)
        {
            Included = included;
            ReducedIncluded = reducedIncluded;
        }
    }

    public static class ExpectedContents
    {
        public static ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> ExceptPrivate(
            this ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> expectedContents) => new ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>(
            expectedContents.Included.Select(
                kvp => new KeyValuePair<EventAccessibilityFilter, string[]>(
                    kvp.Key.WithValue(
                        key => new EventAccessibilityFilter(
                            key.AdderVisibility.SubstractContainedFlags(
                                MemberVisibility.Private),
                            key.RemoverVisibility.SubstractContainedFlags(
                                MemberVisibility.Private))),
                    kvp.Value?.WithValue(
                        value => value.Where(
                            name => !name.Contains("Prv")).ToArray()))).GroupBy(
                kvp => kvp.Key).ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Key.MatchesNone() ? null : kvp.Aggregate(
                    (kvp1, kvp2) =>
                    {
                        string[] retArr = null;

                        if (!kvp1.Key.MatchesNone() && kvp1.Value != null && kvp2.Value != null)
                        {
                            retArr = kvp1.Value.Concat(
                                kvp2.Value).Distinct().ToArray();
                        }

                        return new KeyValuePair<EventAccessibilityFilter, string[]>(
                            kvp1.Key, retArr);
                    }).Value),
            expectedContents.ReducedIncluded);
    }
}
