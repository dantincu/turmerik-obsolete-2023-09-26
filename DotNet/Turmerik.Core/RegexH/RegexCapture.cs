using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Turmerik.Collections;

namespace Turmerik.RegexH
{
    public class RegexCapture
    {
        public RegexCapture(
            int index,
            int length,
            string value)
        {
            Index = index;
            Length = length;
            Value = value;
        }

        public int Index { get; }
        public int Length { get; }
        public string Value { get; }

        public static RegexCapture FromSrc(
            Capture src) => new RegexCapture(
                src.Index,
                src.Length,
                src.Value);

        public static ReadOnlyCollection<RegexCapture> FromSrcCollctn(
            CaptureCollection srcCollctn)
        {
            var list = new List<RegexCapture>();

            foreach (Capture src in srcCollctn)
            {
                list.Add(FromSrc(src));
            }

            return list.RdnlC();
        }
    }
}
