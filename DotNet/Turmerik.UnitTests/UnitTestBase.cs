using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Infrastucture;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.Testing.Core;

namespace Turmerik.UnitTests.Tests
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
