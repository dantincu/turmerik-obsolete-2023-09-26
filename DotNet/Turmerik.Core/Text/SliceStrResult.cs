using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Text
{
    public readonly struct SliceStrResult
    {
        public SliceStrResult(string slicedStr, int startIdx, int endIdx)
        {
            SlicedStr = slicedStr;
            StartIdx = startIdx;
            EndIdx = endIdx;
        }

        public string SlicedStr { get; }
        public int StartIdx { get; }
        public int EndIdx { get; }
    }
}
