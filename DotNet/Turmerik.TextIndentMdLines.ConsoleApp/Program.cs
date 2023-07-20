﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Text;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.TextIndentMdLines.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceProviderContainer.AssureServicesRegistered(
                new ServiceCollection());

            var helper = ServiceProviderContainer.Instance.Value.Services.GetRequiredService<ITimeStampHelper>();
            var str = helper.TmStmp(DateTime.Now, true, TimeStamp.Ticks, true, false, false);
            string argsStr = string.Join("; ", args.Select(a => a.ToString()).ToArray());

            Console.WriteLine($"Hello world: {Environment.CurrentDirectory}; {str} {argsStr}");
        }
    }
}
