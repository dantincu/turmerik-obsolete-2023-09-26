using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Text
{
    public readonly struct SliceStrArgs
    {
        public SliceStrArgs(
            string inputStr,
            int inputLen,
            int startIdx,
            char @char,
            int idx)
        {
            InputStr = inputStr;
            InputLen = inputLen;
            StartIdx = startIdx;
            Char = @char;
            Idx = idx;
        }

        public string InputStr { get; }
        public int InputLen { get; }
        public int StartIdx { get; }
        public char Char { get; }
        public int Idx { get; }
    }
}
