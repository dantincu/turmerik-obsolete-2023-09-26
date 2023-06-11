using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.Copy;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.DriveExplorerCore;
using Turmerik.Text;
using DrvItm = Turmerik.DriveExplorerCore.DriveItem;

namespace Turmerik.MsGraph.OneDriveExplorerCore
{
    public interface IOneDriveExplorerServiceEngine : IOneDriveItemsRetriever, IDriveExplorerServiceEngine
    {
    }

    public class OneDriveExplorerServiceEngine : OneDriveItemsRetriever, IOneDriveExplorerServiceEngine
    {
        public OneDriveExplorerServiceEngine(
            ITimeStampHelper timeStampHelper,
            IGraphServiceClientFactory graphServiceClientFactory) : base(
                timeStampHelper,
                graphServiceClientFactory)
        {
        }

        public async Task<DrvItm.Mtbl> CopyFileAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFileName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            if (string.IsNullOrWhiteSpace(newFileName))
            {
                newFileName = graphItem.Name;
            }

            var itemRef = new ItemReference
            {
                Id = newPrIdnf.Id,
            };

            var item = await fileReqBuilder.Copy.PostAsync(new CopyPostRequestBody
            {
                ParentReference = itemRef,
                Name = newFileName
            });

            DrvItm.Mtbl driveItem = ConvertDriveFile(item, newPrIdnf, false);
            return driveItem;
        }

        public async Task<DrvItm.Mtbl> CopyFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await folderReqBuilder.GetAsync();

            newFolderName = newFolderName ?? graphItem.Name;

            var itemRef = new ItemReference
            {
                Id = newPrIdnf.Id,
            };

            var item = await folderReqBuilder.Copy.PostAsync(new CopyPostRequestBody
            {
                ParentReference = itemRef,
                Name = newFolderName
            });

            DrvItm.Mtbl driveItem = ConvertDriveFolder(item, newPrIdnf, false);

            return driveItem;
        }

        public async Task<DrvItm.Mtbl> CreateFolderAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[prIdnf.Id];

            var newItem = new Microsoft.Graph.Models.DriveItem
            {
                Name = newFolderName,
                Folder = new Folder()
            };

            newItem = await folderReqBuilder.Children.PostAsync(newItem);
            DrvItm.Mtbl driveItem = ConvertDriveFolder(newItem, prIdnf, false);

            return driveItem;
        }

        public async Task<DrvItm.Mtbl> CreateOfficeLikeFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType)
        {
            throw new NotImplementedException();
        }

        public async Task<DrvItm.Mtbl> CreateTextFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            string text)
        {
            byte[] textBytesArr = Encoding.UTF8.GetBytes(text);

            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[prIdnf.Id];

            var newItem = new Microsoft.Graph.Models.DriveItem
            {
                Name = newFileName,
                FileObject = new FileObject(),
                Content = textBytesArr
            };

            newItem = await folderReqBuilder.Children.PostAsync(newItem);
            DrvItm.Mtbl driveItem = ConvertDriveFile(newItem, prIdnf, false);

            return driveItem;
        }

        public async Task<DrvItm.Mtbl> DeleteFileAsync(DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];

            var driveItem = new DrvItm.Mtbl
            {
                Idnf = new DriveItemIdnf.Mtbl
                {
                    Id = idnf.Id,
                }
            };

            await fileReqBuilder.DeleteAsync();
            return driveItem;
        }

        public async Task<DrvItm.Mtbl> DeleteFolderAsync(DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[idnf.Id];

            var driveItem = new DrvItm.Mtbl
            {
                Idnf = new DriveItemIdnf.Mtbl
                {
                    Id = idnf.Id,
                },
                IsFolder = true
            };

            await folderReqBuilder.DeleteAsync();
            return driveItem;
        }

        public async Task<string> GetDriveFolderWebUrlAsync(DriveItemIdnf.IClnbl idnf) => await GetDriveItemUrlAsync(idnf);

        public async Task<string> GetDriveFileWebUrlAsync(DriveItemIdnf.IClnbl idnf) => await GetDriveItemUrlAsync(idnf);

        public async Task<DrvItm.Mtbl> GetTextFileAsync(DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            var graphFileContent = graphItem.Content;
            string text = Encoding.UTF8.GetString(graphFileContent);

            var file = ConvertDriveFile(
                graphItem,
                idnf.GetPrIdnf(), false);

            file.TextFileContent = text;
            return file;
        }

        public async Task<DrvItm.Mtbl> MoveFileAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFileName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            if (string.IsNullOrWhiteSpace(newFileName))
            {
                newFileName = graphItem.Name;
            }

            var itemRef = new ItemReference
            {
                Id = newPrIdnf.Id,
            };

            var item = await fileReqBuilder.PatchAsync(
                new Microsoft.Graph.Models.DriveItem
                {
                    Id = idnf.Id,
                    Name = newFileName,
                    ParentReference = itemRef
                });

            DrvItm.Mtbl driveItem = ConvertDriveFile(item, newPrIdnf, false);
            return driveItem;
        }

        public async Task<DrvItm.Mtbl> MoveFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            if (string.IsNullOrWhiteSpace(newFolderName))
            {
                newFolderName = graphItem.Name;
            }

            var item = new Microsoft.Graph.Models.DriveItem
            {
                Id = idnf.Id,
                Name = newFolderName,
                ParentReference = new ItemReference
                {
                    Id = newPrIdnf.Id,
                }
            };

            item = await fileReqBuilder.PatchAsync(item);
            DrvItm.Mtbl driveItem = ConvertDriveFolder(item, newPrIdnf, false);

            return driveItem;
        }

        public async Task<DrvItm.Mtbl> RenameFileAsync(
            DriveItemIdnf.IClnbl idnf,
            string newFileName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            var item = await fileReqBuilder.PatchAsync(
                new Microsoft.Graph.Models.DriveItem
                {
                    Id = idnf.Id,
                    Name = newFileName
                });

            DrvItm.Mtbl driveItem = ConvertDriveFile(item, idnf.GetPrIdnf(), false);
            return driveItem;
        }

        public async Task<DrvItm.Mtbl> RenameFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            var item = new Microsoft.Graph.Models.DriveItem
            {
                Id = idnf.Id,
                Name = newFolderName
            };

            item = await fileReqBuilder.PatchAsync(item);
            DrvItm.Mtbl driveItem = ConvertDriveFolder(item, idnf.GetPrIdnf(), false);

            return driveItem;
        }

        private async Task<string> GetDriveItemUrlAsync(
            DriveItemIdnf.IClnbl idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            string url = graphItem.WebUrl;
            return url;
        }
    }
}
