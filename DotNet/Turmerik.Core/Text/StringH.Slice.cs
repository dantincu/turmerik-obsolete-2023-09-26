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

        public static SliceStrResult SliceStr(
            this string inputStr,
            Func<SliceStrArgs, int> startCharPredicate,
            Func<SliceStrArgs, int, int> endCharPredicate,
            bool retIdxesOnly = false,
            Action<SliceStrResult> callback = null)
        {
            int inputLen = inputStr.Length;
            int startIdx = -1;
            int endIdx = -1;

            startIdx = GetStartIdx(
                inputStr,
                inputLen,
                startCharPredicate);

            endIdx = -1;
            string retStr = null;

            if (startIdx >= 0)
            {
                endIdx = GetEndIdx(
                    inputStr,
                    inputLen,
                    startIdx,
                    endCharPredicate);
            }

            if (endIdx >= 0 && !retIdxesOnly)
            {
                retStr = inputStr.Substring(
                    startIdx,
                    endIdx - startIdx);
            }

            var result = new SliceStrResult(
                retStr,
                startIdx,
                endIdx);

            callback?.Invoke(result);
            return result;
        }

        public static int GetStartIdx(
            this string inputStr,
            int inputLen,
            Func<SliceStrArgs, int> startCharPredicate)
        {
            int startIdx = -1;
            int i = 0;

            while (i < inputLen)
            {
                char ch = inputStr[i];

                int inc = startCharPredicate(
                    new SliceStrArgs(
                        inputStr,
                        inputLen,
                        ch,
                        i));

                i += inc;

                if (inc <= 0)
                {
                    startIdx = i;
                    break;
                }
            }

            return startIdx;
        }

        public static int GetEndIdx(
            this string inputStr,
            int inputLen,
            int startIdx,
            Func<SliceStrArgs, int, int> endCharPredicate)
        {
            int endIdx = -1;
            int i = 0;

            startIdx++;
            int lenOfRest = inputLen - startIdx;

            while (i < lenOfRest)
            {
                char ch = inputStr[startIdx + i];

                int inc = endCharPredicate(
                    new SliceStrArgs(
                        inputStr,
                        inputLen,
                        ch,
                        i),
                    startIdx);

                if (inc == int.MaxValue)
                {
                    endIdx = inputLen;
                    break;
                }
                else
                {
                    i += inc;

                    if (inc <= 0)
                    {
                        endIdx = startIdx + i;
                        break;
                    }
                }
            }

            return endIdx;
        }

        public static SliceStrResult GetNextWord(
            this string inputStr,
            int startIdx = 0,
            string terminalChars = null,
            Action<SliceStrResult> callback = null)
        {
            terminalChars = terminalChars ?? "";

            var result = SliceStr(
                inputStr,
                args => args.Idx < startIdx || char.IsWhiteSpace(args.Char) ? 1 : 0,
                (args, stIdx) => char.IsWhiteSpace(args.Char) || terminalChars.Contains(args.Char) ? 0 : 1,
                false,
                callback);

            return result;
        }

        public static SliceStrResult GetNextAlphaNumericWord(
            this string inputStr,
            int startIdx = 0,
            string allowedChars = null,
            Action<SliceStrResult> callback = null)
        {
            allowedChars = allowedChars ?? "";

            var result = SliceStr(
                inputStr,
                args => args.Idx >= startIdx && char.IsLetterOrDigit(args.Char) ? 0 : 1,
                (args, stIdx) => char.IsLetterOrDigit(args.Char) || allowedChars.Contains(args.Char) ? 1 : 0,
                false,
                callback);

            return result;
        }

        public static SliceStrResult TryDigestStr(
            this string inputStr,
            string str,
            int startIdx = 0,
            bool retIdxesOnly = false,
            Action<SliceStrResult> callback = null)
        {
            int strLen = inputStr.Length;
            int negStrLen = -strLen;

            var result = SliceStr(
                inputStr,
                args => SliceStr(
                    args.InputStr,
                    startIdx,
                    negStrLen) == str ? 0 : 1,
                (args, stIdx) => int.MaxValue,
                retIdxesOnly,
                callback);

            return result;
        }
    }
}
