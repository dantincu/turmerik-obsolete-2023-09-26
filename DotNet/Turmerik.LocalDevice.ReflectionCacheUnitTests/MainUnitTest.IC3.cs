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
        private static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> iC3OwnEventsTestData = new ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>(
            new Dictionary<EventAccessibilityFilter, string[]>
            {
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC3.C3Event).Arr()
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
                    nameof(IC3.C3Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Private),
                    new string[0]
                }
            }.RdnlD());

        private static readonly ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> iC3OwnPropertiesTestData = new ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>>(
            new Dictionary<PropertyAccessibilityFilter, string[]>
            {
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.Instance,
                        true, false,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC3.C3PubStrVal).Arr(
                        nameof(IC3.C3PubIntVal))
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
                    nameof(IC3.C3PubStrVal).Arr(
                        nameof(IC3.C3PubIntVal))
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

        private static readonly ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> iC3OwnMehodsTestData = new ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>>(
            new Dictionary<MethodAccessibilityFilter, string[]>
            {
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Instance,
                        MemberVisibility.Public),
                    nameof(IC3.GetC3PubStrVal).Arr(
                        nameof(IC3.GetC3PubIntVal))
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
                    nameof(IC3.GetC3PubStrVal).Arr(
                        nameof(IC3.GetC3PubIntVal))
                },
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Static,
                        MemberVisibility.Public),
                    null
                }
            }.RdnlD());

        private static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> iC3AllEventsTestData = MergeData(
            iC3OwnEventsTestData.Arr(iC2AllEventsTestData));

        private static readonly ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> iC3AllPropertiesTestData = MergeData(
            iC3OwnPropertiesTestData.Arr(iC2AllPropertiesTestData));

        private static readonly ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> iC3AllMehodsTestData = MergeData(
            iC3OwnMehodsTestData.Arr(iC2AllMehodsTestData));

        [Fact]
        public void IC3EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC3));
            Assert.False(cachedType.Data.IsGenericType);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new IAttr3() });

            AssertHasAttrs(
                cachedType.InstanceProps.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(IC3.C3PubStrVal)),
                new Attribute[] { new Attr3() });

            Assert.Null(cachedType.InstanceFields.Value);
            Assert.Null(cachedType.StaticFields.Value);
            Assert.Null(cachedType.Constructors.Value);
            Assert.Null(cachedType.StaticProps.Value);
            Assert.Null(cachedType.StaticMethods.Value);

            AssertContains(
                cachedType.Events.Value.Own.Value,
                iC3OwnEventsTestData);

            AssertContains(
                cachedType.Events.Value.AllVisible.Value,
                iC3AllEventsTestData);

            AssertContains(
                cachedType.Events.Value.ExtAsmVisible.Value,
                iC3AllEventsTestData);
        }

        [Fact]
        public void IC3PropertiesTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC3));

            AssertContains(
                cachedType.InstanceProps.Value.Own.Value,
                iC3OwnPropertiesTestData);

            AssertContains(
                cachedType.InstanceProps.Value.AllVisible.Value,
                iC3AllPropertiesTestData);

            AssertContains(
                cachedType.InstanceProps.Value.ExtAsmVisible.Value,
                iC3AllPropertiesTestData);
        }

        [Fact]
        public void IC3MethodsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC3));

            AssertContains(
                cachedType.InstanceMethods.Value.Own.Value,
                iC3OwnMehodsTestData);

            AssertContains(
                cachedType.InstanceMethods.Value.AllVisible.Value,
                iC3AllMehodsTestData);

            AssertContains(
                cachedType.InstanceMethods.Value.ExtAsmVisible.Value,
                iC3AllMehodsTestData);
        }
    }
}
