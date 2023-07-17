using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turmerik.WinForms.ViewModels
{
    public readonly struct ReplaceRegexArgs
    {
        public ReplaceRegexArgs(
            string inputText,
            int inputLength,
            MatchCollection matchesCollctn,
            KeyValuePair<int, Match> match,
            KeyValuePair<int, Group> group,
            KeyValuePair<int, Capture> capture)
        {
            InputText = inputText;
            InputLength = inputLength;
            MatchesCollctn = matchesCollctn;
            Match = match;
            Group = group;
            Capture = capture;
        }

        public string InputText { get; }
        public int InputLength { get; }
        public MatchCollection MatchesCollctn { get; }
        public KeyValuePair<int, Match> Match { get; }
        public KeyValuePair<int, Group> Group { get; }
        public KeyValuePair<int, Capture> Capture { get; }
    }
}
