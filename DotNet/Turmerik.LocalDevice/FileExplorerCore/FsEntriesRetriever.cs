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

namespace Turmerik.LocalDevice.FileExplorerCore
{
    public interface IFsEntriesRetriever : IDriveItemsRetriever
    {
    }

    public class FsEntriesRetriever : DriveItemsRetrieverBase, IFsEntriesRetriever
    {
        public FsEntriesRetriever(ITimeStampHelper timeStampHelper) : base(timeStampHelper)
        {
        }

        public override async Task<DriveItem.Mtbl> GetFolderAsync(
            DriveItemIdnf.IClnbl idnf)
        {
            var entry = new DirectoryInfo(idnf.GetFullPath());
            var folder = GetDriveItem(entry, true);

            var driveItemsArr = entry.EnumerateFileSystemInfos(
                ).Select(fi => GetDriveItem(fi, false)).ToArray();

            folder.SubFolders = new DriveItem.MtblList(
                driveItemsArr.Where(
                    item => item.IsFolder == true));

            folder.FolderFiles = new DriveItem.MtblList(
                driveItemsArr.Where(
                    item => item.IsFolder != true).ToList());

            return folder;
        }

        public override async Task<bool> FolderExistsAsync(
            DriveItemIdnf.IClnbl idnf)
        {
            bool retVal = Directory.Exists(idnf.GetPath());
            return retVal;
        }

        public override async Task<bool> FileExistsAsync(
            DriveItemIdnf.IClnbl idnf)
        {
            bool retVal = File.Exists(idnf.GetPath());
            return retVal;
        }

        protected DriveItem.Mtbl GetDriveItem(
            FileSystemInfo fSysInfo,
            bool isChildItem)
        {
            var fsItemMtbl = new DriveItem.Mtbl
            {
                Name = fSysInfo.Name,
                CreationTimeStr = GetTimeStampStr(fSysInfo.CreationTime),
                LastAccessTimeStr = GetTimeStampStr(fSysInfo.LastAccessTime),
                LastWriteTimeStr = GetTimeStampStr(fSysInfo.LastWriteTime)
            };

            if (!isChildItem)
            {
                string path = fSysInfo.FullName;
                string parentPath = Path.GetDirectoryName(path);

                if (!string.IsNullOrEmpty(parentPath))
                {
                    string parentPrPath = Path.GetDirectoryName(parentPath);
                    string parentName = null;

                    if (!string.IsNullOrEmpty(parentPrPath))
                    {
                        parentName = Path.GetFileName(parentPath);
                    }

                    fsItemMtbl.PrIdnf = new DriveItemIdnf.Mtbl
                    {
                        Name = parentName,
                        PrPath = parentPrPath,
                    };
                }
            }

            if (fSysInfo is DirectoryInfo dirInfo)
            {
                fsItemMtbl.IsFolder = true;
            }
            else if (fSysInfo is FileInfo fInfo)
            {
                string extn = fInfo.Extension.ToLower();
                fsItemMtbl.OfficeLikeFileType = GetOfficeLikeFileType(extn);

                fsItemMtbl.SizeBytesCount = fInfo.Length;

                if (FsH.CommonTextFileExtensions.Contains(extn))
                {
                    fsItemMtbl.IsTextFile = true;
                }
                else if (FsH.CommonImageFileExtensions.Contains(extn))
                {
                    fsItemMtbl.IsImageFile = true;
                }
                else if (FsH.CommonVideoFileExtensions.Contains(extn))
                {
                    fsItemMtbl.IsVideoFile = true;
                }
                else if (FsH.CommonAudioFileExtensions.Contains(extn))
                {
                    fsItemMtbl.IsAudioFile = true;
                }
            }

            return fsItemMtbl;
        }

        protected OfficeLikeFileType? GetOfficeLikeFileType(string extn)
        {
            var matchKvp = OfficeLikeFileTypesFileNameExtensions.SingleOrDefault(
                kvp => kvp.Value.Contains(extn));

            OfficeLikeFileType? retVal = null;

            if (matchKvp.Value != null)
            {
                retVal = matchKvp.Key;
            }

            return retVal;
        }
    }

}
