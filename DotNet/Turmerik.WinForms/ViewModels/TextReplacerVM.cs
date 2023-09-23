using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.RegexH;
using Turmerik.Utils;

namespace Turmerik.WinForms.ViewModels
{
    public interface ITextReplacerVM
    {
        ITrmrkActionResult<string> ReplaceText(ReplaceTextOpts opts);
    }

    public class TextReplacerVM : ViewModelBase, ITextReplacerVM
    {
        private readonly ITextReplacerComponent textReplacer;

        public TextReplacerVM(
            IAppLoggerCreator appLoggerFactory/* ,
            IWinFormsActionComponentFactory actionComponentFactory*/,
            ITextReplacerComponent textReplacer) : base(
                appLoggerFactory/* ,
                actionComponentFactory*/)
        {
            this.textReplacer = textReplacer ?? throw new ArgumentNullException(
                nameof(textReplacer));
        }

        public ITrmrkActionResult<string> ReplaceText(
            ReplaceTextOpts opts)
        {
            throw new NotImplementedException();
        } /* => ActionComponent.Execute(new TrmrkActionComponentOpts<string>
            {
                Action = () => new TrmrkActionResult<string>
                {
                    Data = textReplacer.ReplaceText(opts),
                },
                ActionName = nameof(ReplaceText)
            }); */
    }
}
