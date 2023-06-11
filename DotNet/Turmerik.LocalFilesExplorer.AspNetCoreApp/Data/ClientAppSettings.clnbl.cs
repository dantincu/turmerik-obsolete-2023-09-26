using Turmerik.AspNetCore.Infrastucture;
using static Turmerik.LocalFilesExplorer.AspNetCoreApp.Data.ClientAppSettings;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public partial class ClientAppSettings : ClientAppSettingsCore<IClnbl, Immtbl, Mtbl>
    {
        public interface IClnbl : IClientAppSettingsCore
        {
        }
    }
}
