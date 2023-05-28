using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Collections
{
    public static partial class IListH
    {
        public static void For<T>(
            this IList<T> inArr,
            int startIdx,
            int endIdx,
            ForCallback<T> callback,
            bool reverseOrder) => ForIdx(
                startIdx,
                endIdx,
                (idx, @break) => callback(
                    inArr[idx],
                    idx,
                    @break),
                reverseOrder);

        public static void For<T>(
            this IList<T> inArr,
            int startIdx,
            int endIdx,
            ForCallback<T> callback) => ForIdx(
                startIdx,
                endIdx,
                (idx, @break) => callback(
                    inArr[idx],
                    idx,
                    @break));

        public static void ForRev<T>(
            this IList<T> inArr,
            int startIdx,
            int endIdx,
            ForCallback<T> callback) => ForIdxRev(
                startIdx,
                endIdx,
                (idx, @break) => callback(
                    inArr[idx],
                    idx,
                    @break));

        public static void ForIdx(
            int startIdx,
            int endIdx,
            ForIdxCallback callback,
            bool reverseOrder)
        {
            if (reverseOrder)
            {
                ForIdxRev(startIdx, endIdx, callback);
            }
            else
            {
                ForIdx(startIdx, endIdx, callback);
            }
        }

        public static void ForIdx(
            int startIdx,
            int endIdx,
            ForIdxCallback callback)
        {
            var @break = new MutableValueWrapper<bool>
            {
                Value = false,
            };

            for (int i = startIdx; i < endIdx; i++)
            {
                callback(i, @break);

                if (@break.Value)
                {
                    break;
                }
            }
        }

        public static void ForIdxRev(
            int startIdx,
            int endIdx,
            ForIdxCallback callback)
        {
            var @break = new MutableValueWrapper<bool>
            {
                Value = false,
            };

            for (int i = startIdx; i < endIdx; i--)
            {
                callback(i, @break);

                if (@break.Value)
                {
                    break;
                }
            }
        }
    }
}
