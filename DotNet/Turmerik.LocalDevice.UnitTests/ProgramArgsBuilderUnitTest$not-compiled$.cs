using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.UnitTests
{
    public class ProgramArgsBuilderUnitTest : UnitTestBase
    {
        private readonly IProgramArgsBuilderFactory argsBuilderFactory;
        private readonly IProgramArgsBuilder<Args> argsBuilder;

        public ProgramArgsBuilderUnitTest()
        {
            argsBuilderFactory = ServiceProvider.GetRequiredService<IProgramArgsBuilderFactory>();
            argsBuilder = argsBuilderFactory.Create<Args>();
        }

        [Fact]
        public void MainTest()
        {
            Args expectedArgs = new Args
            {
                Id = 1324,
                Name = "asdfasdf",
                DblVal = 123.456,
                DcmlNllblVal = null,
                TimeStamp = new DateTime(2023, 7, 21, 16, 35, 25)
            };

            Func<ProgramArgsBuilder<Args>.BldArgs, ProgramArgsBuilder<Args>.NextArg> nextArgMsgFactory = bldArgs =>
            {
                ProgramArgsBuilder<Args>.NextArg nextArg = default;
                var args = bldArgs.Args;
                var rawArg = bldArgs.RawArg;

                nextArg = bldArgs.RawArgIdx switch
                {
                    -1 => bldArgs.AskMsg("Type an id"),
                    0 => bldArgs.AskMsg("Type a name"),
                    1 => bldArgs.AskMsg("Type a double value"),
                    2 => bldArgs.AskMsg("Type a decimal value or nothing"),
                    3 => bldArgs.AskMsg("Type a time stamp"),
                    4 => default
                };

                if (rawArg != null)
                {
                    switch (bldArgs.RawArgIdx)
                    {
                        case 0:
                            if (int.TryParse(rawArg, out int intVal))
                            {
                                args.Id = intVal;
                            }
                            else
                            {
                                nextArg = argsBuilder.ErrArgs(bldArgs, "Invalid id");
                            }
                            break;
                        case 1:
                            args.Name = rawArg;
                            break;
                        case 2:
                            if (double.TryParse(rawArg, CultureInfo.InvariantCulture, out double dblVal))
                            {
                                args.DblVal = dblVal;
                            }
                            else
                            {
                                nextArg = argsBuilder.ErrArgs(bldArgs, "Invalid dbl value");
                            }
                            break;
                        case 3:
                            if (!string.IsNullOrWhiteSpace(rawArg))
                            {
                                if (decimal.TryParse(rawArg, out decimal dcmlNllblVal))
                                {
                                    args.DcmlNllblVal = dcmlNllblVal;
                                }
                                else
                                {
                                    nextArg = argsBuilder.ErrArgs(bldArgs, "Invalid decimal value");
                                }
                            }
                            break;
                        case 4:
                            if (DateTime.TryParseExact(
                                rawArg,
                                "yyyy-MM-dd HH:mm:ss",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.AllowInnerWhite,
                                out var timeStamp))
                            {
                                args.TimeStamp = timeStamp;
                            }
                            else
                            {
                                nextArg = argsBuilder.ErrArgs(bldArgs, "Invalid time stamp value");
                            }
                            break;
                    }
                }

                return nextArg;
            };

            Func<ProgramArgsBuilder<Args>.BldArgs, ProgramArgsBuilder<Args>.NextArg, string> nextRawArgFactory = (
                bldArgs, nextArgs) => bldArgs.RawArgIdx switch
                {
                    -1 => "1324",
                    0 => "asdfasdf",
                    1 => "123.456",
                    2 => "",
                    3 => "2023-07-21 16:35:25",
                };

            string[] argsArr = new string[]
            {
                "1324",
                "asdfasdf",
                "123.456",
                "",
                "2023-07-21 16:35:25"
            };

            for (int i = 0; i <= argsArr.Length; i++)
            {
                PerformTest(
                    argsArr.Take(i).ToArray(),
                    nextArgMsgFactory,
                    nextRawArgFactory,
                    expectedArgs);
            }
        }

        private void PerformTest(
            string[] rawArgs,
            Func<ProgramArgsBuilder<Args>.BldArgs, ProgramArgsBuilder<Args>.NextArg> nextArgMsgFactory,
            Func<ProgramArgsBuilder<Args>.BldArgs, ProgramArgsBuilder<Args>.NextArg, string> nextRawArgFactory,
            Args expectedArgs)
        {
            var opts = new ProgramArgsBuilder<Args>.Opts
            {
                RawArgs = rawArgs,
                NextArgMsgFactory = nextArgMsgFactory,
                NextRawArgFactory = nextRawArgFactory
            };

            var actualArgs = argsBuilder.BuildArgs(opts);

            AssertAreEqual(
                expectedArgs,
                actualArgs);
        }

        private void AssertAreEqual(
            Args expectedObj,
            Args actualObj)
        {
            Assert.Equal(
                expectedObj.Id,
                actualObj.Id);

            Assert.Equal(
                expectedObj.Name,
                actualObj.Name);

            Assert.Equal(
                expectedObj.DblVal,
                actualObj.DblVal);

            Assert.Equal(
                expectedObj.DcmlNllblVal,
                actualObj.DcmlNllblVal);

            Assert.Equal(
                expectedObj.TimeStamp,
                actualObj.TimeStamp);
        }

        public class Args
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double DblVal { get; set; }
            public decimal? DcmlNllblVal { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}
