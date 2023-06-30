using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.Core.Logging
{
    public interface ITrmrkJsonFormatterFactory
    {
        TrmrkJsonFormatter CreateFormatter();
    }

    public class TrmrkJsonFormatterFactory : ITrmrkJsonFormatterFactory
    {
        private readonly ITimeStampHelper timeStampHelper;
        private readonly IExceptionSerializer exceptionSerializer;

        public TrmrkJsonFormatterFactory(
            ITimeStampHelper timeStampHelper,
            IExceptionSerializer exceptionSerializer)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            this.exceptionSerializer = exceptionSerializer ?? throw new ArgumentNullException(nameof(exceptionSerializer));
        }

        public TrmrkJsonFormatter CreateFormatter(
            ) => new TrmrkJsonFormatter(
            timeStampHelper,
            exceptionSerializer);
    }
}
