using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.AspNetCore.Infrastucture
{
    public partial class AppSettingsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.IAppSettingsCore
        where TImmtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.AppSettingsCoreImmtbl, TClnbl
        where TMtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.AppSettingsCoreMtbl, TClnbl
    {
        public interface IAppSettingsCore : IClnblCore
        {
            string TrmrkPrefix { get; }
            string ClientAppHost { get; }
        }
    }
}
