using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public static class FindH
    {
        public static KeyValuePair<int, T> FindVal<T>(
            this IEnumerable<T> nmrbl,
            Func<T, int, bool> predicate,
            bool retFirst = true)
        {
            T retVal = default;
            int idx = -1;

            int i = 0;

            foreach (var val in nmrbl)
            {
                if (predicate(val, i))
                {
                    if (idx >= 0)
                    {
                        throw new InvalidOperationException(
                            $"Sequence contains more than 1 element and the ${nameof(retFirst)} flag has been set to false");
                    }

                    retVal = val;
                    idx = i;

                    if (retFirst)
                    {
                        break;
                    }
                }

                i++;
            }

            return new KeyValuePair<int, T>(idx, retVal);
        }

        public static KeyValuePair<int, T> FindVal<T>(
            this IEnumerable<T> nmrbl,
            Func<T, bool> predicate,
            bool retFirst = true)
        {
            var retKvp = nmrbl.FindVal(
                (val, idx) => predicate(val), retFirst);

            return retKvp;
        }
    }
}
