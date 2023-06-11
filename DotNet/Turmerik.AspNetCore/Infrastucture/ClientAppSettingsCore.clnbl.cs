using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.AspNetCore.Infrastucture
{
    public partial class ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClientAppSettingsCore
        where TImmtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ClientAppSettingsCoreImmtbl, TClnbl
        where TMtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ClientAppSettingsCoreMtbl, TClnbl
    {
        public interface IClientAppSettingsCore : IClnblCore
        {
            string TrmrkPrefix { get; }
        }
    }
}
