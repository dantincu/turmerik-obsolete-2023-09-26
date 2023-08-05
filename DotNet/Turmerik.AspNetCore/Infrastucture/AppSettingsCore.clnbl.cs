using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.AspNetCore.Infrastucture
{
    public class AppSettingsCore<TClnbl> : ClnblCore
    {
        public new interface IClnblCore : ClnblCore.IClnblCore
        {
            string TrmrkPrefix { get; }

            IEnumerable<string> GetClientAppHosts();
        }
    }

    public class AppSettingsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public new interface IClnblCore : AppSettingsCore<TClnbl>.IClnblCore, ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        {
        }

        public class ImmtblCore : ImmtblCoreBase, IClnblCore
        {
            public ImmtblCore(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                ClientAppHosts = src.GetClientAppHosts()?.RdnlC();
            }

            public string TrmrkPrefix { get; }
            public ReadOnlyCollection<string> ClientAppHosts { get; }

            public IEnumerable<string> GetClientAppHosts() => ClientAppHosts;
        }

        public class MtblCore : MtblCoreBase, IClnblCore
        {
            public MtblCore()
            {
            }

            public MtblCore(TClnbl src) : base(src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                ClientAppHosts = src.GetClientAppHosts()?.ToList();
            }

            public string TrmrkPrefix { get; set; }
            public List<string> ClientAppHosts { get; set; }

            public IEnumerable<string> GetClientAppHosts() => ClientAppHosts;
        }
    }
}
