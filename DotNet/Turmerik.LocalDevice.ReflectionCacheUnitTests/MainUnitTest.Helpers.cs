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

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        private void AssertContains<TExpected, T, TItem, TFilter>(
            ICachedItemsCollection<T, TItem, TFilter> itemsCllctn,
            ExpectedContents<TExpected> expectedContents,
            Action<ICachedItemsCollection<T, TItem, TFilter>, ExpectedContents<TExpected>> beforeAssertAction,
            Func<ExpectedContents<TExpected>, Dictionary<TFilter, TItem[]>, bool> assertionValidPredicate)
            where TFilter : notnull
            where TItem : ICachedItem<T>
        {
            beforeAssertAction(
                itemsCllctn,
                expectedContents);

            var itemsMap = itemsCllctn.Filtered.GetKeys().ToDictionary(
                key => key,
                key => itemsCllctn.Filtered.Get(key).ToArray());

            bool assertionIsValid = assertionValidPredicate(
                expectedContents,
                itemsMap);

            Assert.True(assertionIsValid);
        }

        private void AssertContains<TMemberInfo, TItem, TFlags, TFilter>(
            ICachedItemsCollection<TMemberInfo, TItem, TFilter> itemsCllctn,
            ExpectedContents<Dictionary<TFilter, string[]>> expectedContents,
            IEqualityComparer<Dictionary<TFilter, string[]>> eqCompr,
            Func<ExpectedContents<Dictionary<TFilter, string[]>>, Dictionary<TFilter, TItem[]>, bool> assertionValidPredicate = null)
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
                        kvp => kvp.Value.Select(
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
            ExpectedContents<Dictionary<MemberVisibility, Dictionary<string, Type>[]>> expectedContents,
            Func<ExpectedContents<Dictionary<MemberVisibility, Dictionary<string, Type>[]>>, Dictionary<MemberVisibility, ICachedConstructorInfo[]>, bool> assertionValidPredicate = null) => AssertContains<Dictionary<MemberVisibility, Dictionary<string, Type>[]>, ConstructorInfo, ICachedConstructorInfo, MemberVisibility>(
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
                                    @param => param.Type.Value.Data)).ToArray();

                            bool isValid = methodParamsDictnrArrEqCompr.Equals(
                                kvp.Value,
                                namesMap);

                            Assert.True(isValid);
                        }
                    }
                },
                (expected, constructorsMap) =>
                {
                    var actualConstrMap = constructorsMap.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Select(
                            constr => constr.Parameters.Value.ToDictionary(
                                @param => param.Name,
                                @param => param.Type.Value.Data)).ToArray());

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
            ExpectedContents<Dictionary<EventAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<Dictionary<EventAccessibilityFilter, string[]>>, Dictionary<EventAccessibilityFilter, ICachedEventInfo[]>, bool> assertionValidPredicate = null) => AssertContains<EventInfo, ICachedEventInfo, CachedEventFlags.IClnbl, EventAccessibilityFilter>(
                cllctn,
                expectedContents,
                eventNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedFieldsCollection cllctn,
            ExpectedContents<Dictionary<FieldAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<Dictionary<FieldAccessibilityFilter, string[]>>, Dictionary<FieldAccessibilityFilter, ICachedFieldInfo[]>, bool> assertionValidPredicate = null) => AssertContains<FieldInfo, ICachedFieldInfo, CachedFieldFlags.IClnbl, FieldAccessibilityFilter>(
                cllctn,
                expectedContents,
                fieldNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedMethodsCollection cllctn,
            ExpectedContents<Dictionary<MethodAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<Dictionary<MethodAccessibilityFilter, string[]>>, Dictionary<MethodAccessibilityFilter, ICachedMethodInfo[]>, bool> assertionValidPredicate = null) => AssertContains<MethodInfo, ICachedMethodInfo, CachedMemberFlags.IClnbl, MethodAccessibilityFilter>(
                cllctn,
                expectedContents,
                methodNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedPropertiesCollection cllctn,
            ExpectedContents<Dictionary<PropertyAccessibilityFilter, string[]>> expectedContents,
            Func<ExpectedContents<Dictionary<PropertyAccessibilityFilter, string[]>>, Dictionary<PropertyAccessibilityFilter, ICachedPropertyInfo[]>, bool> assertionValidPredicate = null) => AssertContains<PropertyInfo, ICachedPropertyInfo, CachedPropertyFlags.IClnbl, PropertyAccessibilityFilter>(
                cllctn,
                expectedContents,
                propNamesDictnrEqCompr,
                assertionValidPredicate);

        private readonly struct ExpectedContents<TContent>
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
    }
}
