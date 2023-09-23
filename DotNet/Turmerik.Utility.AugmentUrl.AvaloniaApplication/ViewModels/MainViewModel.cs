using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Turmerik.Avalonia.ViewModels;
using Turmerik.Utils;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;

public class MainViewModel : ViewModelBase, IMainViewModel
{
    private string rawUrl;
    private string outputText;
    private IBrush outputTextForeground;

    public MainViewModel()
    {
        TitleAndUrlTemplate = "[{0}]({1})";

        RawUrlToClipboard = ReactiveCommand.Create(
            () =>
            {
                RawUrlToClipboardAsync();
            });

        RawUrlFromClipboard = ReactiveCommand.Create(
            () =>
            {
                FetchResourceAsync(
                    TopLevel.Clipboard.GetTextAsync,
                    "Retrieving the url from clipboard...",
                    exc => $"Could not the url from clipboard: {exc.Message}",
                    true);
            });
    }

    public TopLevel TopLevel { get; set; }

    public string RawUrl
    {
        get => rawUrl;

        set => this.RaiseAndSetIfChanged(
            ref rawUrl,
            value);
    }

    public string OutputText
    {
        get => outputText;

        set => this.RaiseAndSetIfChanged(
            ref outputText,
            value);
    }

    public IBrush OutputTextForeground
    {
        get => outputTextForeground;

        set 
        {
            this.RaiseAndSetIfChanged(
                ref outputTextForeground,
                value);
        }
    }

    public IBrush DefaultOutputTextForeground { get; set; }
    public IBrush SuccessOutputTextForeground { get; set; }
    public IBrush ErrorOutputTextForeground { get; set; }

    public string TitleAndUrlTemplate { get; set; }
    public ReactiveCommand<Unit, Unit> Fetch { get; }
    public ReactiveCommand<Unit, Unit> TitleToClipboard { get; }
    public ReactiveCommand<Unit, Unit> AllToClipboard { get; }
    public ReactiveCommand<Unit, Unit> RawUrlToClipboard { get; }
    public ReactiveCommand<Unit, Unit> RawUrlFromClipboard { get; }

    private void ShowUserMsg(string text, bool? isSuccess)
    {
        OutputText = text;

        if (isSuccess.HasValue)
        {
            if (isSuccess.Value)
            {
                OutputTextForeground = SuccessOutputTextForeground;
            }
            else
            {
                OutputTextForeground = ErrorOutputTextForeground;
            }
        }
        else
        {
            OutputTextForeground = DefaultOutputTextForeground;
        }
    }

    private async Task RawUrlToClipboardAsync()
    {
        try
        {
            ShowUserMsg("Copying the provided url to clipboard...", null);
            await TopLevel.Clipboard.SetTextAsync(RawUrl);
            ShowUserMsg("Copyied the provided url to clipboard", true);
        }
        catch (Exception exc)
        {
            ShowUserMsg($"Something went wrong while copying the provided url to clipboard: {exc.Message}", false);
        }
    }

    private async Task AllToClipboardAsync(string titleAndUrl)
    {
        try
        {
            ShowUserMsg("Copying the title and url to clipboard...", null);
            await TopLevel.Clipboard.SetTextAsync(titleAndUrl);
            ShowUserMsg("Copied the title and url to clipboard", true);
        }
        catch (Exception exc)
        {
            ShowUserMsg($"Could not copy the title and url to clipboard: {exc.Message}", false);
        }
    }

    private async Task FetchResourceAsync(
        Func<Task<string>> rawUrlRetriever,
        string initMsg,
        Func<Exception, string> retrieveUrlErrMsgFactory,
        bool copyResultToClipboard)
    {
        Uri uri = null;
        string rawUrl = null;
        string title = null;

        try
        {
            ShowUserMsg(initMsg, null);
            rawUrl = await rawUrlRetriever();
        }
        catch (Exception exc)
        {
            ShowUserMsg(retrieveUrlErrMsgFactory(exc), false);
        }

        if (rawUrl != null)
        {
            try
            {
                ShowUserMsg("Validating the provided url...", null);
                uri = new Uri(rawUrl);
            }
            catch (Exception exc)
            {
                ShowUserMsg($"The provided url is invalid: {exc.Message}", false);
            }
        }

        if (uri != null)
        {
            try
            {
                ShowUserMsg("Retrieving the resource from url...", null);

                var web = new HtmlWeb();
                var doc = web.Load(uri);

                foreach (var node in doc.DocumentNode.ChildNodes)
                {

                }
            }
            catch (Exception exc)
            {
                ShowUserMsg($"Could not retrieve the resource from url: {exc.Message}", false);
            }
        }

        if (title != null)
        {
            string titleAndUrl = string.Format(
                TitleAndUrlTemplate,
                title);

            if (copyResultToClipboard)
            {
                await AllToClipboardAsync(titleAndUrl);
            }
            else
            {
                ShowUserMsg("Retrieved the resource from url", true);
            }
        }
    }
}
