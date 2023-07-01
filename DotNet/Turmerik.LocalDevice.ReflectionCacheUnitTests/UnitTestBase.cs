using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.Reflection.Cache;
using Turmerik.Reflection;
using Turmerik.Testing;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public class UnitTestBase : UnitTestCoreBase
    {
        protected readonly IBasicEqualityComparerFactory BasicEqComprFactory;
        protected readonly ICachedTypesMap CachedTypesMap;
        protected readonly IMemberAccessibiliyFilterEqualityComparerFactory MemberAccessFilterEqComprFactory;

        protected readonly IMethodAccessibiliyFilterEqualityComparer MethodAccessFilterEqCompr;
        protected readonly IFieldAccessibiliyFilterEqualityComparer FieldAccessFilterEqCompr;
        protected readonly IPropertyAccessibiliyFilterEqualityComparer PropAccessFilterEqCompr;
        protected readonly IEventAccessibiliyFilterEqualityComparer EventAccessFilterEqCompr;

        protected readonly IEqualityComparer<string[]> StringArrEqCompr;

        static UnitTestBase()
        {
            RegisterAllServices();
        }

        public UnitTestBase()
        {
            BasicEqComprFactory = ServiceProvider.GetRequiredService<IBasicEqualityComparerFactory>();
            CachedTypesMap = ServiceProvider.GetRequiredService<ICachedTypesMap>();
            MemberAccessFilterEqComprFactory = ServiceProvider.GetRequiredService<IMemberAccessibiliyFilterEqualityComparerFactory>();

            MethodAccessFilterEqCompr = MemberAccessFilterEqComprFactory.Method();
            FieldAccessFilterEqCompr = MemberAccessFilterEqComprFactory.Field();
            PropAccessFilterEqCompr = MemberAccessFilterEqComprFactory.Property();
            EventAccessFilterEqCompr = MemberAccessFilterEqComprFactory.Event();

            StringArrEqCompr = BasicEqComprFactory.GetArrayBasicEqualityComparer<string>();
        }

        private static void RegisterAllServices()
        {
            var services = new ServiceCollection();
            LocalDeviceServiceCollectionBuilder.RegisterAll(services);

            ServiceProviderContainer.Instance.Value.RegisterServices(services);
        }
    }
}
