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
using Turmerik.Avalonia.ActionComponent;
using Turmerik.Avalonia.ViewModels;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;

public class MainViewModel : ViewModelBase, IMainViewModel
{
    private ITrmrkAvlnActionComponentsManagerRetriever actionComponentsManagerRetriever;

    private string rawUrl;
    private string outputText;
    private IBrush outputTextForeground;

    private ITrmrkAvlnActionComponent actionComponent;

    public MainViewModel()
    {
        RawUrlToClipboard = ReactiveCommand.Create(
            () =>
            {
                RawUrlToClipboardAsync();
            });

        RawUrlFromClipboard = ReactiveCommand.Create(
            () =>
            {
                RawUrlFromClipboardAsync();
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

    public ITrmrkAvlnActionComponentsManagerRetriever ActionComponentsManagerRetriever
    {
        get => actionComponentsManagerRetriever;

        set
        {
            actionComponentsManagerRetriever = value;
            actionComponent = ActionComponentFactory.Create(null);
        }
    }

    public ReactiveCommand<Unit, Unit> RawUrlToClipboard { get; }
    public ReactiveCommand<Unit, Unit> RawUrlFromClipboard { get; }

    private Task RawUrlToClipboardAsync() => actionComponent.ExecuteAsync(new TrmrkAsyncActionComponentOpts
    {
        Action = async() =>
        {
            await TopLevel.Clipboard.SetTextAsync(RawUrl);
            return new TrmrkActionResult();
        },
    }.LogMsgFactory(map => "the raw url to clipboard".ActWithValue(
        actionName => map.AddBeforeExecution(
            $"Copying {actionName}...").AddBeforeAlways(
            args => string.Join(": ", $"Trying to copy {actionName} failed", args.Exc?.Message),
            args => $"Copied {actionName}"))));

    private async Task RawUrlFromClipboardAsync()
    {
        Uri uri = null;

        await actionComponent.ExecuteAsync(new TrmrkAsyncActionComponentOpts
        {
            Validation = async() =>
            {
                string rawUrl = await TopLevel.Clipboard.GetTextAsync();

                RawUrl = rawUrl;
                uri = new Uri(rawUrl);

                return new TrmrkActionResult();
            },
            Action = async () =>
            {
                var web = new HtmlWeb();
                var doc = web.Load(uri);

                foreach (var node in doc.DocumentNode.ChildNodes)
                {

                }

                return new TrmrkActionResult();
            },
        }.LogMsgFactory(map => map.AddBeforeExecution(
            "Retrieving the url from clipboard...").AddValidation(
            args => string.Join(": ", "The clipboard does not contain a valid url", args.Exc?.Message)).AddAfterAlways(
            args => string.Join(": ", "Trying to fetch the resource from the provided url failed", args.Exc?.Message),
            args => "Fetched the resource from the provided url and copied to clipboard the resource's title and the raw url")));
    }
}
