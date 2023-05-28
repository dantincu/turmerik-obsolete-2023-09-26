using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public static partial class ArrH
    {
        public static T[] Arr<T>(
            this T firstItem,
            params T[] nextItems)
        {
            T[] retArr = new T[nextItems.Length + 1];
            retArr[0] = firstItem;

            nextItems.CopyTo(retArr, 1);
            return retArr;
        }
    }
}
