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
        public static SignedNumber<TNumber> ToAbs<TNumber>(
            this TNumber number,
            Func<int, TNumber> convertor) where TNumber : INumber<TNumber> => number.CompareTo(0) switch
            {
                -1 => new SignedNumber<TNumber>(number, convertor(-1)),
                0 => new SignedNumber<TNumber>(number, convertor(0)),
                _ => new SignedNumber<TNumber>(number, convertor(1)),
            };

        public static SignedNumber<sbyte> ToAbs(
            this sbyte number) => number.ToAbs(value => (sbyte)value);

        public static SignedNumber<short> ToAbs(
            this short number) => number.ToAbs(value => (short)value);

        public static SignedNumber<int> ToAbs(
            this int number) => number.ToAbs(value => value);

        public static SignedNumber<long> ToAbs(
            this long number) => number.ToAbs(value => value);

        public static SignedNumber<float> ToAbs(
            this float number) => number.ToAbs(value => value);

        public static SignedNumber<double> ToAbs(
            this double number) => number.ToAbs(value => value);

        public static SignedNumber<decimal> ToAbs(
            this decimal number) => number.ToAbs(value => value);
    }
}
