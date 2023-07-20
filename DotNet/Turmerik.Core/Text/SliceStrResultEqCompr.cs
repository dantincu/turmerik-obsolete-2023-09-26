using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.Text
{
    public interface ISliceStrResultEqCompr : IEqualityComparer<SliceStrResult>
    {
    }

    public class SliceStrResultEqCompr : ISliceStrResultEqCompr
    {
        public bool Equals(
            SliceStrResult x,
            SliceStrResult y)
        {
            bool retVal = x.StartIdx == y.StartIdx;
            retVal = retVal && x.EndIdx == y.EndIdx;

            retVal = retVal && x.SlicedStr == y.SlicedStr;
            return retVal;
        }

        public int GetHashCode(
            SliceStrResult obj) => (
                obj.SlicedStr?.GetHashCode() ?? 0).BasicHashCode(
                    obj.StartIdx,
                    obj.EndIdx);
    }
}
