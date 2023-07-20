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
            }
            
            retLine = string.Concat(" > ", retLine);
            return retLine;
        }
    }
}
