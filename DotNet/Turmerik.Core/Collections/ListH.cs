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
    }
}
