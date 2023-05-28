using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Text;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface INetCoreAppEnv : IAppEnv
    {
    }

    public class NetCoreAppEnv : AppEnv, INetCoreAppEnv
    {
        public NetCoreAppEnv(
            ITimeStampHelper timeStampHelper,
            IConfiguration configuration) : base(
                timeStampHelper)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override string AppEnvLocatorFilePath
        {
            get
            {
                string appEnvLocatorFilePath = Configuration.GetValue(
                    typeof(string),
                    AppEnvLocatorFilePathAppSettingsKey) as string;

                return appEnvLocatorFilePath;
            }
        }

        public IConfiguration Configuration { get; }

        protected virtual string AppEnvLocatorFilePathAppSettingsKey => "Trmrk:AppEnvLocatorFilePath";
    }
}
