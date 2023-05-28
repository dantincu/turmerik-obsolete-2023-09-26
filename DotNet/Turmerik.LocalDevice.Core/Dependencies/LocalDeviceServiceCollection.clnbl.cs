using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Dependencies;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.LocalDevice.Core.Logging;

namespace Turmerik.LocalDevice.Core.Dependencies
{
    public interface ILocalDeviceServiceCollection : ITrmrkCoreServiceCollection
    {
        IAppLoggerFactory AppLoggerFactory { get; }
        IAppEnv AppEnv { get; }
        INetCoreAppEnv NetCoreAppEnv { get; }
    }

    public class LocalDeviceServiceCollectionImmtbl : TrmrkCoreServiceCollectionImmtbl, ILocalDeviceServiceCollection
    {
        public LocalDeviceServiceCollectionImmtbl(ILocalDeviceServiceCollection src) : base(src)
        {
            AppLoggerFactory = src.AppLoggerFactory;
            AppEnv = src.AppEnv;
            NetCoreAppEnv = src.NetCoreAppEnv;
        }

        public IAppLoggerFactory AppLoggerFactory { get; }
        public IAppEnv AppEnv { get; }
        public INetCoreAppEnv NetCoreAppEnv { get; }
    }

    public class LocalDeviceServiceCollectionMtbl : TrmrkCoreServiceCollectionMtbl, ILocalDeviceServiceCollection
    {
        public LocalDeviceServiceCollectionMtbl()
        {
        }

        public LocalDeviceServiceCollectionMtbl(ILocalDeviceServiceCollection src) : base(src)
        {
            AppLoggerFactory = src.AppLoggerFactory;
            AppEnv = src.AppEnv;
            NetCoreAppEnv = src.NetCoreAppEnv;
        }

        public IAppLoggerFactory AppLoggerFactory { get; set; }
        public IAppEnv AppEnv { get; set; }
        public INetCoreAppEnv NetCoreAppEnv { get; set; }
    }
}
