using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public class TrmrkActionComponentCore<TManager>
        where TManager : class, ITrmrkActionComponentsManagerCore
    {
        protected const string DEFAULT_LOG_MESSAGE_TEMPLATE = "{0} for action {1}";
        protected const string DEFAULT_ERROR_LOG_MESSAGE_TEMPLATE = "Error at {0} for action {1}: {2}";
        protected const string DEFAULT_UNHANDLED_ERROR_LOG_MESSAGE_TEMPLATE = "Unhandled error at {0} for action {1}: {2}";

        public TrmrkActionComponentCore(
            TManager manager,
            IAppLogger logger)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            Logger = logger;
        }

        protected TManager Manager { get; }
        protected IAppLogger Logger { get; }

        protected virtual string DefaultLogMessageTemplate => DEFAULT_LOG_MESSAGE_TEMPLATE;
        protected virtual string DefaultErrorLogMessageTemplate => DEFAULT_ERROR_LOG_MESSAGE_TEMPLATE;
        protected virtual string DefaultUnhandledErrorLogMessageTemplate => DEFAULT_UNHANDLED_ERROR_LOG_MESSAGE_TEMPLATE;

        protected TActionResult ExecuteCore<TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TActionResult, TActionResult, TMsgTuple> opts)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            TActionResult actionResult = default;
            var actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeExecution;
            Exception excp = null;

            try
            {
                OnBeforeExecute(opts, actionResult, ref actionStepKind);

                if (opts.Validation != null)
                {
                    OnBeforeValidation(opts, actionResult, ref actionStepKind);
                    actionResult = opts.Validation();
                    OnAfterValidation(opts, actionResult, ref actionStepKind);
                }

                if (actionResult?.IsSuccess ?? true)
                {
                    OnBeforeAction(opts, actionResult, ref actionStepKind);
                    actionResult = opts.Action();
                    OnAfterAction(opts, actionResult, ref actionStepKind);
                }

                ExecuteAlwaysCallbackIfReq(opts, actionResult, ref actionStepKind);
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult, actionStepKind, exc);
            }

            ExecuteFinalCallbackIfReq(opts, actionResult, actionStepKind, excp);
            return actionResult;
        }

        protected async Task<TActionResult> ExecuteCoreAsync<TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<Task<TActionResult>, TActionResult, TMsgTuple> opts)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            TActionResult actionResult = default;
            var actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeExecution;
            Exception excp = null;

            try
            {
                OnBeforeExecute(opts, actionResult, ref actionStepKind);

                if (opts.Validation != null)
                {
                    OnBeforeValidation(opts, actionResult, ref actionStepKind);
                    actionResult = await opts.Validation();
                    OnAfterValidation(opts, actionResult, ref actionStepKind);
                }

                if (actionResult?.IsSuccess ?? true)
                {
                    OnBeforeAction(opts, actionResult, ref actionStepKind);
                    actionResult = await opts.Action();
                    OnAfterAction(opts, actionResult, ref actionStepKind);
                }

                ExecuteAlwaysCallbackIfReq(opts, actionResult, ref actionStepKind);
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult, actionStepKind, exc);
            }

            ExecuteFinalCallbackIfReq(opts, actionResult, actionStepKind, excp);
            return actionResult;
        }

        protected virtual void ShowUIMessageAlert(
            ITrmrkActionComponentOptsCore opts,
            ITrmrkActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            ITrmrkActionMessageTuple msgTuple)
        {
            if (msgTuple.ShowBlockingUIMessage.HasValue || (!opts.SuppressUIMessageAlerts && !Manager.SuppressUIMessageAlerts))
            {
                ShowUIMessageAlertCore(
                    opts,
                    actionResult,
                    exc,
                    actionStepKind,
                    msgTuple);
            }
        }

        protected virtual void ShowUIMessageAlertCore(
            ITrmrkActionComponentOptsCore opts,
            ITrmrkActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            ITrmrkActionMessageTuple msgTuple)
        {
        }

        protected virtual ITrmrkActionMessageTuple GetDefaultLogMessage<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            string message;
            LogLevel logLevel;

            if (actionResult.IsSuccess)
            {
                message = string.Format(
                    DefaultLogMessageTemplate,
                    actionStepKind,
                    opts.ActionName);
            }
            else if (exc != null)
            {
                message = string.Format(
                    DefaultUnhandledErrorLogMessageTemplate,
                    actionStepKind,
                    opts.ActionName,
                    GetErrorMessage(actionResult, exc));
            }
            else
            {
                message = string.Format(
                    DefaultErrorLogMessageTemplate,
                    actionStepKind,
                    opts.ActionName,
                    GetErrorMessage(actionResult, exc));
            }

            return new TrmrkActionMessageTuple
            {
                Message = message,
            };
        }

        private void OnBeforeExecute<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            LogMessageIfReq(opts, actionResult, null, actionStepKind);
            opts.BeforeExecute?.Invoke();
        }

        private void OnBeforeValidation<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeValidation;
            LogMessageIfReq(opts, actionResult, null, actionStepKind);
        }

        private void OnAfterValidation<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkUnhandledErrorActionStepKind.AfterValidation;

            if (actionResult.HasError)
            {
                opts.ValidationErrorCallback(actionResult);
            }

            LogMessageIfReq(opts, actionResult, null, actionStepKind);
        }

        private void OnBeforeAction<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeAction;
            LogMessageIfReq(opts, actionResult, null, actionStepKind);
        }

        private void OnAfterAction<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkUnhandledErrorActionStepKind.AfterAction;

            if (actionResult.HasError)
            {
                opts.ActionErrorCallback(actionResult);
            }
            else
            {
                opts.SuccessCallback(actionResult);
            }

            LogMessageIfReq(opts, actionResult, null, actionStepKind);
        }

        private void ExecuteAlwaysCallbackIfReq<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            if (opts.AlwaysCallback != null)
            {
                actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeAlwaysCallback;
                LogMessageIfReq(opts, actionResult, null, actionStepKind);

                opts.AlwaysCallback.Invoke(actionResult);

                actionStepKind = TrmrkUnhandledErrorActionStepKind.AfterAlwaysCallback;
                LogMessageIfReq(opts, actionResult, null, actionStepKind);
            }
        }

        private void OnUnhandledError<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            Exception exc)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            if (opts.UnhandledErrorCallback != null)
            {
                opts.UnhandledErrorCallback(actionResult, exc, actionStepKind);
            }

            LogMessageIfReq(opts, actionResult, exc, actionStepKind);
        }

        private void ExecuteFinalCallbackIfReq<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            Exception exc)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            if (opts.FinalCallback != null)
            {
                opts.FinalCallback(actionResult, exc, actionStepKind);
            }
        }

        private void LogMessageIfReq<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            ITrmrkActionMessageTuple msgTuple;

            if (opts.LogMessageFactory != null)
            {
                msgTuple = opts.LogMessageFactory(
                    opts,
                    actionResult,
                    exc,
                    actionStepKind);
            }
            else
            {
                msgTuple = GetDefaultLogMessage(
                    opts,
                    actionResult,
                    exc,
                    actionStepKind);
            }

            if (msgTuple != null)
            {
                var excp = exc ?? actionResult?.Error?.Exception;

                if (Logger != null)
                {
                    var logLevel = GetLogLevel(
                        opts,
                        actionResult,
                        exc,
                        msgTuple);

                    Logger.Write(
                        logLevel,
                        excp,
                        msgTuple.Message);
                }

                if (msgTuple.ShowBlockingUIMessage.HasValue || excp != null || (actionResult?.HasError ?? false))
                {
                    ShowUIMessageAlert(
                        opts,
                        actionResult,
                        excp,
                        actionStepKind,
                        msgTuple);
                }
            }
        }

        private string GetErrorMessage(
            ITrmrkActionResult actionResult,
            Exception exc)
        {
            string errMsg = null;
            var error = actionResult?.Error;

            if (error != null)
            {
                errMsg = error.Message ?? error.Exception?.Message;
            }

            errMsg = errMsg ?? exc?.Message;
            return errMsg;
        }

        private LogLevel GetLogLevel<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            ITrmrkActionResult actionResult,
            Exception exc,
            ITrmrkActionMessageTuple msgTuple)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            LogLevel logLevel;

            if (msgTuple.LogLevel.HasValue)
            {
                logLevel = msgTuple.LogLevel.Value;
            }
            else
            {
                if (exc == null && (actionResult?.IsSuccess ?? true))
                {
                    logLevel = opts.LogLevel ?? Manager.DefaultLogLevel;
                }
                else
                {
                    logLevel = opts.ErrorLogLevel ?? Manager.DefaultErrorLogLevel;
                }
            }

            return logLevel;
        }
    }
}

