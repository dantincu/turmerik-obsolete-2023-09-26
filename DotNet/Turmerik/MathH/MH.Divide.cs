using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MathH
{
    public static partial class MH
    {
        public static double Divide<T>(
            T divident,
            T divisor,
            Func<T, double> convertor) where T : INumber<T> => convertor(
                divident) / convertor(divisor);

        public static double Divide(
            this long divident,
            long divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this ulong divident,
            ulong divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this int divident,
            int divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this uint divident,
            uint divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this short divident,
            short divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this ushort divident,
            ushort divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this byte divident,
            byte divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());

        public static double Divide(
            this sbyte divident,
            sbyte divisor) => Divide(
                divident,
                divisor,
                val => val.ToDouble());
    }
}
