using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;
using Turmerik.Reflection;
using static Turmerik.Reflection.ReflC.MethodNames;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        private static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> iC1OwnEventsTestData = new ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>(
            new Dictionary<EventAccessibilityFilter, string[]>
            {
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC1<int, string>.C1Event).Arr()
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
                    nameof(IC1<int, string>.C1Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Private),
                    new string[0]
                }
            }.RdnlD());

        private static readonly ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>> iC1OwnPropertiesTestData = new ExpectedContents<IDictionary<PropertyAccessibilityFilter, string[]>>(
            new Dictionary<PropertyAccessibilityFilter, string[]>
            {
                {
                    new PropertyAccessibilityFilter(
                        MemberScope.Instance,
                        true, false,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC1<int, string>.C1PubStrVal).Arr(
                        nameof(IC1<int, string>.C1PubIntVal))
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
                    nameof(IC1<int, string>.C1PubStrVal).Arr(
                        nameof(IC1<int, string>.C1PubIntVal))
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

        private static readonly ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>> iC1OwnMehodsTestData = new ExpectedContents<IDictionary<MethodAccessibilityFilter, string[]>>(
            new Dictionary<MethodAccessibilityFilter, string[]>
            {
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Instance,
                        MemberVisibility.Public),
                    nameof(IC1<int, string>.GetC1PubStrVal).Arr(
                        nameof(IC1<int, string>.GetC1PubIntVal))
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
                    nameof(IC1<int, string>.GetC1PubStrVal).Arr(
                        nameof(IC1<int, string>.GetC1PubIntVal))
                },
                {
                    new MethodAccessibilityFilter(
                        MemberScope.Static,
                        MemberVisibility.Public),
                    null
                }
            }.RdnlD());

        [Fact]
        public void IC1EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC1<int, string>));
            Assert.True(cachedType.Data.IsGenericType);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new BaseIAttr1() });

            AssertHasAttrs(
                cachedType.InstanceProps.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(IC1<int, string>.C1PubStrVal)),
                new Attribute[] { new BaseAttr1() });

            Assert.Null(cachedType.InstanceFields.Value);
            Assert.Null(cachedType.StaticFields.Value);
            Assert.Null(cachedType.Constructors.Value);
            Assert.Null(cachedType.StaticProps.Value);
            Assert.Null(cachedType.StaticMethods.Value);

            foreach (var cllctn in cachedType.Events.Value.Own.Value.Arr(
                cachedType.Events.Value.AllVisible.Value,
                cachedType.Events.Value.ExtAsmVisible.Value))
            {
                AssertContains(
                    cllctn,
                    iC1OwnEventsTestData);
            }
        }

        [Fact]
        public void IC1PropertiesTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC1<int, string>));

            foreach (var cllctn in cachedType.InstanceProps.Value.Own.Value.Arr(
                cachedType.InstanceProps.Value.AllVisible.Value,
                cachedType.InstanceProps.Value.ExtAsmVisible.Value))
            {
                AssertContains(
                    cllctn,
                    iC1OwnPropertiesTestData);
            }
        }

        [Fact]
        public void IC1MethodsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC1<int, string>));

            foreach (var cllctn in cachedType.InstanceMethods.Value.Own.Value.Arr(
                cachedType.InstanceMethods.Value.AllVisible.Value,
                cachedType.InstanceMethods.Value.ExtAsmVisible.Value))
            {
                AssertContains(
                    cllctn,
                    iC1OwnMehodsTestData);
            }
        }
    }
}