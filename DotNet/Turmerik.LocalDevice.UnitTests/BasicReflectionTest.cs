using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Turmerik.LocalDevice.UnitTests
{
    public class BasicReflectionTest : UnitTestBase
    {
        [Fact]
        public void MainTest()
        {
            var propInfos = typeof(Child).GetProperties(
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.FlattenHierarchy);

            Assert.Equal(2, propInfos.Length);
        }

        private class Parent
        {
            public string X { get; set; }
        }

        private class Child : Parent
        {
            public string Y { get; set; }
        }
    }
}
