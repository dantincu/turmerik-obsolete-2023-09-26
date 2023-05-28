using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Synchronized
{
    public class DisposedComponentAccessException : Exception
    {
        public DisposedComponentAccessException()
        {
        }

        public DisposedComponentAccessException(string? message) : base(message)
        {
        }

        public DisposedComponentAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DisposedComponentAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
