using Turmerik.AspNetCore.Infrastucture;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Data;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Services
{
    public interface IAppSettingsService : IAppSettingsServiceCore<AppSettings.IClnbl, AppSettings.Immtbl, AppSettings.Mtbl>
    {
    }

    public class AppSettingsService : AppSettingsServiceCoreBase<AppSettings.IClnbl, AppSettings.Immtbl, AppSettings.Mtbl>, IAppSettingsService
    {
        public AppSettingsService(IConfiguration configuration) : base(configuration)
        {
        }

        protected override AppSettings.Immtbl GetAppSettings()
        {
            var mtbl = new AppSettings.Mtbl();
            AssignClientAppSettingsPropsCore(mtbl);

            var immtbl = new AppSettings.Immtbl(mtbl);
            return immtbl;
        }
    }
}
