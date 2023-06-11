using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;
using Turmerik.Reflection;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Env
{
    public interface IAppEnv
    {
        AppEnvLocator.IClnbl Locator { get; }
        string AppSuiteEnvBasePath { get; }
        string GetPath(AppEnvDir appEnvDir, Type dirNameType, params string[] pathPartsArr);
    }

    public class AppEnv : IAppEnv
    {
        public AppEnv(
            ITimeStampHelper timeStampHelper)
        {
            TimeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            Locator = GetAppEnvLocatorImmtbl();
            AppSuiteEnvBasePath = GetAppSuiteEnvBasePath(Locator);
        }

        public AppEnvLocator.IClnbl Locator { get; }
        public string AppSuiteEnvBasePath { get; }
        protected ITimeStampHelper TimeStampHelper { get; }

        protected virtual string AppEnvLocatorFilePath => "app-env-locator.json";

        public string GetPath(
            AppEnvDir appEnvDir,
            Type dirNameType,
            params string[] pathPartsArr)
        {
            pathPartsArr = new string[] {
                AppSuiteEnvBasePath,
                appEnvDir > 0 ? appEnvDir.ToString() : null,
                dirNameType?.GetTypeFullDisplayName()
            }.Where(part => part != null).Concat(pathPartsArr).ToArray();

            string retPath = Path.Combine(pathPartsArr);
            return retPath;
        }

        protected virtual AppEnvLocator.Mtbl GetAppEnvLocatorMtbl()
        {
            AppEnvLocator.Mtbl appEnvLocator = null;
            string appEnvLocatorFilePath = AppEnvLocatorFilePath;

            if (!string.IsNullOrWhiteSpace(
                appEnvLocatorFilePath) && File.Exists(
                    appEnvLocatorFilePath))
            {
                var appEnvLocatorJson = File.ReadAllText(
                    appEnvLocatorFilePath);

                appEnvLocator = JsonH.FromJson<AppEnvLocator.Mtbl>(
                    appEnvLocatorJson);
            }

            return appEnvLocator;
        }

        protected virtual AppEnvLocator.Mtbl GetDefaultAppEnvLocatorMtbl()
        {
            var mtbl = new AppEnvLocator.Mtbl
            {
                AppSuiteGroupEnvBaseDirPath = GetFolderPath(SpecialFolder.ApplicationData),
                AppSuiteGroupEnvBaseDirName = string.Empty,
                AppSuiteGroupName = DefaultDirNames.APP_SUITE_GROUP_NAME,
                AppSuiteName = DefaultDirNames.APP_SUITE_NAME,
            };

            return mtbl;
        }

        protected string GetStrValue(string value, string defaultValue)
        {
            string retValue = value;

            if (string.IsNullOrWhiteSpace(value))
            {
                retValue = defaultValue;
            }

            return retValue;
        }

        private string GetAppSuiteEnvBasePath(AppEnvLocator.IClnbl appEnvLocator)
        {
            string[] pathParts = new string[]
            {
                appEnvLocator.AppSuiteGroupEnvBaseDirPath,
                appEnvLocator.AppSuiteGroupEnvBaseDirName,
                appEnvLocator.AppSuiteGroupName,
                appEnvLocator.AppSuiteName
            }.Where(part => !string.IsNullOrWhiteSpace(part)).ToArray();

            string retPath = Path.Combine(pathParts);
            return retPath;
        }

        private AppEnvLocator.Immtbl GetAppEnvLocatorImmtbl()
        {
            var mtbl = GetAppEnvLocatorMtbl() ?? GetDefaultAppEnvLocatorMtbl();
            var immtbl = new AppEnvLocator.Immtbl(mtbl);

            return immtbl;
        }

        public static class DefaultDirNames
        {
            public const string APP_SUITE_GROUP_NAME = "Turmerik";
            public const string APP_SUITE_NAME = "UtilityApps";
        }
    }
}
