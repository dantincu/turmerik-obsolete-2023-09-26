using Turmerik.AspNetCore.Infrastucture;
using static Turmerik.LocalFilesExplorer.AspNetCoreApp.Data.AppSettings;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public partial class AppSettings : AppSettingsCore<IClnbl, Imtbl, Mtbl>
    {
        public interface IClnbl : IAppSettingsCore
        {
        }
    }
}
