using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.DriveExplorerCore
{
    public interface IDriveExplorerServiceEngine : IDriveItemsRetriever
    {
        Task<string> GetDriveFolderWebUrlAsync(DriveItemIdnf.IClnbl idnf);
        Task<string> GetDriveFileWebUrlAsync(DriveItemIdnf.IClnbl idnf);
        Task<DriveItem.Mtbl> GetTextFileAsync(DriveItemIdnf.IClnbl idnf);
        Task<DriveItem.Mtbl> CreateFolderAsync(DriveItemIdnf.IClnbl prIdnf, string newFolderName);
        Task<DriveItem.Mtbl> RenameFolderAsync(DriveItemIdnf.IClnbl idnf, string newFolderName);
        Task<DriveItem.Mtbl> CopyFolderAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFolderName);
        Task<DriveItem.Mtbl> MoveFolderAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFolderName);
        Task<DriveItem.Mtbl> DeleteFolderAsync(DriveItemIdnf.IClnbl idnf);
        Task<DriveItem.Mtbl> CreateTextFileAsync(DriveItemIdnf.IClnbl prIdnf, string newFileName, string text);

        Task<DriveItem.Mtbl> CreateOfficeLikeFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType);

        Task<DriveItem.Mtbl> RenameFileAsync(DriveItemIdnf.IClnbl idnf, string newFileName);
        Task<DriveItem.Mtbl> CopyFileAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFileName);
        Task<DriveItem.Mtbl> MoveFileAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFileName);
        Task<DriveItem.Mtbl> DeleteFileAsync(DriveItemIdnf.IClnbl idnf);
    }
}
