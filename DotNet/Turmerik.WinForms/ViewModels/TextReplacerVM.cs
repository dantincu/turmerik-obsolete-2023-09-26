using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.ViewModels
{
    public interface ITextReplacerVM
    {
        string ReplaceText(ReplaceTextOpts opts);
    }

    public class TextReplacerVM : ViewModelBase, ITextReplacerVM
    {
        public TextReplacerVM(
            IAppLoggerFactory appLoggerFactory,
            IWinFormsActionComponentFactory actionComponentFactory) : base(
                appLoggerFactory,
                actionComponentFactory)
        {
        }

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
            var list = new List<string>();
            var inputText = opts.InputText;
            int inputLen = inputText.Length;
            var searchedRegex = opts.SearchedTextRegex;
            var textReplacerFactory = opts.RegexReplacerFactory;
            var replacingTextOpts = opts.ReplacingText;

            var matchCollctn = searchedRegex.Matches(inputText);

            foreach (Match match in matchCollctn)
            {
                foreach (Group group in match.Groups)
                {
                    foreach (Capture capture in group.Captures)
                    {

                    }
                }
            }

            int prevIdx = 0;

            int idx = inputText.IndexOf(
                searchedRegex);

            while (idx >= 0)
            {
                list.Add(
                    inputText.Substring(
                        prevIdx,
                        idx - prevIdx));

                string replacingText = replacingTextOpts;

                if (textReplacerFactory != null)
                {
                    replacingText = textReplacerFactory(
                        new ReplaceRegexArgs(
                            inputText,
                            inputLen,
                            searchedRegex,
                            idx));
                }

                list.Add(replacingText);
                prevIdx = idx;
            }

            list.Add(
                inputText.Substring(
                    prevIdx));

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
            var textReplacerFactory = opts.TextReplacerFactory;
            var replacingTextOpts = opts.ReplacingText;

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
                        null,
                        idx));

                list.Add(replacingText);
                prevIdx = idx;
            }

            list.Add(
                inputText.Substring(
                    prevIdx));

            string result = string.Concat(list);
            return result;
        }

        private void NormalizeOpts(ReplaceTextOpts opts)
        {
            if (opts.TreatInputTextAsDblQuoteEscaped)
            {
                opts.InputText = opts.InputText.Replace(
                    "\\\\", "\\").Replace("\\\"", "\"");
            }

            opts.TextReplacerFactory = opts.TextReplacerFactory.FirstNotNull(
                args => opts.ReplacingText);
        }
    }
}
