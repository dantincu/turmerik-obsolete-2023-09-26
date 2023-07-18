using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Logging
{
    public class SerializedLogEventCore<TLogLevel>
    {
        public DateTimeOffset TimeStamp { get; set; }
        public TLogLevel Level { get; set; }
        public string Message { get; set; }
        public object[] Properties { get; set; }
        public Exception Exception { get; set; }
        public object Data { get; set; }
    }

    public class SerializedLogEventCore : SerializedLogEventCore<LogLevel>
    {
    }
}
