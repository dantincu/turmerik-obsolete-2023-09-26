using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.MathH
{
    public static partial class MH
    {
        public static IDivisionResult<T> DivInt<T>(
            this T divident,
            T divisor) where T : INumber<T>
        {
            T quotient = divident / divisor;
            T remainder = divident % divisor;

            var result = new DivisionResult<T>(quotient, remainder);
            return result;
        }
    }
}
