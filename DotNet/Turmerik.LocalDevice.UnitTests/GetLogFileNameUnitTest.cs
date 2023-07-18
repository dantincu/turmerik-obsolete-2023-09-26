using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;

namespace Turmerik.LocalDevice.UnitTests
{
    public class GetLogFileNameUnitTest : UnitTestBase
    {
        [Fact]
        public void GetLogFileNameMainTest()
        {
            var timeStamp = DateTime.UtcNow;

            string dateStr = timeStamp.ToString(
                LogHelperMethods.LOG_FILE_NAME_TIME_STAMP_TPL);

            PerformGetLogFileNameTest(
                "shared-log-.json",
                $"shared-log-{dateStr}.json",
                timeStamp);

            PerformGetLogFileNameTest(
                "shared-log-.json",
                $"shared-log-{dateStr}-001.json",
                timeStamp,
                1);
        }

        [Fact]
        public void GetLogFilePathMainTest()
        {
            var timeStamp = DateTime.UtcNow;

            string dateStr = timeStamp.ToString(
                LogHelperMethods.LOG_FILE_NAME_TIME_STAMP_TPL);

            PerformGetLogFilePathTest(
                "C:\\shared-log-.json",
                $"C:\\shared-log-{dateStr}.json",
                timeStamp);

            PerformGetLogFilePathTest(
                "C:\\Logs\\shared-log-.json",
                $"C:\\Logs\\shared-log-{dateStr}-001.json",
                timeStamp,
                1);
        }

        private void PerformGetLogFileNameTest(
            string logFileNameTemplate,
            string expectedLogFileName,
            DateTime timeStamp,
            int? uniquifierIdx = null)
        {
            string actualLogFileName = logFileNameTemplate.GetLogFileName(
                timeStamp,
                uniquifierIdx);

            Assert.Equal(
                expectedLogFileName,
                actualLogFileName);
        }

        private void PerformGetLogFilePathTest(
            string logFileNameTemplate,
            string expectedLogFileName,
            DateTime timeStamp,
            int? uniquifierIdx = null)
        {
            string actualLogFileName = logFileNameTemplate.GetLogFilePath(
                timeStamp,
                uniquifierIdx);

            Assert.Equal(
                expectedLogFileName,
                actualLogFileName);
        }
    }
}
