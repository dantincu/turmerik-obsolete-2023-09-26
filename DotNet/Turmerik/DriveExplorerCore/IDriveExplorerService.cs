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
        Task<HttpActionResult<DriveItemMtbl>> GetFolderAsync(IDriveItemIdnf idnf);
        Task<HttpActionResult<DriveItemMtbl>> GetTextFileAsync(IDriveItemIdnf idnf);
        Task<HttpActionResult<DriveItemMtbl>> CreateFolderAsync(IDriveItemIdnf prIdnf, string newFolderName);
        Task<HttpActionResult<DriveItemMtbl>> RenameFolderAsync(IDriveItemIdnf idnf, string newFolderName);
        Task<HttpActionResult<DriveItemMtbl>> CopyFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName);
        Task<HttpActionResult<DriveItemMtbl>> MoveFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName);
        Task<HttpActionResult<DriveItemMtbl>> DeleteFolderAsync(IDriveItemIdnf idnf);
        Task<HttpActionResult<DriveItemMtbl>> CreateTextFileAsync(IDriveItemIdnf prIdnf, string newFileName, string text);

        Task<HttpActionResult<DriveItemMtbl>> CreateOfficeLikeFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType);

        Task<HttpActionResult<DriveItemMtbl>> RenameFileAsync(IDriveItemIdnf idnf, string newFileName);
        Task<HttpActionResult<DriveItemMtbl>> CopyFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName);
        Task<HttpActionResult<DriveItemMtbl>> MoveFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName);
        Task<HttpActionResult<DriveItemMtbl>> DeleteFileAsync(IDriveItemIdnf idnf);
    }

    public class DriveExplorerService : IDriveExplorerService
    {
        private readonly IDriveExplorerServiceEngine driveExplorerServiceEngine;

        public DriveExplorerService(
            IDriveExplorerServiceEngine driveExplorerServiceEngine)
        {
            this.driveExplorerServiceEngine = driveExplorerServiceEngine ?? throw new ArgumentNullException(
                nameof(driveExplorerServiceEngine));

            DriveItemDefaultExceptionHandler = GetDefaultExceptionHandler<DriveItemMtbl>();
            DriveItemsDefaultExceptionHandler = GetDefaultExceptionHandler<DriveItemMtbl[]>();
        }

        protected Func<Exception, HttpActionResult<DriveItemMtbl>> DriveItemDefaultExceptionHandler { get; }
        protected Func<Exception, HttpActionResult<DriveItemMtbl[]>> DriveItemsDefaultExceptionHandler { get; }

        public async Task<HttpActionResult<DriveItemMtbl>> CopyFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CopyFileAsync(
                    idnf, newPrIdnf, newFileName));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> CopyFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CopyFolderAsync(
                    idnf, newPrIdnf, newFolderName));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> CreateFolderAsync(IDriveItemIdnf prIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateFolderAsync(
                    prIdnf, newFolderName));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> CreateOfficeLikeFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateOfficeLikeFileAsync(
                    prIdnf, newFileName, officeLikeFileType));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> CreateTextFileAsync(IDriveItemIdnf prIdnf, string newFileName, string text)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateTextFileAsync(
                    prIdnf, newFileName, text));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> DeleteFileAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.DeleteFileAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> DeleteFolderAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.DeleteFolderAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> GetFolderAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.GetFolderAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> GetTextFileAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.GetTextFileAsync(idnf));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> MoveFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.MoveFileAsync(
                    idnf, newPrIdnf, newFileName));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> MoveFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.MoveFolderAsync(
                    idnf, newPrIdnf, newFolderName));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> RenameFileAsync(IDriveItemIdnf idnf, string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.RenameFileAsync(
                    idnf, newFileName));

            return result;
        }

        public async Task<HttpActionResult<DriveItemMtbl>> RenameFolderAsync(IDriveItemIdnf idnf, string newFolderName)
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

        private async Task<HttpActionResult<DriveItemMtbl>> ExecuteDriveItemCoreAsync(
            Func<Task<DriveItemMtbl>> action,
            Func<Exception, HttpActionResult<DriveItemMtbl>> excHandler = null)
        {
            excHandler = excHandler.FirstNotNull(DriveItemDefaultExceptionHandler);

            var actionResult = await ExecuteCoreAsync(
                async () => new HttpActionResult<DriveItemMtbl>(
                    true, await action()), excHandler);

            return actionResult;
        }
    }
}
