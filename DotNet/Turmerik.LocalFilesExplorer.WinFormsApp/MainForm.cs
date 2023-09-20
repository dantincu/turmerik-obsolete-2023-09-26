using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.LocalFilesExplorer.WinFormsApp.ViewModels;
using Turmerik.Logging;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Dependencies;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Turmerik.LocalFilesExplorer.WinFormsApp
{
    public partial class MainForm : Form
    {
        private readonly ServiceProviderContainer svcProvContnr;
        private readonly bool svcRegistered;
        private readonly IServiceProvider svcProv;
        private readonly IMainFormVM viewModel;

        private readonly ITrmrkWinFormsActionComponentsManagerRetriever trmrkWinFormsActionComponentsManagerRetriever;
        private readonly ITrmrkWinFormsActionComponentsManager trmrkWinFormsActionComponentsManager;
        private readonly IAppLogger logger;
        private readonly IAppLoggerCreator appLoggerCreator;
        private readonly ITrmrkWinFormsActionComponentFactory trmrkWinFormsActionComponentFactory;
        private readonly ITrmrkWinFormsActionComponent actionComponent;

        public MainForm()
        {
            this.svcProvContnr = ServiceProviderContainer.Instance.Value;
            this.svcRegistered = svcProvContnr.AreServicesRegistered();

            if (this.svcRegistered)
            {
                this.svcProv = this.svcProvContnr.Services;
            }

            InitializeComponent();

            if (this.svcRegistered)
            {
                this.trmrkWinFormsActionComponentsManagerRetriever = this.svcProv.GetRequiredService<ITrmrkWinFormsActionComponentsManagerRetriever>();
                this.trmrkWinFormsActionComponentsManagerRetriever.ToolStripStatusLabel = this.toolStripStatusLabelMain;

                this.trmrkWinFormsActionComponentsManager = this.trmrkWinFormsActionComponentsManagerRetriever.Retrieve();
                this.trmrkWinFormsActionComponentsManager.EnableUIMessages = true;

                this.viewModel = this.svcProv.GetRequiredService<IMainFormVM>();

                this.appLoggerCreator = this.svcProv.GetRequiredService<IAppLoggerCreator>();
                this.logger = this.appLoggerCreator.GetSharedAppLogger(GetType());
                this.trmrkWinFormsActionComponentFactory = this.svcProv.GetRequiredService<ITrmrkWinFormsActionComponentFactory>();
                this.actionComponent = this.trmrkWinFormsActionComponentFactory.Create(this.logger);
            }
        }

        #region UI Event Handlers

        private void MainForm_Load(object sender, EventArgs e) => actionComponent?.Execute(
            new TrmrkActionComponentOpts
            {
                Action = () => new TrmrkActionResult
                {
                    ResponseMessage = "Welcome",
                },
                ActionName = nameof(MainForm_Load),
            }.LogMsgFactory(map => map.AddFromActionResult(
                "The main window opened",
                MsLogLevel.Information)));

        private void StatusStripMain_MouseUp(object sender, MouseEventArgs e) => actionComponent?.Execute(
            new TrmrkActionComponentOpts
            {
                Action = () =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        this.trmrkWinFormsActionComponentsManager.UIMessagesListForm.Show();
                    }

                    return new TrmrkActionResult();
                },
                ActionName = nameof(MainForm_Load),
            });

        #endregion UI Event Handlers
    }
}
