using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                UseUIBlockingMessagePopups = showBlockingUIMessage,
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
    }
}
