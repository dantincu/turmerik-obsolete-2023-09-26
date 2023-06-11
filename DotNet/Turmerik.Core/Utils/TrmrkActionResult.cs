using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Utils
{
    public interface ITrmrkActionError
    {
        string Message { get; }
        Exception Exception { get; }
    }

    public interface ITrmrkActionResult
    {
        bool IsSuccess { get; }
        TrmrkActionError Error { get; }
    }

    public interface ITrmrkActionResult<TData> : ITrmrkActionResult
    {
        TData Data { get; }
    }

    public interface IHttpActionResult : ITrmrkActionResult
    {
        HttpStatusCode? HttpStatusCode { get; }
    }

    public interface IHttpActionResult<TData> : ITrmrkActionResult<TData>, IHttpActionResult
    {
    }

    public class InternalAppError : Exception
    {
        public InternalAppError(HttpStatusCode? httpStatusCode = null)
        {
            HttpStatusCode = httpStatusCode;
        }

        public InternalAppError(
            string message,
            HttpStatusCode? httpStatusCode = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public InternalAppError(
            string message,
            Exception innerException,
            HttpStatusCode? httpStatusCode = null) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }

        protected InternalAppError(
            SerializationInfo info,
            StreamingContext context,
            HttpStatusCode? httpStatusCode = null) : base(info, context)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode? HttpStatusCode { get; }
    }

    public class TrmrkActionError : ITrmrkActionError
    {
        public TrmrkActionError(
            string message,
            Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        public string Message { get; }
        public Exception Exception { get; }
    }

    public static class ActionResult
    {
        public static TTrmrkActionResult Then<TTrmrkActionResult>(
            this TTrmrkActionResult actionResult,
            Action<TTrmrkActionResult> successCallback,
            Action<TTrmrkActionResult> errorCallback = null,
            Action<TTrmrkActionResult> finalCallback = null)
            where TTrmrkActionResult : ITrmrkActionResult
        {
            if (actionResult.IsSuccess)
            {
                successCallback?.Invoke(actionResult);
            }
            else
            {
                errorCallback?.Invoke(actionResult);
            }

            finalCallback?.Invoke(actionResult);
            return actionResult;
        }
    }

    public class TrmrkActionResult : ITrmrkActionResult
    {
        public TrmrkActionResult(ITrmrkActionResult src)
        {
            IsSuccess = src.IsSuccess;
            Error = src.Error;
        }

        public TrmrkActionResult(
            bool isSuccess,
            TrmrkActionError error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public TrmrkActionError Error { get; }
    }

    public class TrmrkActionResult<TData> : TrmrkActionResult, ITrmrkActionResult<TData>
    {
        public TrmrkActionResult(ITrmrkActionResult<TData> src) : base(src)
        {
        }

        public TrmrkActionResult(
            ITrmrkActionResult src,
            TData data) : base(src)
        {
            this.Data = data;
        }

        public TrmrkActionResult(
            bool isSuccess,
            TData data,
            TrmrkActionError error) : base(isSuccess, error)
        {
            this.Data = data;
        }

        public TData Data { get; }
    }

    public class HttpActionResult : TrmrkActionResult, IHttpActionResult
    {
        public HttpActionResult(IHttpActionResult src) : base(src)
        {
            HttpStatusCode = src.HttpStatusCode;
        }

        public HttpActionResult(
            bool isSuccess,
            TrmrkActionError error = null,
            HttpStatusCode? httpStatusCode = null) : base(
                isSuccess,
                error)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode? HttpStatusCode { get; }
    }

    public class HttpActionResult<TData> : TrmrkActionResult<TData>, IHttpActionResult<TData>
    {
        public HttpActionResult(
            IHttpActionResult<TData> src,
            HttpStatusCode? httpStatusCode = null) : base(src)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpActionResult(
            IHttpActionResult src,
            TData data,
            HttpStatusCode? httpStatusCode = null) : base(src, data)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpActionResult(
            bool isSuccess,
            TData data,
            TrmrkActionError error = null,
            HttpStatusCode? httpStatusCode = null) : base(
                isSuccess,
                data,
                error)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode? HttpStatusCode { get; }
    }
}
