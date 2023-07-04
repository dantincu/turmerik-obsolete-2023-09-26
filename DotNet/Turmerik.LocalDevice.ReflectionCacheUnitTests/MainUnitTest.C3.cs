using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        [Fact]
        public void C3EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(C3));
            Assert.False(cachedType.Data.IsGenericType);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new CAttr3() },
                false);

            AssertHasAttrs(
                cachedType,
                new Attribute[] { new CAttr3(), new MidCAttr2(), new BaseCAttr1() });

            AssertHasAttrs(
                cachedType.InstanceProps.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(C3.C3PubIntVal)),
                new Attribute[] { new Attr3() });

            AssertHasAttrs(
                cachedType.InstanceMethods.Value.Own.Value.Items.Single(
                    prop => prop.Name == nameof(C3.GetC3PubIntVal)),
                new Attribute[] { new Attr3() });

            AssertHasAttrs(
                cachedType.Constructors.Value.Own.Value.Items.Single(
                    ctr => ctr.Flags.Value.IsFamily && ctr.Parameters.Value.None()),
                new Attribute[] { new Attr3() });
        }
    }
}
