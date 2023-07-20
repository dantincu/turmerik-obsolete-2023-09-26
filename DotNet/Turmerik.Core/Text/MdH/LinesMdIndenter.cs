using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Turmerik.RegexH;
using Turmerik.Utils;

namespace Turmerik.Text.MdH
{
    public interface ILinesMdIndenter
    {
        string AddIdent(
            string line,
            int emphasizeLinkMaxLength);
    }

    public class LinesMdIndenter : ILinesMdIndenter
    {
        private readonly ITextReplacerComponent textReplacer;

        public LinesMdIndenter(
            ITextReplacerComponent textReplacer)
        {
            this.textReplacer = textReplacer ?? throw new ArgumentNullException(nameof(textReplacer));
        }

        public string AddIdent(
            string line,
            int emphasizeLinkMaxLength)
        {
            string retLine = line;

            if (emphasizeLinkMaxLength > 0)
            {
                var opts = GetTextReplaceOpts(line, emphasizeLinkMaxLength);
                retLine = textReplacer.ReplaceText(opts);
            }
            
            retLine = string.Concat(" > ", retLine);
            return retLine;
        }

        private ReplaceTextOpts GetTextReplaceOpts(
            string line,
            int emphasizeLinkMaxLength) => new ReplaceTextOpts
            {
                InputText = line,
                SearchedTextStartDelim = "[",
                SearchedTextEndDelim = "]",
                TextReplacerFactory = args => EmphasizeTextIfReq(
                    args.MatchingText,
                    emphasizeLinkMaxLength)
            };

        private string EmphasizeTextIfReq(
            string matchingText,
            int emphasizeLinkMaxLength)
        {
            if (matchingText.Length <= emphasizeLinkMaxLength)
            {
                matchingText = $"<u>{matchingText}</u>";
            }

            return matchingText;
        }
    }
}
