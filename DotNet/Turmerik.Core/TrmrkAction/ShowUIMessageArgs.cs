using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public readonly struct ShowUIMessageArgs
    {
        public ShowUIMessageArgs(
            ITrmrkActionComponentOptsCore opts,
            ITrmrkActionResult actionResult,
            Exception exc,
            TrmrkActionStepKind actionStepKind,
            ITrmrkActionMessageTuple msgTuple,
            LogLevel logLevel)
        {
            Opts = opts;
            ActionResult = actionResult;
            Exc = exc;
            ActionStepKind = actionStepKind;
            MsgTuple = msgTuple;
            LogLevel = logLevel;
        }

        public ITrmrkActionComponentOptsCore Opts { get; }
        public ITrmrkActionResult ActionResult { get; }
        public Exception Exc { get; }
        public TrmrkActionStepKind ActionStepKind { get; }
        public ITrmrkActionMessageTuple MsgTuple { get; }
        public LogLevel LogLevel { get; }
    }
}
