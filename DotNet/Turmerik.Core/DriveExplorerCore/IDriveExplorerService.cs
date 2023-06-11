using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.DriveExplorerCore
{
    public interface IDriveExplorerService
    {
        Task<HttpActionResult<DriveItem.Mtbl>> GetFolderAsync(DriveItemIdnf.IClnbl idnf);
        Task<HttpActionResult<DriveItem.Mtbl>> GetTextFileAsync(DriveItemIdnf.IClnbl idnf);
        Task<HttpActionResult<DriveItem.Mtbl>> CreateFolderAsync(DriveItemIdnf.IClnbl prIdnf, string newFolderName);
        Task<HttpActionResult<DriveItem.Mtbl>> RenameFolderAsync(DriveItemIdnf.IClnbl idnf, string newFolderName);
        Task<HttpActionResult<DriveItem.Mtbl>> CopyFolderAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFolderName);
        Task<HttpActionResult<DriveItem.Mtbl>> MoveFolderAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFolderName);
        Task<HttpActionResult<DriveItem.Mtbl>> DeleteFolderAsync(DriveItemIdnf.IClnbl idnf);
        Task<HttpActionResult<DriveItem.Mtbl>> CreateTextFileAsync(DriveItemIdnf.IClnbl prIdnf, string newFileName, string text);

        Task<HttpActionResult<DriveItem.Mtbl>> CreateOfficeLikeFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType);

        Task<HttpActionResult<DriveItem.Mtbl>> RenameFileAsync(DriveItemIdnf.IClnbl idnf, string newFileName);
        Task<HttpActionResult<DriveItem.Mtbl>> CopyFileAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFileName);
        Task<HttpActionResult<DriveItem.Mtbl>> MoveFileAsync(DriveItemIdnf.IClnbl idnf, DriveItemIdnf.IClnbl newPrIdnf, string newFileName);
        Task<HttpActionResult<DriveItem.Mtbl>> DeleteFileAsync(DriveItemIdnf.IClnbl idnf);
    }

    public class DriveExplorerService : IDriveExplorerService
    {
        private readonly IDriveExplorerServiceEngine driveExplorerServiceEngine;

        public DriveExplorerService(
            IDriveExplorerServiceEngine driveExplorerServiceEngine)
        {
            this.driveExplorerServiceEngine = driveExplorerServiceEngine ?? throw new ArgumentNullException(
                nameof(driveExplorerServiceEngine));

            DriveItemDefaultExceptionHandler = GetDefaultExceptionHandler<DriveItem.Mtbl>();
            DriveItemsDefaultExceptionHandler = GetDefaultExceptionHandler<DriveItem.Mtbl[]>();
        }

        protected Func<Exception, HttpActionResult<DriveItem.Mtbl>> DriveItemDefaultExceptionHandler { get; }
        protected Func<Exception, HttpActionResult<DriveItem.Mtbl[]>> DriveItemsDefaultExceptionHandler { get; }

        public async Task<HttpActionResult<DriveItem.Mtbl>> CopyFileAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CopyFileAsync(
                    idnf, newPrIdnf, newFileName));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> CopyFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CopyFolderAsync(
                    idnf, newPrIdnf, newFolderName));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> CreateFolderAsync(DriveItemIdnf.IClnbl prIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateFolderAsync(
                    prIdnf, newFolderName));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> CreateOfficeLikeFileAsync(
            DriveItemIdnf.IClnbl prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateOfficeLikeFileAsync(
                    prIdnf, newFileName, officeLikeFileType));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> CreateTextFileAsync(DriveItemIdnf.IClnbl prIdnf, string newFileName, string text)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateTextFileAsync(
                    prIdnf, newFileName, text));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> DeleteFileAsync(DriveItemIdnf.IClnbl idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.DeleteFileAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> DeleteFolderAsync(DriveItemIdnf.IClnbl idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.DeleteFolderAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> GetFolderAsync(DriveItemIdnf.IClnbl idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.GetFolderAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> GetTextFileAsync(DriveItemIdnf.IClnbl idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.GetTextFileAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> MoveFileAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.MoveFileAsync(
                    idnf, newPrIdnf, newFileName));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> MoveFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            DriveItemIdnf.IClnbl newPrIdnf,
            string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.MoveFolderAsync(
                    idnf, newPrIdnf, newFolderName));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> RenameFileAsync(
            DriveItemIdnf.IClnbl idnf,
            string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.RenameFileAsync(
                    idnf, newFileName));

            return result;
        }

        public async Task<HttpActionResult<DriveItem.Mtbl>> RenameFolderAsync(
            DriveItemIdnf.IClnbl idnf,
            string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.RenameFolderAsync(
                    idnf, newFolderName));

            return result;
        }

        private Func<Exception, HttpActionResult<TData>> GetDefaultExceptionHandler<TData>()
        {
            Func<Exception, HttpActionResult<TData>> handler = exc => HandleException<TData>(exc);
            return handler;
        }

        private HttpStatusCode? GetHttpStatusCode(Exception exc)
        {
            HttpStatusCode? httpStatusCode = null;

            if (exc is InternalAppError err)
            {
                httpStatusCode = err.HttpStatusCode;
            }

            return httpStatusCode;
        }

        private HttpActionResult<TData> HandleException<TData>(Exception exc)
        {
            var httpStatusCode = GetHttpStatusCode(exc);
            var errViewModel = new TrmrkActionError(null, exc);

            var result = new HttpActionResult<TData>(
                false, default, errViewModel, httpStatusCode);

            return result;
        }

        private TResult ExecuteCore<TResult>(
            Func<TResult> action,
            Func<Exception, TResult> excHandler)
        {
            TResult actionResult;

            try
            {
                actionResult = action();
            }
            catch (Exception exc)
            {
                actionResult = excHandler(exc);
            }

            return actionResult;
        }

        private async Task<TResult> ExecuteCoreAsync<TResult>(
            Func<Task<TResult>> action,
            Func<Exception, TResult> excHandler)
        {
            TResult actionResult;

            try
            {
                actionResult = await action();
            }
            catch (Exception exc)
            {
                actionResult = excHandler(exc);
            }

            return actionResult;
        }

        private async Task<HttpActionResult<DriveItem.Mtbl>> ExecuteDriveItemCoreAsync(
            Func<Task<DriveItem.Mtbl>> action,
            Func<Exception, HttpActionResult<DriveItem.Mtbl>> excHandler = null)
        {
            excHandler = excHandler.FirstNotNull(DriveItemDefaultExceptionHandler);

            var actionResult = await ExecuteCoreAsync(
                async () => new HttpActionResult<DriveItem.Mtbl>(
                    true, await action()), excHandler);

            return actionResult;
        }
    }
}
