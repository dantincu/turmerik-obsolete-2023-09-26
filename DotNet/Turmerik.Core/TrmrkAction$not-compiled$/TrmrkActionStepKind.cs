using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TrmrkAction
{
    public enum TrmrkActionStepKind
    {
        BeforeExecution = 0,
        BeforeValidation,
        Validation,
        AfterValidation,
        BeforeAction,
        Action,
        AfterAction,
        BeforeAlways,
        Always,
        AfterAlways
    }
}
