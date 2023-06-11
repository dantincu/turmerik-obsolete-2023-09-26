using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.LocalDevice.Env
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
            string AppSuiteGroupName { get; }
            string AppSuiteName { get; }
            string AppSuiteGroupEnvBaseDirPath { get; }
            string AppSuiteGroupEnvBaseDirName { get; }
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                AppSuiteGroupName = src.AppSuiteGroupName;
                AppSuiteName = src.AppSuiteName;
                AppSuiteGroupEnvBaseDirPath = src.AppSuiteGroupEnvBaseDirPath;
                AppSuiteGroupEnvBaseDirName = src.AppSuiteGroupEnvBaseDirName;
            }

            public string AppSuiteGroupName { get; }
            public string AppSuiteName { get; }
            public string AppSuiteGroupEnvBaseDirPath { get; }
            public string AppSuiteGroupEnvBaseDirName { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                AppSuiteGroupName = src.AppSuiteGroupName;
                AppSuiteName = src.AppSuiteName;
                AppSuiteGroupEnvBaseDirPath = src.AppSuiteGroupEnvBaseDirPath;
                AppSuiteGroupEnvBaseDirName = src.AppSuiteGroupEnvBaseDirName;
            }

            public string AppSuiteGroupName { get; set; }
            public string AppSuiteName { get; set; }
            public string AppSuiteGroupEnvBaseDirPath { get; set; }
            public string AppSuiteGroupEnvBaseDirName { get; set; }
        }
    }
}
