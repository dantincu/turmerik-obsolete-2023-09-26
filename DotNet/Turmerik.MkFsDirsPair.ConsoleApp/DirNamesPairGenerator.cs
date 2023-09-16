using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MkFsDirsPair.ConsoleApp
{
    public class DirNamesPairGenerator
    {
        public const char ENTRY_NAME_SPECIAL_CHAR = '?';

        private readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        private readonly char[] miscWsChars = new char[] { '\n', '\r', '\t' };

        public DirNamesPair Generate(
            string[] args)
        {
            string docFileName = GetArgs(
                args,
                out string shortDirName,
                out string fullDirName);

            return new DirNamesPair(
                shortDirName,
                fullDirName,
                docFileName);
        }

        private string GetArgs(
            string[] args,
            out string shortDirName,
            out string fullDirName)
        {
            string docFileName = GetArgs(
                args,
                out shortDirName,
                out string fullDirNamePart,
                out string fullDirNameJoinStr);

            fullDirName = string.Join(
                fullDirNameJoinStr,
                shortDirName,
                fullDirNamePart);

            return docFileName;
        }

        private string GetArgs(
            string[] args,
            out string shortDirName,
            out string fullDirNamePart,
            out string fullDirNameJoinStr)
        {
            if (args.Length > 3)
            {
                throw new ArgumentException(
                    $"Expected 3 arguments but received {args.Length}");
            }

            int idx = 0;
            string docFileName = null;

            shortDirName = GetArg(
                args,
                idx++,
                "Type the short dir name");

            fullDirNameJoinStr = GetArg(
                args,
                idx++,
                "Type the full dir name join str");

            fullDirNamePart = GetArg(
                args,
                idx++,
                "Type the full dir name part",
                (rawArg, i, arg) =>
                {
                    int rawArgLen = rawArg.Length;
                    int argLen = arg.Length;

                    var lastChar = rawArg.Last();

                    if (lastChar == ENTRY_NAME_SPECIAL_CHAR)
                    {
                        docFileName = $"{arg}.md";
                    }
                    else if (rawArgLen >= 2 && rawArg[rawArgLen - 2] == ENTRY_NAME_SPECIAL_CHAR)
                    {
                        arg = arg.Substring(0, argLen - 1);

                        switch (lastChar)
                        {
                            case 'd':
                                docFileName = $"{arg}.docx";
                                break;
                            default:
                                throw new ArgumentException(
                                $"Full dir name part cannot end in {rawArg.Substring(
                                    rawArgLen - 2)}");
                        }
                    }

                    return arg;
                });

            return docFileName;
        }

        private string GetArg(
            string[] argsArr,
            int idx,
            string message,
            Func<string, int, string, string> callback = null)
        {
            string rawArg;

            if (idx < argsArr.Length)
            {
                rawArg = argsArr[idx];
            }
            else
            {
                rawArg = GetArg(message);
            }

            string arg = rawArg.Trim();

            if (arg == "?")
            {
                arg = string.Empty;
            }
            else
            {
                arg = NormalizeArg(arg);
            }

            if (callback != null)
            {
                arg = callback(rawArg, idx, arg);
            }
            
            return arg;
        }

        private string NormalizeArg(
            string arg)
        {
            arg = arg.Replace(':', ' ').Replace("/", "%").Replace("\\", "%");
            arg = string.Join("", arg.Split(invalidFileNameChars));
            arg = string.Join(" ", arg.Split(miscWsChars));
            return arg;
        }

        private string GetArg(
            string message)
        {
            Console.WriteLine(message);
            Console.Write("> ");

            string arg = Console.ReadLine();
            Console.WriteLine();

            return arg;
        }
    }

    public class DirNamesPair
    {
        public DirNamesPair(
            string shortDirName,
            string fullDirName,
            string docFileName)
        {
            ShortDirName = shortDirName ?? throw new ArgumentNullException(nameof(shortDirName));
            FullDirName = fullDirName ?? throw new ArgumentNullException(nameof(fullDirName));
            DocFileName = docFileName;
        }

        public string ShortDirName { get; }
        public string FullDirName { get; }
        public string DocFileName { get; }
    }
}
