using Avalonia.Controls;
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
            viewModel = DataContext as IMainViewModel;
            viewModel.TopLevel = TopLevel.GetTopLevel(this);

            viewModel.DefaultOutputTextForeground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            viewModel.SuccessOutputTextForeground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            viewModel.ErrorOutputTextForeground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }
    }

    #endregion UI Event Handlers
}
