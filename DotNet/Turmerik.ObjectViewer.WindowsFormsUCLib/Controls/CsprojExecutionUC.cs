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
using Turmerik.ObjectViewer.WindowsFormsUCLib.Components;
using Turmerik.ObjectViewer.WindowsFormsUCLib.Properties;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Components;
using Turmerik.WinForms.Controls;
using Turmerik.WinForms.Dependencies;

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
        private readonly IImageListDecoratorFactory imageListDecoratorFactory;
        private readonly ITreeViewDataAdapterFactory treeViewDataAdapterFactory;
        private readonly ImageList treeViewFilesImageList;
        private readonly CsProjTreeViewFilesImageList treeViewFilesImageListDecorator;
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
                this.imageListDecoratorFactory = this.svcProv.GetRequiredService<IImageListDecoratorFactory>();
                this.treeViewDataAdapterFactory = this.svcProv.GetRequiredService<ITreeViewDataAdapterFactory>();
                this.appSettings = this.svcProv.GetRequiredService<IAppSettings>();
            }

            InitializeComponent();

            if (this.svcRegistered)
            {
                (this.treeViewFilesImageList,
                    this.treeViewFilesImageListDecorator) = GetTreeViewFilesImageList();

                this.treeViewFiles.ImageList = treeViewFilesImageList;
                this.treeViewDataAdapterFiles = CreateTreeViewDataAdapterFiles();

                this.editableFolderPathUCCsprojFile.FolderPathChanged += EditableFolderPathUCCsprojFile_FolderPathChanged;

                this.editableFolderPathUCCsprojFile.SetFolderPath(
                    appSettings.Data.CsProjDirPath ?? string.Empty);
            }
        }

        private Tuple<ImageList, CsProjTreeViewFilesImageList> GetTreeViewFilesImageList()
        {
            var imageList = new ImageList();
            var imageListDecorator = imageListDecoratorFactory.Create<CsProjTreeViewFilesImageList>(
                new ImageListDecoratorOpts.Mtbl
                {
                    ImageList = imageList,
                    ImageMap = new Dictionary<string, Image>
                    {
                        { nameof(CsProjTreeViewFilesImageList.FolderKey), Resources.folder_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.FolderOpenKey), Resources.folder_open_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.FolderZipKey), Resources.folder_zip_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.HardDriveKey), Resources.hard_drive_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.NoteKey), Resources.note_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.CodeKey), Resources.code_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.Package2Key), Resources.package_2_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.ImageKey), Resources.image_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.AudioFileKey), Resources.audio_file_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.VideoFileKey), Resources.video_file_FILL0_wght400_GRAD0_opsz48 },
                        { nameof(CsProjTreeViewFilesImageList.UnknownDocumentKey), Resources.unknown_document_FILL0_wght400_GRAD0_opsz48 },
                    }
                });

            return Tuple.Create(
                imageList,
                imageListDecorator);
        }

        private ITreeViewDataAdapterAsync<DriveItem.Mtbl> CreateTreeViewDataAdapterFiles(
            ) => treeViewDataAdapterFactory.Create(
                new TreeViewDataAdapterIconFactoriesOpts.Mtbl<DriveItem.Mtbl>
                {
                    TreeView = treeViewFiles,
                    NodeTextFactory = arg => arg.Value.Name,
                    NodeIcon = GetNodeIcon,
                    SelectedNodeIcon = GetNodeIcon,
                    StateNodeIcon = GetNodeIcon
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
                    DriveItem.Mtbl[] children;

                    if (parent.IsFolder == true)
                    {
                        parent = await fsEntriesRetriever.GetFolderAsync(parent);
                        children = GetChildren(parent);
                    }
                    else
                    {
                        children = new DriveItem.Mtbl[0];
                    }
                    
                    return children;
                }).ActWithValue(adapter =>
                {
                    adapter.RefreshOnDoubleClick = true;
                    adapter.ChildrenRefreshDepth = 1;
                });

        private KeyValuePair<int, string> GetNodeIcon(
            TreeNodeArg<DriveItem.Mtbl> node) => new KeyValuePair<int, string>(
                (node.Value.IsFolder ?? false) ? GetFolderNodeIconIdx(
                    node) : GetFileNodeIconIdx(
                        node), null);

        private int GetFolderNodeIconIdx(
            TreeNodeArg<DriveItem.Mtbl> node) => treeViewFilesImageListDecorator.FolderKey;

        private int GetFileNodeIconIdx(
            TreeNodeArg<DriveItem.Mtbl> node)
        {
            int idx;

            switch (node.Value.FileNameExtension)
            {
                case ".txt":
                case ".md":
                    idx = treeViewFilesImageListDecorator.NoteKey;
                    break;
                case ".cs":
                case ".js":
                case ".ts":
                case ".json":
                case ".jsx":
                case ".tsx":
                case ".csx":
                case ".cshtml":
                case ".csproj":
                case ".sln":
                case ".xml":
                case ".yml":
                case ".xaml":
                case ".html":
                case ".c":
                case ".h":
                case ".cpp":
                case ".vb":
                case ".vbproj":
                case ".vbx":
                case ".java":
                case ".config":
                    idx = treeViewFilesImageListDecorator.CodeKey;
                    break;
                case ".bin":
                case ".exe":
                case ".lib":
                case ".jar":
                    idx = treeViewFilesImageListDecorator.Package2Key;
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".giff":
                case ".tiff":
                case ".img":
                case ".ico":
                case ".bmp":
                case ".heic":
                    idx = treeViewFilesImageListDecorator.ImageKey;
                    break;
                case ".mp3":
                case ".flac":
                case ".aac":
                case ".wav":
                    idx = treeViewFilesImageListDecorator.AudioFileKey;
                    break;
                case ".mpg":
                case ".mpeg":
                case ".avi":
                case ".mp4":
                case ".m4a":
                    idx = treeViewFilesImageListDecorator.VideoFileKey;
                    break;
                default:
                    idx = treeViewFilesImageListDecorator.UnknownDocumentKey;
                    break;
            }

            return idx;
        }

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
