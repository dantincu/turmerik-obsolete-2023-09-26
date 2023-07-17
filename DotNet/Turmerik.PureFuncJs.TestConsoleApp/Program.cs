using Jint;
using Microsoft.Extensions.DependencyInjection;
using Turmerik.Dependencies;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalDevice.Core.TrmrkRepo;
using Turmerik.PureFuncJs.Core.JintCompnts;

var services = new ServiceCollection();
LocalDeviceServiceCollectionBuilder.RegisterAll(services);

ServiceProviderContainer.Instance.Value.RegisterServices(services);
var svcProv = ServiceProviderContainer.Instance.Value.Services;

var factory = svcProv.GetRequiredService<IJintComponentFactory>();
string jsFilePath = Path.Combine(TrmrkRepoH.TrmrkRepoPath, "YarnWs\\packages.jint.NET\\turmerik-jint-fs-notes\\dist\\test-bundle.js");

var component = factory.Create(File.ReadAllText(jsFilePath));
component.Console.OnLog += Console_OnLog;

void Console_OnLog(object obj)
{
    Console.WriteLine(obj);
}

while (true)
{
    Console.Write("> ");
    var statement = Console.ReadLine();
    var result = component.Execute(statement);
    Console.WriteLine(result);
}

public class ServiceProviderContainer : SimpleServiceProviderContainer
{
    public static readonly Lazy<ServiceProviderContainer> Instance = new Lazy<ServiceProviderContainer>(
        () => new ServiceProviderContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

    private ServiceProviderContainer()
    {
    }
}
