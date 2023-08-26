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
    public class AppLoggerCreator : IAppLoggerCreator
    {
        public const string BUFFERED_LOGGER_DIR_NAME_TPL = "{0:D4}";

        public static readonly string AppProcessIdnfDumpFileName = $"{nameof(AppProcessIdentifierData)}.json";
        public static readonly Type AppProcessIdnfDumpDirType = typeof(AppProcessIdentifierData);

        private readonly IAppEnv appEnv;
        private readonly IAppLoggerConfig appLoggerConfig;
        private readonly IAppProcessIdentifier appProcessIdentifier;
        private readonly ITimeStampHelper timeStampHelper;
        private readonly ITrmrkJsonFormatterFactory trmrkJsonFormatterFactory;
        private readonly IStringTemplateParser stringTemplateParser;

        private volatile int bufferedLoggerDirNameIdx;
        private volatile int appProcessIdnfDumped;

        public AppLoggerCreator(
            IAppEnv appEnv,
            IAppLoggerConfig appLoggerConfig,
            IAppProcessIdentifier appProcessIdentifier,
            ITimeStampHelper timeStampHelper,
            ITrmrkJsonFormatterFactory trmrkJsonFormatterFactory,
            IStringTemplateParser stringTemplateParser,
            bool useAppProcessIdnfByDefault = false)
        {
            this.appEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));
            this.appLoggerConfig = appLoggerConfig ?? throw new ArgumentNullException(nameof(appLoggerConfig));
            this.appProcessIdentifier = appProcessIdentifier ?? throw new ArgumentNullException(nameof(appProcessIdentifier));
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            this.trmrkJsonFormatterFactory = trmrkJsonFormatterFactory ?? throw new ArgumentNullException(nameof(trmrkJsonFormatterFactory));
            this.stringTemplateParser = stringTemplateParser ?? throw new ArgumentNullException(nameof(stringTemplateParser));
            this.UseAppProcessIdnfByDefault = useAppProcessIdnfByDefault;

            MinLogLevel = appLoggerConfig.Data.MinLogLevel;

            if (useAppProcessIdnfByDefault)
            {
                AssureAppProcessIdnfDumped();
            }
        }

        public bool UseAppProcessIdnfByDefault { get; }

        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;

        public void AssureAppProcessIdnfDumped()
        {
            if (Interlocked.CompareExchange(ref appProcessIdnfDumped, 1, 0) == 0)
            {
                DumpAppProcessIdnf();
            }
        }

        public void DumpAppProcessIdnf()
        {
            string json = appProcessIdentifier.ToJson(false);

            string dumpDirPath = appEnv.GetTypePath(
                AppEnvDir.Data,
                AppProcessIdnfDumpDirType,
                appProcessIdentifier.ProcessDirName);

            Directory.CreateDirectory(dumpDirPath);

            string dumpFilePath = Path.Combine(
                dumpDirPath,
                AppProcessIdnfDumpFileName);

            File.WriteAllText(dumpFilePath, json);
        }

        public IAppLogger GetAppLogger(
            string loggerRelPath,
            LogLevel? logEventLevel = null,
            bool? useAppProcessIdnf = null)
        {
            var opts = new AppLoggerOpts.Mtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel ?? MinLogLevel,
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
            LogLevel? logEventLevel = null,
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
            LogLevel? logEventLevel = null,
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
                LogLevel = logEventLevel ?? MinLogLevel,
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
            LogLevel? logEventLevel = null,
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
            LogLevel? logEventLevel = null,
            bool? useAppProcessIdnf = null)
        {
            var opts = new AppLoggerOpts.Mtbl
            {
                AppEnv = appEnv,
                LogLevel = logEventLevel ?? MinLogLevel,
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
            LogLevel? logEventLevel = null,
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
