using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Turmerik.LaunchApp.Components
{
    public class ProgramComponent
    {
        public const string DEBUG_TEST_CMD = "0";

        private readonly AppConfigRetriever appConfigRetriever;

        private volatile bool keepRunning;

        public ProgramComponent()
        {
            appConfigRetriever = new AppConfigRetriever();
            Config = appConfigRetriever.GetAppConfig();
            Logger = LaunchAppLogger.Instance.Value;
            Logger.MinLogLevel = Config.MinLogLevel;
            keepRunning = true;
        }

        protected AppConfig Config { get; }
        protected LaunchAppLogger Logger { get; }

        public void Run(string[] args)
        {
            var argsList = args.ToList();
            AskForArgsIfNoneProvided(argsList);

            string cmd = argsList[0];
            argsList.RemoveAt(0);

            if (cmd == DEBUG_TEST_CMD)
            {
                PerformDebugTest(argsList);
            }
            else
            {
                Run(argsList, cmd);
            }
        }

        public void PerformDebugTest(List<string> args)
        {
            string xml = XmlH.SerializeXml(Config);

            // In order to rigorously distinguish log message heads to log message contents,
            // the logger replaces <<<< with <<<<>>>> everywhere in the log message contents

            // string logMsg = $"<<<<>>>> [APPCONFIG]: {xml}";
            string logMsg = $"[APPCONFIG]: {xml}";
            // If we comment out the previous line and uncomment the one behind it, the following line will appear in the log file:
            // <<<<>>>>>>>> [APPCONFIG]: <? xml version = "1.0" encoding = "utf-16" ?>

            // Otherwise default, the line that appears in the log file is:
            // [APPCONFIG]: <? xml version = "1.0" encoding = "utf-16" ?>

            // So there is no way for a log message contents line to start with <<<< followed by a space (or anything else then >>>>).

            Logger.Debug(logMsg);

            Console.WriteLine();
            Console.WriteLine(logMsg);
            Console.WriteLine();

            if (args.Count >= 1)
            {
                var firstArg = args[0];

                switch (firstArg)
                {
                    case "--gen-df-config":
                        GenerateDefaultConfig(args);
                        break;
                    default:
                        break;
                }
            }
        }

        private void AskForArgsIfNoneProvided(List<string> args)
        {
            if (!args.Any())
            {
                Console.WriteLine("No args have been provided, so you'll have to provide them interativelly. How many args do you want to pass?");
                Console.Write("> ");

                string input = Console.ReadLine();
                int argsCount = int.Parse(input);

                for (int i = 0; i < argsCount; i++)
                {
                    Console.WriteLine();
                    Console.Write($"ARG {i + 1}> ");

                    input = Console.ReadLine();
                    args.Add(input);
                }
            }
        }

        private void GenerateDefaultConfig(List<string> args)
        {
            var appConfig = appConfigRetriever.GetDefaultAppConfig();
            string xml = XmlH.SerializeXml(appConfig);

            File.WriteAllText(
                AppConfigRetriever.DefaultAppConfigFileName,
                xml);
        }

        private void Run(
            List<string> args,
            string cmd)
        {
            var matchingApp = Config.Apps.SingleOrDefault(
                app => app.CmdName == cmd) ?? throw new ArgumentException(
                    $"Invalid app name: {cmd}");

            Run(args, matchingApp);
        }

        private void Run(
            List<string> args,
            MappedApp app)
        {
            int argsToSkip = app.ArgsToSkip;

            /* new ConsoleAppManager(
                app.AssemblyFile.AbsPath,
                app.AssemblyDeployDir.AbsPath).ExecuteAsync(
                    args.ToArray()); */

            var startInfo = new ProcessStartInfo
            {
                FileName = app.AssemblyFile.AbsPath,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                WorkingDirectory = app.AssemblyDeployDir.AbsPath,
                UseShellExecute = false,
            };

            for (int i = argsToSkip; i < args.Count; i++)
            {
                startInfo.ArgumentList.Add(args[i]);
            }

            Process process = new Process
            {
                StartInfo = startInfo,
            };

            if (process.Start())
            {
                Console.WriteLine("Process started");

                Read(process.StandardOutput, false);
                Read(process.StandardError, true);
                HandleWrite(process);

                process.WaitForExit();
                keepRunning = false;
            }
            else
            {
                Console.WriteLine("Process not started");
            }
        }

        private void Read(
            StreamReader reader,
            bool isError)
        {
            new Thread(() =>
            {
                while (keepRunning)
                {
                    int current;
                    while ((current = reader.Read()) >= 0)
                    {
                        if (isError)
                        {
                            Console.Error.Write((char)current);
                        }
                        else
                        {
                            Console.Out.Write((char)current);
                        }
                    }
                }
            }).Start();
        }

        private void HandleWrite(
            Process process)
        {
            var writer = process.StandardInput;
            var buff = new char[1024];

            Action<Task<int>> callback = null;

            Action startNext = () =>
            {
                Console.In.ReadAsync(
                    buff,
                    0,
                    buff.Length).ContinueWith(
                        callback);
            };

            callback = inputTask =>
            {
                int buffSize = inputTask.Result;
                var chars = buff.Take(buffSize).ToArray();

                if (keepRunning)
                {
                    writer.Write(chars);
                    startNext();
                }
            };

            startNext();
        }
    }
}
