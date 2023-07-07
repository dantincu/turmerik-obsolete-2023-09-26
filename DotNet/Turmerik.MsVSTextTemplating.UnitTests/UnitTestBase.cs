using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Testing.NetFx;

namespace Turmerik.MsVSTextTemplating.UnitTests
{
    public abstract class UnitTestBase : UnitTestCoreBase
    {
        protected UnitTestBase()
        {
            AppEnv = ServiceProvider.GetRequiredService<IAppEnv>();
        }

        protected override IServiceProvider ServiceProvider => Turmerik.MsVSTextTemplating.ServiceProviderContainer.Instance.Value.Services;

        protected IAppEnv AppEnv { get; }
    }
}
