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
        public TrmrkActionComponentCore(
            TManager manager,
            IAppLogger logger)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            Logger = logger;
        }

        protected TManager Manager { get; }
        protected IAppLogger Logger { get; }

        protected TActionResult ExecuteCore<TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TActionResult, TActionResult, TMsgTuple> opts)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            var actionStepKind = TrmrkActionStepKind.BeforeExecution;
            TActionResult actionResult = default;
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

                if (opts.Validation == null || (actionResult?.IsSuccess ?? false))
                {
                    OnBeforeAction(opts, actionResult, ref actionStepKind);
                    actionResult = opts.Action();
                    OnAfterAction(opts, actionResult, ref actionStepKind);
                }
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult, actionStepKind, exc);
            }

            try
            {
                ExecuteAlwaysCallbackIfReq(opts, actionResult, ref actionStepKind, excp);
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
            var actionStepKind = TrmrkActionStepKind.BeforeExecution;
            TActionResult actionResult = default;
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

                if (opts.Validation == null || (actionResult?.IsSuccess ?? false))
                {
                    OnBeforeAction(opts, actionResult, ref actionStepKind);
                    actionResult = await opts.Action();
                    OnAfterAction(opts, actionResult, ref actionStepKind);
                }
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult, actionStepKind, exc);
            }

            try
            {
                ExecuteAlwaysCallbackIfReq(opts, actionResult, ref actionStepKind, excp);
            }
            catch (Exception exc)
            {
                excp = exc;
                OnUnhandledError(opts, actionResult, actionStepKind, exc);
            }

            ExecuteFinalCallbackIfReq(opts, actionResult, actionStepKind, excp);
            return actionResult;
        }

        private void OnBeforeExecute<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            LogMessageIfReq(opts, actionResult, null, actionStepKind);
            opts.BeforeExecute?.Invoke();
        }

        private void OnBeforeValidation<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkActionStepKind.BeforeValidation;
            LogMessageIfReq(opts, actionResult, null, actionStepKind);
            actionStepKind = TrmrkActionStepKind.Validation;
        }

        private void OnAfterValidation<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkActionStepKind.AfterValidation;

            if (actionResult.HasError)
            {
                opts.ValidationErrorCallback?.Invoke(actionResult);
            }

            LogMessageIfReq(opts, actionResult, null, actionStepKind);
        }

        private void OnBeforeAction<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkActionStepKind.BeforeAction;
            LogMessageIfReq(opts, actionResult, null, actionStepKind);
            actionStepKind = TrmrkActionStepKind.Action;
        }

        private void OnAfterAction<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            ref TrmrkActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkActionStepKind.AfterAction;

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
            ref TrmrkActionStepKind actionStepKind,
            Exception excp)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            actionStepKind = TrmrkActionStepKind.BeforeAlways;
            LogMessageIfReq(opts, actionResult, excp, actionStepKind);
            actionStepKind = TrmrkActionStepKind.Always;

            opts.AlwaysCallback?.Invoke(actionResult);

            actionStepKind = TrmrkActionStepKind.AfterAlways;
            LogMessageIfReq(opts, actionResult, excp, actionStepKind);
        }

        private void OnUnhandledError<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            TActionResult actionResult,
            TrmrkActionStepKind actionStepKind,
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
            TrmrkActionStepKind actionStepKind,
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
            TrmrkActionStepKind actionStepKind)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            if (opts.LogMessageFactory != null)
            {
                var msgTuple = opts.LogMessageFactory(
                    new LogMsgFactoryArgs(
                        opts,
                        actionResult,
                        exc,
                        actionStepKind));

                if (msgTuple != null)
                {
                    var excp = exc ?? actionResult?.Exception;

                    if (Logger != null && msgTuple.LogMessage != null)
                    {
                        Logger.Write(
                            GetLogLevel(
                                opts,
                                actionResult,
                                excp,
                                msgTuple.LogLevel),
                            excp,
                            msgTuple.LogMessage);
                    }

                    if (msgTuple.UIMessage != null)
                    {
                        Manager.ShowUIMessage(
                            new ShowUIMessageArgs(
                                opts,
                                actionResult,
                                excp,
                                actionStepKind,
                                msgTuple,
                                GetLogLevel(
                                    opts,
                                    actionResult,
                                    excp,
                                    msgTuple.UILogLevel)));
                    }
                }
            }
        }

        private LogLevel GetLogLevel<TResult, TActionResult, TMsgTuple>(
            ITrmrkActionComponentOptsCore<TResult, TActionResult, TMsgTuple> opts,
            ITrmrkActionResult actionResult,
            Exception exc,
            LogLevel? nllblLogLevel)
            where TActionResult : ITrmrkActionResult
            where TMsgTuple : ITrmrkActionMessageTuple
        {
            LogLevel logLevel;

            if (nllblLogLevel.HasValue)
            {
                logLevel = nllblLogLevel.Value;
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

