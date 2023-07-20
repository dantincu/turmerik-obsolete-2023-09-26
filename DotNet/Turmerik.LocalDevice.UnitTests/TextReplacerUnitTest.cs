using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.RegexH;
using Turmerik.Text;

namespace Turmerik.LocalDevice.UnitTests
{
    public class TextReplacerUnitTest : UnitTestBase
    {
        private readonly ITextReplacerComponent textReplacer;

        public TextReplacerUnitTest()
        {
            textReplacer = ServiceProvider.GetRequiredService<ITextReplacerComponent>();
        }

        [Fact]
        public void MainTest()
        {
            PerformTest(new ReplaceTextOpts
            {
                InputText = "asdfqwerzxcv",
                SearchedText = "qwer",
                TextReplacerFactory = args => "12345"
            },
            "asdf12345zxcv");

            PerformTest(new ReplaceTextOpts
            {
                InputText = "asdfqwerzxcv",
                SearchedText = "qwer",
                TextReplacerFactory = args => "123"
            },
            "asdf123zxcv");

            PerformTest(new ReplaceTextOpts
            {
                InputText = "asdf0987asdf1234zxcv0987zxcv",
                SearchedTextRegex = new Regex(@"[a-z]+"),
                RegexReplacerFactory = args => ">>>"
            },
            ">>>0987>>>1234>>>0987>>>");

            PerformTest(new ReplaceTextOpts
            {
                InputText = "asdf0987asdf1234zxcv0987zxcv",
                SearchedTextRegex = new Regex(@"[a-z]+"),
                RegexReplacerFactory = args => ">>>>>"
            },
            ">>>>>0987>>>>>1234>>>>>0987>>>>>");

            PerformTest(new ReplaceTextOpts
            {
                InputText = "asdf0987zxcv1234zxcv0987asdf",
                SearchedTextStartCharPredicate = args => args.InputStr.StartsWithStr(
                    args.Idx, "as").ToIncIdxAnswer(0, 1),
                SearchedTextEndCharPredicate = (args, stIdx) => (args.Idx - args.StartIdx >= 2 && args.InputStr.EndsWithStr(
                    args.Idx + 1, "df")).ToIncIdxAnswer(1),
                TextReplacerFactory = args => ">>>>>"
            },
            ">>>>>0987zxcv1234zxcv0987>>>>>");

            PerformTest(new ReplaceTextOpts
            {
                InputText = "asdf0987zxcv1234zxcv0987asdf",
                SearchedTextStartCharPredicate = args => args.InputStr.StartsWithStr(
                    args.Idx, "as").ToIncIdxAnswer(0, 1),
                SearchedTextEndCharPredicate = (args, stIdx) => (args.Idx - args.StartIdx >= 2 && args.InputStr.EndsWithStr(
                    args.Idx + 1, "df")).ToIncIdxAnswer(1),
                TextReplacerFactory = args => ">>>"
            },
            ">>>0987zxcv1234zxcv0987>>>");
        }

        private void PerformTest(
            ReplaceTextOpts opts,
            string expectedOutputText)
        {
            string actualOutputText = textReplacer.ReplaceText(opts);
            Assert.Equal(expectedOutputText, actualOutputText);
        }
    }
}
