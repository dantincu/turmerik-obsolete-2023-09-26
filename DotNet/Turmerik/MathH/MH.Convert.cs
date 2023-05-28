using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MathH
{
    public static partial class MH
    {
        public static double ToDouble(this long value) => Convert.ToDouble(value);
        public static double ToDouble(this ulong value) => Convert.ToDouble(value);
        public static double ToDouble(this int value) => Convert.ToDouble(value);
        public static double ToDouble(this uint value) => Convert.ToDouble(value);
        public static double ToDouble(this short value) => Convert.ToDouble(value);
        public static double ToDouble(this ushort value) => Convert.ToDouble(value);
        public static double ToDouble(this byte value) => Convert.ToDouble(value);
        public static double ToDouble(this sbyte value) => Convert.ToDouble(value);

        public static float ToSingle(this long value) => Convert.ToSingle(value);
        public static float ToSingle(this ulong value) => Convert.ToSingle(value);
        public static float ToSingle(this int value) => Convert.ToSingle(value);
        public static float ToSingle(this uint value) => Convert.ToSingle(value);
        public static float ToSingle(this short value) => Convert.ToSingle(value);
        public static float ToSingle(this ushort value) => Convert.ToSingle(value);
        public static float ToSingle(this byte value) => Convert.ToSingle(value);
        public static float ToSingle(this sbyte value) => Convert.ToSingle(value);

        public static decimal ToDecimal(this long value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this ulong value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this int value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this uint value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this short value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this ushort value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this byte value) => Convert.ToDecimal(value);
        public static decimal ToDecimal(this sbyte value) => Convert.ToDecimal(value);
    }
}
