using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Logging
{
    public class SerializedLogEvent
    {
        public DateTimeOffset TimeStamp { get; set; }
        public LogEventLevel Level { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public Exception Exception { get; set; }
    }
}
