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
        [Fact]
        public void BaseC1EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(BaseC1<int, string>));
            Assert.True(cachedType.Data.IsGenericType);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new BaseCAttr1() });

            AssertHasAttrs(
                cachedType.InstanceProps.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(BaseC1<int, string>.C1PubIntVal)),
                new Attribute[] { new BaseAttr1() });

            AssertHasAttrs(
                cachedType.InstanceMethods.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(BaseC1<int, string>.GetC1PubIntVal)),
                new Attribute[] { new BaseAttr1() });

            AssertHasAttrs(
                cachedType.Constructors.Value.Own.Value.Items.Single(
                    ctr => ctr.Flags.Value.IsFamily && ctr.Parameters.Value.None()),
                new Attribute[] { new BaseAttr1() });

            AssertContains(
                cachedType.Events.Value.Own.Value,
                BaseC1<int, string>.OwnEventsTestData);

            AssertContains(
                cachedType.Events.Value.AllVisible.Value,
                BaseC1<int, string>.AllVisibleEventsTestData);

            AssertContains(
                cachedType.Events.Value.AsmVisible.Value,
                BaseC1<int, string>.AsmVisibleEventsTestData);
        }
    }
}
