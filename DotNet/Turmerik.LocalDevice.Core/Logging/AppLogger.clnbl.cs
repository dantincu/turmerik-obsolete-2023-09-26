using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.LocalDevice.Core.Env;

namespace Turmerik.LocalDevice.Core.Logging
{
    public interface IAppLoggerOpts
    {
        string LoggerRelPath { get; }
        IAppEnv AppEnv { get; }
        LogLevel LogLevel { get; }
        bool IsLoggerBuffered { get; }
        bool IsLoggerShared { get; }
    }

    public class AppLoggerOptsImmtbl : IAppLoggerOpts
    {
        public AppLoggerOptsImmtbl(IAppLoggerOpts src)
        {
            LoggerRelPath = src.LoggerRelPath;
            AppEnv = src.AppEnv;
            LogLevel = src.LogLevel;
            IsLoggerBuffered = src.IsLoggerBuffered;
            IsLoggerShared = src.IsLoggerShared;
        }

        public string LoggerRelPath { get; }
        public IAppEnv AppEnv { get; }
        public LogLevel LogLevel { get; }
        public bool IsLoggerBuffered { get; }
        public bool IsLoggerShared { get; }
    }

    public class AppLoggerOptsMtbl : IAppLoggerOpts
    {
        public AppLoggerOptsMtbl()
        {
        }

        public AppLoggerOptsMtbl(IAppLoggerOpts src)
        {
            LoggerRelPath = src.LoggerRelPath;
            AppEnv = src.AppEnv;
            LogLevel = src.LogLevel;
            IsLoggerBuffered = src.IsLoggerBuffered;
            IsLoggerShared = src.IsLoggerShared;
        }

        public string LoggerRelPath { get; set; }
        public IAppEnv AppEnv { get; set; }
        public LogLevel LogLevel { get; set; }
        public bool IsLoggerBuffered { get; set; }
        public bool IsLoggerShared { get; set; }
    }
}
