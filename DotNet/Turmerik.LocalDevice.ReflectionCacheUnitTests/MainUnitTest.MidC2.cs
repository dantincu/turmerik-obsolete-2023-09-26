using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest
    {
        [Fact]
        public void MidC2EventsTest()
        {
            var cachedType = CachedTypesMap.Get(typeof(MidC2<int>));
            Assert.True(cachedType.Data.IsGenericType);
        }
    }
}
