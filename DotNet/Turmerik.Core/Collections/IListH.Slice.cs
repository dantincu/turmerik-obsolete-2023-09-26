using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Utils;

namespace Turmerik.Core.Collections
{
    public static partial class IListH
    {
        public static T[] SliceA<T>(
            this IList<T> inputArr,
            int startIdx = 0,
            int count = -1) => SliceA<T, IList<T>, T[]>(
                inputArr,
                arr => arr.Count,
                (arr, startIdxVal, countVal) => new T[countVal].ActWithValue(
                    array => inputArr.CopyTo(array, startIdxVal)),
                startIdx,
                count);

        public static T[] SliceA<T>(
            this T[] inputArr,
            int startIdx = 0,
            int count = -1) => SliceA<T, T[], T[]>(
                inputArr,
                arr => arr.Length,
                (arr, startIdxVal, countVal) => new T[countVal].ActWithValue(
                    array => Array.Copy(
                        inputArr,
                        startIdxVal,
                        array,
                        0, 
                        countVal)),
                startIdx,
                count);

        public static TOutArr SliceA<T, TInArr, TOutArr>(
            TInArr inputArr,
            Func<TInArr, int> counter,
            ArraySliceFactory<T, TInArr, TOutArr> outArrFactory,
            int startIdx = 0,
            int count = -1)
        {
            var idxes = ArrSlice.NormalizeSliceIndexes(
                new ArrSlice.Args
                {
                    StartIdx = startIdx,
                    Count = count,
                    TotalCount = counter(inputArr)
                });

            var outArr = outArrFactory(
                inputArr,
                idxes.StartIdxVal,
                idxes.CountVal);

            return outArr;
        }

        private static class ArrSlice
        {
            public static Args NormalizeSliceIndexes(
                Args args)
            {
                if (args.TotalCount == 0)
                {
                    args.StartIdxVal = 0;
                    args.CountVal = 0;
                }
                else
                {
                    args.StartIdxVal = args.StartIdx;
                    args.CountVal = args.Count;

                    if (args.StartIdx >= 0)
                    {
                        if (args.Count < 0)
                        {
                            args.CountVal += args.TotalCount + 1 - args.StartIdxVal;
                        }
                    }
                    else
                    {
                        args.StartIdxVal += args.TotalCount;

                        if (args.Count < 0)
                        {
                            args.CountVal *= -1;
                            args.StartIdxVal += args.Count;
                        }
                    }
                }

                return args;
            }

            public class Args
            {
                public int StartIdx { get; set; }
                public int Count { get; set; }
                public int TotalCount { get; set; }
                public int StartIdxVal { get; set; }
                public int CountVal { get; set; }
            }
        }
    }
}
