using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public static class TrmrkActionH
    {
        public static readonly Type TrmrkActionResultGenericTypeDef = typeof(
            TrmrkActionResult<object>).GetGenericTypeDefinition();

        public static TrmrkActionMessageTuple TrmrkMsgTuple(
            this string message,
            string caption = null,
            bool? showBlockingUIMessage = null,
            LogLevel? logLevel = null) => new TrmrkActionMessageTuple
            {
                Message = message,
                Caption = caption,
                ShowUIMessage = showBlockingUIMessage,
                LogLevel = logLevel
            };

        public static ITrmrkActionComponentOpts TrmrkActionOpts(
            this string actionName,
            Func<ITrmrkActionResult> action,
            Action beforeExecute = null,
            Action<ITrmrkActionResult> alwaysCallback = null) => new TrmrkActionComponentOpts
            {
                ActionName = actionName,
                Action = action,
                BeforeExecute = beforeExecute,
                AlwaysCallback = alwaysCallback
            };

        public static ITrmrkActionComponentOpts<TData> TrmrkActionOpts<TData>(
            this string actionName,
            Func<ITrmrkActionResult<TData>> action,
            Action beforeExecute = null,
            Action<ITrmrkActionResult<TData>> alwaysCallback = null) => new TrmrkActionComponentOpts<TData>
            {
                ActionName = actionName,
                Action = action,
                BeforeExecute = beforeExecute,
                AlwaysCallback = alwaysCallback
            };

        public static ITrmrkAsyncActionComponentOpts TrmrkAsyncActionOpts(
            this string actionName,
            Func<Task<ITrmrkActionResult>> action,
            Action beforeExecute = null,
            Action<ITrmrkActionResult> alwaysCallback = null) => new TrmrkAsyncActionComponentOpts
            {
                ActionName = actionName,
                Action = action,
                BeforeExecute = beforeExecute,
                AlwaysCallback = alwaysCallback
            };

        public static ITrmrkAsyncActionComponentOpts<TData> TrmrkAsyncActionOpts<TData>(
            this string actionName,
            Func<Task<ITrmrkActionResult<TData>>> action,
            Action beforeExecute = null,
            Action<ITrmrkActionResult<TData>> alwaysCallback = null) => new TrmrkAsyncActionComponentOpts<TData>
            {
                ActionName = actionName,
                Action = action,
                BeforeExecute = beforeExecute,
                AlwaysCallback = alwaysCallback
            };

        public static TRetActRslt Then<TRetActRslt, TInActRslt>(
            this TInActRslt inActRslt,
            Func<TInActRslt, TRetActRslt> successCallback,
            Func<TInActRslt, TRetActRslt> errorCallback = null)
            where TRetActRslt : ITrmrkActionResult
            where TInActRslt : ITrmrkActionResult
        {
            TRetActRslt retActRslt;

            if (inActRslt.IsSuccess)
            {
                retActRslt = successCallback(inActRslt);
            }
            else if (errorCallback != null)
            {
                retActRslt = errorCallback(inActRslt);
            }
            else
            {
                var retActRsltType = typeof(TRetActRslt);

                if (retActRsltType.IsGenericType)
                {
                    retActRsltType = TrmrkActionResultGenericTypeDef.MakeGenericType(
                        retActRsltType.GetGenericArguments());
                }
                else
                {
                    retActRsltType = typeof(TrmrkActionResult);
                }

                retActRslt = retActRsltType.CreateInstance<TRetActRslt>();

                retActRslt.HasError = true;
                retActRslt.Exception = inActRslt.Exception;

                retActRslt.ResponseMessage = inActRslt.ResponseMessage;
                retActRslt.ResponseCaption = inActRslt.ResponseCaption;
            }

            return retActRslt;
        }

        public static TOpts LogMsgFactory<TOpts, TResult, TActionResult>(
            TOpts options,
            Action<Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>>> mapBuilder)
            where TOpts : TrmrkActionComponentOptsCore<TResult, TActionResult>
            where TActionResult : ITrmrkActionResult
        {
            var map = new Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>>().ActWithValue(mapBuilder);

            options.LogMessageFactory = (opts, result, excp, step) =>
            {
                ITrmrkActionMessageTuple msgTuple = null;

                if (map.TryGetValue(step, out var callback))
                {
                    msgTuple = callback?.Invoke(opts, result, excp, step);
                }

                return msgTuple;
            };

            return options;
        }

        public static TrmrkActionComponentOpts LogMsgFactory(
            this TrmrkActionComponentOpts opts,
            Action<Dictionary<
                TrmrkUnhandledErrorActionStepKind,
                Func<ITrmrkActionComponentOptsCore,
                    ITrmrkActionResult,
                    Exception,
                    TrmrkUnhandledErrorActionStepKind,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkActionComponentOpts,
                        ITrmrkActionResult,
                        ITrmrkActionResult>(opts, mapBuilder);

        public static TrmrkActionComponentOpts<TData> LogMsgFactory<TData>(
            this TrmrkActionComponentOpts<TData> opts,
            Action<Dictionary<
                TrmrkUnhandledErrorActionStepKind,
                Func<ITrmrkActionComponentOptsCore,
                    ITrmrkActionResult,
                    Exception,
                    TrmrkUnhandledErrorActionStepKind,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkActionComponentOpts<TData>,
                        ITrmrkActionResult<TData>,
                        ITrmrkActionResult<TData>>(opts, mapBuilder);

        public static TrmrkAsyncActionComponentOpts LogMsgFactory(
            this TrmrkAsyncActionComponentOpts opts,
            Action<Dictionary<
                TrmrkUnhandledErrorActionStepKind,
                Func<ITrmrkActionComponentOptsCore,
                    ITrmrkActionResult,
                    Exception,
                    TrmrkUnhandledErrorActionStepKind,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkAsyncActionComponentOpts,
                        Task<ITrmrkActionResult>,
                        ITrmrkActionResult>(opts, mapBuilder);

        public static TrmrkAsyncActionComponentOpts<TData> LogMsgFactory<TData>(
            this TrmrkAsyncActionComponentOpts<TData> opts,
            Action<Dictionary<
                TrmrkUnhandledErrorActionStepKind,
                Func<ITrmrkActionComponentOptsCore,
                    ITrmrkActionResult,
                    Exception,
                    TrmrkUnhandledErrorActionStepKind,
                    ITrmrkActionMessageTuple>>> mapBuilder) => LogMsgFactory<
                        TrmrkAsyncActionComponentOpts<TData>,
                        Task<ITrmrkActionResult<TData>>,
                        ITrmrkActionResult<TData>>(opts, mapBuilder);

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddBeforeExecution(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.BeforeExecution] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddBeforeValidation(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.BeforeValidation] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddAfterValidation(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.AfterValidation] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddBeforeAction(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.BeforeAction] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddAfterAction(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.AfterAction] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddBeforeAlwaysCallback(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.BeforeAlwaysCallback] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddAfterAlwaysCallback(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple> callback)
        {
            map[TrmrkUnhandledErrorActionStepKind.AfterAlwaysCallback] = callback;
            return map;
        }

        public static Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> AddFromActionResult(
            this Dictionary<TrmrkUnhandledErrorActionStepKind, Func<ITrmrkActionComponentOptsCore, ITrmrkActionResult, Exception, TrmrkUnhandledErrorActionStepKind, ITrmrkActionMessageTuple>> map,
            string logMsg = null,
            LogLevel? logLevel = null,
            TrmrkUnhandledErrorActionStepKind? step = null,
            Action<TrmrkActionMessageTuple> callback = null)
        {
            map[step ?? TrmrkUnhandledErrorActionStepKind.AfterAction] = (opts, result, excp, stp) => new TrmrkActionMessageTuple
            {
                Caption = result.ResponseCaption,
                Message = result.ResponseMessage,
                LogMessage = logMsg ?? string.Join(
                    ": ",
                    result.ResponseCaption.Arr(
                    result.ResponseMessage).NotNull().ToArray()),
                LogLevel = logLevel,
            }.ActWithValue(callback);

            return map;
        }
    }
}
