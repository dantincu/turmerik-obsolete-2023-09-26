using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.AspNetCore.Infrastucture
{
    public partial class ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ICoreClnbl
        where TImmtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.CoreImmtbl, TClnbl
        where TMtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.CoreMtbl, TClnbl
    {
        public interface ICoreClnbl : IClnblCore
        {
            string TrmrkPrefix { get; }
        }

        public class CoreImmtbl : ImmtblCoreBase, ICoreClnbl
        {
            public CoreImmtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
            }

            public string TrmrkPrefix { get; }
        }

        public class CoreMtbl : MtblCoreBase, ICoreClnbl
        {
            public CoreMtbl()
            {
            }

            public CoreMtbl(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
            }

            public string TrmrkPrefix { get; set; }
        }
    }
}
