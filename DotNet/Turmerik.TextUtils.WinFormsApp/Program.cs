using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.TextUtils.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.TextUtils.WinFormsApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();

            WinFormsServiceCollectionBuilder.RegisterAll(services);
            services.AddSingleton<IAppBehaviour, AppBehaviour>();

            services.AddTransient<IMainFormVM, MainFormVM>();

            ServiceProviderContainer.AssureServicesRegistered(services);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
