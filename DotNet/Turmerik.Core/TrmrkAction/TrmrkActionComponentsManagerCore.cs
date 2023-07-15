using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionComponentsManagerCore
    {
        LogLevel DefaultLogLevel { get; set; }
        LogLevel DefaultErrorLogLevel { get; set; }
        bool SuppressUIMessageAlerts { get; set; }
    }

    public class TrmrkActionComponentsManagerCore : ITrmrkActionComponentsManagerCore
    {
        public LogLevel DefaultLogLevel { get; set; }
        public LogLevel DefaultErrorLogLevel { get; set; }
        public bool SuppressUIMessageAlerts { get; set; }
    }
}
