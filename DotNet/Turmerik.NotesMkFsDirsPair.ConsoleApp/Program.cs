using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.NotesMakeFsDirPairs.ConsoleApp.Components;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.NotesMakeFsDirPairs.ConsoleApp
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
        }
    }
}
