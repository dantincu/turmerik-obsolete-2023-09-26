using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.RegexH
{
    public interface ITextReplacerComponent
    {
        string ReplaceText(ReplaceTextOpts opts);
    }

    public class TextReplacerComponent : ITextReplacerComponent
    {
        public string ReplaceText(ReplaceTextOpts opts)
        {
            NormalizeOpts(opts);
            string result;

            if (opts.SearchedTextRegex != null)
            {
                result = ReplaceSearchedRegex(opts);
            }
            else
            {
                result = ReplaceSearchedText(opts);
            }

            return result;
        }

        private string ReplaceSearchedRegex(
            ReplaceTextOpts opts)
        {
            var inputText = opts.InputText;
            int inputLen = inputText.Length;
            var searchedRegex = opts.SearchedTextRegex;
            var regexReplacerFactory = opts.RegexReplacerFactory;

            var matchCllctn = searchedRegex.Matches(inputText);
            var matchesCollctn = RegexMatch.FromSrcCollctn(matchCllctn);
            int matchesCount = matchCllctn.Count;
            string result = inputText;

            for (int i = 0; i < matchesCount; i++)
            {
                var match = matchCllctn[i];
                var regexMatch = matchesCollctn[i];

                var args = new ReplaceRegexArgs(
                    inputText,
                    inputLen,
                    matchesCollctn,
                    new KeyValuePair<int, RegexMatch>(
                        i, regexMatch));

                string replacingText = regexReplacerFactory(args);
                replacingText = NormalizeReplacingText(opts, replacingText);

                result = match.Result(replacingText);
            }

            return result;
        }

        private string ReplaceSearchedText(
            ReplaceTextOpts opts)
        {
            var list = new List<string>();
            var inputText = opts.InputText;
            int inputLen = inputText.Length;
            var searchedText = opts.SearchedText;
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
                prevIdx = idx;
            }

            list.Add(
                inputText.Substring(
                    prevIdx));

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
