using Avalonia.Controls;
using Turmerik.Avalonia.ActionComponent;
using Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;
using Turmerik.Avalonia.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using Turmerik.Utils;
using System;
using Avalonia.Media;
using Avalonia.Controls.Primitives;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.Views;

public partial class MainView : UserControl
{
    private readonly IServiceProvider svcProv;

    private ITrmrkAvlnActionComponentsManagerRetriever actionComponentsManagerRetriever;
    private IMainViewModel viewModel;

    public MainView()
    {
        svcProv = ServiceProviderContainer.Instance.Value.Services;
        InitializeComponent();

        this.Loaded += MainView_Loaded;
    }

    #region UI Event Handlers

    private void MainView_Loaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (viewModel == null)
        {
            actionComponentsManagerRetriever = svcProv.GetRequiredService<ITrmrkAvlnActionComponentsManagerRetriever>();

            actionComponentsManagerRetriever.MsgTextBoxDefaultForeground = new SolidColorBrush(
                Color.FromArgb(255, 0, 0, 255));

            actionComponentsManagerRetriever.MsgTextBoxSuccessForeground = new SolidColorBrush(
                Color.FromArgb(255, 0, 255, 0));

            actionComponentsManagerRetriever.MsgTextBoxErrorForeground = new SolidColorBrush(
                Color.FromArgb(255, 255, 0, 0));

            actionComponentsManagerRetriever.MsgTextBoxContentGetter = () => viewModel.OutputText;
            actionComponentsManagerRetriever.MsgTextBoxContentSetter = value => viewModel.OutputText = value;

            actionComponentsManagerRetriever.MsgTextBoxForegroundGetter = () => viewModel.OutputTextForeground;
            actionComponentsManagerRetriever.MsgTextBoxForegroundSetter = value => viewModel.OutputTextForeground = value;

            viewModel = DataContext as IMainViewModel;
            viewModel.TopLevel = TopLevel.GetTopLevel(this);

            viewModel.DefaultOutputTextForeground = actionComponentsManagerRetriever.MsgTextBoxDefaultForeground;
            viewModel.SuccessOutputTextForeground = actionComponentsManagerRetriever.MsgTextBoxSuccessForeground;
            viewModel.ErrorOutputTextForeground = actionComponentsManagerRetriever.MsgTextBoxErrorForeground;

            viewModel.OutputTextForeground = actionComponentsManagerRetriever.MsgTextBoxDefaultForeground;
            viewModel.ActionComponentsManagerRetriever = actionComponentsManagerRetriever;
        }
    }

    #endregion UI Event Handlers
}
