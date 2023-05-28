using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Turmerik.Text;

namespace Turmerik.LocalDevice.UnitTests
{
    public class StringUnitTest : UnitTestBase
    {
        [Fact]
        public void ReplaceAllCharsTest()
        {
            var replDictnr = new Dictionary<char, char>
            {
                { '+', '-' },
                { '/', '_' },
            };

            string baseInputStr = "pwbx0o+F+EO7NzvDH/WmhsCpk+JU64tGuRkHV0vTKJzxRE6EP9zKQqeMiw+WmUCl7M1XjysXxEKG/oNyFxL0MQ==";
            string baseOutputStr = "pwbx0o-F-EO7NzvDH_WmhsCpk-JU64tGuRkHV0vTKJzxRE6EP9zKQqeMiw-WmUCl7M1XjysXxEKG_oNyFxL0MQ==";

            AssertReplaceAllChars(baseInputStr, replDictnr, baseOutputStr);
            AssertReplaceAllChars(baseInputStr + "+", replDictnr, baseOutputStr + "-");
            AssertReplaceAllChars("/" + baseInputStr + "+", replDictnr, "_" + baseOutputStr + "-");
            AssertReplaceAllChars("/" + baseInputStr, replDictnr, "_" + baseOutputStr);

            replDictnr = new Dictionary<char, char>
            {
                { '+', '/' },
                { '/', '=' },
                { '=', '+' },
            };

            string outputStr1 = "pwbx0o/F/EO7NzvDH=WmhsCpk/JU64tGuRkHV0vTKJzxRE6EP9zKQqeMiw/WmUCl7M1XjysXxEKG=oNyFxL0MQ++";
            string outputStr2 = "pwbx0o=F=EO7NzvDH+WmhsCpk=JU64tGuRkHV0vTKJzxRE6EP9zKQqeMiw=WmUCl7M1XjysXxEKG+oNyFxL0MQ//";

            AssertReplaceAllChars(baseInputStr, replDictnr, outputStr1);
            AssertReplaceAllChars(outputStr1, replDictnr, outputStr2);
            AssertReplaceAllChars(outputStr2, replDictnr, baseInputStr);
        }

        [Fact]
        public void SubStrTest()
        {
            AssertSubStr("qwerasdf", 0, 0, "");
            AssertSubStr("qwerasdf", 0, 1, "q");
            AssertSubStr("qwerasdf", 0, -1, "qwerasdf");
            AssertSubStr("qwerasdf", 0, -2, "qwerasd");

            AssertSubStr("qwerasdf", 1, 0, "");
            AssertSubStr("qwerasdf", 1, 1, "w");
            AssertSubStr("qwerasdf", 1, -1, "werasdf");
            AssertSubStr("qwerasdf", 1, -2, "werasd");

            AssertSubStr("qwerasdf", -1, 0, "");
            AssertSubStr("qwerasdf", -1, -1, "d");
            AssertSubStr("qwerasdf", -1, -2, "sd");
            AssertSubStr("qwerasdf", -2, 1, "d");
            AssertSubStr("qwerasdf", -2, 2, "df");
        }

        [Fact]
        public void SliceTest()
        {
            AssertSlice("qwerasdf", 0, 0, "");
            AssertSlice("qwerasdf", 0, 1, "q");
            AssertSlice("qwerasdf", 0, -1, "qwerasdf");
            AssertSlice("qwerasdf", 0, -2, "qwerasd");

            AssertSlice("qwerasdf", 1, 0, "");
            AssertSlice("qwerasdf", 1, 1, "w");
            AssertSlice("qwerasdf", 1, -1, "werasdf");
            AssertSlice("qwerasdf", 1, -2, "werasd");

            AssertSlice("qwerasdf", -1, 0, "");
            AssertSlice("qwerasdf", -1, -1, "d");
            AssertSlice("qwerasdf", -1, -2, "sd");
            AssertSlice("qwerasdf", -2, 1, "d");
            AssertSlice("qwerasdf", -2, 2, "df");
        }

        private void AssertReplaceAllChars(string input, Dictionary<char, string> replDictnr, string expectedOutput)
        {
            string actualOutput = input.ReplaceAllChars(replDictnr);
            Assert.Equal(expectedOutput, actualOutput);
        }

        private void AssertReplaceAllChars(string input, Dictionary<char, char> replDictnr, string expectedOutput)
        {
            string actualOutput = input.ReplaceAllChars(replDictnr);
            Assert.Equal(expectedOutput, actualOutput);
        }

        private void AssertSubStr(
            string inputStr,
            int startIdx,
            int count,
            string expectedOutput)
        {
            string actualOutput = inputStr.SliceStr(startIdx, count);
            Assert.Equal(expectedOutput, actualOutput);
        }

        private void AssertSlice(
            char[] inputArr,
            int startIdx,
            int count,
            char[] expectedOutput)
        {
            string actualOutput = inputArr.ToStr().SliceStr(startIdx, count);
            AssertSequenceEqual(expectedOutput, actualOutput);
        }

        private void AssertSlice(
            string inputString,
            int startIdx,
            int count,
            string expectedOutput)
        {
            string actualOutput = inputString.SliceStr(startIdx, count);
            AssertSequenceEqual(expectedOutput, actualOutput);
        }
    }
}
