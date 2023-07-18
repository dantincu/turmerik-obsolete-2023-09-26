using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Utils
{
    public static class ActRsltH
    {
        public static ITrmrkActionResult AsIntf(
            this ITrmrkActionResult actionResult) => actionResult;

        public static ITrmrkActionResult<TResult> AsIntf<TResult>(
            this ITrmrkActionResult<TResult> actionResult) => actionResult;
    }
}
