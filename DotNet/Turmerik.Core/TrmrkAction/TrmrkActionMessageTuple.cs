using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionMessageTuple
    {
        string Message { get; set; }
        string Caption { get; set; }
        string LogMessage { get; set; }
        bool? ShowUIMessage { get; set; }
        LogLevel? LogLevel { get; set; }
    }

    public class TrmrkActionMessageTuple : ITrmrkActionMessageTuple
    {
        public string Message { get; set; }
        public string Caption { get; set; }
        public string LogMessage { get; set; }
        public bool? ShowUIMessage { get; set; }
        public LogLevel? LogLevel { get; set; }
    }
}
