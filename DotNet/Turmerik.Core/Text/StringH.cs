﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Text
{
    public static partial class StringH
    {
        public static string UPPER_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string LOWER_LETTERS = "abcdefghijklmnopqrstuvwxyz";
        public const string DIGITS = "0123456789";
        public const string PARENS = "{[()]}";
        public const string CODE_OPERATORS = "./%+-*|&<>=:?";
        public const string CODE_IDNF_ALLOWED_NON_ALPHANUMERICS = "@$_";
        public const string OTHER_PUNCTUATION_CHARS = "`,;'\"";

        public static readonly string NL = Environment.NewLine;

        public static bool IsNewLineChar(char c)
        {
            bool retVal = c == '\n';
            return retVal;
        }

        public static StringComparison GetStringComparison(bool ignoreCase)
        {
            StringComparison stringComparison;

            if (ignoreCase)
            {
                stringComparison = StringComparison.InvariantCultureIgnoreCase;
            }
            else
            {
                stringComparison = StringComparison.InvariantCulture;
            }

            return stringComparison;
        }

        public static string NwLns(int count)
        {
            string retStr = string.Concat(
                Enumerable.Range(0, count).Select(
                    i => NL).ToArray());

            return retStr;
        }

        public static int IndexOfStr(
            this string input,
            string str,
            bool ignoreCase = false,
            int startIndex = 0)
        {
            var stringComparison = GetStringComparison(ignoreCase);
            int idx = input.IndexOf(str, startIndex, stringComparison);

            return idx;
        }

        public static string JoinStrRange(
            int rangeCount,
            Func<int, string> strFactory,
            string joinStr = null)
        {
            string[] strArr = Enumerable.Range(0, rangeCount).Select(
                idx => strFactory(idx)).ToArray();

            string retStr;

            if (joinStr != null)
            {
                retStr = string.Join(joinStr, strArr);
            }
            else
            {
                retStr = string.Concat(strArr);
            }

            return retStr;
        }

        public static string JoinStrRange(
            int rangeCount,
            string str,
            string joinStr = null)
        {
            string retStr = JoinStrRange(
                rangeCount,
                idx => str, joinStr);

            return retStr;
        }

        public static string WrapWithBracketsForFileName(this string inputStr, bool toUpper = false)
        {
            string outputStr = $"[{inputStr}]";

            if (toUpper)
            {
                outputStr = outputStr.ToUpper();
            }

            return outputStr;
        }

        public static bool IsIdxDiffFromLast<T>(
            this string str,
            int idx,
            int offset = 0) => idx == str.Length - 1 - offset;

        public static string Nullify(
            this string str,
            bool ignoreWhitespaces = false)
        {
            if (str != null)
            {
                string checkStr = str;

                if (ignoreWhitespaces)
                {
                    checkStr = checkStr.Trim();
                }

                if (string.IsNullOrEmpty(checkStr))
                {
                    str = null;
                }
            }

            return str;
        }

        public static string JoinNotNullStr(
            this string joinStr,
            string[] strArr,
            bool excludeAllWhitespaces = true)
        {
            strArr = strArr.Where(str => str.Nullify(
                excludeAllWhitespaces) != null).ToArray();

            string retStr = string.Empty;

            if (strArr.Any())
            {
                retStr = string.Join(joinStr, strArr);
            }

            return retStr;
        }
    }
}
