using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TrmrkAction
{
    public enum TrmrkUnhandledErrorActionStepKind
    {
        BeforeExecution = 0,
        Validation,
        AfterValidation,
        Action,
        AfterAction,
        AlwaysCallback,
        AfterAlwaysCallback
    }
}
