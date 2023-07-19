// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Timers;
using Turmerik.Dependencies;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.Logging;
using Turmerik.SharedLogFileTestConsoleApp;

var services = new ServiceCollection();
LocalDeviceServiceCollectionBuilder.RegisterAll(services);

ServiceProviderContainer.Instance.Value.RegisterServices(services);
var svcProv = ServiceProviderContainer.Instance.Value.Services;

var appEnv = svcProv.GetRequiredService<IAppEnv>();

string outputDirPath = appEnv.GetTypePath(
    AppEnvDir.Temp,
    typeof(ServiceProviderContainer));

Directory.CreateDirectory(outputDirPath);

string outputFilePath = Path.Combine(
    outputDirPath,
    "out.json");

var loggerFactory = svcProv.GetRequiredService<IAppLoggerCreator>();

var logger = loggerFactory.GetSharedAppLogger(
    typeof(ServiceProviderContainer),
    LogLevel.Debug);

string loggerFilePath = logger.LogFilePath.GetLogFilePath();
string loggerDirPath = Path.GetDirectoryName(loggerFilePath);
string logFileName = Path.GetFileName(loggerFilePath);

logger.DebugData(
    Guid.NewGuid(),
    new Exception(Guid.NewGuid().ToString()),
    Guid.NewGuid().ToString());

var timer = new System.Timers.Timer(
    TimeSpan.FromSeconds(1));

timer.Elapsed += (sender, e) =>
{
    logger.DebugData(
        Guid.NewGuid(),
        new Exception(Guid.NewGuid().ToString()),
        Guid.NewGuid().ToString());
};

var fileWatcher = new FileSystemWatcher(
    loggerDirPath,
    logFileName);

Console.WriteLine(loggerDirPath);
Console.WriteLine(loggerFilePath);
Console.WriteLine(logFileName);

fileWatcher.Changed += (sender, e) =>
{
    string text;

    using (var sr = new StreamReader(
        loggerFilePath,
        new FileStreamOptions
        {
            Access = FileAccess.Read,
            Mode = FileMode.Open,
            Share = FileShare.ReadWrite
        }))
    {
        text = sr.ReadToEnd();
    }

    File.WriteAllText(outputFilePath, text);
};

fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
fileWatcher.EnableRaisingEvents = true;

timer.Enabled = true;
Console.ReadKey();
