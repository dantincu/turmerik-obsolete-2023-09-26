using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Infrastucture;

namespace Turmerik.AspNetCore.Infrastucture
{
    public interface IAppSettingsServiceCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        TImmtbl AppSettings { get; }
    }

    public abstract class AppSettingsServiceCoreBase<TClnbl, TImmtbl, TMtbl> : IAppSettingsServiceCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : AppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        protected AppSettingsServiceCoreBase(
            IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            AppSettings = GetAppSettings();
        }

        public TImmtbl AppSettings { get; }

        protected IConfiguration Configuration { get; }

        protected virtual string ClientHostsConfigKey => "ClientHosts";

        protected abstract TImmtbl GetAppSettings();

        protected void AssignClientAppSettingsPropsCore(
            TMtbl mtbl)
        {
            mtbl.TrmrkPrefix = TurmerikPrefixes.TRMRK;
            mtbl.ClientAppHosts = GetClientAppHosts();
        }

        protected List<string> GetClientAppHosts()
        {
            var clientAppHosts = Configuration.GetRequiredSection(ClientHostsConfigKey).AsEnumerable().Select(
                kvp => kvp.Value).NotNull().ToList();

            return clientAppHosts;
        }
    }
}
