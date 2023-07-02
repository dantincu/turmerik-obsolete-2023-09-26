using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.AspNetCore.Infrastucture
{
    public class ClientAppSettingsCore<TClnbl> : ClnblCore
    {
        public new interface IClnblCore : ClnblCore.IClnblCore
        {
            string TrmrkPrefix { get; }
        }
    }

    public class ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public new interface IClnblCore : ClientAppSettingsCore<TClnbl>.IClnblCore, ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        {
        }

        public class ImmtblCore : ImmtblCoreBase, IClnblCore
        {
            public ImmtblCore(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
            }

            public string TrmrkPrefix { get; }
        }

        public class MtblCore : MtblCoreBase, IClnblCore
        {
            public MtblCore()
            {
            }

            public MtblCore(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
            }

            public string TrmrkPrefix { get; set; }
        }
    }
}
