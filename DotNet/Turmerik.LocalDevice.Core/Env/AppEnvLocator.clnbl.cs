using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppEnvLocator
    {
        string AppSuiteGroupName { get; }
        string AppSuiteName { get; }
        string AppSuiteGroupEnvBaseDirPath { get; }
        string AppSuiteGroupEnvBaseDirName { get; }
    }

    public class AppEnvLocatorImmtbl : IAppEnvLocator
    {
        public AppEnvLocatorImmtbl(IAppEnvLocator src)
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

    public class AppEnvLocatorMtbl : IAppEnvLocator
    {
        public AppEnvLocatorMtbl()
        {
        }

        public AppEnvLocatorMtbl(IAppEnvLocator src)
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
}
