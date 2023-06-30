﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Text
{
    public static partial class StringH
    {
        public static Tuple<string, string> SubStr(
            this string inputStr,
            IdxRetriever<char, string> splitter)
        {
            Tuple<string, string> retTpl;
            int inputLen = inputStr.Length;

            int idx = splitter(inputStr, inputLen);

            if (idx >= 0)
            {
                string firstStr = inputStr.Substring(0, idx);
                string secondStr = inputStr.Substring(idx);

                retTpl = new Tuple<string, string>(
                    firstStr, secondStr);
            }
            else
            {
                retTpl = new Tuple<string, string>(
                    inputStr, null);
            }

            return retTpl;
        }

        public static string SubStr(
            this string inputStr,
            int startIdx,
            int count,
            bool trimEntry = false)
        {
            int startIdxVal, length;

            if (startIdx >= 0)
            {
                startIdxVal = startIdx;

                if (count >= 0)
                {
                    length = count;
                }
                else
                {
                    length = inputStr.Length + count - startIdx;
                }
            }
            else
            {
                if (count >= 0)
                {
                    length = count;
                    startIdxVal = inputStr.Length + startIdx;
                }
                else
                {
                    length = -1 * count;
                    startIdxVal = inputStr.Length + startIdx - length;
                }
            }

            string subStr = inputStr.Substring(startIdxVal, length);

            if (trimEntry)
            {
                subStr = subStr.Trim();
            }

            return subStr;
        }
    }
}
