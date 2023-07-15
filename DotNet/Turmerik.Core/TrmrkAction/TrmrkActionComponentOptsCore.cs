using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionComponentOptsCore
    {
        string ActionName { get; set; }
        Action BeforeExecute { get; set; }
        bool EnableUIBlockingMessagePopups { get; set; }
        LogLevel? LogLevel { get; set; }
        LogLevel? ErrorLogLevel { get; set; }
    }

    public interface ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> : ITrmrkActionComponentOptsCore
        where TActionResult : ITrmrkActionResult
        where TMsgTuple : ITrmrkActionMessageTuple
    {
        Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, TMsgTuple> LogMessageFactory { get; set; }

        Func<TResult> Validation { get; set; }
        Func<TResult> Action { get; set; }
        Action<TActionResult> SuccessCallback { get; set; }
        Action<TActionResult> ActionErrorCallback { get; set; }
        Action<TActionResult> ValidationErrorCallback { get; set; }
        Action<TActionResult> AlwaysCallback { get; set; }
        Action<TActionResult, Exception, TrmrkUnhandledErrorActionStepKind> UnhandledErrorCallback { get; set; }
        Action<TActionResult, Exception, TrmrkUnhandledErrorActionStepKind> FinalCallback { get; set; }
    }

    public interface ITrmrkActionComponentOpts : ITrmrkActionComponentOptsCore<ITrmrkActionResult, ITrmrkActionResult, ITrmrkActionMessageTuple>
    {
    }

    public interface ITrmrkActionComponentOpts<TData> : ITrmrkActionComponentOptsCore<ITrmrkActionResult<TData>, ITrmrkActionResult<TData>, ITrmrkActionMessageTuple>
    {
    }

    public interface ITrmrkAsyncActionComponentOpts : ITrmrkActionComponentOptsCore<Task<ITrmrkActionResult>, ITrmrkActionResult, ITrmrkActionMessageTuple>
    {
    }

    public interface ITrmrkAsyncActionComponentOpts<TData> : ITrmrkActionComponentOptsCore<Task<ITrmrkActionResult<TData>>, ITrmrkActionResult<TData>, ITrmrkActionMessageTuple>
    {
    }

    public class TrmrkActionComponentOptsCore<TResult, TActionResult> : ITrmrkActionComponentOptsCore<TResult, TActionResult, ITrmrkActionMessageTuple>
        where TActionResult : ITrmrkActionResult
    {
        public string ActionName { get; set; }
        public Action BeforeExecute { get; set; }
        public bool EnableUIBlockingMessagePopups { get; set; }
        public LogLevel? LogLevel { get; set; }
        public LogLevel? ErrorLogLevel { get; set; }

        public Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> LogMessageFactory { get; set; }

        public Func<TResult> Validation { get; set; }
        public Func<TResult> Action { get; set; }
        public Action<TActionResult> SuccessCallback { get; set; }
        public Action<TActionResult> ActionErrorCallback { get; set; }
        public Action<TActionResult> ValidationErrorCallback { get; set; }
        public Action<TActionResult> AlwaysCallback { get; set; }
        public Action<TActionResult, Exception, TrmrkUnhandledErrorActionStepKind> UnhandledErrorCallback { get; set; }
        public Action<TActionResult, Exception, TrmrkUnhandledErrorActionStepKind> FinalCallback { get; set; }
    }

    public class TrmrkActionComponentOpts : TrmrkActionComponentOptsCore<ITrmrkActionResult, ITrmrkActionResult>, ITrmrkActionComponentOpts
    {
    }

    public class TrmrkActionComponentOpts<TData> : TrmrkActionComponentOptsCore<ITrmrkActionResult<TData>, ITrmrkActionResult<TData>>, ITrmrkActionComponentOpts<TData>
    {
    }

    public class TrmrkAsyncActionComponentOpts : TrmrkActionComponentOptsCore<Task<ITrmrkActionResult>, ITrmrkActionResult>, ITrmrkAsyncActionComponentOpts
    {
    }

    public class TrmrkAsyncActionComponentOpts<TData> : TrmrkActionComponentOptsCore<Task<ITrmrkActionResult<TData>>, ITrmrkActionResult<TData>>, ITrmrkAsyncActionComponentOpts<TData>
    {
    }
}
