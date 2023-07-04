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
        public void MainIC3Test()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC3));
            Assert.False(cachedType.Data.IsGenericType);
        }
    }
}
