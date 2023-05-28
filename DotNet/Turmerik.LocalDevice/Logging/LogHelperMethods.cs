using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Logging;

namespace Turmerik.LocalDevice.Logging
{
    public static class LogHelperMethods
    {
        public static LogEventLevel GetLogLevel(this LogLevel logLevel)
        {
            LogEventLevel retVal = (LogEventLevel)((int)logLevel);
            return retVal;
        }

        public static IAppLogger GetAppLogger(
            this IServiceProvider serviceProvider,
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information)
        {
            var appLoggerFactory = serviceProvider.GetRequiredService<IAppLoggerFactory>();
            var appLogger = appLoggerFactory.GetAppLogger(loggerNameType, logEventLevel);

            return appLogger;
        }

        public static IAppLogger GetSharedAppLogger(
            this IServiceProvider serviceProvider,
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information)
        {
            var appLoggerFactory = serviceProvider.GetRequiredService<IAppLoggerFactory>();
            var appLogger = appLoggerFactory.GetSharedAppLogger(loggerNameType, logEventLevel);

            return appLogger;
        }
    }
}
