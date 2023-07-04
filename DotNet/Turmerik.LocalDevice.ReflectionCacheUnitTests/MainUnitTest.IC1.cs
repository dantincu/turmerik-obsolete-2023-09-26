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
        public void MainIC1Test()
        {
            var cachedType = CachedTypesMap.Get(typeof(IC1<int, string>));
            Assert.True(cachedType.Data.IsGenericType);

            AssertContains(
                cachedType.Events.Value.Own.Value,
                new ExpectedContents<Dictionary<EventAccessibilityFilter, string[]>>(
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
                                MemberVisibility.Public,
                                MemberVisibility.Public,
                                MemberVisibility.Public),
                            new string[0]
                        }
                    },
                    new Dictionary<EventAccessibilityFilter, string[]>
                    {
                        {
                            new EventAccessibilityFilter(
                                MemberVisibility.Public | MemberVisibility.Private,
                                MemberVisibility.Public | MemberVisibility.Private,
                                MemberVisibility.None),
                            nameof(IC1<int, string>.C1Event).Arr()
                        },
                        {
                            new EventAccessibilityFilter(
                                MemberVisibility.Public | MemberVisibility.Private,
                                MemberVisibility.Public | MemberVisibility.Private,
                                MemberVisibility.Public | MemberVisibility.Private),
                            new string[0]
                        }
                    }
                ));
        }
    }
}
