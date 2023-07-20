using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text.MdH;

namespace Turmerik.LocalDevice.UnitTests
{
    public class TabsToMdTableConverterUnitTest : UnitTestBase
    {
        private readonly ITabsToMdTableConverter tabsToMdTableConverter;

        public TabsToMdTableConverterUnitTest()
        {
            tabsToMdTableConverter = ServiceProvider.GetRequiredService<ITabsToMdTableConverter>();
        }

        [Fact]
        public void MainTest()
        {
            PerformLineToMdTableTest(
                "asdf\tzxcv",
                " | asdf | zxcv |  ");

            PerformLineToMdTableTest(
                "asdf\tzxcv\tqwer",
                " | asdf | zxcv | qwer |  ");

            PerformLinesToMdTableTest(
                "asdf\tzxcv\r\nasdf\tzxcv\r\nasdf\tzxcv",
                false,
                " | asdf | zxcv |  \n | asdf | zxcv |  \n | asdf | zxcv |  ");

            PerformLinesToMdTableTest(
                "asdf\tzxcv\r\nasdf\tzxcv\nasdf\tzxcv",
                true,
                " | asdf | zxcv |  \n | -- | -- |  \n | asdf | zxcv |  \n | asdf | zxcv |  ");

            PerformLinesToMdTableTest(
                "asdf\tzxcv".Arr("asdf\tzxcv", "asdf\tzxcv"),
                false,
                " | asdf | zxcv |  ".Arr(" | asdf | zxcv |  ", " | asdf | zxcv |  "));

            PerformLinesToMdTableTest(
                "asdf\tzxcv".Arr("asdf\tzxcv", "asdf\tzxcv"),
                true,
                " | asdf | zxcv |  ".Arr(" | -- | -- |  ", " | asdf | zxcv |  ", " | asdf | zxcv |  "));
        }

        private void PerformLineToMdTableTest(
            string line,
            string expectedOutput)
        {
            string actualOutput = tabsToMdTableConverter.LineToMdTable(line);
            Assert.Equal(expectedOutput, actualOutput);
        }

        private void PerformLinesToMdTableTest(
            string[] linesArr,
            bool firstLineIsHeader,
            string[] expectedOutputArr)
        {
            string[] actualOutputArr = tabsToMdTableConverter.LinesToMdTable(
                linesArr,
                firstLineIsHeader);

            AssertSequenceEqual(expectedOutputArr, actualOutputArr);
        }

        private void PerformLinesToMdTableTest(
            string lines,
            bool firstLineIsHeader,
            string expectedOutput)
        {
            string actualOutput = tabsToMdTableConverter.LinesToMdTable(
                lines,
                firstLineIsHeader);

            Assert.Equal(expectedOutput, actualOutput);
        }
    }
}
