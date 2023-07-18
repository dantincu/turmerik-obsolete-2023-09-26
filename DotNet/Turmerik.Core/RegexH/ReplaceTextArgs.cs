using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turmerik.RegexH
{
    public readonly struct ReplaceTextArgs
    {
        public ReplaceTextArgs(
            string inputText,
            int inputLength,
            string matchingText,
            int matchingStartIdx)
        {
            InputText = inputText;
            InputLength = inputLength;
            MatchingText = matchingText;
            MatchingStartIdx = matchingStartIdx;
        }

        public string InputText { get; }
        public int InputLength { get; }
        public string MatchingText { get; }
        public int MatchingStartIdx { get; }
    }
}
