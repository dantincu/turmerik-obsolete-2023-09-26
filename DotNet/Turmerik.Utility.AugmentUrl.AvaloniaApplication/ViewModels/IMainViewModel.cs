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
        string ResourceTitle { get; set; }
        string TitleAndUrl { get; set; }
        string TitleAndUrlTemplate { get; set; }
        string OutputText { get; set; }
        IBrush DefaultOutputTextForeground { get; set; }
        IBrush SuccessOutputTextForeground { get; set; }
        IBrush ErrorOutputTextForeground { get; set; }
        IBrush OutputTextForeground { get; set; }

        
        ReactiveCommand<Unit, Unit> Fetch { get; }
        ReactiveCommand<Unit, Unit> RawUrlToClipboard { get; }
        ReactiveCommand<Unit, Unit> ResourceTitleToClipboard { get; }
        ReactiveCommand<Unit, Unit> TitleAndUrlToClipboard { get; }
        ReactiveCommand<Unit, Unit> RawUrlFromClipboard { get; }
    }
}
