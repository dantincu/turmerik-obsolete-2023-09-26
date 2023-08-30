using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalFilesExplorer.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.LocalFilesExplorer.WinFormsApp
{
    public partial class MainForm : Form
    {
        private readonly ServiceProviderContainer svcProvContnr;
        private readonly bool svcRegistered;
        private readonly IServiceProvider svcProv;
        private readonly IMainFormVM viewModel;

        public MainForm()
        {
            this.svcProvContnr = ServiceProviderContainer.Instance.Value;
            this.svcRegistered = svcProvContnr.AreServicesRegistered();

            if (this.svcRegistered)
            {
                this.svcProv = this.svcProvContnr.Services;
                this.viewModel = this.svcProv.GetRequiredService<IMainFormVM>();
            }

            InitializeComponent();
        }
    }
}
