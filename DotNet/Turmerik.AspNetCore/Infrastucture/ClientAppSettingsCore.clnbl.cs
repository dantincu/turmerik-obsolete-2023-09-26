using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.AspNetCore.Infrastucture
{
    public interface IClientAppSettingsCore
    {
        string TrmrkPrefix { get; }
    }

    public static class ClientAppSettingsCore
    {
        public static ClientAppSettingsCoreImmtbl ToImmtbl(
            this IClientAppSettingsCore src) => new ClientAppSettingsCoreImmtbl(src);

        public static ClientAppSettingsCoreMtbl ToMtbl(
            this IClientAppSettingsCore src) => new ClientAppSettingsCoreMtbl(src);
    }

    public class ClientAppSettingsCoreImmtbl : IClientAppSettingsCore
    {
        public ClientAppSettingsCoreImmtbl(IClientAppSettingsCore src)
        {
            TrmrkPrefix = src.TrmrkPrefix;
        }

        public string TrmrkPrefix { get; }
    }

    public class ClientAppSettingsCoreMtbl : IClientAppSettingsCore
    {
        public ClientAppSettingsCoreMtbl()
        {
        }

        public ClientAppSettingsCoreMtbl(IClientAppSettingsCore src)
        {
            TrmrkPrefix = src.TrmrkPrefix;
        }

        public string TrmrkPrefix { get; set; }
    }
}
