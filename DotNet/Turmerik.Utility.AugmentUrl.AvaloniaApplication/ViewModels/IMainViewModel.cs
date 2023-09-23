using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Avalonia.ActionComponent;
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
        ITrmrkAvlnActionComponentsManagerRetriever ActionComponentsManagerRetriever { get; set; }

        ReactiveCommand<Unit, Unit> RawUrlToClipboard { get; }
        ReactiveCommand<Unit, Unit> RawUrlFromClipboard { get; }
    }
}
