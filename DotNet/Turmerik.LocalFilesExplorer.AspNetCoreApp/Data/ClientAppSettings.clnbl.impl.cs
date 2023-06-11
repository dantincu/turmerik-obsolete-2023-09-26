
namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public partial class ClientAppSettings
    {
        public class Immtbl : ClientAppSettingsCoreImmtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : ClientAppSettingsCoreMtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
            }
        }
    }
}
