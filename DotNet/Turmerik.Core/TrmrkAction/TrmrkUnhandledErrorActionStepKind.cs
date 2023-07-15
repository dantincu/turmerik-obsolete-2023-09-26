using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TrmrkAction
{
    public enum TrmrkUnhandledErrorActionStepKind
    {
        BeforeExecution = 0,
        BeforeValidation,
        AfterValidation,
        BeforeAction,
        AfterAction,
        BeforeAlwaysCallback,
        AfterAlwaysCallback
    }
}
