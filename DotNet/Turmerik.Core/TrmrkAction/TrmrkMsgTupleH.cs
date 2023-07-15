using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TrmrkAction
{
    public static class TrmrkMsgTupleH
    {
        public static TrmrkActionMessageTuple TrmrkMsgTuple(
            this string message,
            string caption = null,
            bool? showBlockingUIMessage = null,
            LogLevel? logLevel = null) => new TrmrkActionMessageTuple
            {
                Message = message,
                Caption = caption,
                ShowBlockingUIMessage = showBlockingUIMessage,
                LogLevel = logLevel
            };
    }
}
