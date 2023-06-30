using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.Text;
using Turmerik.Collections;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

namespace Turmerik.LocalDevice.UnitTests
{
    public class StringTemplateParserUnitTest : UnitTestBase
    {
        private readonly IStringTemplateParser stringTemplateParser;

        public StringTemplateParserUnitTest()
        {
            stringTemplateParser = ServiceProvider.GetRequiredService<IStringTemplateParser>();
        }

        [Fact]
        public void MainTest()
        {
            PerformTest(
                "Some Test ".Arr("{0}", " asdfasdf"),
                testData => new Dictionary<int, IStringTemplateToken>
                {
                    {
                        testData.Item2[0],
                        new StringLiteralToken(testData.Item1[0])
                    },
                    {
                        testData.Item2[1],
                        new StringTemplateToken(0, null)
                    },
                    {
                        testData.Item2[2],
                        new StringLiteralToken(testData.Item1[2])
                    },
                });

            PerformTest(
                "Some Test ".Arr("{8:a}", "{1}", " asdfasdf"),
                testData => new Dictionary<int, IStringTemplateToken>
                {
                    {
                        testData.Item2[0],
                        new StringLiteralToken(testData.Item1[0])
                    },
                    {
                        testData.Item2[1],
                        new StringTemplateToken(8, "a")
                    },
                    {
                        testData.Item2[2],
                        new StringTemplateToken(1, null)
                    },
                    {
                        testData.Item2[3],
                        new StringLiteralToken(testData.Item1[3])
                    },
                });

            PerformTest(
                "Some Test zxcvzxcv asdfasdf".Arr(),
                testData => new Dictionary<int, IStringTemplateToken>
                {
                    {
                        testData.Item2[0],
                        new StringLiteralToken(testData.Item1[0])
                    },
                });

            PerformTest(
                "{1}".Arr("Some Test asdfasdf"),
                testData => new Dictionary<int, IStringTemplateToken>
                {
                    {
                        testData.Item2[0],
                        new StringTemplateToken(1, null)
                    },
                    {
                        testData.Item2[1],
                        new StringLiteralToken(testData.Item1[1])
                    }
                });

            PerformTest(
                "Some Test asdfasdf ".Arr("{33:asdf}"),
                testData => new Dictionary<int, IStringTemplateToken>
                {
                    {
                        testData.Item2[0],
                        new StringLiteralToken(testData.Item1[0])
                    },
                    {
                        testData.Item2[1],
                        new StringTemplateToken(33, "asdf")
                    }
                });
        }

        private void PerformTest(
            string[] strArr,
            Func<Tuple<string[], int[]>, Dictionary<int, IStringTemplateToken>> expectedResultFactory)
        {
            var testData = GetTestData(strArr);
            var expectedResult = expectedResultFactory(testData);

            string templateStr = string.Concat(strArr);
            var actualResult = stringTemplateParser.Parse(templateStr);

            AssertEqual(expectedResult, actualResult);
        }

        private void AssertEqual(
            Dictionary<int, IStringTemplateToken> expectedResult,
            Dictionary<int, IStringTemplateToken> actualResult)
        {
            this.AssertSequenceEqual(
                expectedResult.Keys,
                actualResult.Keys);

            foreach (var kvp in expectedResult)
            {
                bool hasMatch = actualResult.TryGetValue(
                    kvp.Key, out var match);

                Assert.True(hasMatch);

                AssertEqual(
                    kvp.Value,
                    match);
            }
        }

        private void AssertEqual(
            IStringTemplateToken expectedResult,
            IStringTemplateToken actualResult)
        {
            if (expectedResult is StringTemplateToken expectedTemplateToken)
            {
                var actualTemplateToken = actualResult as StringTemplateToken;
                Assert.NotNull(actualTemplateToken);

                AssertEqual(
                    expectedTemplateToken,
                    actualTemplateToken);
            }
            else if (expectedResult is StringLiteralToken expectedLiteralToken)
            {
                var actualLiteralToken = actualResult as StringLiteralToken;
                Assert.NotNull(actualLiteralToken);

                AssertEqual(
                    expectedLiteralToken,
                    actualLiteralToken);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void AssertEqual(
            StringTemplateToken expectedResult,
            StringTemplateToken actualResult)
        {
            Assert.Equal(
                expectedResult.Idx,
                actualResult.Idx);

            Assert.Equal(
                expectedResult.Format,
                actualResult.Format);
        }

        private void AssertEqual(
            StringLiteralToken expectedResult,
            StringLiteralToken actualResult) => Assert.Equal(
                expectedResult.Value,
                actualResult.Value);

        private Tuple<string[], int[]> GetTestData(
            string[] strArr)
        {
            var idxList = new List<int>();
            int len = 0;

            for (int i = 0; i < strArr.Length; i++)
            {
                idxList.Add(len);
                len += strArr[i].Length;
            }

            return Tuple.Create(
                strArr,
                idxList.ToArray());
        }
    }
}
