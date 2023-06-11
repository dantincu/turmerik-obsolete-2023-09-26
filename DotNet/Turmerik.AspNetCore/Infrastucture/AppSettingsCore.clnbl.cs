using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.AspNetCore.Infrastucture
{
    public partial class AppSettingsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.ICoreClnbl
        where TImmtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.CoreImmtbl, TClnbl
        where TMtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.CoreMtbl, TClnbl
    {
        public interface ICoreClnbl : IClnblCore
        {
            string TrmrkPrefix { get; }
            string ClientAppHost { get; }
        }

        public class CoreImmtbl : ImmtblCoreBase, ICoreClnbl
        {
            public CoreImmtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                ClientAppHost = src.ClientAppHost;
            }

            public string TrmrkPrefix { get; }
            public string ClientAppHost { get; }
        }

        public class CoreMtbl : MtblCoreBase, ICoreClnbl
        {
            public CoreMtbl()
            {
            }

            public CoreMtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                ClientAppHost = src.ClientAppHost;
            }

            public string TrmrkPrefix { get; set; }
            public string ClientAppHost { get; set; }
        }
    }
}
