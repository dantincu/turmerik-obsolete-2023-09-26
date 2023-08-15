using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.DriveExplorerCore;
using Turmerik.FileSystem;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.FileExplorerCore
{
    public interface IFsExplorerServiceEngine : IFsEntriesRetriever, IDriveExplorerServiceEngine
    {
    }

    public class FsExplorerServiceEngine : FsEntriesRetriever, IFsExplorerServiceEngine
    {
        public FsExplorerServiceEngine(
            ITimeStampHelper timeStampHelper) : base(timeStampHelper)
        {
        }

        public async Task<DriveItem.Mtbl> CopyFileAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFileName)
        {
            string newPath = Path.Combine(
                newPrIdnf.GetFullPath(DirSeparator),
                newFileName);

            File.Copy(
                idnf.GetFullPath(DirSeparator),
                newPath);

            var newEntry = new FileInfo(newPath);
            var item = GetDriveItem(newEntry, false);

            return item;
        }

        public async Task<DriveItem.Mtbl> CopyFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFolderName)
        {
            string newPath = Path.Combine(
                newPrIdnf.GetFullPath(DirSeparator),
                newFolderName);

            FsH.CopyDirectory(
                idnf.GetFullPath(DirSeparator),
                newPath);

            var newEntry = new DirectoryInfo(newPath);
            var item = GetDriveItem(newEntry, false);

            return item;
        }

        public async Task<DriveItem.Mtbl> CreateFolderAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFolderName)
        {
            string newPath = Path.Combine(
                prIdnf.GetFullPath(DirSeparator),
                newFolderName);

            Directory.CreateDirectory(newPath);

            var newEntry = new DirectoryInfo(newPath);
            var item = GetDriveItem(newEntry, false);

            return item;
        }

        public async Task<DriveItem.Mtbl> CreateOfficeLikeFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType)
        {
            var result = await CreateTextFileAsync(
                prIdnf,
                newFileName,
                string.Empty);

            return result;
        }

        public async Task<DriveItem.Mtbl> CreateTextFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            string text)
        {
            string newPath = Path.Combine(
                prIdnf.GetFullPath(DirSeparator),
                newFileName);

            File.WriteAllText(newPath, text);

            var newEntry = new FileInfo(newPath);
            var item = GetDriveItem(newEntry, false);

            return item;
        }

        public async Task<DriveItem.Mtbl> DeleteFileAsync(DriveItemIdnf.IClnbl idnf)
        {
            var fileInfo = new FileInfo(
                idnf.GetFullPath(DirSeparator));

            var driveItem = GetDriveItem(fileInfo, false);

            fileInfo.Delete();
            return driveItem;
        }

        public async Task<DriveItem.Mtbl> DeleteFolderAsync(DriveItemIdnf.IClnbl idnf)
        {
            var dirInfo = new DirectoryInfo(
                idnf.GetFullPath(DirSeparator));

            var driveItem = GetDriveItem(dirInfo, false);
            dirInfo.Delete(true);

            return driveItem;
        }

        public async Task<string> GetDriveFolderWebUrlAsync(
            DriveItemIdnf.IClnbl idnf) => GetDriveItemUrl(
                idnf.GetFullPath(DirSeparator));

        public async Task<string> GetDriveFileWebUrlAsync(
            DriveItemIdnf.IClnbl idnf) => GetDriveItemUrl(
                idnf.GetFullPath(DirSeparator));

        /* public async Task<DriveItemMtbl> GetRootFolderAsync()
        {
            var fsEntriesList = new List<DriveItemMtbl>();
            string userHomePath = GetFolderPath(SpecialFolder.UserProfile);

            var drives = DriveInfo.GetDrives(
                ).Where(d => d.IsReady).Select(
                d => new DriveItemMtbl
                {
                    Id = d.Name,
                    Name = d.Name,
                    IsFolder = true,
                });

            var folders = new Dictionary<SpecialFolder, string>
                    {
                        { SpecialFolder.UserProfile, "User Home" },
                        { SpecialFolder.ApplicationData, "Application Data" },
                        { SpecialFolder.MyDocuments, "Documents" },
                        { SpecialFolder.MyPictures, "Pictures" },
                        { SpecialFolder.MyVideos, "Videos" },
                        { SpecialFolder.MyMusic, "Music" },
                        { SpecialFolder.Desktop, "Desktop" }
                    }.Select(
                kvp =>
                {
                    string path = GetFolderPath(kvp.Key);
                    string name = path;

                    if (name.StartsWith(userHomePath))
                    {
                        name = name.Substring(userHomePath.Length).TrimStart('/', '\\');
                        name = $"~{Path.DirectorySeparatorChar}{name}";
                    }

                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                    var item = GetDriveItem(dirInfo);

                    return item;
                });

            fsEntriesList.AddRange(drives);
            fsEntriesList.AddRange(folders);

            var rootFolder = new DriveItemMtbl
            {
                Idnf = 
                Id = string.Empty,
                Name = "This PC",
                IsFolder = true,
                IsRootFolder = true,
                SubFolders = fsEntriesList,
                FolderFiles = new List<DriveItemMtbl>()
            };

            return rootFolder;
        }*/

        public async Task<DriveItem.Mtbl> GetTextFileAsync(DriveItemIdnf.IClnbl idnf)
        {
            string path = idnf.GetFullPath(DirSeparator);
            var entry = new FileInfo(path);

            var fileItem = GetDriveItem(entry, false);
            fileItem.TextFileContent = File.ReadAllText(path);

            return fileItem;
        }

        public async Task<DriveItem.Mtbl> MoveFileAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFileName)
        {
            string path = idnf.GetFullPath(DirSeparator);
            string newPath = Path.Combine(path, newFileName);

            File.Move(path, newPath);
            var newEntry = new FileInfo(newPath);

            var item = GetDriveItem(newEntry, false);
            return item;
        }

        public async Task<DriveItem.Mtbl> MoveFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFolderName)
        {
            string path = idnf.GetFullPath(DirSeparator);
            string newPrPath = newPrIdnf.GetFullPath(DirSeparator);

            string newPath = Path.Combine(
                newPrPath,
                newFolderName);

            FsH.MoveDirectory(path, newPath);

            var newEntry = new DirectoryInfo(newPath);
            var item = GetDriveItem(newEntry, false);

            return item;
        }

        public async Task<DriveItem.Mtbl> RenameFileAsync(
            DriveItemIdnf.IClnbl idnf,
            string newFileName)
        {
            var result = await MoveFileAsync(
                idnf,
                idnf.GetPrIdnf(),
                newFileName);

            return result;
        }

        public async Task<DriveItem.Mtbl> RenameFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            string newFolderName)
        {
            var result = await MoveFolderAsync(
                idnf,
                idnf.GetPrIdnf(),
                newFolderName);

            return result;
        }

        private string GetDriveItemUrl(string path)
        {
            string driveItemUrl = $"file://{path}";
            return driveItemUrl;
        }
    }
}
