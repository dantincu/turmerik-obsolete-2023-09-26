using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Dependencies
{
    /// <summary>
    /// To be used ONLY where the Host Builder patter is not applicable (like in unit tests)
    /// </summary>
    public class SimpleServiceProviderContainer
    {
        private readonly object syncRoot = new object();

        private IServiceProvider serviceProvider;

        protected SimpleServiceProviderContainer()
        {
        }

        public void RegisterServices(IServiceCollection services)
        {
            lock (syncRoot)
            {
                if (serviceProvider == null)
                {
                    serviceProvider = services.BuildServiceProvider();
                }
                else
                {
                    throw new InvalidOperationException(
                        "The service collection can only be registered once");
                }
            }
        }

        public void AssureServicesRegistered(IServiceCollection services)
        {
            lock (syncRoot)
            {
                if (serviceProvider == null)
                {
                    serviceProvider = services.BuildServiceProvider();
                }
            }
        }

        public IServiceProvider Services
        {
            get
            {
                var serviceProvider = this.serviceProvider;

                if (serviceProvider == null)
                {
                    lock (syncRoot)
                    {
                        serviceProvider = this.serviceProvider;
                    }
                }

                if (serviceProvider == null)
                {
                    throw new InvalidOperationException(
                        "The service collection must be registered before use");
                }

                return serviceProvider;
            }
        }
    }
}
