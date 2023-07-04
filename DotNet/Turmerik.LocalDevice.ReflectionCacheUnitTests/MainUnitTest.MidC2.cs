using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;
using Turmerik.Collections;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        [Fact]
        public void MidC2EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(MidC2<int>));
            Assert.True(cachedType.Data.IsGenericType);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new MidCAttr2(), new BaseCAttr1()});

            AssertHasAttrs(
                cachedType.InstanceProps.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(MidC2<int>.C2PubIntVal)),
                new Attribute[] { new MidAttr2() });

            AssertHasAttrs(
                cachedType.InstanceMethods.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(MidC2<int>.GetC2PubIntVal)),
                new Attribute[] { new MidAttr2() });

            AssertHasAttrs(
                cachedType.Constructors.Value.Own.Value.Items.Single(
                    ctr => ctr.Flags.Value.IsFamily && ctr.Parameters.Value.None()),
                new Attribute[] { new MidAttr2() });
        }
    }
}
