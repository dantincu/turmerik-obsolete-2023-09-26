using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;

public class MainViewModel : ViewModelBase, IMainViewModel
{
    private readonly IBrush errorOutputTextForeground = new SolidColorBrush(
        Color.FromArgb(255, 255, 0, 0));

    private string rawUrl;
    private string outputText;
    private IBrush outputTextForeground;
    private IBrush initialOutputTextForeground;

    public MainViewModel()
    {
    }

    public TopLevel TopLevel { get; set; }

    public string RawUrl
    {
        get => rawUrl;

        set => this.RaiseAndSetIfChanged(
            ref rawUrl,
            value,
            nameof(RawUrl));
    }

    public string OutputText
    {
        get => outputText;

        set => this.RaiseAndSetIfChanged(
            ref outputText,
            value,
            nameof(OutputText));
    }

    public IBrush OutputTextForeground
    {
        get => outputTextForeground;

        set 
        {
            initialOutputTextForeground ??= value;

            this.RaiseAndSetIfChanged(
                ref outputTextForeground,
                value,
                nameof(OutputTextForeground));
        }
    }

    public async Task FromClipboardAsync()
    {
        string rawUrl = await TopLevel.Clipboard.GetTextAsync();
        RawUrl = rawUrl;

        Uri uri = null;

        try
        {
            uri = new Uri(rawUrl); 
        }
        catch
        {

        }
    }
}
