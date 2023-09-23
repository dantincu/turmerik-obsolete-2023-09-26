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
    public interface ITrmrkActionResult
    {
        bool IsSuccess { get; }
        bool HasError { get; set; }
        bool HasValidationError { get; set; }
        string ResponseMessage { get; set; }
        Exception Exception { get; set; }

        object GetData();
    }

    public interface ITrmrkActionResult<TData> : ITrmrkActionResult
    {
        TData Data { get; set; }
    }

    public interface IHttpActionResult : ITrmrkActionResult
    {
        HttpStatusCode? HttpStatusCode { get; set; }
    }

    public interface IHttpActionResult<TData> : ITrmrkActionResult<TData>, IHttpActionResult
    {
    }

    public class InternalAppError : Exception
    {
        public HttpStatusCode? HttpStatusCode { get; }
    }

    public class TrmrkActionResult : ITrmrkActionResult
    {
        public bool IsSuccess => !HasError;
        public bool HasError { get; set; }
        public bool HasValidationError { get; set; }
        public string ResponseCaption { get; set; }
        public string ResponseMessage { get; set; }
        public Exception Exception { get; set; }

        public virtual object GetData() => null;
    }

    public class TrmrkActionResult<TData> : TrmrkActionResult, ITrmrkActionResult<TData>
    {
        public TData Data { get; set; }

        public override object GetData() => Data;
    }

    public class HttpActionResult : TrmrkActionResult, IHttpActionResult
    {
        public HttpStatusCode? HttpStatusCode { get; set; }
    }

    public class HttpActionResult<TData> : TrmrkActionResult<TData>, IHttpActionResult<TData>
    {
        public HttpStatusCode? HttpStatusCode { get; set; }
    }
}
