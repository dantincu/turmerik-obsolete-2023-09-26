using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.Testing;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.UnitTests
{
    public class LoggingUnitTest : UnitTestBase
    {
        private readonly IAppLoggerFactory appLoggerFactory;

        public LoggingUnitTest()
        {
            appLoggerFactory = ServiceProvider.GetRequiredService<IAppLoggerFactory>();
        }

        [Fact]
        public void MainTest()
        {
            using (var logger = appLoggerFactory.GetSharedAppLogger(
                GetType(),
                LogLevel.Trace))
            {
                var dictnr = new Dictionary<string, object>
                    {
                        {
                            "asdfasdf",
                            "asdfasdfasdfads"
                        },
                        {
                            "aasdfasdf",
                            new Dictionary<string, object>
                            {
                                {
                                    "asdfasdf",
                                    LogLevel.Information
                                }
                            }
                        }
                    };

                logger.Verbose(
                    "Some log \"message\" {0}",
                    dictnr);

                logger.VerboseData(
                    dictnr,
                    "Some log \"message\"");

                var aggExc = new AggregateException(
                    new TrmrkException<Tuple<int, string>>(
                        Tuple.Create(123, "asdfasdf")),
                    new TrmrkException<Tuple<Guid, int[]>>(
                        Tuple.Create(Guid.NewGuid(), 123.Arr(456, 798))));

                logger.Verbose(
                    aggExc,
                    "Some log \"message\"");

                logger.VerboseData(
                    dictnr,
                    aggExc,
                    "Some log \"message\"");
            }
        }
    }
}
