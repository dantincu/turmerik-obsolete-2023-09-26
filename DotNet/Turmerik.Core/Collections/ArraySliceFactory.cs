using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public delegate TOutArr ArraySliceFactory<T, TInArr, TOutArr>(
        TInArr inputArr,
        int startIdx,
        int count);
}
