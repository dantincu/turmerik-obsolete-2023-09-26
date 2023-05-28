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
        public static T Round<T>(
            this T value,
            Func<T, T> toFloor,
            Func<T, T> toCeil,
            Func<T, T> round,
            bool? roundToCeil) where T : INumber<T>
        {
            T retVal;

            if (roundToCeil.HasValue)
            {
                if (roundToCeil.Value)
                {
                    retVal = toCeil(value);
                }
                else
                {
                    retVal = toFloor(value);
                }
            }
            else
            {
                retVal = round(value);
            }

            return retVal;
        }

        public static decimal Round(
            this decimal value,
            bool? roundToCeil) => Round(
                value,
                Math.Floor,
                Math.Ceiling,
                Math.Round,
                roundToCeil);

        public static double Round(
            this double value,
            bool? roundToCeil) => Round(
                value,
                Math.Floor,
                Math.Ceiling,
                Math.Round,
                roundToCeil);
    }
}
