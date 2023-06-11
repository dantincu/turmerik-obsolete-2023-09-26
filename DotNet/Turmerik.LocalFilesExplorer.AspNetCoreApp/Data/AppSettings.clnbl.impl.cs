using Turmerik.AspNetCore.Infrastucture;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public partial class AppSettings
    {
        public class Imtbl : AppSettingsCoreImmtbl, IClnbl
        {
            public Imtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : AppSettingsCoreMtbl, IClnbl
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
