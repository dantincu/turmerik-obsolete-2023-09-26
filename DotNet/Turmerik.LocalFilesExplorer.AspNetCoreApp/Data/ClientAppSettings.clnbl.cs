using Turmerik.AspNetCore.Infrastucture;
using static Turmerik.LocalFilesExplorer.AspNetCoreApp.Data.ClientAppSettings;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public partial class ClientAppSettings : ClientAppSettingsCore<IClnbl, Immtbl, Mtbl>
    {
        public interface IClnbl : ICoreClnbl
        {
        }

        public class Immtbl : CoreImmtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : CoreMtbl, IClnbl
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
