using Jint;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;
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

var sysAsm = Assembly.GetAssembly(typeof(int));

void Console_OnLog(object obj)
{
    Console.WriteLine(obj);
}

var rslt = component.Call(
    "(function(a, b) { return a + b; }());",
    true,
    new object[]
    {
        123,
        "asdfasdf\"zxcvzcxv\"qwerqewr"
    });

Console.WriteLine(rslt);

rslt = component.Call(
    "console.log();",
    true,
    new object[]
    {
        123,
        "asdfasdf\"zxcvzcxv\"qwerqewr"
    });

Console.WriteLine(rslt);

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

while (true)
{
    Console.Write(" >>>> ARGS COUNT >>>> ");

    var argsCount = int.Parse(Console.ReadLine());
    object[] argsArr = new object[argsCount];

    for (int i = 0 ; i < argsCount; i++)
    {
        Console.WriteLine($"ARG {i}:");
        Console.WriteLine();
        Console.WriteLine($"Arg {i} type name: ");

        string argTypeName = Console.ReadLine();
        Type argType = sysAsm.GetType(argTypeName, true);

        Console.WriteLine();
        Console.WriteLine($"Arg {i} json value: ");
        string argValueStr = Console.ReadLine();

        object argValue = JsonConvert.DeserializeObject(
            argValueStr,
            argType);

        argsArr[i] = argValue;
        Console.WriteLine();
        Console.WriteLine();
    }

    Console.Write(" >>>> STATEMENT >>>> ");
    var statement = Console.ReadLine();

    var result = component.Call(
        statement,
        true,
        argsArr);

    Console.Write(" >>>> RESULT >>>> ");
    Console.WriteLine(result);

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
}

public class ServiceProviderContainer : SimpleServiceProviderContainer
{
    public static readonly Lazy<ServiceProviderContainer> Instance = new Lazy<ServiceProviderContainer>(
        () => new ServiceProviderContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

    private ServiceProviderContainer()
    {
    }
}
