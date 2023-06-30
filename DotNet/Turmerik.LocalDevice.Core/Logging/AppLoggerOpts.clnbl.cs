using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Infrastucture;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Logging
{
    public partial class AppLoggerOpts : ClnblCore<AppLoggerOpts.IClnbl, AppLoggerOpts.Immtbl, AppLoggerOpts.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            string LogDirRelPath { get; }
            IAppEnv AppEnv { get; }
            IAppProcessIdentifier AppProcessIdentifier { get; }
            ITextFormatter TextFormatter { get; }
            IStringTemplateParser StringTemplateParser { get; }
            LogLevel LogLevel { get; }
            bool IsLoggerBuffered { get; }
            bool IsLoggerShared { get; }
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                LogDirRelPath = src.LogDirRelPath;
                AppEnv = src.AppEnv;
                AppProcessIdentifier = src.AppProcessIdentifier;
                TextFormatter = src.TextFormatter;
                StringTemplateParser = src.StringTemplateParser;
                LogLevel = src.LogLevel;
                IsLoggerBuffered = src.IsLoggerBuffered;
                IsLoggerShared = src.IsLoggerShared;
            }

            public string LogDirRelPath { get; }
            public IAppEnv AppEnv { get; }
            public IAppProcessIdentifier AppProcessIdentifier { get; }
            public ITextFormatter TextFormatter { get; }
            public IStringTemplateParser StringTemplateParser { get; }
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
                LogDirRelPath = src.LogDirRelPath;
                AppEnv = src.AppEnv;
                AppProcessIdentifier = src.AppProcessIdentifier;
                TextFormatter = src.TextFormatter;
                StringTemplateParser = src.StringTemplateParser;
                LogLevel = src.LogLevel;
                IsLoggerBuffered = src.IsLoggerBuffered;
                IsLoggerShared = src.IsLoggerShared;
            }

            public string LogDirRelPath { get; set; }
            public IAppEnv AppEnv { get; set; }
            public IAppProcessIdentifier AppProcessIdentifier { get; set; }
            public ITextFormatter TextFormatter { get; set; }
            public IStringTemplateParser StringTemplateParser { get; set; }
            public LogLevel LogLevel { get; set; }
            public bool IsLoggerBuffered { get; set; }
            public bool IsLoggerShared { get; set; }
        }
    }
}
