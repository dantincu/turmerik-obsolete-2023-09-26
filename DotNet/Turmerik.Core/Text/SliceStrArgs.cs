using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Text
{
    public readonly struct SliceStrArgs
    {
        public SliceStrArgs(string inputStr, int inputLen, char @char, int idx)
        {
            InputStr = inputStr;
            InputLen = inputLen;
            Char = @char;
            Idx = idx;
        }

        public string InputStr { get; }
        public int InputLen { get; }
        public char Char { get; }
        public int Idx { get; }
    }
}
