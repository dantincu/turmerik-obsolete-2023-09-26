using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Dependencies;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.MsVSTextTemplating.Components;

namespace Turmerik.MsVSTextTemplating
{
    public class ServiceProviderContainer : SimpleServiceProviderContainer
    {
        public static readonly Lazy<ServiceProviderContainer> Instance = new Lazy<ServiceProviderContainer>(
            () => new ServiceProviderContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

        private ServiceProviderContainer()
        {
        }

        public static void AssureServicesRegistered(Action<IServiceCollection> callback = null)
        {
            var services = new ServiceCollection();
            MsVsTextTemplatingServiceCollectionBuilder.RegisterAll(services);

            callback?.Invoke(services);
            Instance.Value.AssureServicesRegistered(services);
        }
    }
}
