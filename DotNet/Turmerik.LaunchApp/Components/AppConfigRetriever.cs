using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Turmerik.LaunchApp.Components
{
    public class AppConfigRetriever
    {
        public static readonly string AppConfigFileName = string.Join(".",
            AssmbH.AssemblyName,
            nameof(AppConfig),
            "xml");

        public static readonly string DefaultAppConfigFileName = string.Join(".",
            AssmbH.AssemblyName,
            nameof(AppConfig),
            "Default",
            "xml");

        public AppConfig GetAppConfig()
        {
            var appConfig = GetAppConfigCore();
            NormalizeAppConfig(appConfig);

            return appConfig;
        }

        public void NormalizeAppConfig(
            AppConfig appConfig)
        {
            NormalizeAppConfigCore(appConfig);
            NormalizeApps(appConfig);
        }

        public AppConfig GetDefaultAppConfig() => new AppConfig
        {
            Apps = new MappedApp[]
            {
                new MappedApp
                {
                    CmdName = "dp",
                    AssemblyName = "Turmerik.NotesMkFsDirsPair.ConsoleApp"
                },
                new MappedApp
                {
                    CmdName = "tx",
                    ArgsToSkip = 1,
                    AssemblyName = "Turmerik.TextUtils.ConsoleApp"
                },
            }
        };

        private void NormalizeAppConfigCore(
            AppConfig appConfig)
        {
            appConfig.EnvDir = appConfig.EnvDir ?? GetEnvDirDfLocator();
            appConfig.EnvBinDir = appConfig.EnvBinDir ?? GetEnvBinDirDfLocator();

            NormalizeAppConfigEnvDir(appConfig);
            NormalizeAppConfigEnvBinDir(appConfig);
        }

        private void NormalizeApps(
            AppConfig appConfig)
        {
            foreach (var app in appConfig.Apps)
            {
                NormalizeApp(appConfig, app);
            }
        }

        private void NormalizeApp(
            AppConfig appConfig,
            MappedApp app)
        {
            NormalizeAppCore(app);
            NormalizeAppConfigAssemblyDir(appConfig, app);
            NormalizeAppConfigAssemblyDeployDir(app);
            NormalizeAppConfigAssemblyFile(app);
        }

        private void NormalizeAppCore(
            MappedApp app)
        {
            app.AssemblyDir = app.AssemblyDir ?? GetAssemblyDirDfLocator(
                app.AssemblyName);

            app.AssemblyDeployDir = app.AssemblyDeployDir ?? GetAssemblyDeployDirDfLocator();

            app.AssemblyFile = app.AssemblyFile ?? GetAssemblyFileDfLocator(
                app.AssemblyName);
        }

        private void NormalizeAppConfigEnvDir(
            AppConfig appConfig)
        {
            appConfig.EnvDir.AbsPath = appConfig.EnvDir.AbsPath ?? Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                appConfig.EnvDir.RelPath);
        }

        private void NormalizeAppConfigEnvBinDir(
            AppConfig appConfig)
        {
            appConfig.EnvBinDir.AbsPath = appConfig.EnvBinDir.AbsPath ?? Path.Combine(
                appConfig.EnvDir.AbsPath,
                appConfig.EnvBinDir.RelPath);
        }

        private void NormalizeAppConfigAssemblyDir(
            AppConfig appConfig,
            MappedApp app)
        {
            app.AssemblyDir.AbsPath = app.AssemblyDir.AbsPath ?? Path.Combine(
                appConfig.EnvBinDir.AbsPath,
                app.AssemblyDir.RelPath);
        }

        private void NormalizeAppConfigAssemblyDeployDir(
            MappedApp app)
        {
            app.AssemblyDeployDir.AbsPath = app.AssemblyDeployDir.AbsPath ?? Path.Combine(
                app.AssemblyDir.AbsPath,
                app.AssemblyDeployDir.RelPath);
        }

        private void NormalizeAppConfigAssemblyFile(
            MappedApp app)
        {
            app.AssemblyFile.AbsPath = app.AssemblyFile.AbsPath ?? Path.Combine(
                app.AssemblyDeployDir.AbsPath,
                app.AssemblyFile.RelPath);
        }

        private FsEntryLocator GetEnvDirDfLocator() => new FsEntryLocator
        {
            RelPath = Path.Combine("Turmerik", "Apps")
        };

        private FsEntryLocator GetEnvBinDirDfLocator() => new FsEntryLocator
        {
            RelPath = "Bin"
        };

        private FsEntryLocator GetAssemblyDirDfLocator(
            string assemblyName) => new FsEntryLocator
            {
                RelPath = assemblyName
            };

        private FsEntryLocator GetAssemblyDeployDirDfLocator() => new FsEntryLocator
        {
            RelPath = "Release"
        };

        private FsEntryLocator GetAssemblyFileDfLocator(
            string assemblyName) => new FsEntryLocator
            {
                RelPath = string.Join(".", assemblyName, "exe")
            };

        private AppConfig GetAppConfigCore() => TryLoadAppConfig() ?? GetDefaultAppConfig();

        private AppConfig TryLoadAppConfig() => XmlH.TryDeserializeXmlFromFile<AppConfig>(AppConfigFileName);
    }
}
