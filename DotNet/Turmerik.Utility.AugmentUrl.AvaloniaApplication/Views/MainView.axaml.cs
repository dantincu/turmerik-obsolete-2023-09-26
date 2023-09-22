using Avalonia.Controls;
using Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication.Views;

public partial class MainView : UserControl
{
    private IMainViewModel viewModel;

    public MainView()
    {
        InitializeComponent();

        this.Initialized += MainView_Initialized;
        this.Loaded += MainView_Loaded;
    }

    #region UI Event Handlers

    private void MainView_Initialized(object? sender, System.EventArgs e)
    {
    }

    private void MainView_Loaded(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.viewModel = this.DataContext as IMainViewModel;
        this.viewModel.TopLevel = TopLevel.GetTopLevel(this);
    }

    #endregion UI Event Handlers
}
