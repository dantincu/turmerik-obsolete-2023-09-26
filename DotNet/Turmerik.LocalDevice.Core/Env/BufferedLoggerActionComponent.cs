using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Logging;
using Turmerik.LocalDevice.Core.Logging;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IBufferedLoggerActionComponent
    {
        void ExecuteAction(
            IAppLogger parentLogger,
            string actionName,
            Action<IAppLogger> action,
            bool rethrowError = true,
            LogLevel logLevel = LogLevel.Debug);

        Task ExecuteActionAsync(
            IAppLogger parentLogger,
            string actionName,
            Func<IAppLogger, Task> action,
            bool rethrowError = true,
            LogLevel logLevel = LogLevel.Debug);

        void ExecuteActionSync(
            IAppLogger parentLogger,
            string actionName,
            Func<IAppLogger, Task> action,
            bool rethrowError = true,
            LogLevel logLevel = LogLevel.Debug);
    }

    public class BufferedLoggerActionComponent : IBufferedLoggerActionComponent
    {
        private readonly IAppLoggerFactory appLoggerFactory;

        public BufferedLoggerActionComponent(IAppLoggerFactory appLoggerFactory)
        {
            this.appLoggerFactory = appLoggerFactory ?? throw new ArgumentNullException(nameof(appLoggerFactory));
        }

        public void ExecuteAction(
            IAppLogger parentLogger,
            string actionName,
            Action<IAppLogger> action,
            bool rethrowError = true,
            LogLevel logLevel = LogLevel.Debug)
        {
            int bufferedLoggerDirNameIdx;

            using (var bufferedLogger = appLoggerFactory.GetBufferedAppLogger(
                parentLogger.LoggerRelPath,
                out bufferedLoggerDirNameIdx,
                logLevel))
            {
                try
                {
                    parentLogger.Information("Starting execution of action [{0}] with buffered logger [{1}]", actionName, bufferedLoggerDirNameIdx);
                    action(bufferedLogger);
                    parentLogger.Information("Execution of action [{0}] with buffered logger [{1}] completed", actionName, bufferedLoggerDirNameIdx);
                }
                catch (Exception exc)
                {
                    parentLogger.Error(exc, "Execution of action [{0}] with buffered logger [{1}] crashed", actionName, bufferedLoggerDirNameIdx);

                    if (rethrowError)
                    {
                        throw;
                    }
                }
            }
        }

        public async Task ExecuteActionAsync(
            IAppLogger parentLogger,
            string actionName,
            Func<IAppLogger, Task> action,
            bool rethrowError = true,
            LogLevel logLevel = LogLevel.Debug)
        {
            int bufferedLoggerDirNameIdx;

            using (var bufferedLogger = appLoggerFactory.GetBufferedAppLogger(
                parentLogger.LoggerRelPath,
                out bufferedLoggerDirNameIdx,
                logLevel))
            {
                try
                {
                    parentLogger.Information("Starting execution of action [{0}] with buffered logger [{1}]", actionName, bufferedLoggerDirNameIdx);
                    await action(bufferedLogger);
                    parentLogger.Information("Execution of action [{0}] with buffered logger [{1}] completed", actionName, bufferedLoggerDirNameIdx);
                }
                catch (Exception exc)
                {
                    parentLogger.Error(exc, "Execution of action [{0}] with buffered logger [{1}] crashed", actionName, bufferedLoggerDirNameIdx);

                    if (rethrowError)
                    {
                        throw;
                    }
                }
            }
        }

        public void ExecuteActionSync(
            IAppLogger parentLogger,
            string actionName,
            Func<IAppLogger, Task> action,
            bool rethrowError = true,
            LogLevel logLevel = LogLevel.Debug)
        {
            ExecuteAction(
                parentLogger,
                actionName,
                (bufferedLogger) =>
                {
                    Task.WaitAll(action(bufferedLogger));
                },
                rethrowError,
                logLevel);
        }
    }
}
