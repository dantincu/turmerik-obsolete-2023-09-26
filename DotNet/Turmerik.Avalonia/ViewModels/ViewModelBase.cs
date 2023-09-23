using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Turmerik.Avalonia.Dependencies;

namespace Turmerik.Avalonia.ViewModels;

public class ViewModelBase : ReactiveObject, IViewModel
{
    public ViewModelBase()
    {
        SvcProv = ServiceProviderContainer.Instance.Value.Services;
        // ActionComponentFactory = SvcProv.GetRequiredService<ITrmrkAvlnActionComponentFactory>();
    }

    protected IServiceProvider SvcProv { get; }
    // protected ITrmrkAvlnActionComponentFactory ActionComponentFactory { get; }
}
