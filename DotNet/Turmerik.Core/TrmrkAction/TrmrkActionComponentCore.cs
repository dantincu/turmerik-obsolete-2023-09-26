using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Reflection;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public class TrmrkActionComponentCore<TManager>
        where TManager : class, ITrmrkActionComponentsManager
    {
        protected const string DEFAULT_LOG_MESSAGE_TEMPLATE = "{0} action {1}";
        protected const string DEFAULT_ERROR_LOG_MESSAGE_TEMPLATE = "Error {0} action {1}";
        protected const string DEFAULT_UNHANDLED_ERROR_LOG_MESSAGE_TEMPLATE = "Unhandled error {0} action {1}";

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

        protected virtual void ShowUIMessageIfReq(
            ShowUIMessageArgs args)
        {
            if (args.MsgTuple.ShowUIMessage != false && args.Opts.ShowUIMessage != false && Manager.EnableUIMessages)
            {
                bool showUIMessage = args.MsgTuple.ShowUIMessage == true && args.Opts.ShowUIMessage == true;

                Manager.ShowUIMessage(
                    args,
                    showUIMessage);
            }
        }

        protected virtual ITrmrkActionMessageTuple GetDefaultLogMessage<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            Exception exc,
            TrmrkUnhandledErrorActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            string adverb = GetActionStepStr(actionStepKind);
            string msgTemplate = GetMessageTemplate(actionResult, exc);

            string message = string.Format(
                msgTemplate,
                adverb,
                opts.ActionName);

            if (actionResult != null)
            {
                message = "; ".JoinNotNullStr(
                    message.Arr(
                        ": ".JoinNotNullStr(
                            actionResult.ResponseCaption.Arr(
                                actionResult.ResponseMessage))));
            }

            /* if (exc != null)
            {
                message = "; ".JoinNotNullStr(
                    message.Arr(
                        ": ".JoinNotNullStr(
                            exc.GetType().FullName.Arr(
                                exc.Message))));
            } */

            return new TrmrkActionMessageTuple
            {
                Message = message,
            };
        }

        private string GetErrorMessage(
            ITrmrkActionResult actionResult,
            Exception exc) => ": ".JoinNotNullStr(
                actionResult.ResponseCaption.Arr(
                    actionResult.ResponseMessage,
                    actionResult.Exception?.Message));

        private string GetActionStepStr(
            TrmrkUnhandledErrorActionStepKind actionStepKind)
        {
            string adverb;

            switch (actionStepKind)
            {
                case TrmrkUnhandledErrorActionStepKind.BeforeExecution:
                    adverb = "before";
                    break;
                case TrmrkUnhandledErrorActionStepKind.Validation:
                    adverb = "at validation of";
                    break;
                case TrmrkUnhandledErrorActionStepKind.AfterValidation:
                    adverb = "after validation of";
                    break;
                case TrmrkUnhandledErrorActionStepKind.Action:
                    adverb = "at";
                    break;
                case TrmrkUnhandledErrorActionStepKind.AfterAction:
                    adverb = "after";
                    break;
                case TrmrkUnhandledErrorActionStepKind.AlwaysCallback:
                    adverb = "at always callback of";
                    break;
                case TrmrkUnhandledErrorActionStepKind.AfterAlwaysCallback:
                    adverb = "after always callback of";
                    break;
                default:
                    throw new NotSupportedException();
            }

            return adverb;
        }

        private string GetMessageTemplate<TActionResult>(
            TActionResult actionResult,
            Exception exc)
            where TActionResult : ITrmrkActionResult
        {
            string msgTemplate;

            if (actionResult?.IsSuccess ?? false)
            {
                msgTemplate = DefaultLogMessageTemplate;
            }
            else
            {
                msgTemplate = DefaultErrorLogMessageTemplate;

                if (exc != null)
                {
                    msgTemplate = DefaultUnhandledErrorLogMessageTemplate;
                }
            }

            return msgTemplate;
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
            actionStepKind = TrmrkUnhandledErrorActionStepKind.Validation;
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
            actionStepKind = TrmrkUnhandledErrorActionStepKind.Action;
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
                actionStepKind = TrmrkUnhandledErrorActionStepKind.AlwaysCallback;
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
            ITrmrkActionMessageTuple msgTuple = null;

            if (opts.LogMessageFactory != null)
            {
                msgTuple = opts.LogMessageFactory(
                    opts,
                    actionResult,
                    exc,
                    actionStepKind);
            }

            msgTuple = msgTuple ?? GetDefaultLogMessage(
                opts,
                actionResult,
                exc,
                actionStepKind);

            LogMessageIfReqCore(
                opts,
                actionResult,
                exc,
                actionStepKind,
                msgTuple);
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

            ShowUIMessageIfReq(
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
                string message = msgTuple.LogMessage ?? ": ".JoinNotNullStr(
                    msgTuple.Caption.Arr(msgTuple.Message));

                if (!string.IsNullOrEmpty(message))
                {
                    Logger.Write(
                        logLevel,
                        excp,
                        message);
                }
            }
        }

        private void ShowUIMessageIfReq<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            Exception excp,
            TrmrkUnhandledErrorActionStepKind actionStepKind,
            ITrmrkActionMessageTuple msgTuple,
            LogLevel logLevel)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            ShowUIMessageIfReq(
                new ShowUIMessageArgs(
                    opts,
                    actionResult,
                    excp,
                    actionStepKind,
                    msgTuple,
                    logLevel));
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

