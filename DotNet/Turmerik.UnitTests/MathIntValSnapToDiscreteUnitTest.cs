using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.MathH;
using Xunit;

namespace Turmerik.UnitTests.Tests
{
    public class MathIntValSnapToDiscreteUnitTest : UnitTestBase
    {
        [Fact]
        public void TestSnapToDiscrete()
        {
            this.TestSnapToDiscreteCore(29, 3, false, 27);
            this.TestSnapToDiscreteCore(29, 3, true, 30);
        }

        [Fact]
        public void TestSnapToInterval()
        {
            this.TestSnapToIntervalCore(7, 29, 37, 3, false, 29);
            this.TestSnapToIntervalCore(7, 29, 37, 3, true, 32);
        }

        private void TestSnapToDiscreteCore(int value, int snapVal, bool addToSnap, int expectedValue)
        {
            int actualValue = MH.SnapToDiscrete(value, snapVal, addToSnap);
            Assert.Equal(expectedValue, actualValue);
        }

        private void TestSnapToIntervalCore(int value, int minVal, int maxVal, int snapVal, bool addToSnap, int expectedValue)
        {
            int actualValue = MH.SnapToInterval(value, minVal, maxVal, snapVal, addToSnap);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
