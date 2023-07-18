using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Turmerik.Reflection;
using Turmerik.Text;
using Turmerik.Logging;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Infrastucture;
using Turmerik.Utils;
using System.Runtime.CompilerServices;

namespace Turmerik.LocalDevice.Core.Logging
{
    public class AppLoggerFactory : IAppLoggerFactory
    {
        public const string BUFFERED_LOGGER_DIR_NAME_TPL = "{0:D4}";

        private readonly IAppEnv appEnv;
        private readonly IAppProcessIdentifier appProcessIdentifier;
        private readonly ITimeStampHelper timeStampHelper;
        private readonly ITrmrkJsonFormatterFactory trmrkJsonFormatterFactory;
        private readonly IStringTemplateParser stringTemplateParser;
        private readonly Lazy<IAppLogger> logger;

        private volatile int bufferedLoggerDirNameIdx;
        private volatile int appProcessIdnfDumped;

        public AppLoggerFactory(
            IAppEnv appEnv,
            ITimeStampHelper timeStampHelper,
            IAppProcessIdentifier appProcessIdentifier,
            ITrmrkJsonFormatterFactory trmrkJsonFormatterFactory,
            IStringTemplateParser stringTemplateParser)
        {
            this.appEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));
            this.appProcessIdentifier = appProcessIdentifier ?? throw new ArgumentNullException(nameof(appProcessIdentifier));
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            this.trmrkJsonFormatterFactory = trmrkJsonFormatterFactory ?? throw new ArgumentNullException(nameof(trmrkJsonFormatterFactory));
            this.stringTemplateParser = stringTemplateParser ?? throw new ArgumentNullException(nameof(stringTemplateParser));
            this.logger = LazyH.Lazy(() => GetAppLogger(GetType()));

            if (UseAppProcessIdnfByDefault)
            {
                AssureAppProcessIdnfDumped();
            }
        }

        public static bool UseAppProcessIdnfByDefault { get; set; }

        public void AssureAppProcessIdnfDumped()
        {
            if (Interlocked.CompareExchange(ref appProcessIdnfDumped, 1, 0) == 0)
            {
                DumpAppProcessIdnf();
            }
        }

        public void DumpAppProcessIdnf()
        {
            logger.Value.InformationData(
                appProcessIdentifier,
                nameof(appProcessIdentifier));
        }

        public IAppLogger GetAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null)
        {
            var opts = new AppLoggerOpts.Mtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel,
                LogDirRelPath = loggerRelPath,
                AppProcessIdentifier = GetIAppProcessIdentifier(useAppProcessIdnf),
                TextFormatter = trmrkJsonFormatterFactory.CreateFormatter(),
                StringTemplateParser = stringTemplateParser
            };

            var logger = new AppLogger(opts);
            return logger;
        }

        public IAppLogger GetAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null)
        {
            var appLogger = GetAppLogger(
                loggerNameType.GetTypeFullDisplayName(),
                logEventLevel,
                useAppProcessIdnf);

            return appLogger;
        }

        public IAppLogger GetBufferedAppLogger(
            string loggerRelPath,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null)
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

            var opts = new AppLoggerOpts.Mtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel,
                LogDirRelPath = loggerRelPath,
                IsLoggerBuffered = true,
                AppProcessIdentifier = GetIAppProcessIdentifier(useAppProcessIdnf),
                TextFormatter = trmrkJsonFormatterFactory.CreateFormatter(),
                StringTemplateParser = stringTemplateParser
            };

            var logger = new AppLogger(opts);
            return logger;
        }

        public IAppLogger GetBufferedAppLogger(
            Type loggerNameType,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null)
        {
            var appLogger = GetBufferedAppLogger(
                loggerNameType.GetTypeFullDisplayName(),
                out bufferedLoggerDirNameIdx,
                logEventLevel,
                useAppProcessIdnf);

            return appLogger;
        }

        public IAppLogger GetSharedAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Debug,
            bool? useAppProcessIdnf = null)
        {
            var opts = new AppLoggerOpts.Mtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel,
                LogDirRelPath = loggerRelPath,
                IsLoggerShared = true,
                AppProcessIdentifier = GetIAppProcessIdentifier(useAppProcessIdnf),
                TextFormatter = trmrkJsonFormatterFactory.CreateFormatter(),
                StringTemplateParser = stringTemplateParser
            };

            var logger = new AppLogger(opts);
            return logger;
        }

        public IAppLogger GetSharedAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Debug,
            bool? useAppProcessIdnf = null)
        {
            var appLogger = GetSharedAppLogger(
                loggerNameType.GetTypeFullDisplayName(),
                logEventLevel,
                useAppProcessIdnf);

            return appLogger;
        }

        private IAppProcessIdentifier GetIAppProcessIdentifier(
            bool? useAppProcessIdnf) => (
            useAppProcessIdnf ?? UseAppProcessIdnfByDefault) ? appProcessIdentifier : null;
    }
}
