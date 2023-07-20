using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.RegexH
{
    public class RegexMatch
    {
        public RegexMatch(
            ReadOnlyCollection<RegexCapture> captures,
            ReadOnlyCollection<RegexGroup> groups,
            RegexCapture mainCapture)
        {
            Captures = captures;
            Groups = groups;
            MainCapture = mainCapture;
        }

        public ReadOnlyCollection<RegexCapture> Captures { get; }
        public ReadOnlyCollection<RegexGroup> Groups { get; }

        public RegexCapture MainCapture { get; }

        public static RegexMatch FromSrc(
            Match src) => RegexCapture.FromSrcCollctn(
                    src.Captures).WithValue(
                capturesCllctn => new RegexMatch(
                    capturesCllctn,
                    RegexGroup.FromSrcCollctn(
                        src.Groups),
                    capturesCllctn.OrderByDescending(
                        capture => capture.Length).First()));

        public static ReadOnlyCollection<RegexMatch> FromSrcCollctn(
            MatchCollection srcCollctn)
        {
            var list = new List<RegexMatch>();

            foreach (Match src in srcCollctn)
            {
                list.Add(FromSrc(src));
            }

            return list.RdnlC();
        }
    }
}
