using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public readonly struct ShowUIMessageAlertArgs
    {
        public ShowUIMessageAlertArgs(
            ITrmrkActionComponentOptsCore opts,
            ITrmrkActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
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
        public TrmrkUnhandledErrorActionStepKind ActionStepKind { get; }
        public ITrmrkActionMessageTuple MsgTuple { get; }
        public LogLevel LogLevel { get; }
    }
}
