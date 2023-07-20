using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Collections
{
    public static partial class NmrblH
    {
        public static IEnumerable<T> ConcatNmrbl<T>(this IEnumerable<T> inputNmrbl, params T[] nextItemsArr)
        {
            IEnumerable<T> retNmrbl = inputNmrbl.Concat(nextItemsArr);
            return retNmrbl;
        }

        public static IEnumerable<object> Nmrbl(this IEnumerable nmrbl)
        {
            var retVal = nmrbl.Cast<object>();
            return retVal;
        }

        public static bool NmrblIdxIsInRange<T>(
            this IEnumerable<T> nmrbl,
            int idx)
        {
            bool retVal = idx >= 0 && idx < nmrbl.Count();
            return retVal;
        }

        public static int NormalizeIdx<T>(this IEnumerable<T> nmrbl, int idx)
        {
            if (idx < 0)
            {
                idx = nmrbl.Count() + idx; // idx is negative, so its absolute value is actually substracted from the list count
            }

            return idx;
        }

        public static bool IsIdxDiffFromLast<T>(
            this IEnumerable<T> nmrbl,
            int idx,
            int offset = 0) => idx == nmrbl.Count() - 1 - offset;
    }
}
