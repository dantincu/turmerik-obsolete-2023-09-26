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
using Turmerik.LocalDevice.Core.FileExplorerCore;
using Turmerik.Logging;
using Turmerik.ObjectViewer.Lib.Components;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Components;
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
        private readonly IFsEntriesRetriever fsEntriesRetriever;
        private readonly ITreeViewDataAdapterFactory treeViewDataAdapterFactory;
        private readonly ITreeViewDataAdapterAsync<DriveItem.Mtbl> treeViewDataAdapterFiles;

        private readonly IAppSettings appSettings;

        private string csprojFolderPath;

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
                this.fsEntriesRetriever = this.svcProv.GetRequiredService<IFsEntriesRetriever>();
                this.treeViewDataAdapterFactory = this.svcProv.GetRequiredService<ITreeViewDataAdapterFactory>();
                this.appSettings = this.svcProv.GetRequiredService<IAppSettings>();
            }

            InitializeComponent();

            if (this.svcRegistered)
            {
                this.treeViewDataAdapterFiles = CreateTreeViewDataAdapterFiles();
                editableFolderPathUCCsprojFile.FolderPathChanged += EditableFolderPathUCCsprojFile_FolderPathChanged;
                editableFolderPathUCCsprojFile.SetFolderPath(appSettings.Data.CsProjDirPath ?? string.Empty);
            }
        }

        private ITreeViewDataAdapterAsync<DriveItem.Mtbl> CreateTreeViewDataAdapterFiles(
            ) => treeViewDataAdapterFactory.Create(
                new TreeViewDataAdapterIconFactoriesOpts.Mtbl<DriveItem.Mtbl>
                {
                    TreeView = treeViewFiles,
                    NodeTextFactory = arg => arg.Value.Name,
                    NodeIcon = GetNodeIcon,
                    SelectedNodeIcon = GetSelectedNodeIcon,
                    StateNodeIcon = GetStateNodeIcon
                },
                async () =>
                {
                    var parent = await fsEntriesRetriever.GetFolderAsync(
                        DriveItemIdnfH.FromPath(csprojFolderPath));

                    var children = GetChildren(parent);
                    return children;
                },
                async (parent) =>
                {
                    parent = await fsEntriesRetriever.GetFolderAsync(parent);

                    var children = GetChildren(parent);
                    return children;
                });

        private KeyValuePair<int, string> GetNodeIcon(
            TreeNodeArg<DriveItem.Mtbl> node) => new KeyValuePair<int, string>(
                (node.Value.IsFolder ?? false) ? 0 : 1, null);

        private KeyValuePair<int, string> GetSelectedNodeIcon(
            TreeNodeArg<DriveItem.Mtbl> node) => new KeyValuePair<int, string>(
                (node.Value.IsFolder ?? false) ? 0 : 1, null);

        private KeyValuePair<int, string> GetStateNodeIcon(
            TreeNodeArg<DriveItem.Mtbl> node) => new KeyValuePair<int, string>(
                (node.Value.IsFolder ?? false) ? 0 : 1, null);

        private DriveItem.Mtbl[] GetChildren(
            DriveItem.Mtbl parent)
        {
            var children = parent.SubFolders.ToList();
            children.AddRange(parent.FolderFiles);

            foreach (var child in children)
            {
                child.PrIdnf = new DriveItemIdnf.Mtbl(parent);
            }

            return children.ToArray();
        }

        #region UI Event Handlers

        private void EditableFolderPathUCCsprojFile_FolderPathChanged(
            Utils.MutableValueWrapper<string> obj) => actionComponent.ExecuteAsync(
                new TrmrkAsyncActionComponentOpts
                {
                    ActionName = nameof(EditableFolderPathUCCsprojFile_FolderPathChanged),
                    Action = async () =>
                    {
                        this.csprojFolderPath = obj.Value;
                        
                        this.appSettings.Update((ref AppSettingsData.Mtbl mtbl) =>
                        {
                            mtbl.CsProjDirPath = obj.Value;
                        });

                        if (!string.IsNullOrWhiteSpace(this.csprojFolderPath))
                        {
                            await treeViewDataAdapterFiles.RefreshRootNodesAsync();
                        }
                        
                        return new TrmrkActionResult();
                    }
                });

        #endregion UI Event Handlers
    }
}
