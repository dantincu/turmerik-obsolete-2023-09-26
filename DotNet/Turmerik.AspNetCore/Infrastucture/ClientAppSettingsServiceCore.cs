using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Infrastucture;

namespace Turmerik.AspNetCore.Infrastucture
{
    public interface IClientAppSettingsServiceCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        TImmtbl ClientAppSettings { get; }
    }

    public abstract class ClientAppSettingsServiceCoreBase<TClnbl, TImmtbl, TMtbl> : IClientAppSettingsServiceCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        protected ClientAppSettingsServiceCoreBase()
        {
            ClientAppSettings = GetClientAppSettings();
        }

        public TImmtbl ClientAppSettings { get; }

        protected abstract TImmtbl GetClientAppSettings();

        protected void AssignClientAppSettingsPropsCore(
            TMtbl mtbl)
        {
            mtbl.TrmrkPrefix = TurmerikPrefixes.TRMRK;
            mtbl.MaxDecimalValue = decimal.MaxValue;
            mtbl.MinDecimalValue = decimal.MinValue;
        }
    }
}
