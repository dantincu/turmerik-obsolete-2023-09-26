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

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        private void AssertContains<TExpectd, T, TItem, TFilter>(
            ICachedItemsCollection<T, TItem, TFilter> itemsCllctn,
            TExpectd expectdContents,
            Func<TExpectd, Dictionary<TFilter, TItem[]>, bool> assertionValidPredicate)
            where TFilter : notnull
            where TItem : ICachedItem<T>
        {
            var itemsMap = itemsCllctn.Filtered.GetKeys().ToDictionary(
                key => key,
                key => itemsCllctn.Filtered.Get(key).ToArray());

            bool assertionIsValid = assertionValidPredicate(
                expectdContents,
                itemsMap);

            Assert.True(assertionIsValid);
        }

        private void AssertContains<TMemberInfo, TItem, TFlags, TFilter>(
            ICachedItemsCollection<TMemberInfo, TItem, TFilter> itemsCllctn,
            Dictionary<TFilter, string[]> expectdItemsMap,
            IEqualityComparer<Dictionary<TFilter, string[]>> eqCompr,
            Func<Dictionary<TFilter, string[]>, Dictionary<TFilter, TItem[]>, bool> assertionValidPredicate = null)
            where TFilter : notnull
            where TItem : ICachedMemberInfo<TMemberInfo, TFlags>
            where TMemberInfo : MemberInfo => AssertContains(
                itemsCllctn,
                expectdItemsMap,
                (expectedNamesMap, itemsMap) =>
                {
                    var actualNamesMap = itemsMap.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Select(
                            item => item.Name).ToArray());

                    bool retVal = eqCompr.Equals(
                        expectedNamesMap,
                        actualNamesMap);

                    if (retVal && assertionValidPredicate != null)
                    {
                        retVal = assertionValidPredicate(
                            expectedNamesMap,
                            itemsMap);
                    }

                    return retVal;
                });

        private void AssertContains(
            ICachedConstructorsCollection cllctn,
            Dictionary<MemberVisibility, Dictionary<string, Type>[]> expectedConstructorsMap,
            Func<Dictionary<MemberVisibility, Dictionary<string, Type>[]>, Dictionary<MemberVisibility, ICachedConstructorInfo[]>, bool> assertionValidPredicate = null) => AssertContains<Dictionary<MemberVisibility, Dictionary<string, Type>[]>, ConstructorInfo, ICachedConstructorInfo, MemberVisibility>(
                cllctn, expectedConstructorsMap, (expectedConstrMap, constructorsMap) =>
                {
                    var actualConstrMap = constructorsMap.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Select(
                            constr => constr.Parameters.Value.ToDictionary(
                                @param => param.Name,
                                @param => param.Type.Value.Data)).ToArray());

                    bool retVal = constructorsDictnrArrEqCompr.Equals(
                        expectedConstrMap,
                        actualConstrMap);

                    if (retVal && assertionValidPredicate != null)
                    {
                        retVal = assertionValidPredicate(
                            expectedConstrMap,
                            constructorsMap);
                    }

                    return retVal;
                });

        private void AssertContains(
            ICachedEventsCollection cllctn,
            Dictionary<EventAccessibilityFilter, string[]> expectedNamesMap,
            Func<Dictionary<EventAccessibilityFilter, string[]>, Dictionary<EventAccessibilityFilter, ICachedEventInfo[]>, bool> assertionValidPredicate = null) => AssertContains<EventInfo, ICachedEventInfo, CachedEventFlags.IClnbl, EventAccessibilityFilter>(
                cllctn,
                expectedNamesMap,
                eventNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedFieldsCollection cllctn,
            Dictionary<FieldAccessibilityFilter, string[]> expectedNamesMap,
            Func<Dictionary<FieldAccessibilityFilter, string[]>, Dictionary<FieldAccessibilityFilter, ICachedFieldInfo[]>, bool> assertionValidPredicate = null) => AssertContains<FieldInfo, ICachedFieldInfo, CachedFieldFlags.IClnbl, FieldAccessibilityFilter>(
                cllctn,
                expectedNamesMap,
                fieldNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedMethodsCollection cllctn,
            Dictionary<MethodAccessibilityFilter, string[]> expectedNamesMap,
            Func<Dictionary<MethodAccessibilityFilter, string[]>, Dictionary<MethodAccessibilityFilter, ICachedMethodInfo[]>, bool> assertionValidPredicate = null) => AssertContains<MethodInfo, ICachedMethodInfo, CachedMemberFlags.IClnbl, MethodAccessibilityFilter>(
                cllctn,
                expectedNamesMap,
                methodNamesDictnrEqCompr,
                assertionValidPredicate);

        private void AssertContains(
            ICachedPropertiesCollection cllctn,
            Dictionary<PropertyAccessibilityFilter, string[]> expectedNamesMap,
            Func<Dictionary<PropertyAccessibilityFilter, string[]>, Dictionary<PropertyAccessibilityFilter, ICachedPropertyInfo[]>, bool> assertionValidPredicate = null) => AssertContains<PropertyInfo, ICachedPropertyInfo, CachedPropertyFlags.IClnbl, PropertyAccessibilityFilter>(
                cllctn,
                expectedNamesMap,
                propNamesDictnrEqCompr,
                assertionValidPredicate);
    }
}
