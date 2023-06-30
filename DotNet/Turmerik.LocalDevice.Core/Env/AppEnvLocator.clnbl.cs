using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.LocalDevice.Core.Env
{
    public enum AppEnvDir
    {
        Config = 1,
        Data,
        Logs,
        Content,
        Bin,
        Temp,
        Src
    }

    public partial class AppEnvLocator : ClnblCore<AppEnvLocator.IClnbl, AppEnvLocator.Immtbl, AppEnvLocator.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            string AppEnvDirBasePath { get; }
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                AppEnvDirBasePath = src.AppEnvDirBasePath;
            }

            public string AppEnvDirBasePath { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                AppEnvDirBasePath = src.AppEnvDirBasePath;
            }

            public string AppEnvDirBasePath { get; set; }
        }
    }
}
