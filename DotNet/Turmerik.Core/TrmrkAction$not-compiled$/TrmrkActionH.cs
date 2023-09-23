using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Turmerik.Collections;
using Turmerik.Utils;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Turmerik.TrmrkAction
{
    public static class TrmrkActionH
    {
        public static readonly Type TrmrkActionResultGenericTypeDef = typeof(
            TrmrkActionResult<object>).GetGenericTypeDefinition();

        public static TOpts LogMsgFactory<TOpts, TResult, TActionResult>(
            TOpts options,
            Action<Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>>> mapBuilder)
            where TOpts : TrmrkActionComponentOptsCore<TResult, TActionResult>
            where TActionResult : ITrmrkActionResult
        {
            var map = new Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>>().ActWithValue(mapBuilder);

            options.LogMessageFactory = args =>
            {
                ITrmrkActionMessageTuple msgTuple = null;

                if (map.TryGetValue(args.Step, out var callback))
                {
                    msgTuple = callback?.Invoke(args);
                }

                return msgTuple;
            };

            return options;
        }

        public static TrmrkActionComponentOpts LogMsgFactory(
            this TrmrkActionComponentOpts opts,
            Action<Dictionary<
                TrmrkActionStepKind,
                Func<LogMsgFactoryArgs,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkActionComponentOpts,
                        ITrmrkActionResult,
                        ITrmrkActionResult>(opts, mapBuilder);

        public static TrmrkActionComponentOpts<TData> LogMsgFactory<TData>(
            this TrmrkActionComponentOpts<TData> opts,
            Action<Dictionary<
                TrmrkActionStepKind,
                Func<LogMsgFactoryArgs,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkActionComponentOpts<TData>,
                        ITrmrkActionResult<TData>,
                        ITrmrkActionResult<TData>>(opts, mapBuilder);

        public static TrmrkAsyncActionComponentOpts LogMsgFactory(
            this TrmrkAsyncActionComponentOpts opts,
            Action<Dictionary<
                TrmrkActionStepKind,
                Func<LogMsgFactoryArgs,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkAsyncActionComponentOpts,
                        Task<ITrmrkActionResult>,
                        ITrmrkActionResult>(opts, mapBuilder);

        public static TrmrkAsyncActionComponentOpts<TData> LogMsgFactory<TData>(
            this TrmrkAsyncActionComponentOpts<TData> opts,
            Action<Dictionary<
                TrmrkActionStepKind,
                Func<LogMsgFactoryArgs,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkAsyncActionComponentOpts<TData>,
                        Task<ITrmrkActionResult<TData>>,
                        ITrmrkActionResult<TData>>(opts, mapBuilder);

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeExecution(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.BeforeExecution] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeExecution(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            string message, MsLogLevel? logLevel = MsLogLevel.Information) => map.AddBeforeExecution(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = message,
                    UILogLevel = logLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeValidation(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.BeforeValidation] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeValidation(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            string message, MsLogLevel? logLevel = MsLogLevel.Information) => map.AddBeforeValidation(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = message,
                    UILogLevel = logLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddValidation(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.Validation] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddValidation(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddValidation(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAfterValidation(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.AfterValidation] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAfterValidation(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddAfterValidation(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeAction(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.BeforeAction] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeAction(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddBeforeAction(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAction(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.Action] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAction(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddAction(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAfterAction(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.AfterAction] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAfterAction(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddAfterAction(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeAlways(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.BeforeAlways] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddBeforeAlways(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddBeforeAlways(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAlways(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.Always] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAlways(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddAlways(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAfterAlways(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkActionStepKind.AfterAlways] = callback;
            return map;
        }

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddAfterAlways(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            Func<LogMsgFactoryArgs, string> errorMsgFactory,
            Func<LogMsgFactoryArgs, string> successMsgFactory = null,
            MsLogLevel? successLogLevel = MsLogLevel.Information,
            MsLogLevel? errorLogLevel = MsLogLevel.Error) => map.AddAfterAlways(
                args => (args.Result?.IsSuccess ?? false).WithValue(
                isSuccess => new TrmrkActionMessageTuple
                {
                    UIMessage = isSuccess switch
                    {
                        false => errorMsgFactory?.Invoke(args),
                        true => successMsgFactory?.Invoke(args),
                    },
                    UILogLevel = isSuccess ? successLogLevel : errorLogLevel
                }));

        public static Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> AddFromActionResult(
            this Dictionary<TrmrkActionStepKind, Func<LogMsgFactoryArgs, ITrmrkActionMessageTuple>> map,
            string logMsg = null,
            LogLevel? logLevel = null,
            TrmrkActionStepKind? step = null,
            Action<TrmrkActionMessageTuple> callback = null)
        {
            map[step ?? TrmrkActionStepKind.AfterAction] = args => new TrmrkActionMessageTuple
            {
                UIMessage = args.Result?.ResponseMessage,
                LogMessage = logMsg,
                LogLevel = logLevel ?? LogLevel.Information,
            }.ActWithValue(callback);

            return map;
        }
    }
}
