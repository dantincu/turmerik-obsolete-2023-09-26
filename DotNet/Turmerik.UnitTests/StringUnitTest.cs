using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Turmerik.Core.Text;

namespace Turmerik.UnitTests.Tests
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

            this.AssertReplaceAllChars(baseInputStr, replDictnr, baseOutputStr);
            this.AssertReplaceAllChars(baseInputStr + "+", replDictnr, baseOutputStr + "-");
            this.AssertReplaceAllChars("/" + baseInputStr + "+", replDictnr, "_" + baseOutputStr + "-");
            this.AssertReplaceAllChars("/" + baseInputStr, replDictnr, "_" + baseOutputStr);

            replDictnr = new Dictionary<char, char>
            {
                { '+', '/' },
                { '/', '=' },
                { '=', '+' },
            };

            string outputStr1 = "pwbx0o/F/EO7NzvDH=WmhsCpk/JU64tGuRkHV0vTKJzxRE6EP9zKQqeMiw/WmUCl7M1XjysXxEKG=oNyFxL0MQ++";
            string outputStr2 = "pwbx0o=F=EO7NzvDH+WmhsCpk=JU64tGuRkHV0vTKJzxRE6EP9zKQqeMiw=WmUCl7M1XjysXxEKG+oNyFxL0MQ//";

            this.AssertReplaceAllChars(baseInputStr, replDictnr, outputStr1);
            this.AssertReplaceAllChars(outputStr1, replDictnr, outputStr2);
            this.AssertReplaceAllChars(outputStr2, replDictnr, baseInputStr);
        }

        [Fact]
        public void SubStrTest()
        {
            this.AssertSubStr("qwerasdf", 0, 0, "");
            this.AssertSubStr("qwerasdf", 0, 1, "q");
            this.AssertSubStr("qwerasdf", 0, -1, "qwerasdf");
            this.AssertSubStr("qwerasdf", 0, -2, "qwerasd");

            this.AssertSubStr("qwerasdf", 1, 0, "");
            this.AssertSubStr("qwerasdf", 1, 1, "w");
            this.AssertSubStr("qwerasdf", 1, -1, "werasdf");
            this.AssertSubStr("qwerasdf", 1, -2, "werasd");

            this.AssertSubStr("qwerasdf", -1, 0, "");
            this.AssertSubStr("qwerasdf", -1, -1, "d");
            this.AssertSubStr("qwerasdf", -1, -2, "sd");
            this.AssertSubStr("qwerasdf", -2, 1, "d");
            this.AssertSubStr("qwerasdf", -2, 2, "df");
        }

        [Fact]
        public void SliceTest()
        {
            this.AssertSlice("qwerasdf", 0, 0, "");
            this.AssertSlice("qwerasdf", 0, 1, "q");
            this.AssertSlice("qwerasdf", 0, -1, "qwerasdf");
            this.AssertSlice("qwerasdf", 0, -2, "qwerasd");

            this.AssertSlice("qwerasdf", 1, 0, "");
            this.AssertSlice("qwerasdf", 1, 1, "w");
            this.AssertSlice("qwerasdf", 1, -1, "werasdf");
            this.AssertSlice("qwerasdf", 1, -2, "werasd");

            this.AssertSlice("qwerasdf", -1, 0, "");
            this.AssertSlice("qwerasdf", -1, -1, "d");
            this.AssertSlice("qwerasdf", -1, -2, "sd");
            this.AssertSlice("qwerasdf", -2, 1, "d");
            this.AssertSlice("qwerasdf", -2, 2, "df");
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
            this.AssertSequenceEqual(expectedOutput, actualOutput);
        }

        private void AssertSlice(
            string inputString,
            int startIdx,
            int count,
            string expectedOutput)
        {
            string actualOutput = inputString.SliceStr(startIdx, count);
            this.AssertSequenceEqual(expectedOutput, actualOutput);
        }
    }
}
