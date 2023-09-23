using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Turmerik.Avalonia.Dependencies;
using Turmerik.Utility.AugmentUrl.AvaloniaApplication.Dependencies;
using Turmerik.Utility.AugmentUrl.AvaloniaApplication.ViewModels;
using Turmerik.Utility.AugmentUrl.AvaloniaApplication.Views;

namespace Turmerik.Utility.AugmentUrl.AvaloniaApplication;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        AugmentUrlServiceCollectionBuilder.RegisterAll(services);
        ServiceProviderContainer.Instance.Value.AssureServicesRegisteredCore(services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
