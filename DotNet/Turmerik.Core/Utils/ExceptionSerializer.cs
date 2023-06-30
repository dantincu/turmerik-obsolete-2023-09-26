using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Turmerik.Utils
{
    public interface IExceptionSerializer
    {
        SerializedException SerializeException(Exception exc);
    }

    public class ExceptionSerializer : IExceptionSerializer
    {
        public SerializedException SerializeException(Exception exc)
        {
            var serExc = new SerializedException
            {
                Message = exc.Message,
                TypeFullName = exc.GetType().FullName,
                StackTrace = exc.StackTrace?.Split('n'),
                Source = exc.Source
            };

            if (exc is AggregateException aggExc)
            {
                serExc.Inners = aggExc.InnerExceptions?.Select(
                    excp => SerializeException(excp)).ToArray();
            }
            else if (exc is TrmrkException trmrkExc)
            {
                serExc.AdditionalData = trmrkExc.GetAdditionalData();
            }
            else
            {
                serExc.Inner = exc.InnerException?.WithValue(
                    inner => SerializeException(inner));
            }

            return serExc;
        }
    }
}
