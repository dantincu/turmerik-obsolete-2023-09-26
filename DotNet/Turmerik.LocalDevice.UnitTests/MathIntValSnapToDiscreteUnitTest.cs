using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.MathH;
using Xunit;

namespace Turmerik.LocalDevice.UnitTests
{
    public class MathIntValSnapToDiscreteUnitTest : UnitTestBase
    {
        [Fact]
        public void TestSnapToDiscrete()
        {
            TestSnapToDiscreteCore(29, 3, false, 27);
            TestSnapToDiscreteCore(29, 3, true, 30);
        }

        [Fact]
        public void TestSnapToInterval()
        {
            TestSnapToIntervalCore(7, 29, 37, 3, false, 29);
            TestSnapToIntervalCore(7, 29, 37, 3, true, 32);
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
