using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turmerik.RegexH
{
    public readonly struct ReplaceRegexArgs
    {
        public ReplaceRegexArgs(
            string inputText,
            int inputLength,
            ReadOnlyCollection<RegexMatch> matchesCollctn,
            KeyValuePair<int, RegexMatch> match)
        {
            InputText = inputText;
            InputLength = inputLength;
            MatchesCollctn = matchesCollctn;
            Match = match;
        }

        public string InputText { get; }
        public int InputLength { get; }
        public ReadOnlyCollection<RegexMatch> MatchesCollctn { get; }
        public KeyValuePair<int, RegexMatch> Match { get; }
    }
}
