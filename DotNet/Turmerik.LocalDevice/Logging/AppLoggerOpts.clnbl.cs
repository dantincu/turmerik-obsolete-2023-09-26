using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.LocalDevice.Env;

namespace Turmerik.LocalDevice.Logging
{
    public partial class AppLoggerOpts : ClnblCore<AppLoggerOpts.IClnbl, AppLoggerOpts.Immtbl, AppLoggerOpts.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            string LoggerRelPath { get; }
            IAppEnv AppEnv { get; }
            LogLevel LogLevel { get; }
            bool IsLoggerBuffered { get; }
            bool IsLoggerShared { get; }
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
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

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
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
}
