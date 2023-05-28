using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Collections
{
    public static partial class NmrblH
    {
        public static int ForEach<T>(
            this IEnumerable<T> inputNmrbl,
            ForCallback<T> callback)
        {
            int loopsCount = 0;

            var @break = new Utils.MutableValueWrapper<bool>
            {
                Value = false
            };

            foreach (var item in inputNmrbl)
            {
                callback(item, loopsCount, @break);
                loopsCount++;

                if (@break.Value)
                {
                    break;
                }
            }

            return loopsCount;
        }
    }
}
