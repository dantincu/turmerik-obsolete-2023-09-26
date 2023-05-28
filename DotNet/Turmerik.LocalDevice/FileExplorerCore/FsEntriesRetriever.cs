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

        public override async Task<DriveItemMtbl> GetFolderAsync(
            IDriveItemIdnf idnf)
        {
            var entry = new DirectoryInfo(idnf.GetFullPath());
            var folder = GetDriveItem(entry, true);

            var driveItemsArr = entry.EnumerateFileSystemInfos(
                ).Select(fi => GetDriveItem(fi, false)).ToArray();

            folder.SubFolders = driveItemsArr.Where(
                item => item.IsFolder == true).ToList();

            folder.FolderFiles = driveItemsArr.Where(
                item => item.IsFolder != true).ToList();

            return folder;
        }

        public override async Task<bool> FolderExistsAsync(IDriveItemIdnf idnf)
        {
            bool retVal = Directory.Exists(idnf.GetPath());
            return retVal;
        }

        public override async Task<bool> FileExistsAsync(IDriveItemIdnf idnf)
        {
            bool retVal = File.Exists(idnf.GetPath());
            return retVal;
        }

        protected DriveItemMtbl GetDriveItem(
            FileSystemInfo fSysInfo,
            bool isChildItem)
        {
            var fsItemMtbl = new DriveItemMtbl
            {
                Idnf = new DriveItemIdnfMtbl
                {
                    Name = fSysInfo.Name,
                },
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

                    fsItemMtbl.Idnf.PrIdnf = new DriveItemIdnfMtbl
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
