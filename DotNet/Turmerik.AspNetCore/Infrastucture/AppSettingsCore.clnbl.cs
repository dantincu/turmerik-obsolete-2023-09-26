using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.AspNetCore.Infrastucture
{
    public interface IAppSettingsCore
    {
        string TrmrkPrefix { get; }
        string ClientAppHost { get; }
    }

    public static class AppSettingsCore
    {
        public static AppSettingsCoreImmtbl ToImmtbl(
            this IAppSettingsCore src) => new AppSettingsCoreImmtbl(src);

        public static AppSettingsCoreMtbl ToMtbl(
            this IAppSettingsCore src) => new AppSettingsCoreMtbl(src);
    }

    public class AppSettingsCoreImmtbl : IAppSettingsCore
    {
        public AppSettingsCoreImmtbl(IAppSettingsCore src)
        {
            TrmrkPrefix = src.TrmrkPrefix;
            ClientAppHost = src.ClientAppHost;
        }

        public string TrmrkPrefix { get; }
        public string ClientAppHost { get; }
    }

    public class AppSettingsCoreMtbl : IAppSettingsCore
    {
        public AppSettingsCoreMtbl()
        {
        }

        public AppSettingsCoreMtbl(IAppSettingsCore src)
        {
            TrmrkPrefix = src.TrmrkPrefix;
            ClientAppHost = src.ClientAppHost;
        }

        public string TrmrkPrefix { get; set; }
        public string ClientAppHost { get; set; }
    }
}
