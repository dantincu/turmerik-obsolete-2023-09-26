using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Collections
{
    public static partial class NmrblH
    {
        public static IEnumerable<T> Slice<T>(
            this IEnumerable<T> inputNmrbl,
            int startIdx,
            int count) => inputNmrbl.Skip(startIdx).Take(count);

        public static IEnumerable<T> SliceN<T>(
            this IEnumerable<T> inputNmrbl,
            int startIdx,
            int count) => inputNmrbl.SkipN(startIdx).TakeN(count);

        public static IEnumerable<T> SkipN<T>(
            this IEnumerable<T> inputNmrbl,
            int takeCount) => OpN(
                inputNmrbl,
                (nmrbl, count) => nmrbl.Skip(count),
                takeCount);

        public static IEnumerable<T> TakeN<T>(
            this IEnumerable<T> inputNmrbl,
            int takeCount) => OpN(
                inputNmrbl,
                (nmrbl, count) => nmrbl.Take(count),
                takeCount);

        public static IEnumerable<T> OpN<T>(
            IEnumerable<T> inputNmrbl,
            Func<IEnumerable<T>, int, IEnumerable<T>> op,
            int count)
        {
            IEnumerable<T> retNmrbl;

            if (count >= 0)
            {
                retNmrbl = op(inputNmrbl, count);
            }
            else
            {
                retNmrbl = op(inputNmrbl.Reverse(), -count);
            }

            return retNmrbl;
        }
    }
}
