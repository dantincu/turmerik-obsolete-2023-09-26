using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MathH
{
    public static partial class MatH
    {
        public static T SnapToDiscrete<T>(
            T value,
            T snapVal,
            Func<T, double> convertor,
            Func<double, T> revConvertor,
            bool addToSnap) where T : INumber<T>
        {
            double raport = convertor(value) / convertor(snapVal);
            raport = raport.Round(addToSnap);

            T intRaport = revConvertor(raport);
            T retVal = intRaport * snapVal;

            return retVal;
        }

        public static long SnapToDiscrete(
            long value,
            long snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToInt64(val),
                addToSnap);

        public static ulong SnapToDiscrete(
            ulong value,
            ulong snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToUInt64(val),
                addToSnap);

        public static int SnapToDiscrete(
            int value,
            int snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToInt32(val),
                addToSnap);

        public static uint SnapToDiscrete(
            uint value,
            uint snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToUInt32(val),
                addToSnap);

        public static short SnapToDiscrete(
            short value,
            short snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToInt16(val),
                addToSnap);

        public static ushort SnapToDiscrete(
            ushort value,
            ushort snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToUInt16(val),
                addToSnap);

        public static byte SnapToDiscrete(
            byte value,
            byte snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToByte(val),
                addToSnap);

        public static sbyte SnapToDiscrete(
            sbyte value,
            sbyte snapVal,
            bool addToSnap = true) => SnapToDiscrete(
                value,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToSByte(val),
                addToSnap);
    }
}
