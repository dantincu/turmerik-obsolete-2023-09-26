using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Logging
{
    public interface IAppLoggerCreator
    {
        void AssureAppProcessIdnfDumped();
        void DumpAppProcessIdnf();

        IAppLogger GetAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null);

        IAppLogger GetAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null);

        IAppLogger GetSharedAppLogger(
            string loggerRelPath,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null);

        IAppLogger GetSharedAppLogger(
            Type loggerNameType,
            LogLevel logEventLevel = LogLevel.Information,
            bool? useAppProcessIdnf = null);

        IAppLogger GetBufferedAppLogger(
            string loggerRelPath,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Debug,
            bool? useAppProcessIdnf = null);

        IAppLogger GetBufferedAppLogger(
            Type loggerNameType,
            out int bufferedLoggerDirNameIdx,
            LogLevel logEventLevel = LogLevel.Debug,
            bool? useAppProcessIdnf = null);
    }
}
