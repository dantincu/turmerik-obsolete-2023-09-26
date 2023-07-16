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
using Turmerik.TextUtils.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.TextUtils.WinFormsApp
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider svcProv;
        private readonly IMainFormVM viewModel;

        public MainForm()
        {
            svcProv = ServiceProviderContainer.Instance.Value.Services;
            viewModel = svcProv.GetRequiredService<IMainFormVM>();

            InitializeComponent();
        }
    }
}
