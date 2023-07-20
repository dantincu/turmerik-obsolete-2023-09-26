// See https://aka.ms/new-console-template for more information
using System.Collections.ObjectModel;
using System.Diagnostics;

App app;
AppSuffix appSuffix = AppSuffix.ConsoleApp;
int argsToSkip = 1;

switch (args[0])
{
    case "nt":
        app = App.NotesMkFsDirsPair;
        argsToSkip = 0;
        break;
    case "in":
        app = App.TextIndentMdLines;
        break;
    case "tb":
        app = App.TextTabsToMdTable;
        break;
    default: throw new ArgumentException(
        $"Invalid app name: {args[0]}");
}

string exeRelFilePath = string.Join(
    ".",
    "Turmerik",
    app.ToString(),
    appSuffix.ToString());

exeRelFilePath = Path.Combine(
    exeRelFilePath,
    "Release",
    string.Join(".",
        exeRelFilePath,
        "exe"));

string binFilePath = Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData);

binFilePath = Path.Combine(
    binFilePath,
    "Turmerik",
    "Apps",
    "Bin",
    exeRelFilePath);

var startInfo = new ProcessStartInfo
{
    FileName = binFilePath,
    CreateNoWindow = true,
    RedirectStandardError = true,
    RedirectStandardInput = true,
    RedirectStandardOutput = true,
    WorkingDirectory = Path.GetDirectoryName(
        binFilePath),
    UseShellExecute = false,
};

for (int i = argsToSkip; i < args.Length; i++)
{
    startInfo.ArgumentList.Add(args[i]);
}

Process process = new Process
{
    StartInfo = startInfo,
};

bool keepRunning = true;

if (process.Start())
{
    Console.WriteLine("Process started");

    Read(process.StandardOutput);
    Read(process.StandardError);

    process.WaitForExit();
    keepRunning = false;
}
else
{
    Console.WriteLine("Process not started");
}

void Read(StreamReader reader)
{
    new Thread(() =>
    {
        while (keepRunning)
        {
            int current;
            while ((current = reader.Read()) >= 0)
                Console.Write((char)current);
        }
    }).Start();
}

enum App
{
    NotesMkFsDirsPair,
    TextIndentMdLines,
    TextTabsToMdTable
}

enum AppSuffix
{
    ConsoleApp,
    WinFormsApp
}