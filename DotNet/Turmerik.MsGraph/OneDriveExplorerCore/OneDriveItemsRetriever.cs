using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph.Drives.Item.List.Drive;
using Microsoft.Graph.Drives.Item;
using Turmerik.DriveExplorerCore;
using Turmerik.Text;
using DrvItm = Turmerik.DriveExplorerCore.DriveItem;

namespace Turmerik.MsGraph.OneDriveExplorerCore
{
    public interface IOneDriveItemsRetriever : IDriveItemsRetriever
    {
    }

    public class OneDriveItemsRetriever : DriveItemsRetrieverBase, IOneDriveItemsRetriever
    {
        private Drive myDrive;

        public OneDriveItemsRetriever(
            ITimeStampHelper timeStampHelper,
            IGraphServiceClientFactory graphServiceClientFactory) : base(timeStampHelper)
        {
            GraphServiceClientFactory = graphServiceClientFactory ?? throw new ArgumentNullException(
                nameof(graphServiceClientFactory));

            GraphServiceClient = new Lazy<GraphServiceClient>(
                () => graphServiceClientFactory.GetGraphServiceClient());
        }

        protected IGraphServiceClientFactory GraphServiceClientFactory { get; }
        protected Lazy<GraphServiceClient> GraphServiceClient { get; }

        protected async Task<Drive> AssureMyDriveAsync()
        {
            myDrive = myDrive ?? await GraphServiceClient.Value.Me.Drive.GetAsync();
            return myDrive;
        }

        protected async Task<DriveItemRequestBuilder> GetMyDriveRequestBuilderAsync()
        {
            var myDrive = await AssureMyDriveAsync();
            var myDriveRequestBuilder = GraphServiceClient.Value.Drives[myDrive.Id];

            return myDriveRequestBuilder;
        }

        public override Task<bool> FileExistsAsync(DriveItemIdnf.IClnbl idnf) => DriveItemExistsAsync(idnf);
        public override Task<bool> FolderExistsAsync(DriveItemIdnf.IClnbl idnf) => DriveItemExistsAsync(idnf);

        /*
         * Get item with path:
         * from https://stackoverflow.com/questions/39965330/getting-microsoft-graph-drive-items-by-path-using-the-net-sdk :
         * var items = await graphClient.Me.Drive.Root.ItemWithPath("/this/is/the/path").Children.Request().GetAsync();
         */

        public override async Task<DrvItm.Mtbl> GetFolderAsync(
            DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var graphItem = await myDriveRequestBuilder.Items[idnf.Id].GetAsync();

            var driveItem = await GetFolderCoreAsync(
                graphItem,
                idnf);

            return driveItem;
        }

        protected async Task<bool> DriveItemExistsAsync(DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var graphItem = await myDriveRequestBuilder.Items[idnf.Id].GetAsync();

            bool itemExists = graphItem != null;
            return itemExists;
        }

        protected DrvItm.Mtbl ConvertDriveFolder(
            Microsoft.Graph.Models.DriveItem graphItem,
            DriveItemIdnf.IClnbl prIdnf,
            bool forList)
        {
            var driveItem = ConvertItem(
                graphItem,
                true,
                graphItem.ParentReference == null);

            if (!forList)
            {
                driveItem.WebUrl = graphItem.WebUrl;
            }

            return driveItem;
        }

        protected DrvItm.Mtbl ConvertDriveFile(
            Microsoft.Graph.Models.DriveItem graphItem,
            DriveItemIdnf.IClnbl prIdnf,
            bool forList)
        {
            var driveItem = ConvertItem(
                graphItem,
                false,
                false);

            if (!forList)
            {
                driveItem.WebUrl = graphItem.WebUrl;
            }

            return driveItem;
        }

        protected DrvItm.Mtbl ConvertItem(
            Microsoft.Graph.Models.DriveItem graphItem,
            bool isFolder,
            bool isRootFolder)
        {
            var driveItem = new DrvItm.Mtbl
            {
                Id = graphItem.Id,
                Name = graphItem.Name,
                IsFolder = isFolder,
                IsRootFolder = isRootFolder,
                CreationTimeStr = GetTimeStampStr(graphItem.CreatedDateTime?.DateTime),
                LastWriteTimeStr = GetTimeStampStr(graphItem.LastModifiedDateTime?.DateTime),
            };

            return driveItem;
        }

        protected async Task<DrvItm.Mtbl> GetFolderCoreAsync(
            Microsoft.Graph.Models.DriveItem graphItem,
            DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            graphItem = await myDriveRequestBuilder.Items[graphItem.Id].GetAsync();

            var children = graphItem.Children;

            var driveItem = ConvertDriveFolder(graphItem, idnf, true);
            var childrenArr = children.ToArray();

            var filesArr = children.Where(item => item.FileObject != null).ToArray();
            var foldersArr = children.Where(item => item.Folder != null).ToArray();

            driveItem.FolderFiles = new DrvItm.MtblList(
                filesArr.Select(
                    item => ConvertDriveFile(
                        item,
                        new DriveItemIdnf.Mtbl
                        {
                            Id = graphItem.Id
                        }, true)));

            driveItem.SubFolders = new DrvItm.MtblList(
                foldersArr.Select(
                    item => ConvertDriveFolder(
                        item,
                        new DriveItemIdnf.Mtbl
                        {
                            Id = graphItem.Id
                        }, true)));

            return driveItem;
        }
    }
}
