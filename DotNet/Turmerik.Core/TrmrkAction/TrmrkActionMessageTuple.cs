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
        bool? UseUIBlockingMessagePopups { get; set; }
        LogLevel? LogLevel { get; set; }
    }

    public class TrmrkActionMessageTuple : ITrmrkActionMessageTuple
    {
        public string Message { get; set; }
        public string Caption { get; set; }
        public bool? UseUIBlockingMessagePopups { get; set; }
        public LogLevel? LogLevel { get; set; }
    }
}
