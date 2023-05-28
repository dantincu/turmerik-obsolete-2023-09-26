using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Dependencies;

namespace Turmerik.MauiCore.Dependencies
{
    public class ServiceProviderContainer : SimpleServiceProviderContainer
    {
        public static readonly Lazy<ServiceProviderContainer> Instance = new Lazy<ServiceProviderContainer>(
            () => new ServiceProviderContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

        protected ServiceProviderContainer()
        {
        }
    }
}
