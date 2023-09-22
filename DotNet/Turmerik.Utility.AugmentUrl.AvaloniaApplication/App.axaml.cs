using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
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
        var svcProv = ServiceProviderContainer.Instance.Value.AssureServicesRegisteredCore(services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = svcProv.GetRequiredService<IMainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = svcProv.GetRequiredService<IMainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
