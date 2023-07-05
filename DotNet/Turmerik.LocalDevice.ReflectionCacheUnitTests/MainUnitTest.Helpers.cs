using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;
using Turmerik.Reflection;
using Turmerik.Reflection.Cache;
using Turmerik.Utils;
using Turmerik.Collections;
using System.Collections.ObjectModel;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        private void AssertContains<TExpected, T, TItem, TFilter>(
            ICachedItemsCollection<T, TItem, TFilter> itemsCllctn,
            ExpectedContents<TExpected> expectedContents,
            Action<ICachedItemsCollection<T, TItem, TFilter>, ExpectedContents<TExpected>> beforeAssertAction,
            Func<ExpectedContents<TExpected>, IDictionary<TFilter, TItem[]>, bool> assertionValidPredicate)
            where TFilter : notnull
            where TItem : ICachedItem<T>
        {
            beforeAssertAction(
                itemsCllctn,
                expectedContents);

            var itemsMap = itemsCllctn.Filtered.GetKeys().ToDictionary(
                key => key,
                key => itemsCllctn.Filtered.Get(key)?.ToArray());

            bool assertionIsValid = assertionValidPredicate(
                expectedContents,
                itemsMap);

            Assert.True(assertionIsValid);
        }

        private void AssertContains<TMemberInfo, TItem, TFlags, TFilter>(
            ICachedItemsCollection<TMemberInfo, TItem, TFilter> itemsCllctn,
            ExpectedContents<IDictionary<TFilter, string[]>> expectedContents,
            IEqualityComparer<IDictionary<TFilter, string[]>> eqCompr,
            Func<ExpectedContents<IDictionary<TFilter, string[]>>, IDictionary<TFilter, TItem[]>, bool> assertionValidPredicate = null)
            where TFilter : notnull
            where TItem : ICachedMemberInfo<TMemberInfo, TFlags>
            where TMemberInfo : MemberInfo => AssertContains(
                itemsCllctn,
                expectedContents,
                beforeAssertAction: (cllctn, expected) =>
                {
                    foreach (var map in expected.Included.Arr(
                        expected.ReducedIncluded))
                    {
                        foreach (var kvp in map)
                        {
                            var namesArr = cllctn.Filtered.Get(kvp.Key)?.Select(
                                item => item.Name).ToArray();

                            AssertSequenceEqual(
                                kvp.Value,
                                namesArr);
                        }
                    }
                },
                (expected, itemsMap) =>
                {
                    var actualNamesMap = itemsMap.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Select(
                            item => item.Name).ToArray());

                    bool retVal = eqCompr.Equals(
                        expected.Included,
                        actualNamesMap);

                    if (retVal && assertionValidPredicate != null)
                    {
                        retVal = assertionValidPredicate(
                            expected,
                            itemsMap);
                    }

                    return retVal;
                });

        private void AssertContains(
            ICachedConstructorsCollection cllctn,
            ExpectedContents<IDictionary<MemberVisibility, IDictionary<string, Type>[]>> expectedContents,
            Func<ExpectedContents<IDictionary<MemberVisibility, IDictionary<string, Type>[]>>, IDictionary<MemberVisibility, ICachedConstructorInfo[]>, bool> assertionValidPredicate = null) => AssertContains<IDictionary<MemberVisibility, IDictionary<string, Type>[]>, ConstructorInfo, ICachedConstructorInfo, MemberVisibility>(
                cllctn,
                expectedContents,
                beforeAssertAction: (cllctn, expected) =>
                {
                    foreach (var map in expected.Included.Arr(
                        expected.ReducedIncluded))
                    {
                        foreach (var kvp in map)
                        {
                            var namesMap = cllctn.Filtered.Get(kvp.Key).Select(
                                item => item.Parameters.Value.ToDictionary(
                                    @param => param.Name,
                                    @param => param.Type.Value.Data) as IDictionary<string, Type>).ToArray();

                            bool isValid = methodParamsDictnrArrEqCompr.Equals(
                                kvp.Value,
                                namesMap);

                            Assert.True(isValid);
                        }
                    }
                },
                (ExpectedContents<IDictionary<MemberVisibility, IDictionary<string, Type>[]>> expected, IDictionary<MemberVisibility, ICachedConstructorInfo[]> constructorsMap) =>
                {
                    var actualConstrMap = constructorsMap.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Select(
                            constr => constr.Parameters.Value.ToDictionary(
                                @param => param.Name,
                                @param => param.Type.Value.Data) as IDictionary<string, Type>).ToArray()) as IDictionary<MemberVisibility, IDictionary<string, Type>[]>;

                    bool retVal = constructorsDictnrArrEqCompr.Equals(
                        expected.Included,
                        actualConstrMap);

                    if (retVal && assertionValidPredicate != null)
                    {
                        retVal = assertionValidPredicate(
                            expected,
                            constructorsMap);
                    }

                    return retVal;
                });

        private void AssertContains(
            ICachedEventsCollection cllctn,
            ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>, IDictionary<EventAccessibilityFilter, ICachedEventInfo[]>, bool> assertionValidPredicate = null) => AssertContains<EventInfo, ICachedEventInfo, CachedEventFlags.IClnbl, EventAccessibilityFilter>(
                cllctn,
                expectedContents,
                eventNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedFieldsCollection cllctn,
            ExpectedContents<IDictionary<FieldAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<IDictionary<FieldAccessibilityFilter, string[]>>, IDictionary<FieldAccessibilityFilter, ICachedFieldInfo[]>, bool> assertionValidPredicate = null) => AssertContains<FieldInfo, ICachedFieldInfo, CachedFieldFlags.IClnbl, FieldAccessibilityFilter>(
                cllctn,
                expectedContents,
                fieldNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedMethodsCollection cllctn,
            ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>>, IDictionary<MethodAccessibilityFilter, ICachedMethodInfo[]>, bool> assertionValidPredicate = null) => AssertContains<MethodInfo, ICachedMethodInfo, CachedMemberFlags.IClnbl, MethodAccessibilityFilter>(
                cllctn,
                expectedContents,
                methodNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedPropertiesCollection cllctn,
            ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>>, IDictionary<PropertyAccessibilityFilter, ICachedPropertyInfo[]>, bool> assertionValidPredicate = null) => AssertContains<PropertyInfo, ICachedPropertyInfo, CachedPropertyFlags.IClnbl, PropertyAccessibilityFilter>(
                cllctn,
                expectedContents,
                propNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertHasAttrs<TMemberInfo, TFlags>(
            ICachedMemberInfo<TMemberInfo, TFlags> cachedItem,
            Attribute[] expectedAttrsArr,
            bool includeInherited = true,
            Func<Attribute, Attribute, int, bool> equalsPredicate = null)
            where TMemberInfo : MemberInfo
        {
            var attrsArr = cachedItem.GetAttributes(includeInherited).Value.Where(
                attr => !attrNamesToIgnore.Contains(attr.GetType().Name)).ToArray();

            Assert.Equal(
                expectedAttrsArr.Length,
                attrsArr.Length);

            for (int i =  0; i < attrsArr.Length; i++)
            {
                var expectedAttr = expectedAttrsArr[i];
                var actualAttr = attrsArr[i];

                Assert.Equal(
                    expectedAttr.GetType(),
                    actualAttr.GetType());

                if (equalsPredicate != null)
                {
                    bool areEqual = equalsPredicate(
                        expectedAttr, actualAttr, i);

                    Assert.True(areEqual);
                }
            }
        }

        private static ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> MergeData(
            ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>[] contentsArr) => MergeData(
                contentsArr,
                (left, right) => left.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Concat(
                        right[kvp.Key] ?? new string[0]).ToArray() ?? right[kvp.Key]));

        private static ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> MergeData(
            ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>>[] contentsArr) => MergeData(
                contentsArr,
                (left, right) => left.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Concat(
                        right[kvp.Key] ?? new string[0]).ToArray() ?? right[kvp.Key]));

        private static ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> MergeData(
            ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>>[] contentsArr) => MergeData(
                contentsArr,
                (left, right) => left.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Concat(
                        right[kvp.Key] ?? new string[0]).ToArray() ?? right[kvp.Key]));

        private static ExpectedContents<IDictionary<FieldAccessibilityFilter, string[]>> MergeData(
            ExpectedContents<IDictionary<FieldAccessibilityFilter, string[]>>[] contentsArr) => MergeData(
                contentsArr,
                (left, right) => left.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Concat(
                        right[kvp.Key] ?? new string[0]).ToArray() ?? right[kvp.Key]));

        private static ExpectedContents<IDictionary<MemberVisibility, IDictionary<string, Type>[]>> MergeData(
            ExpectedContents<IDictionary<MemberVisibility, IDictionary<string, Type>[]>>[] contentsArr) => MergeData(
                contentsArr,
                (left, right) => left.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Concat(
                        right[kvp.Key] ?? new IDictionary<string, Type>[0]).ToArray() ?? right[kvp.Key]));

        private static ExpectedContents<TContent> MergeData<TContent>(
            ExpectedContents<TContent> leftContent,
            ExpectedContents<TContent> rightContent,
            Func<TContent, TContent, TContent> aggregator) => new ExpectedContents<TContent>(
                aggregator(
                    leftContent.Included,
                    rightContent.Included),
                aggregator(
                    leftContent.ReducedIncluded,
                    rightContent.ReducedIncluded));

        private static ExpectedContents<TContent> MergeData<TContent>(
            ExpectedContents<TContent>[] contentsArr,
            Func<TContent, TContent, TContent> aggregator) => contentsArr.Aggregate(
                (left, right) => MergeData(left, right, aggregator));
    }
}
