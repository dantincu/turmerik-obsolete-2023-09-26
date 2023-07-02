using Turmerik.AspNetCore.Infrastucture;
using static Turmerik.LocalFilesExplorer.AspNetCoreApp.Data.AppSettings;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Data
{
    public partial class AppSettings : AppSettingsCore<IClnbl, Imtbl, Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
        }

        public class Imtbl : ImmtblCore, IClnbl
        {
            public Imtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : MtblCore, IClnbl
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
