using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Turmerik.Core.Logging;
using Turmerik.Core.Reflection;
using Turmerik.Core.Text;
using Turmerik.LocalDevice.Core.Env;

namespace Turmerik.LocalDevice.Core.Logging
{
    public class AppLoggerFactory : IAppLoggerFactory
    {
        public const string BUFFERED_LOGGER_DIR_NAME_TPL = "{0:D4}";

        private readonly IAppEnv appEnv;
        private readonly ITimeStampHelper timeStampHelper;
        private volatile int bufferedLoggerDirNameIdx;

        public AppLoggerFactory(
            IAppEnv appEnv,
            ITimeStampHelper timeStampHelper)
        {
            this.appEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
        }

        public IAppLogger GetAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Information)
        {
            var opts = new AppLoggerOptsMtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel,
                LoggerRelPath = loggerRelPath
            };

            var logger = new AppLogger(opts);
            return logger;
        }

        public IAppLogger GetAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information)
        {
            var appLogger = GetAppLogger(
                loggerNameType.GetTypeFullDisplayName(),
                logEventLevel);

            return appLogger;
        }

        public IAppLogger GetBufferedAppLogger(
            string loggerRelPath,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Information)
        {
            bufferedLoggerDirNameIdx = Interlocked.Increment(ref this.bufferedLoggerDirNameIdx);

            string bufferedLoggerDirName = string.Format(
                BUFFERED_LOGGER_DIR_NAME_TPL,
                bufferedLoggerDirNameIdx);

            string timeStampStr = timeStampHelper.TmStmp(
                DateTime.UtcNow,
                true,
                TimeStamp.Ticks,
                true,
                true);

            loggerRelPath = Path.Combine(
                loggerRelPath,
                timeStampStr,
                bufferedLoggerDirName);

            var opts = new AppLoggerOptsMtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel,
                LoggerRelPath = loggerRelPath,
                IsLoggerBuffered = true
            };

            var logger = new AppLogger(opts);
            return logger;
        }

        public IAppLogger GetBufferedAppLogger(
            Type loggerNameType,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Information)
        {
            var appLogger = GetBufferedAppLogger(
                loggerNameType.GetTypeFullDisplayName(),
                out bufferedLoggerDirNameIdx,
                logEventLevel);

            return appLogger;
        }

        public IAppLogger GetSharedAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Debug)
        {
            var opts = new AppLoggerOptsMtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel,
                LoggerRelPath = loggerRelPath,
                IsLoggerShared = true
            };

            var logger = new AppLogger(opts);
            return logger;
        }

        public IAppLogger GetSharedAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Debug)
        {
            var appLogger = GetSharedAppLogger(
                loggerNameType.GetTypeFullDisplayName(),
                logEventLevel);

            return appLogger;
        }
    }
}
