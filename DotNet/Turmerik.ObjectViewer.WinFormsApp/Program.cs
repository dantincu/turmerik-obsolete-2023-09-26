using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.ObjectViewer.WinFormsApp.Dependencies;
using Turmerik.ObjectViewer.WinFormsApp.Properties;
using Turmerik.WinForms.Dependencies;
using Turmerik.WinForms.Utils;

namespace Turmerik.ObjectViewer.WinFormsApp
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
            AppServiceCollectionBuilder.RegisterAll(services);

            var svcProvContnr = ServiceProviderContainer.Instance.Value;
            svcProvContnr.RegisterServices(services);
            svcProvContnr.AddIconsFontFile();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
