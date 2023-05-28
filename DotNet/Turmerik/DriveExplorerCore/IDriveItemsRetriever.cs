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
        Task<DriveItemMtbl> GetFolderAsync(IDriveItemIdnf idnf);
        Task<bool> FolderExistsAsync(IDriveItemIdnf idnf);
        Task<bool> FileExistsAsync(IDriveItemIdnf idnf);
    }

    public interface IDriveItemsObjMirrorRetriever : IDriveItemsRetriever
    {
        IDriveItem RoodDriveFolder { get; }
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
        }

        protected ITimeStampHelper TimeStampHelper { get; }

        public abstract Task<bool> FileExistsAsync(IDriveItemIdnf idnf);
        public abstract Task<bool> FolderExistsAsync(IDriveItemIdnf idnf);
        public abstract Task<DriveItemMtbl> GetFolderAsync(IDriveItemIdnf idnf);

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
        private DriveItemImmtbl driveItem;

        public IDriveItem RoodDriveFolder
        {
            get
            {
                return driveItem;
            }

            set
            {
                driveItem = new DriveItemImmtbl(value);
            }
        }

        public async Task<DriveItemMtbl> GetFolderAsync(IDriveItemIdnf idnf)
        {
            DriveItemMtbl retMtbl = TryGetItem(driveItem, idnf, true, true);

            return retMtbl;
        }

        public async Task<bool> FolderExistsAsync(IDriveItemIdnf idnf)
        {
            bool retVal = TryGetItem(driveItem, idnf, true, true) != null;
            return retVal;
        }

        public async Task<bool> FileExistsAsync(IDriveItemIdnf idnf)
        {
            bool retVal = TryGetItem(driveItem, idnf, false, true) != null;
            return retVal;
        }

        protected virtual DriveItemMtbl TryGetItem(
            DriveItemImmtbl folder,
            IDriveItemIdnf idnf,
            bool isFolder,
            bool isRootFolder)
        {
            DriveItemMtbl retMtbl = null;

            if (isFolder)
            {
                if (isRootFolder || !isRootFolder && folder.Idnf.Equals(idnf))
                {
                    retMtbl = new DriveItemMtbl(folder);
                }
            }
            else
            {
                var immtbl = folder.FolderFiles.FirstOrDefault(
                    item => item.Idnf.Equals(idnf));

                if (immtbl != null)
                {
                    retMtbl = new DriveItemMtbl(immtbl);
                }
            }

            if (retMtbl == null)
            {
                retMtbl = folder.SubFolders?.Select(
                    item => TryGetItem(
                        item, idnf, isFolder, false)).FirstOrDefault(
                    item => item != null);
            }

            return retMtbl;
        }
    }
}
