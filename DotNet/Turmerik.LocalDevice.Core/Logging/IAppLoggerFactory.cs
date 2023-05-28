using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Core.Logging;

namespace Turmerik.LocalDevice.Core.Logging
{
    public interface IAppLoggerFactory
    {
        IAppLogger GetAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Information);

        IAppLogger GetAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information);

        IAppLogger GetSharedAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Information);

        IAppLogger GetSharedAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information);

        IAppLogger GetBufferedAppLogger(
            string loggerRelPath,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Debug);

        IAppLogger GetBufferedAppLogger(
            Type loggerNameType,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Debug);
    }
}
