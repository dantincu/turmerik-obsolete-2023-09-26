using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionComponentsManagerFactoryCore
    {
        ITrmrkActionComponentsManagerCore Create();
    }

    public class TrmrkActionComponentsManagerFactoryCore : ITrmrkActionComponentsManagerFactoryCore
    {
        public ITrmrkActionComponentsManagerCore Create() => new TrmrkActionComponentsManagerCore
        {
            DefaultLogLevel = LogLevel.Debug,
            DefaultErrorLogLevel = LogLevel.Error,
        };
    }
}
