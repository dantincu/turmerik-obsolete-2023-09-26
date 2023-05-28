using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Infrastucture;
using Turmerik.LocalDevice.Dependencies;
using Turmerik.Testing;

namespace Turmerik.LocalDevice.UnitTests
{
    public class UnitTestBase : UnitTestCoreBase
    {
        static UnitTestBase()
        {
            RegisterAllServices();
        }

        public UnitTestBase()
        {
            Services = ServiceProviderContainer.Instance.Value.Services;
        }

        protected IServiceProvider Services { get; }

        private static void RegisterAllServices()
        {
            var services = new ServiceCollection();
            LocalDeviceServiceCollectionBuilder.RegisterAll(services);

            ServiceProviderContainer.Instance.Value.RegisterServices(services);
        }
    }
}
