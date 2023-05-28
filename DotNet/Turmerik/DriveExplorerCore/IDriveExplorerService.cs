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
        Task<TrmrkActionResult<DriveItemMtbl>> GetFolderAsync(IDriveItemIdnf idnf);
        Task<TrmrkActionResult<DriveItemMtbl>> GetTextFileAsync(IDriveItemIdnf idnf);
        Task<TrmrkActionResult<DriveItemMtbl>> CreateFolderAsync(IDriveItemIdnf prIdnf, string newFolderName);
        Task<TrmrkActionResult<DriveItemMtbl>> RenameFolderAsync(IDriveItemIdnf idnf, string newFolderName);
        Task<TrmrkActionResult<DriveItemMtbl>> CopyFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName);
        Task<TrmrkActionResult<DriveItemMtbl>> MoveFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName);
        Task<TrmrkActionResult<DriveItemMtbl>> DeleteFolderAsync(IDriveItemIdnf idnf);
        Task<TrmrkActionResult<DriveItemMtbl>> CreateTextFileAsync(IDriveItemIdnf prIdnf, string newFileName, string text);

        Task<TrmrkActionResult<DriveItemMtbl>> CreateOfficeLikeFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType);

        Task<TrmrkActionResult<DriveItemMtbl>> RenameFileAsync(IDriveItemIdnf idnf, string newFileName);
        Task<TrmrkActionResult<DriveItemMtbl>> CopyFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName);
        Task<TrmrkActionResult<DriveItemMtbl>> MoveFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName);
        Task<TrmrkActionResult<DriveItemMtbl>> DeleteFileAsync(IDriveItemIdnf idnf);
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

        protected Func<Exception, TrmrkActionResult<DriveItemMtbl>> DriveItemDefaultExceptionHandler { get; }
        protected Func<Exception, TrmrkActionResult<DriveItemMtbl[]>> DriveItemsDefaultExceptionHandler { get; }

        public async Task<TrmrkActionResult<DriveItemMtbl>> CopyFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CopyFileAsync(
                    idnf, newPrIdnf, newFileName));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> CopyFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CopyFolderAsync(
                    idnf, newPrIdnf, newFolderName));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> CreateFolderAsync(IDriveItemIdnf prIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateFolderAsync(
                    prIdnf, newFolderName));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> CreateOfficeLikeFileAsync(
            IDriveItemIdnf prIdnf,
            string newFileName,
            OfficeLikeFileType officeLikeFileType)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateOfficeLikeFileAsync(
                    prIdnf, newFileName, officeLikeFileType));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> CreateTextFileAsync(IDriveItemIdnf prIdnf, string newFileName, string text)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.CreateTextFileAsync(
                    prIdnf, newFileName, text));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> DeleteFileAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.DeleteFileAsync(idnf));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> DeleteFolderAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.DeleteFolderAsync(idnf));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> GetFolderAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.GetFolderAsync(idnf));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> GetTextFileAsync(IDriveItemIdnf idnf)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.GetTextFileAsync(idnf));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> MoveFileAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.MoveFileAsync(
                    idnf, newPrIdnf, newFileName));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> MoveFolderAsync(IDriveItemIdnf idnf, IDriveItemIdnf newPrIdnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.MoveFolderAsync(
                    idnf, newPrIdnf, newFolderName));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> RenameFileAsync(IDriveItemIdnf idnf, string newFileName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.RenameFileAsync(
                    idnf, newFileName));

            return result;
        }

        public async Task<TrmrkActionResult<DriveItemMtbl>> RenameFolderAsync(IDriveItemIdnf idnf, string newFolderName)
        {
            var result = await ExecuteDriveItemCoreAsync(
                async () => await driveExplorerServiceEngine.RenameFolderAsync(
                    idnf, newFolderName));

            return result;
        }

        private Func<Exception, TrmrkActionResult<TData>> GetDefaultExceptionHandler<TData>()
        {
            Func<Exception, TrmrkActionResult<TData>> handler = exc => HandleException<TData>(exc);
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

        private TrmrkActionResult<TData> HandleException<TData>(Exception exc)
        {
            var httpStatusCode = GetHttpStatusCode(exc);
            var errViewModel = new ErrorViewModel(null, exc);

            var result = new TrmrkActionResult<TData>(
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

        private async Task<TrmrkActionResult<DriveItemMtbl>> ExecuteDriveItemCoreAsync(
            Func<Task<DriveItemMtbl>> action,
            Func<Exception, TrmrkActionResult<DriveItemMtbl>> excHandler = null)
        {
            excHandler = excHandler.FirstNotNull(DriveItemDefaultExceptionHandler);

            var actionResult = await ExecuteCoreAsync(
                async () => new TrmrkActionResult<DriveItemMtbl>(
                    true, await action()), excHandler);

            return actionResult;
        }
    }
}
