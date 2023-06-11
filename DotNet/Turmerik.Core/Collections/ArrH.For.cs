using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Collections
{
    public static partial class ArrH
    {
        public static void For<T>(
            this T[] inArr,
            int startIdx,
            int endIdx,
            ForCallback<T> callback,
            bool reverseOrder) => IListH.ForIdx(
                startIdx,
                endIdx,
                (idx, @break) => callback(
                    inArr[idx],
                    idx,
                    @break),
                reverseOrder);

        public static void For<T>(
            this T[] inArr,
            int startIdx,
            int endIdx,
            ForCallback<T> callback) => IListH.ForIdx(
                startIdx,
                endIdx,
                (idx, @break) => callback(
                    inArr[idx],
                    idx,
                    @break));

        public static void ForRev<T>(
            this T[] inArr,
            int startIdx,
            int endIdx,
            ForCallback<T> callback) => IListH.ForIdxRev(
                startIdx,
                endIdx,
                (idx, @break) => callback(
                    inArr[idx],
                    idx,
                    @break));
    }
}
