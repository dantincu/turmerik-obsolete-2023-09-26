using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionMessageTuple
    {
        string UIMessage { get; set; }
        string LogMessage { get; set; }
        LogLevel? LogLevel { get; set; }
        LogLevel? UILogLevel { get; set; }
    }

    public class TrmrkActionMessageTuple : ITrmrkActionMessageTuple
    {
        public string UIMessage { get; set; }
        public string LogMessage { get; set; }
        public LogLevel? LogLevel { get; set; }
        public LogLevel? UILogLevel { get; set; }
    }

    public struct LogMsgFactoryArgs
    {
        public LogMsgFactoryArgs(
            ITrmrkActionComponentOptsCore opts,
            ITrmrkActionResult result,
            Exception exc,
            TrmrkActionStepKind step)
        {
            Opts = opts;
            Result = result;
            Exc = exc;
            Step = step;
        }

        public ITrmrkActionComponentOptsCore Opts { get; set; }
        public ITrmrkActionResult Result { get; set; }
        public Exception Exc { get; set; }
        public TrmrkActionStepKind Step { get; set; }
    }
}
