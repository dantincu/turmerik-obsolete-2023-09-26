using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalFilesExplorer.WinFormsApp.Dependencies;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.LocalFilesExplorer.WinFormsApp
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

            AppServiceCollectionBuilder.RegisterAll(
                services,
                registerFsExplorerServiceEngineAsDefault: true);

            var svcProvContnr = ServiceProviderContainer.Instance.Value;
            svcProvContnr.RegisterServices(services);
            svcProvContnr.AddIconsFontFile();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
