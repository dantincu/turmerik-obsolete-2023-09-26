using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Turmerik.MathH;

namespace Turmerik.MathH
{
    public static partial class MatH
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
