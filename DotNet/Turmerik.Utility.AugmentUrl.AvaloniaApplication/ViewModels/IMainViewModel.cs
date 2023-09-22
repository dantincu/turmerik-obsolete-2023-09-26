using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels
{
    public interface IMainViewModel : IViewModel
    {
        TopLevel TopLevel { get; set; }
        string RawUrl { get; set; }
        string OutputText { get; set; }
        IBrush OutputTextForeground { get; set; }

        Task FromClipboardAsync();
    }
}
