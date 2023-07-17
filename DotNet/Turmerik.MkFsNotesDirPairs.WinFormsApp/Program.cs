﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.MkFsNotesDirPairs.WinFormsApp.Components;
using Turmerik.MkFsNotesDirPairs.WinFormsApp.ViewModels;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.MkFsNotesDirPairs.WinFormsApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();

            WinFormsServiceCollectionBuilder.RegisterAll(services);
            services.AddSingleton<IAppBehaviour, AppBehaviour>();

            services.AddTransient<IMainFormVM, MainFormVM>();

            ServiceProviderContainer.AssureServicesRegistered(services);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}