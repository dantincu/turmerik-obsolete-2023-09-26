using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Turmerik.Collections;

namespace Turmerik.RegexH
{
    public class RegexMatch
    {
        public RegexMatch(
            ReadOnlyCollection<RegexGroup> groups)
        {
            Groups = groups;
        }

        public ReadOnlyCollection<RegexGroup> Groups { get; }

        public static RegexMatch FromSrc(
            Match src) => new RegexMatch(
                RegexGroup.FromSrcCollctn(
                    src.Groups));
    }
}
