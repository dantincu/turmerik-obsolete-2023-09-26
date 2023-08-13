using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Dependencies;
using Turmerik.LocalDevice.Core.Dependencies;

namespace Turmerik.WinForms.Dependencies
{
    public class ServiceProviderContainer : SimpleServiceProviderContainer, IDisposable
    {
        public static readonly string MaterialUIIconsFontFileRelPath = Path.Combine(
            "Resources",
            "MaterialIcons-Regular.ttf");

        public static readonly Lazy<ServiceProviderContainer> Instance = new Lazy<ServiceProviderContainer>(
            () => new ServiceProviderContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

        private ServiceProviderContainer()
        {
            IconsFont = new PrivateFontCollection();
        }

        public static IServiceProvider AssureServicesRegistered(
            IServiceCollection services,
            Action<IServiceCollection> callback = null,
            string iconsFontFile = null)
        {
            WinFormsServiceCollectionBuilder.RegisterAll(services);
            callback?.Invoke(services);

            var svcProv = Instance.Value.AssureServicesRegisteredCore(services);
            return svcProv;
        }

        public void AddIconsFontFile(
            string iconsFontFile = null)
        {
            iconsFontFile = iconsFontFile ?? MaterialUIIconsFontFileRelPath;
            Instance.Value.IconsFont.AddFontFile(iconsFontFile);
        }

        public PrivateFontCollection IconsFont { get; }

        public void Dispose()
        {
            IconsFont.Dispose();
        }
    }
}
