using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Turmerik.Collections;

namespace Turmerik.RegexH
{
    public class RegexGroup
    {
        public RegexGroup(
            bool success,
            ReadOnlyCollection<RegexCapture> captures)
        {
            Success = success;
            Captures = captures;
        }

        public bool Success { get; }
        public ReadOnlyCollection<RegexCapture> Captures { get; }

        public static RegexGroup FromSrc(
            Group src) => new RegexGroup(
                src.Success,
                RegexCapture.FromSrcCollctn(
                    src.Captures));

        public static ReadOnlyCollection<RegexGroup> FromSrcCollctn(
            GroupCollection srcCollctn)
        {
            var list = new List<RegexGroup>();

            foreach (Group src in srcCollctn)
            {
                list.Add(FromSrc(src));
            }

            return list.RdnlC();
        }
    }
}
