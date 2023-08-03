using Turmerik.AspNetCore.Infrastucture;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Data;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Services
{
    public interface IClientAppSettingsService : IClientAppSettingsServiceCore<ClientAppSettings.IClnbl, ClientAppSettings.Immtbl, ClientAppSettings.Mtbl>
    {
    }

    public class ClientAppSettingsService : ClientAppSettingsServiceCoreBase<ClientAppSettings.IClnbl, ClientAppSettings.Immtbl, ClientAppSettings.Mtbl>, IClientAppSettingsService
    {
        protected override ClientAppSettings.Immtbl GetClientAppSettings()
        {
            var mtbl = new ClientAppSettings.Mtbl();
            AssignClientAppSettingsPropsCore(mtbl);

            var immtbl = new ClientAppSettings.Immtbl(mtbl);
            return immtbl;
        }
    }
}
