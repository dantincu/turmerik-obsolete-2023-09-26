using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Text
{
    public static partial class StringH
    {
        public static string SliceStr(
            this string inputStr,
            int startIdx = 0,
            int count = -1) => IListH.SliceA<char, string, string>(
                inputStr,
                str => str.Length,
                (str, startIdxVal, countVal) => str.Substring(startIdxVal, countVal),
                startIdx,
                count);
    }
}
