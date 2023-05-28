using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Utils;

namespace Turmerik.Core.MathH
{
    public static partial class MH
    {
        public static readonly int Int32MaxValueDigitsCount = int.MaxValue.ToString().Length;

        public static T SnapToInterval<T>(
            T value,
            T minVal,
            T maxVal,
            T snapVal,
            Func<T, double> convertor,
            Func<double, T> revConvertor,
            bool addToSnap) where T : INumber<T>
        {
            T intvLen = maxVal - minVal;

            if (value < minVal)
            {
                T toAdd = SnapToDiscrete(
                    (minVal - value),
                    intvLen,
                    convertor,
                    revConvertor,
                    true);

                value += toAdd;
            }
            else if (value > maxVal)
            {
                T toSubstract = SnapToDiscrete(
                    (value - maxVal),
                    intvLen,
                    convertor,
                    revConvertor,
                    false);

                value -= toSubstract;
            }

            snapVal = snapVal.FirstNotNull(intvLen);

            value = SnapToDiscrete(
                (value - minVal),
                snapVal,
                convertor,
                revConvertor,
                addToSnap) + minVal;

            if (value < minVal)
            {
                value += intvLen;
            }
            else if (value > maxVal)
            {
                value -= intvLen;
            }

            return value;
        }

        public static long SnapToInterval(
            long value,
            long minVal,
            long maxVal,
            long snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToInt64(val),
                addToSnap);

        public static ulong SnapToInterval(
            ulong value,
            ulong minVal,
            ulong maxVal,
            ulong snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToUInt64(val),
                addToSnap);

        public static int SnapToInterval(
            int value,
            int minVal,
            int maxVal,
            int snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToInt32(val),
                addToSnap);

        public static uint SnapToInterval(
            uint value,
            uint minVal,
            uint maxVal,
            uint snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToUInt32(val),
                addToSnap);

        public static short SnapToInterval(
            short value,
            short minVal,
            short maxVal,
            short snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToInt16(val),
                addToSnap);

        public static ushort SnapToInterval(
            ushort value,
            ushort minVal,
            ushort maxVal,
            ushort snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToUInt16(val),
                addToSnap);

        public static byte SnapToInterval(
            byte value,
            byte minVal,
            byte maxVal,
            byte snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToByte(val),
                addToSnap);

        public static sbyte SnapToInterval(
            sbyte value,
            sbyte minVal,
            sbyte maxVal,
            sbyte snapVal = 0,
            bool addToSnap = true) => SnapToInterval(
                value,
                minVal,
                maxVal,
                snapVal,
                val => val.ToDouble(),
                val => Convert.ToSByte(val),
                addToSnap);
    }
}
