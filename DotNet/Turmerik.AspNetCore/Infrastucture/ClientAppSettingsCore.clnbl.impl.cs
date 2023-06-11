using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.AspNetCore.Infrastucture
{
    public partial class ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>
    {
        public class ClientAppSettingsCoreImmtbl : ImmtblCoreBase, IClientAppSettingsCore
        {
            public ClientAppSettingsCoreImmtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
            }

            public string TrmrkPrefix { get; }
        }

        public class ClientAppSettingsCoreMtbl : MtblCoreBase, IClientAppSettingsCore
        {
            public ClientAppSettingsCoreMtbl()
            {
            }

            public ClientAppSettingsCoreMtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
            }

            public string TrmrkPrefix { get; set; }
        }
    }
}
