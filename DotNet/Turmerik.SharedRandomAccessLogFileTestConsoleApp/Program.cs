// See https://aka.ms/new-console-template for more information
using Esprima.Ast;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Timers;
using Turmerik.Dependencies;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.SharedRandomAccessLogFileTestConsoleApp;

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

using (var sw = new StreamWriter(
    outputFilePath,
    Encoding.UTF8,
    new FileStreamOptions
    {
        Access = FileAccess.Write,
        Share = FileShare.Read,
        Mode = FileMode.Create,
    }))
{
    var loggerFactory = svcProv.GetRequiredService<IAppLoggerFactory>();

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

    var timer1 = new System.Timers.Timer(
        TimeSpan.FromSeconds(1));

    timer1.Elapsed += (sender, e) =>
    {
        logger.DebugData(
            Guid.NewGuid(),
            new Exception(Guid.NewGuid().ToString()),
            Guid.NewGuid().ToString());
    };

    var timer2 = new System.Timers.Timer(
        TimeSpan.FromSeconds(0.5));

    long position = 0;
    int maxReadCount = int.MaxValue / 2000;

    var syncRoot = new object();

    timer2.Elapsed += (sender, e) =>
    {
        lock (syncRoot)
        {
            byte[] bytesArr = null;

            using (var fs = new FileStream(
                loggerFilePath,
                new FileStreamOptions
                {
                    Access = FileAccess.Read,
                    Mode = FileMode.Open,
                    Share = FileShare.ReadWrite,
                    Options = FileOptions.RandomAccess
                }))
            {
                long pos = position;
                long availlableLen = fs.Length - pos;
                
                position += availlableLen;

                if (availlableLen > 0)
                {
                    int availlableIntLen = (int)Math.Min(
                        availlableLen,
                        maxReadCount);

                    fs.Position = pos;
                    bytesArr = new byte[availlableIntLen];

                    fs.Read(bytesArr, 0, availlableIntLen);
                }
            }

            if (bytesArr != null)
            {
                string text = Encoding.UTF8.GetString(bytesArr);
                sw.Write(text);
            }
        }
    };

    Console.WriteLine(loggerDirPath);
    Console.WriteLine(loggerFilePath);
    Console.WriteLine(logFileName);

    timer1.Enabled = true;
    timer2.Enabled = true;

    Console.ReadKey();
}

    
