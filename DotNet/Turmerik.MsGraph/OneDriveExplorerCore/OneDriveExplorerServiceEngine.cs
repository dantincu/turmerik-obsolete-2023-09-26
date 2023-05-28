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

        public async Task<DriveItemMtbl> CopyFileAsync(
            IDriveItemIdnf idnf,
            IDriveItemIdnf newPrIdnf,
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

            DriveItemMtbl driveItem = ConvertDriveFile(item, newPrIdnf, false);
            return driveItem;
        }

        public async Task<DriveItemMtbl> CopyFolderAsync(
            IDriveItemIdnf idnf,
            IDriveItemIdnf newPrIdnf,
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

            DriveItemMtbl driveItem = ConvertDriveFolder(item, newPrIdnf, false);

            return driveItem;
        }

        public async Task<DriveItemMtbl> CreateFolderAsync(
            IDriveItemIdnf prIdnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[prIdnf.Id];

            var newItem = new DriveItem
            {
                Name = newFolderName,
                Folder = new Folder()
            };

            newItem = await folderReqBuilder.Children.PostAsync(newItem);
            DriveItemMtbl driveItem = ConvertDriveFolder(newItem, prIdnf, false);

            return driveItem;
        }

        public async Task<DriveItemMtbl> CreateOfficeLikeFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType)
        {
            throw new NotImplementedException();
        }

        public async Task<DriveItemMtbl> CreateTextFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            string text)
        {
            byte[] textBytesArr = Encoding.UTF8.GetBytes(text);

            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[prIdnf.Id];

            var newItem = new DriveItem
            {
                Name = newFileName,
                FileObject = new FileObject(),
                Content = textBytesArr
            };

            newItem = await folderReqBuilder.Children.PostAsync(newItem);
            DriveItemMtbl driveItem = ConvertDriveFile(newItem, prIdnf, false);

            return driveItem;
        }

        public async Task<DriveItemMtbl> DeleteFileAsync(IDriveItemIdnf idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];

            var driveItem = new DriveItemMtbl
            {
                Idnf = new DriveItemIdnfMtbl
                {
                    Id = idnf.Id,
                }
            };

            await fileReqBuilder.DeleteAsync();
            return driveItem;
        }

        public async Task<DriveItemMtbl> DeleteFolderAsync(IDriveItemIdnf idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var folderReqBuilder = myDriveRequestBuilder.Items[idnf.Id];

            var driveItem = new DriveItemMtbl
            {
                Idnf = new DriveItemIdnfMtbl
                {
                    Id = idnf.Id,
                },
                IsFolder = true
            };

            await folderReqBuilder.DeleteAsync();
            return driveItem;
        }

        public async Task<string> GetDriveFolderWebUrlAsync(IDriveItemIdnf idnf) => await GetDriveItemUrlAsync(idnf);

        public async Task<string> GetDriveFileWebUrlAsync(IDriveItemIdnf idnf) => await GetDriveItemUrlAsync(idnf);

        public async Task<DriveItemMtbl> GetTextFileAsync(IDriveItemIdnf idnf)
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

        public async Task<DriveItemMtbl> MoveFileAsync(
            IDriveItemIdnf idnf,
            IDriveItemIdnf newPrIdnf,
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
                new DriveItem
                {
                    Id = idnf.Id,
                    Name = newFileName,
                    ParentReference = itemRef
                });

            DriveItemMtbl driveItem = ConvertDriveFile(item, newPrIdnf, false);
            return driveItem;
        }

        public async Task<DriveItemMtbl> MoveFolderAsync(
            IDriveItemIdnf idnf,
            IDriveItemIdnf newPrIdnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            if (string.IsNullOrWhiteSpace(newFolderName))
            {
                newFolderName = graphItem.Name;
            }

            var item = new DriveItem
            {
                Id = idnf.Id,
                Name = newFolderName,
                ParentReference = new ItemReference
                {
                    Id = newPrIdnf.Id,
                }
            };

            item = await fileReqBuilder.PatchAsync(item);
            DriveItemMtbl driveItem = ConvertDriveFolder(item, newPrIdnf, false);

            return driveItem;
        }

        public async Task<DriveItemMtbl> RenameFileAsync(
            IDriveItemIdnf idnf,
            string newFileName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            var item = await fileReqBuilder.PatchAsync(
                new DriveItem
                {
                    Id = idnf.Id,
                    Name = newFileName
                });

            DriveItemMtbl driveItem = ConvertDriveFile(item, idnf.GetPrIdnf(), false);
            return driveItem;
        }

        public async Task<DriveItemMtbl> RenameFolderAsync(
            IDriveItemIdnf idnf,
            string newFolderName)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            var item = new DriveItem
            {
                Id = idnf.Id,
                Name = newFolderName
            };

            item = await fileReqBuilder.PatchAsync(item);
            DriveItemMtbl driveItem = ConvertDriveFolder(item, idnf.GetPrIdnf(), false);

            return driveItem;
        }

        private async Task<string> GetDriveItemUrlAsync(
            IDriveItemIdnf idnf)
        {
            var myDriveRequestBuilder = await GetMyDriveRequestBuilderAsync();
            var fileReqBuilder = myDriveRequestBuilder.Items[idnf.Id];
            var graphItem = await fileReqBuilder.GetAsync();

            string url = graphItem.WebUrl;
            return url;
        }
    }
}
