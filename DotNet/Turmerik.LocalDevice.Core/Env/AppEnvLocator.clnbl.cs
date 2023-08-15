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

    public partial class AppEnvLocator
    {
        public interface IClnbl
        {
            string AppEnvDirBasePath { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                AppEnvDirBasePath = src.AppEnvDirBasePath;
            }

            public string AppEnvDirBasePath { get; }
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                AppEnvDirBasePath = src.AppEnvDirBasePath;
            }

            public string AppEnvDirBasePath { get; set; }
        }
    }
}
