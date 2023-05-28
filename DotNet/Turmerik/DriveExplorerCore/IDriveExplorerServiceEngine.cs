using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.DriveExplorerCore
{
    public interface IDriveExplorerServiceEngine : IDriveItemsRetriever
    {
        Task<string> GetDriveFolderWebUrlAsync(IDriveItemIdnf idnf);
        Task<string> GetDriveFileWebUrlAsync(IDriveItemIdnf idnf);
        Task<DriveItemMtbl> GetTextFileAsync(IDriveItemIdnf idnf);
        Task<DriveItemMtbl> CreateFolderAsync(IDriveItemIdnf prIdnf, string newFolderName);
        Task<DriveItemMtbl> RenameFolderAsync(IDriveItemIdnf idnf, string newFolderName);
        Task<DriveItemMtbl> CopyFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName);
        Task<DriveItemMtbl> MoveFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName);
        Task<DriveItemMtbl> DeleteFolderAsync(IDriveItemIdnf idnf);
        Task<DriveItemMtbl> CreateTextFileAsync(IDriveItemIdnf prIdnf, string newFileName, string text);

        Task<DriveItemMtbl> CreateOfficeLikeFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType);

        Task<DriveItemMtbl> RenameFileAsync(IDriveItemIdnf idnf, string newFileName);
        Task<DriveItemMtbl> CopyFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName);
        Task<DriveItemMtbl> MoveFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName);
        Task<DriveItemMtbl> DeleteFileAsync(IDriveItemIdnf idnf);
    }
}
