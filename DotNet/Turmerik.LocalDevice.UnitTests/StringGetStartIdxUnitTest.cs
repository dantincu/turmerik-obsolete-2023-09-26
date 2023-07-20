using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text;

namespace Turmerik.LocalDevice.UnitTests
{
    public class StringGetStartIdxUnitTest : UnitTestBase
    {
        [Fact]
        public void MainTest()
        {
            PerformTest(
                "asdfqwer",
                0,
                args => args.InputStr.EndsWithStr(
                    args.Idx + 1, "qwer").ToIncIdxAnswer(Math.Max(-args.Idx, - 1), 1),
                6);

            PerformTest(
                "asdfqwer",
                0,
                args => args.InputStr.EndsWithStr(
                    args.Idx + 1, "qwer").ToIncIdxAnswer(0, 1),
                7);

            PerformTest(
                "asdfqwer",
                0,
                args => args.InputStr.EndsWithStr(
                    args.Idx + 1, "qwer").ToIncIdxAnswer(1),
                8);
        }

        private void PerformTest(
            string inputStr,
            int startIdx,
            Func<SliceStrArgs, IncIdxAnswer> startCharPredicate,
            int expectedRetIdx)
        {
            int actualRetIdx = inputStr.GetStartIdx(
                startIdx,
                startCharPredicate);

            Assert.Equal(expectedRetIdx, actualRetIdx);
        }
    }
}
