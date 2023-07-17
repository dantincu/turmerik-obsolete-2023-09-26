using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turmerik.WinForms.ViewModels
{
    public class ReplaceTextOpts
    {
        public string InputText { get; set; }
        public bool TreatInputTextAsDblQuoteEscaped { get; set; }
        public Regex SearchedTextRegex { get; set; }
        public string SearchedText { get; set; }
        public string ReplacingText { get; set; }
        public Func<ReplaceTextArgs, string> TextReplacerFactory { get; set; }
        public Func<ReplaceRegexArgs, string> RegexReplacerFactory { get; set; }
    }
}
