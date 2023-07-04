using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;
using Turmerik.Reflection;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        private static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> iC2OwnEventsTestData = new ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>(
            new Dictionary<EventAccessibilityFilter, string[]>
            {
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC2<int>.C2Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.None,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    new string[0]
                }
            }.RdnlD(),
            new Dictionary<EventAccessibilityFilter, string[]>
            {
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public,
                        MemberVisibility.Public,
                        MemberVisibility.Public),
                    nameof(IC2<int>.C2Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Private),
                    new string[0]
                }
            }.RdnlD());

        private static readonly ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> iC2OwnPropertiesTestData = new ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>>(
            new Dictionary<PropertyAccessibilityFilter, string[]>
            {
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.Instance,
                        true, false,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC2<int>.C2PubStrVal).Arr(
                        nameof(IC2<int>.C2PubIntVal))
                },
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.Instance,
                        true, true,
                        MemberVisibility.None,
                        MemberVisibility.Public),
                    new string[0]
                },
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.None,
                        true, true,
                        MemberVisibility.Public,
                        MemberVisibility.Public),
                    new string[0]
                }
            }.RdnlD(),
            new Dictionary<PropertyAccessibilityFilter, string[]>
            {
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.Static | MemberScope.Instance,
                        true, false,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC2<int>.C2PubStrVal).Arr(
                        nameof(IC2<int>.C2PubIntVal))
                },
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.Static,
                        true, true,
                        MemberVisibility.Public,
                        MemberVisibility.Public),
                    new string[0]
                }
            }.RdnlD());

        private static readonly ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> iC2OwnMehodsTestData = new ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>>(
            new Dictionary<MethodAccessibilityFilter, string[]>
            {
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Instance,
                        MemberVisibility.Public),
                    nameof(IC2<int>.GetC2PubStrVal).Arr(
                        nameof(IC2<int>.GetC2PubIntVal))
                },
                {
                    new MethodAccessibilityFilter(
                        MemberScope.None,
                        MemberVisibility.Public),
                    null
                }
            }.RdnlD(),
            new Dictionary<MethodAccessibilityFilter, string[]>
            {
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Static | MemberScope.Instance,
                        MemberVisibility.Public),
                    nameof(IC2<int>.GetC2PubStrVal).Arr(
                        nameof(IC2<int>.GetC2PubIntVal))
                },
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Static,
                        MemberVisibility.Public),
                    null
                }
            }.RdnlD());

        private static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> iC2AllEventsTestData = MergeData(
            iC2OwnEventsTestData.Arr(iC1OwnEventsTestData));

        private static readonly ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> iC2AllPropertiesTestData = MergeData(
            iC2OwnPropertiesTestData.Arr(iC1OwnPropertiesTestData));

        private static readonly ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> iC2AllMehodsTestData = MergeData(
            iC2OwnMehodsTestData.Arr(iC1OwnMehodsTestData));

        [Fact]
        public void IC2EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC2<int>));
            Assert.True(cachedType.Data.IsGenericType);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new MidIAttr2() });

            AssertHasAttrs(
                cachedType.InstanceProps.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(IC2<int>.C2PubStrVal)),
                new Attribute[] { new MidAttr2() });

            Assert.Null(cachedType.InstanceFields.Value);
            Assert.Null(cachedType.StaticFields.Value);
            Assert.Null(cachedType.Constructors.Value);
            Assert.Null(cachedType.StaticProps.Value);
            Assert.Null(cachedType.StaticMethods.Value);

            AssertContains(
                cachedType.Events.Value.Own.Value,
                iC2OwnEventsTestData);

            AssertContains(
                cachedType.Events.Value.AllVisible.Value,
                iC2AllEventsTestData);

            AssertContains(
                cachedType.Events.Value.AsmVisible.Value,
                iC2AllEventsTestData);
        }

        [Fact]
        public void IC2PropertiesTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC2<int>));

            AssertContains(
                cachedType.InstanceProps.Value.Own.Value,
                iC2OwnPropertiesTestData);

            AssertContains(
                cachedType.InstanceProps.Value.AllVisible.Value,
                iC2AllPropertiesTestData);

            AssertContains(
                cachedType.InstanceProps.Value.AsmVisible.Value,
                iC2AllPropertiesTestData);
        }

        [Fact]
        public void IC2MethodsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC2<int>));

            AssertContains(
                cachedType.InstanceMethods.Value.Own.Value,
                iC2OwnMehodsTestData);

            AssertContains(
                cachedType.InstanceMethods.Value.AllVisible.Value,
                iC2AllMehodsTestData);

            AssertContains(
                cachedType.InstanceMethods.Value.AsmVisible.Value,
                iC2AllMehodsTestData);
        }
    }
}
