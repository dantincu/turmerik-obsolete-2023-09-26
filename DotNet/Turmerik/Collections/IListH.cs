using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.MathH;

namespace Turmerik.Collections
{
    public static partial class IListH
    {
        public static T GetFromIdx<T>(this IList<T> list, int idx)
        {
            idx = list.NormalizeIdx(idx);
            T retVal = list[idx];

            return retVal;
        }

        public static T ReplaceFromIdx<T>(this IList<T> list, int idx, T newVal)
        {
            idx = list.NormalizeIdx(idx);
            T retVal = list[idx];

            list[idx] = newVal;
            return retVal;
        }

        public static T RemoveFromIdx<T>(this IList<T> list, int idx)
        {
            idx = list.NormalizeIdx(idx);
            T retVal = list[idx];

            list.RemoveAt(idx);
            return retVal;
        }

        public static void ReplaceAtIdx<T>(this IList<T> list, int idx, T newVal)
        {
            idx = list.NormalizeIdx(idx);
            list[idx] = newVal;
        }

        public static void RemoveAtIdx<T>(this IList<T> list, int idx)
        {
            idx = list.NormalizeIdx(idx);
            list.RemoveAt(idx);
        }
    }
}
