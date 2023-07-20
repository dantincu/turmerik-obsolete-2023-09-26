using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.RegexH
{
    public interface ITextReplacerComponent
    {
        string ReplaceText(ReplaceTextOpts opts);
    }

    public class TextReplacerComponent : ITextReplacerComponent
    {
        private readonly ISliceStrResultEqCompr sliceStrResultEqCompr;

        public TextReplacerComponent(
            ISliceStrResultEqCompr sliceStrResultEqCompr)
        {
            this.sliceStrResultEqCompr = sliceStrResultEqCompr ?? throw new ArgumentNullException(nameof(sliceStrResultEqCompr));
        }

        public string ReplaceText(ReplaceTextOpts opts)
        {
            NormalizeOpts(opts);
            string result;

            if (opts.SearchedTextRegex != null)
            {
                result = ReplaceSearchedRegex(opts);
            }
            else if (opts.SearchedText != null)
            {
                result = ReplaceSearchedText(opts);
            }
            else if (!string.IsNullOrEmpty(opts.InputText))
            {
                result = ReplaceTextCore(opts);
            }
            else
            {
                result = opts.InputText;
            }

            return result;
        }

        private string ReplaceSearchedRegex(
            ReplaceTextOpts opts)
        {
            var list = new List<string>();
            var inputText = opts.InputText;
            int inputLen = inputText.Length;
            var searchedRegex = opts.SearchedTextRegex;
            var regexReplacerFactory = opts.RegexReplacerFactory;

            var matchCllctn = searchedRegex.Matches(inputText);
            var matchesCollctn = RegexMatch.FromSrcCollctn(matchCllctn);
            int matchesCount = matchCllctn.Count;

            int endIdx = 0;
            int prevStartIdx = 0;

            for (int i = 0; i < matchesCount; i++)
            {
                var match = matchCllctn[i];
                var regexMatch = matchesCollctn[i];

                var capture = regexMatch.MainCapture;
                endIdx = capture.Index;

                string text = inputText.Substring(
                    prevStartIdx,
                    endIdx - prevStartIdx);

                list.Add(text);
                prevStartIdx = endIdx + capture.Length;

                var args = new ReplaceRegexArgs(
                    inputText,
                    inputLen,
                    matchesCollctn,
                    new KeyValuePair<int, RegexMatch>(
                        i, regexMatch));

                string replacingText = regexReplacerFactory(args);
                replacingText = NormalizeReplacingText(opts, replacingText);
                list.Add(replacingText);
            }

            string result = string.Concat(list);
            return result;
        }

        private string ReplaceSearchedText(
            ReplaceTextOpts opts)
        {
            var list = new List<string>();
            var inputText = opts.InputText;
            int inputLen = inputText.Length;
            var searchedText = opts.SearchedText;
            int searchedLen = searchedText.Length;
            var textReplacerFactory = opts.TextReplacerFactory;

            int prevIdx = 0;

            int idx = inputText.IndexOf(
                searchedText);

            while (idx >= 0)
            {
                list.Add(
                    inputText.Substring(
                        prevIdx,
                        idx - prevIdx));

                string replacingText = textReplacerFactory(
                    new ReplaceTextArgs(
                        inputText,
                        inputLen,
                        searchedText,
                        idx));

                replacingText = NormalizeReplacingText(opts, replacingText);

                list.Add(replacingText);
                prevIdx = idx + searchedLen;

                idx = inputText.IndexOf(
                    searchedText,
                    prevIdx);
            }

            list.Add(
                inputText.Substring(
                    prevIdx));

            string result = string.Concat(list);
            return result;
        }

        private string ReplaceTextCore(ReplaceTextOpts opts)
        {
            var list = new List<string>();
            var inputText = opts.InputText;
            int inputLen = inputText.Length;
            string searchedText = null;

            var searchedTextStartCharPredicate = opts.SearchedTextStartCharPredicate;
            var searchedTextEndCharPredicate = opts.SearchedTextEndCharPredicate;

            var textReplacerFactory = opts.TextReplacerFactory;

            SliceStrResult prevStart = default;
            SliceStrResult start = default;

            start = inputText.SliceStr(
                searchedTextStartCharPredicate,
                searchedTextEndCharPredicate, 0);

            bool sameResult = false;

            while (!sameResult && start.SlicedStr != null)
            {
                list.Add(
                    inputText.Substring(
                        prevStart.EndIdx,
                        start.StartIdx - prevStart.EndIdx));

                string replacingText = textReplacerFactory(
                    new ReplaceTextArgs(
                        inputText,
                        inputLen,
                        start.SlicedStr,
                        start.StartIdx));

                replacingText = NormalizeReplacingText(opts, replacingText);

                list.Add(replacingText);
                prevStart = start;

                start = inputText.SliceStr(
                    searchedTextStartCharPredicate,
                    searchedTextEndCharPredicate,
                    prevStart.EndIdx);

                sameResult = sliceStrResultEqCompr.Equals(
                    prevStart,
                    start);
            }

            list.Add(
                inputText.Substring(
                    prevStart.EndIdx));

            string result = string.Concat(list);
            return result;
        }

        private string NormalizeReplacingText(
            ReplaceTextOpts opts,
            string replacingText)
        {
            if (opts.TreatTextReplacerAsDblQuoteEscaped)
            {
                replacingText = replacingText.Replace(
                    "\\\\", "\\").Replace("\\\"", "\"");
            }

            return replacingText;
        }

        private void NormalizeOpts(ReplaceTextOpts opts)
        {
        }
    }
}
