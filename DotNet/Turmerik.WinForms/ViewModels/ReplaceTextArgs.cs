using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turmerik.WinForms.ViewModels
{
    public readonly struct ReplaceTextArgs
    {
        public ReplaceTextArgs(
            string inputText,
            int inputLength,
            string matchingText,
            MatchCollection matches,
            int matchingStartIdx)
        {
            InputText = inputText;
            InputLength = inputLength;
            MatchingText = matchingText;
            Matches = matches;
            MatchingStartIdx = matchingStartIdx;
        }

        public string InputText { get; }
        public int InputLength { get; }
        public string MatchingText { get; }
        public MatchCollection Matches { get; }
        public int MatchingStartIdx { get; }
    }
}
