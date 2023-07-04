﻿using System;
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
        public void MainIC2Test()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC2<int>));
            Assert.True(cachedType.Data.IsGenericType);
        }
    }
}
