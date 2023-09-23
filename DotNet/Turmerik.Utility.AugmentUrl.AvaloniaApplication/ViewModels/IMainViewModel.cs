using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Avalonia.ViewModels;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels
{
    public interface IMainViewModel : IViewModel
    {
        TopLevel TopLevel { get; set; }
        string RawUrl { get; set; }
        string OutputText { get; set; }
        IBrush DefaultOutputTextForeground { get; set; }
        IBrush SuccessOutputTextForeground { get; set; }
        IBrush ErrorOutputTextForeground { get; set; }
        IBrush OutputTextForeground { get; set; }

        string TitleAndUrlTemplate { get; set; }
        ReactiveCommand<Unit, Unit> Fetch { get; }
        ReactiveCommand<Unit, Unit> TitleToClipboard { get; }
        ReactiveCommand<Unit, Unit> AllToClipboard { get; }
        ReactiveCommand<Unit, Unit> RawUrlToClipboard { get; }
        ReactiveCommand<Unit, Unit> RawUrlFromClipboard { get; }
    }
}
