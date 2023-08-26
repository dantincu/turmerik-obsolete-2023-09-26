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
        where TManager : class, ITrmrkActionComponentsManager
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
            ITrmrkActionComponentOptsCore<TActionResult, TActionResult, TMsgTuple> opts,
            TActionResult defaultSuccessActionResult,
            TActionResult defaultErrorActionResult)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            var actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeExecution;
            TActionResult actionResult = default;
            Exception excp = null;

            try
            {
                OnBeforeExecute(opts, defaultSuccessActionResult, ref actionStepKind);

                if (opts.Validation != null)
                {
                    OnBeforeValidation(opts, defaultSuccessActionResult, ref actionStepKind);
                    actionResult = opts.Validation();
                    OnAfterValidation(opts, actionResult.FirstNotNull(defaultErrorActionResult), ref actionStepKind);
                }

                if (opts.Validation == null || (actionResult?.IsSuccess ?? false))
                {
                    OnBeforeAction(opts, actionResult.FirstNotNull(defaultSuccessActionResult), ref actionStepKind);
                    actionResult = opts.Action();
                    OnAfterAction(opts, actionResult.FirstNotNull(defaultErrorActionResult), ref actionStepKind);
                }

                ExecuteAlwaysCallbackIfReq(opts, actionResult.FirstNotNull(defaultErrorActionResult), ref actionStepKind);
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult.FirstNotNull(defaultErrorActionResult), actionStepKind, exc);
            }

            ExecuteFinalCallbackIfReq(opts, actionResult.FirstNotNull(defaultErrorActionResult), actionStepKind, excp);
            return actionResult;
        }

        protected async Task<TActionResult> ExecuteCoreAsync<TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<Task<TActionResult>, TActionResult, TMsgTuple> opts,
            TActionResult defaultSuccessActionResult,
            TActionResult defaultErrorActionResult)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            var actionStepKind = TrmrkUnhandledErrorActionStepKind.BeforeExecution;
            TActionResult actionResult = default;
            Exception excp = null;

            try
            {
                OnBeforeExecute(opts, defaultSuccessActionResult, ref actionStepKind);

                if (opts.Validation != null)
                {
                    OnBeforeValidation(opts, defaultSuccessActionResult, ref actionStepKind);
                    actionResult = await opts.Validation();
                    OnAfterValidation(opts, actionResult.FirstNotNull(defaultErrorActionResult), ref actionStepKind);
                }

                if (opts.Validation == null || (actionResult?.IsSuccess ?? false))
                {
                    OnBeforeAction(opts, actionResult.FirstNotNull(defaultSuccessActionResult), ref actionStepKind);
                    actionResult = await opts.Action();
                    OnAfterAction(opts, actionResult.FirstNotNull(defaultErrorActionResult), ref actionStepKind);
                }

                ExecuteAlwaysCallbackIfReq(opts, actionResult.FirstNotNull(defaultErrorActionResult), ref actionStepKind);
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult.FirstNotNull(defaultErrorActionResult), actionStepKind, exc);
            }

            ExecuteFinalCallbackIfReq(opts, actionResult.FirstNotNull(defaultErrorActionResult), actionStepKind, excp);
            return actionResult;
        }

        protected virtual void ShowUIMessageAlert(
            ShowUIMessageAlertArgs args)
        {
            bool useUIBlockingMessagePopup = args.MsgTuple.UseUIBlockingMessagePopups != false && args.Opts.EnableUIBlockingMessagePopups != false && Manager.EnableUIBlockingMessagePopups;

            Manager.ShowUIMessageAlert(
                args,
                useUIBlockingMessagePopup);
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

            if (actionResult?.IsSuccess ?? false)
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

        protected virtual string GetErrorMessage(
            ITrmrkActionResult actionResult,
            Exception exc)
        {
            var msgParts = new List<string>();

            if (actionResult != null)
            {
                msgParts.AddRange(
                    actionResult.ResponseCaption,
                    actionResult.ResponseMessage,
                    actionResult.Exception?.Message);
            }

            msgParts.Add(exc?.Message);
            var msgPartsArr = msgParts.NotNull().ToArray();

            string msg = null;

            if (msgPartsArr.Any())
            {
                msg = string.Join(": ", msgPartsArr);
            }

            return msg;
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
                opts.ValidationErrorCallback?.Invoke(actionResult);
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
                opts.ActionErrorCallback?.Invoke(actionResult);
            }
            else
            {
                opts.SuccessCallback?.Invoke(actionResult);
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
                LogMessageIfReqCore(
                    opts,
                    actionResult,
                    exc,
                    actionStepKind,
                    msgTuple);
            }
        }

        private void LogMessageIfReqCore<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            ITrmrkActionMessageTuple msgTuple)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            var excp = exc ?? actionResult?.Exception;

            var logLevel = GetLogLevel(
                opts,
                actionResult,
                excp,
                msgTuple);

            LogMessageIfReqCore(excp, msgTuple, logLevel);

            ShowUIMessageAlertIfReq(
                opts,
                actionResult,
                excp,
                actionStepKind,
                msgTuple,
                logLevel);
        }

        private void LogMessageIfReqCore(
            Exception excp,
            ITrmrkActionMessageTuple msgTuple,
            LogLevel logLevel)
        {
            if (Logger != null)
            {
                string message = string.Join(
                    ": ",
                    msgTuple.Caption.Arr(
                        msgTuple.Message).NotNull(
                        ).ToArray());

                Logger.Write(
                    logLevel,
                    excp,
                    message);
            }
        }

        private void ShowUIMessageAlertIfReq<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            Exception excp,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            ITrmrkActionMessageTuple msgTuple,
            LogLevel logLevel)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            if (msgTuple.UseUIBlockingMessagePopups.HasValue || excp != null || (actionResult?.HasError ?? false))
            {
                ShowUIMessageAlert(
                    new ShowUIMessageAlertArgs(
                        opts,
                        actionResult,
                        excp,
                        actionStepKind,
                        msgTuple,
                        logLevel));
            }
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
                if (exc == null && (actionResult?.IsSuccess ?? false))
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

