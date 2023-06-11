using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.Text
{
    public static class DigestH
    {
        public static int TryDigest(
            string input,
            int startIdx,
            string expectedStr,
            out string actualStr,
            Func<string, string, bool> matchPredicate = null)
        {
            int endIdx = -1;
            actualStr = null;

            bool matches = input != null && expectedStr != null;

            if (matches)
            {
                int inputLen = input.Length;
                int expectedLen = expectedStr.Length;
                int expectedEndIdx = startIdx + expectedLen;

                matches = matches && inputLen >= 0 && expectedLen >= 0;
                matches = matches && expectedEndIdx <= inputLen;

                if (matches)
                {
                    actualStr = input.Substring(
                        startIdx,
                        expectedLen);

                    matchPredicate = matchPredicate.FirstNotNull(
                        (expStr, actStr) => expStr == actStr);

                    matches = matchPredicate(expectedStr, actualStr);
                }

                if (matches)
                {
                    endIdx = expectedEndIdx;
                }
            }

            return endIdx;
        }

        public static bool StrEquals(this string target, string reference, bool ignoreCase = false)
        {
            bool retVal = string.Compare(target, reference, ignoreCase) == 0;
            return retVal;
        }
    }
}
