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
        public void MainBaseC1Test()
        {
            var cachedType = CachedTypesMap.Get(typeof(BaseC1<int, string>));
            Assert.True(cachedType.Data.IsGenericType);
        }
    }
}
