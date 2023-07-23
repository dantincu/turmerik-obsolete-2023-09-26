using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Text;
using Turmerik.TextUtils.ConsoleApp.Components;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.TextUtils.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceProviderContainer.AssureServicesRegistered(
                new ServiceCollection(), services =>
                {
                    services.AddScoped<IProgramComponent, ProgramComponent>();
                });

            var svcProv = ServiceProviderContainer.Instance.Value.Services;
            var component = svcProv.GetService<ProgramComponent>();

            component.Run(args);

            var helper = ServiceProviderContainer.Instance.Value.Services.GetRequiredService<ITimeStampHelper>();
            var str = helper.TmStmp(DateTime.Now, true, TimeStamp.Ticks, true, false, false);
            string argsStr = string.Join("; ", args.Select(a => a.ToString()).ToArray());

            Console.WriteLine($"Hello world: {Environment.CurrentDirectory}; {str} {argsStr}");

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("> ");
                string input = Console.ReadLine();
                Console.WriteLine(input);
            }
        }
    }
}
