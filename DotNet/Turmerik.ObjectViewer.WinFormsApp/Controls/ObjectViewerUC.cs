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
using Turmerik.ObjectViewer.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.ObjectViewer.WinFormsApp.Controls
{
    public partial class ObjectViewerUC : UserControl
    {
        private readonly ServiceProviderContainer svcProvContnr;
        private readonly bool svcRegistered;
        private readonly IServiceProvider svcProv;
        private readonly IObjectViewerVM viewModel;

        public ObjectViewerUC()
        {
            this.svcProvContnr = ServiceProviderContainer.Instance.Value;
            this.svcRegistered = svcProvContnr.AreServicesRegistered();

            if (this.svcRegistered)
            {
                this.svcProv = svcProvContnr.Services;
                this.viewModel = this.svcProv.GetRequiredService<IObjectViewerVM>();
            }

            InitializeComponent();
        }
    }
}
