using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
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
        public static ExpectedContents<IDictionary<EventAccessibilityFilter, string[]?>> ExceptPrivate(
            this ExpectedContents<IDictionary<EventAccessibilityFilter, string[]?>> expectedContents,
            bool excludeInternal)
        {
            Func<string, bool> memberNameFilter;

            if (excludeInternal)
            {
                memberNameFilter = name => !name.Contains("Prv") && (
                    !name.Contains("Internal") || name.Contains("ProtInternal"));
            }
            else
            {
                memberNameFilter = name => !name.Contains("Prv") || name.Contains("PrvProt");
            }

            var retContents = new ExpectedContents<IDictionary<EventAccessibilityFilter, string[]?>>(
                expectedContents.Included.Select(
                    kvp => new KeyValuePair<EventAccessibilityFilter, string[]?>(
                        kvp.Key.WithValue(
                            key => new EventAccessibilityFilter(
                                key.AdderVisibility.SubstractContainedFlags(
                                    MemberVisibility.Private),
                                key.RemoverVisibility.SubstractContainedFlags(
                                    MemberVisibility.Private))),
                        kvp.Value?.WithValue(
                            value => value.Where(memberNameFilter).ToArray()))).GroupBy(
                    kvp => kvp.Key).ToDictionary(
                    kvp => kvp.Key,
                    kvp => MatchesNone(
                        kvp.Key,
                        excludeInternal) ? null : kvp.Aggregate(
                            (kvp1, kvp2) =>
                            {
                                string[]? retArr = null;

                                if (!kvp1.Key.MatchesNone() && kvp1.Value != null && kvp2.Value != null)
                                {
                                    retArr = kvp1.Value.Concat(
                                        kvp2.Value).Distinct().ToArray();
                                }

                                return new KeyValuePair<EventAccessibilityFilter, string[]?>(
                                    kvp1.Key, retArr);
                            }).Value).Where(
                    kvp => kvp.Key.MatchesNone() ? true : kvp.Value != null).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value),
                expectedContents.ReducedIncluded);

            return retContents;
        }

        private static bool MatchesNone(
            EventAccessibilityFilter filter,
            bool excludeInternal)
        {
            bool matchesNone = filter.MatchesNone();

            if (!matchesNone && excludeInternal)
            {
                matchesNone = filter.AdderVisibility.Arr(
                    filter.RemoverVisibility).All(
                    visibility => MatchesNone(
                        visibility,
                        excludeInternal));
            }

            return matchesNone;
        }

        private static bool MatchesNone(
            MemberVisibility memberVisibility,
            bool excludeInternal)
        {
            bool matchesNone = memberVisibility.MatchesNone();

            if (!matchesNone && excludeInternal)
            {
                matchesNone = !memberVisibility.HasFlag(MemberVisibility.Public);
                matchesNone = matchesNone && !memberVisibility.HasFlag(MemberVisibility.ProtectedInternal);
                matchesNone = matchesNone && !memberVisibility.HasFlag(MemberVisibility.Protected);
            }

            return matchesNone;
        }
    }
}
