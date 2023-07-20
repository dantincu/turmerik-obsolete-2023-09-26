using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Text.MdH;

namespace Turmerik.LocalDevice.UnitTests
{
    public class LinesMdIndenterUnitTest : UnitTestBase
    {
        private readonly ILinesMdIndenter linesMdIndenter;

        public LinesMdIndenterUnitTest()
        {
            linesMdIndenter = ServiceProvider.GetRequiredService<ILinesMdIndenter>();
        }

        [Fact]
        public void MainTest()
        {
            PerformTest(
                "asdfasdfasdf",
                20,
                " > asdfasdfasdf");

            PerformTest(
                "asdfasdfasdf[qwer]zxcvzxcv",
                20,
                " > asdfasdfasdf<u>[qwer]</u>zxcvzxcv");

            PerformTest(
                "asdfasdfasdf[ qwer]zxcvzxcv",
                20,
                " > asdfasdfasdf<u>[ qwer]</u>zxcvzxcv");

            PerformTest(
                "asdfasdfasdf[qwer ]zxcvzxcv",
                20,
                " > asdfasdfasdf<u>[qwer ]</u>zxcvzxcv");

            PerformTest(
                "asdfasdfasdf[ qwer ]zxcvzxcv",
                20,
                " > asdfasdfasdf<u>[ qwer ]</u>zxcvzxcv");

            PerformTest(
                "asdfasdfasdf [qwer]zxcvzxcv",
                20,
                " > asdfasdfasdf <u>[qwer]</u>zxcvzxcv");

            PerformTest(
                "asdfasdfasdf [qwer] zxcvzxcv",
                20,
                " > asdfasdfasdf <u>[qwer]</u> zxcvzxcv");

            PerformTest(
                "asdfasdfasdf[qwer] zxcvzxcv",
                20,
                " > asdfasdfasdf<u>[qwer]</u> zxcvzxcv");

            PerformTest(
                "asdfasdfasdf[123456789012345678901]zxcvzxcv",
                20,
                " > asdfasdfasdf[123456789012345678901]zxcvzxcv");

            PerformTest(
                "asdfasdfasdf[123456789012345678901] zxcvzxcv",
                20,
                " > asdfasdfasdf[123456789012345678901] zxcvzxcv");

            PerformTest(
                "asdfasdfasdf [123456789012345678901] zxcvzxcv",
                20,
                " > asdfasdfasdf [123456789012345678901] zxcvzxcv");
        }

        private void PerformTest(
            string line,
            int emphasizeLinkMaxLength,
            string expectedOutput)
        {
            var actualOutput = linesMdIndenter.AddIdent(
                line,
                emphasizeLinkMaxLength);

            Assert.Equal(expectedOutput, actualOutput);
        }
    }
}
