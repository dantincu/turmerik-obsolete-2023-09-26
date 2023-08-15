using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text;

namespace Turmerik.DriveExplorerCore
{
    public interface IDriveItemsRetriever
    {
        Task<DriveItem.Mtbl> GetFolderAsync(DriveItemIdnf.IClnbl idnf);
        Task<bool> FolderExistsAsync(DriveItemIdnf.IClnbl idnf);
        Task<bool> FileExistsAsync(DriveItemIdnf.IClnbl idnf);

        string DirSeparator { get; }
    }

    public interface IDriveItemsObjMirrorRetriever : IDriveItemsRetriever
    {
        DriveItem.IClnbl RoodDriveFolder { get; }
    }

    public abstract class DriveItemsRetrieverBase : IDriveItemsRetriever
    {
        protected static readonly ReadOnlyDictionary<OfficeLikeFileType, ReadOnlyCollection<string>> OfficeLikeFileTypesFileNameExtensions;

        static DriveItemsRetrieverBase()
        {
            OfficeLikeFileTypesFileNameExtensions = new Dictionary<OfficeLikeFileType, ReadOnlyCollection<string>>
            {
                { OfficeLikeFileType.Docs, new string[] { ".docx", ".doc" }.RdnlC() },
                { OfficeLikeFileType.Sheets, new string[] { ".xlsx", ".xls" }.RdnlC() },
                { OfficeLikeFileType.Slides, new string[] { ".pptx" , ".ppt" }.RdnlC() },
            }.RdnlD();
        }

        public DriveItemsRetrieverBase(
            ITimeStampHelper timeStampHelper)
        {
            TimeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            DirSeparator = GetDirSeparator();
        }

        public string DirSeparator { get; }

        protected ITimeStampHelper TimeStampHelper { get; }

        public abstract Task<bool> FileExistsAsync(DriveItemIdnf.IClnbl idnf);
        public abstract Task<bool> FolderExistsAsync(DriveItemIdnf.IClnbl idnf);
        public abstract Task<DriveItem.Mtbl> GetFolderAsync(DriveItemIdnf.IClnbl idnf);

        protected abstract string GetDirSeparator();

        protected string GetTimeStampStr(DateTime? dateTime)
        {
            string timeStampStr = null;

            if (dateTime.HasValue)
            {
                DateTime dateTimeValue = dateTime.Value;

                timeStampStr = TimeStampHelper.TmStmp(
                    dateTimeValue,
                    true,
                    TimeStamp.Seconds);
            }

            return timeStampStr;
        }
    }

    public class DriveItemsObjMirrorRetriever : IDriveItemsObjMirrorRetriever
    {
        private DriveItem.Immtbl driveItem;

        public DriveItemsObjMirrorRetriever()
        {
            DirSeparator = GetDirSeparator();
        }

        public string DirSeparator { get; }

        public DriveItem.IClnbl RoodDriveFolder
        {
            get
            {
                return driveItem;
            }

            set
            {
                driveItem = new DriveItem.Immtbl(value);
            }
        }

        public async Task<DriveItem.Mtbl> GetFolderAsync(DriveItemIdnf.IClnbl idnf)
        {
            DriveItem.Mtbl retMtbl = TryGetItem(driveItem, idnf, true, true);

            return retMtbl;
        }

        public async Task<bool> FolderExistsAsync(DriveItemIdnf.IClnbl idnf)
        {
            bool retVal = TryGetItem(driveItem, idnf, true, true) != null;
            return retVal;
        }

        public async Task<bool> FileExistsAsync(DriveItemIdnf.IClnbl idnf)
        {
            bool retVal = TryGetItem(driveItem, idnf, false, true) != null;
            return retVal;
        }

        protected string GetDirSeparator() => Path.DirectorySeparatorChar.ToString();

        protected virtual DriveItem.Mtbl TryGetItem(
            DriveItem.Immtbl folder,
            DriveItemIdnf.IClnbl idnf,
            bool isFolder,
            bool isRootFolder)
        {
            DriveItem.Mtbl retMtbl = null;

            if (isFolder)
            {
                if (isRootFolder || (!isRootFolder && folder.Equals(idnf)))
                {
                    retMtbl = new DriveItem.Mtbl(folder);
                }
            }
            else
            {
                var immtbl = folder.FolderFiles.FirstOrDefault(
                    item => item.Equals(idnf));

                if (immtbl != null)
                {
                    retMtbl = new DriveItem.Mtbl(immtbl);
                }
            }

            if (retMtbl == null)
            {
                retMtbl = ((IEnumerable<DriveItem.Immtbl>)folder.SubFolders)?.Select(
                    item => TryGetItem(
                        item, idnf, isFolder, false)).FirstOrDefault(
                    item => item != null);
            }

            return retMtbl;
        }
    }
}
