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
            Func<SliceStrArgs, IncIdxAnswer> startCharPredicate,
            Func<SliceStrArgs, int, IncIdxAnswer> endCharPredicate,
            int startIdx = 0,
            bool retIdxesOnly = false,
            Action<SliceStrResult> callback = null)
        {
            int inputLen = inputStr.Length;
            int endIdx = -1;

            int startIdxVal = GetStartIdx(
                inputStr,
                inputLen,
                startIdx,
                startCharPredicate);

            endIdx = -1;
            string retStr = null;

            if (startIdxVal >= 0)
            {
                endIdx = GetStartIdx(
                    inputStr,
                    inputLen,
                    startIdxVal,
                    args => endCharPredicate(
                        args,
                        startIdx));
            }

            if (endIdx >= 0 && !retIdxesOnly)
            {
                retStr = inputStr.Substring(
                    startIdxVal,
                    endIdx - startIdxVal);
            }

            var result = new SliceStrResult(
                retStr,
                startIdxVal,
                endIdx);

            callback?.Invoke(result);
            return result;
        }

        public static int GetStartIdx(
            this string inputStr,
            int startIdx,
            Func<SliceStrArgs, IncIdxAnswer> startCharPredicate) => GetStartIdx(
                inputStr,
                inputStr.Length,
                startIdx,
                startCharPredicate);

        public static int GetStartIdx(
            this string inputStr,
            int inputLen,
            int startIdx,
            Func<SliceStrArgs, IncIdxAnswer> startCharPredicate)
        {
            int endIdx = -1;
            int i = 0;

            int lenOfRest = inputLen - startIdx;

            while (i < lenOfRest)
            {
                int idx = startIdx + i;
                char ch = inputStr[idx];

                var match = startCharPredicate(
                    new SliceStrArgs(
                        inputStr,
                        inputLen,
                        startIdx,
                        ch,
                        idx));

                int incIdx = match.IncIdx ?? 1;
                i += incIdx;

                if (match.Answer || incIdx == 0)
                {
                    endIdx = startIdx + i;
                    break;
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
                args => new IncIdxAnswer(char.IsWhiteSpace(args.Char), 0),
                (args, stIdx) => new IncIdxAnswer(char.IsWhiteSpace(args.Char) || terminalChars.Contains(args.Char), 0),
                startIdx,
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
                args => new IncIdxAnswer(char.IsLetterOrDigit(args.Char), 0),
                (args, stIdx) => new IncIdxAnswer(char.IsLetterOrDigit(args.Char) || allowedChars.Contains(args.Char), 0),
                startIdx,
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
                args => new IncIdxAnswer(
                    args.InputStr.StartsWithStr(
                        args.Idx, str), str.Length),
                (args, stIdx) => new IncIdxAnswer(true, strLen - args.Idx),
                    startIdx,
                    retIdxesOnly,
                    callback);

            return result;
        }
    }
}
