using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.AspNetCore.Infrastucture
{
    public partial class AppSettingsCore<TClnbl, TImmtbl, TMtbl>
    {
        public class AppSettingsCoreImmtbl : ImmtblCoreBase, IAppSettingsCore
        {
            public AppSettingsCoreImmtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                ClientAppHost = src.ClientAppHost;
            }

            public string TrmrkPrefix { get; }
            public string ClientAppHost { get; }
        }

        public class AppSettingsCoreMtbl : MtblCoreBase, IAppSettingsCore
        {
            public AppSettingsCoreMtbl()
            {
            }

            public AppSettingsCoreMtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                ClientAppHost = src.ClientAppHost;
            }

            public string TrmrkPrefix { get; set; }
            public string ClientAppHost { get; set; }
        }
    }
}
