using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;
using Turmerik.Reflection;
using Turmerik.Text;
using System.IO;
using Turmerik.Collections;

namespace Turmerik.LocalDevice.Core.Env
{
    public interface IAppEnv
    {
        AppEnvLocator.IClnbl Locator { get; }
        string AppEnvDirBasePath { get; }
        string GetPath(AppEnvDir appEnvDir, params string[] pathPartsArr);
        string GetTypePath(AppEnvDir appEnvDir, Type dirNameType, params string[] pathPartsArr);
    }

    public class AppEnv : IAppEnv
    {
        public AppEnv(
            ITimeStampHelper timeStampHelper)
        {
            TimeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            Locator = GetAppEnvLocatorImmtbl();
            AppEnvDirBasePath = GetAppEnvDirBasePath(Locator);
        }

        public AppEnvLocator.IClnbl Locator { get; }
        public string AppEnvDirBasePath { get; }
        protected ITimeStampHelper TimeStampHelper { get; }

        protected virtual string AppEnvLocatorFilePath => "app-env-locator.json";

        public string GetPath(
            AppEnvDir appEnvDir,
            params string[] pathPartsArr)
        {
            pathPartsArr = new string[] {
                AppEnvDirBasePath,
                appEnvDir > 0 ? appEnvDir.ToString() : null,
            }.NotNull().Concat(pathPartsArr).ToArray();

            string retPath = Path.Combine(pathPartsArr);
            return retPath;
        }

        public string GetTypePath(
            AppEnvDir appEnvDir,
            Type dirNameType,
            params string[] pathPartsArr)
        {
            pathPartsArr = new string[] {
                AppEnvDirBasePath,
                appEnvDir > 0 ? appEnvDir.ToString() : null,
                dirNameType?.GetTypeFullDisplayName()
            }.NotNull().Concat(pathPartsArr).ToArray();

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
                AppEnvDirBasePath = DefaultDirNames.APP_ENV_DIR_BASE_REL_PATH,
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

        private string GetAppEnvDirBasePath(AppEnvLocator.IClnbl appEnvLocator)
        {
            string appEnvDirBasePath = appEnvLocator.AppEnvDirBasePath;

            if (!Path.IsPathRooted(appEnvDirBasePath))
            {
                appEnvDirBasePath = Path.Combine(
                    GetFolderPath(
                        SpecialFolder.ApplicationData),
                    appEnvDirBasePath);
            }

            return appEnvDirBasePath;
        }

        private AppEnvLocator.Immtbl GetAppEnvLocatorImmtbl()
        {
            var mtbl = GetAppEnvLocatorMtbl() ?? GetDefaultAppEnvLocatorMtbl();
            var immtbl = new AppEnvLocator.Immtbl(mtbl);

            return immtbl;
        }

        public static class DefaultDirNames
        {
            public const string APP_ENV_DIR_BASE_REL_PATH = "Turmerik/App";
        }
    }
}
