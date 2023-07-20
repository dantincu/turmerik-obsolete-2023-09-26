using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Collections
{
    public static class ListH
    {
        public static void AddRange<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items);
        }

        public static bool IsIdxDiffFromLast<T>(
            this List<T> list,
            int idx,
            int offset = 0) => idx == list.Count - 1 - offset;
    }
}
