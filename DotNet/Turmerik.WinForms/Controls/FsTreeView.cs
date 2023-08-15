using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.DriveExplorerCore;
using Turmerik.Logging;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.WinForms.Controls
{
    public class FsTreeView : TreeView
    {
        private readonly ServiceProviderContainer svcProvContnr;
        private readonly bool svcRegistered;
        private readonly IServiceProvider svcProv;

        private readonly IAppLoggerCreator appLoggerFactory;
        private readonly IWinFormsActionComponentFactory actionComponentFactory;

        private IAppLogger logger;
        private IWinFormsActionComponent actionComponent;
        private IDriveItemsRetriever driveItemsRetriever;
        private DriveItemIdnf.IClnbl rootIdnf;

        private DriveItem.Mtbl rootItem;

        private FsTreeNode<DriveItem.Mtbl>[] rootFolderNodes;
        private FsTreeNode<DriveItem.Mtbl>[] rootFileNodes;

        private int folderIconIdx = -1;
        private int defaultFileIconIdx = -1;
        private Func<DriveItem.Mtbl, int> fileIconFactoryIdx;

        private Action<FsTreeNode<DriveItem.Mtbl>> newFolderNodeCallback;
        private Action<FsTreeNode<DriveItem.Mtbl>> newFileNodeCallback;

        public FsTreeView()
        {
            this.svcProvContnr = ServiceProviderContainer.Instance.Value;
            this.svcRegistered = svcProvContnr.AreServicesRegistered();

            if (this.svcRegistered)
            {
                this.svcProv = svcProvContnr.Services;
                this.appLoggerFactory = this.svcProv.GetRequiredService<IAppLoggerCreator>();
                this.logger = this.appLoggerFactory.GetSharedAppLogger(GetType());
                this.actionComponentFactory = this.svcProv.GetRequiredService<IWinFormsActionComponentFactory>();
                this.actionComponent = this.actionComponentFactory.Create(this.logger);
                this.driveItemsRetriever = this.svcProv.GetRequiredService<IDriveItemsRetriever>();
            }
        }

        public IAppLogger GetLogger() => logger;

        public void SetLogger(
            IAppLogger logger) => this.logger = logger;

        public IWinFormsActionComponent GetActionComponent() => actionComponent;

        public void SetActionComponent(
            IWinFormsActionComponent actionComponent) => this.actionComponent = actionComponent;

        public IDriveItemsRetriever GetDriveItemsRetriever() => this.driveItemsRetriever;

        public void SetDriveItemsRetriever(
            IDriveItemsRetriever driveItemsRetriever)
        {
            this.driveItemsRetriever = driveItemsRetriever;
        }

        public DriveItemIdnf.IClnbl GetRootIdnf() => rootIdnf;

        public async Task SetRootIdnfAsync(DriveItemIdnf.IClnbl rootIdnf)
        {
            this.rootIdnf = rootIdnf;
            await this.RefreshRootNodesAsync();
        }

        public async Task RefreshRootNodesAsync()
        {
            this.Nodes.Clear();

            if (rootIdnf != null)
            {
                rootItem = await driveItemsRetriever.GetFolderAsync(rootIdnf);

                if (rootItem.SubFolders != null)
                {
                    for (int i = 0; i < rootItem.SubFolders.Count; i++)
                    {
                        rootItem.SubFolders[i] = await driveItemsRetriever.GetFolderAsync(
                            new DriveItemIdnf.Mtbl(rootItem.SubFolders[i]));
                    }
                }

                AddRootNodes(rootItem);
            }
            else
            {
                rootItem = null;
            }
        }

        private void AddRootNodes(
            DriveItem.Mtbl rootItem)
        {
            rootFolderNodes = ((IEnumerable<DriveItem.Mtbl>)rootItem.SubFolders)?.Select(
                CreateFolderNode).ToArray();

            rootFileNodes = ((IEnumerable<DriveItem.Mtbl>)rootItem.FolderFiles)?.Select(
                CreateFolderNode).ToArray();

            this.Nodes.AddRange(rootFolderNodes);
            this.Nodes.AddRange(rootFileNodes);
        }

        private FsTreeNode<DriveItem.Mtbl> CreateFolderNode(DriveItem.Mtbl folder)
        {
            throw new NotImplementedException();
        }

        private FsTreeNode<DriveItem.Mtbl> CreateFileNode(DriveItem.Mtbl file)
        {
            throw new NotImplementedException();
        }
    }
}
