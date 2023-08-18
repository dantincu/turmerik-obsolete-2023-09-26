using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.DriveExplorerCore;
using Turmerik.Logging;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Controls;
using Turmerik.WinForms.Dependencies;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Controls
{
    public partial class CsprojExecutionUC : UserControl
    {
        private readonly ServiceProviderContainer svcProvContnr;
        private readonly bool svcRegistered;
        private readonly IServiceProvider svcProv;

        private readonly IAppLoggerCreator appLoggerFactory;
        private readonly IWinFormsActionComponentFactory actionComponentFactory;

        private readonly IAppLogger logger;
        private readonly IWinFormsActionComponent actionComponent;

        private readonly TrmrkFsTreeView fsTreeView;

        public CsprojExecutionUC()
        {
            this.svcProvContnr = ServiceProviderContainer.Instance.Value;
            this.svcRegistered = svcProvContnr.AreServicesRegistered();

            if (this.svcRegistered)
            {
                this.svcProv = this.svcProvContnr.Services;
                this.appLoggerFactory = this.svcProv.GetRequiredService<IAppLoggerCreator>();
                this.logger = this.appLoggerFactory.GetSharedAppLogger(GetType());
                this.actionComponentFactory = this.svcProv.GetRequiredService<IWinFormsActionComponentFactory>();
                this.actionComponent = this.actionComponentFactory.Create(this.logger);
            }

            InitializeComponent();

            editableFolderPathUCCsprojFile.FolderPathChosen += EditableFolderPathUCCsprojFile_FolderPathChosen;

            fsTreeView = new TrmrkFsTreeView();
            fsTreeView.Dock = DockStyle.Fill;
            fsTreeView.DriveItemsRetriever = svcProv.GetRequiredService<IDriveItemsRetriever>();
            Controls.Add(fsTreeView);
        }

        #region UI Event Handlers

        private void EditableFolderPathUCCsprojFile_FolderPathChosen(
            Utils.MutableValueWrapper<string> obj) => actionComponent.ExecuteAsync(
                new TrmrkAsyncActionComponentOpts
                {
                    ActionName = nameof(EditableFolderPathUCCsprojFile_FolderPathChosen),
                    Action = async () =>
                    {
                        await fsTreeView.SetRootItemIdnfAsync(new DriveItemIdnf.Mtbl
                        {
                            Name = Path.GetFileName(obj.Value),
                            PrPath = Path.GetDirectoryName(obj.Value)
                        });

                        var nodesCount = fsTreeView.Nodes.Count;

                        return new TrmrkActionResult();
                    },
                    LogLevel = Microsoft.Extensions.Logging.LogLevel.Information
                });

        #endregion UI Event Handlers
    }
}
