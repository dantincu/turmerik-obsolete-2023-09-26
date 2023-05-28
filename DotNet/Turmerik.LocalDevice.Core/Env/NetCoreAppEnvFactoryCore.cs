using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Core.Reflection;
using Turmerik.Core.Text;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface INetCoreAppEnvFactoryCore
    {
        NetCoreAppEnv GetAppEnv(IConfiguration configuration);
    }

    public class NetCoreAppEnvFactoryCore : INetCoreAppEnvFactoryCore
    {
        private readonly ITimeStampHelper timeStampHelper;

        public NetCoreAppEnvFactoryCore(ITimeStampHelper timeStampHelper)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
        }

        public virtual NetCoreAppEnv GetAppEnv(IConfiguration configuration)
        {
            var appEnv = new NetCoreAppEnv(timeStampHelper, configuration);
            return appEnv;
        }
    }
}
