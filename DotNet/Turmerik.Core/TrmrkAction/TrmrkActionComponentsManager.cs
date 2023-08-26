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
    public interface ITrmrkActionComponentsManager
    {
        LogLevel DefaultLogLevel { get; set; }
        LogLevel DefaultErrorLogLevel { get; set; }
        bool EnableUIBlockingMessagePopups { get; set; }

        void ShowUIMessageAlert(
            ShowUIMessageAlertArgs args,
            bool useUIBlockingMessagePopup);
    }

    public class TrmrkActionComponentsManager : ITrmrkActionComponentsManager
    {
        public virtual LogLevel DefaultLogLevel { get; set; } = LogLevel.Trace;
        public virtual LogLevel DefaultErrorLogLevel { get; set; } = LogLevel.Error;
        public virtual bool EnableUIBlockingMessagePopups { get; set; } = true;

        public virtual void ShowUIMessageAlert(
            ShowUIMessageAlertArgs args,
            bool useUIBlockingMessagePopup)
        {
        }
    }
}
