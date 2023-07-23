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
        public DirNamesPair Generate(
            string[] args,
            string[] existingEntriesArr)
        {
            GetArgs(
                args,
                existingEntriesArr,
                out string shortDirName,
                out string fullDirName);

            return new DirNamesPair(
                shortDirName,
                fullDirName);
        }

        private void GetArgs(
            string[] args,
            string[] existingEntriesArr,
            out string shortDirName,
            out string fullDirName)
        {
            GetArgs(
                args,
                existingEntriesArr,
                out shortDirName,
                out string fullDirNamePart,
                out string fullDirNameJoinStr);

            fullDirName = string.Join(
                fullDirNameJoinStr,
                shortDirName,
                fullDirNamePart);
        }

        private void GetArgs(
            string[] args,
            string[] existingEntriesArr,
            out string shortDirName,
            out string fullDirNamePart,
            out string fullDirNameJoinStr)
        {
            int idx = 0;

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
                "Type the full dir name part");

            if (string.IsNullOrEmpty(fullDirNamePart))
            {
                fullDirNamePart = GetFullDirNamePart(
                    existingEntriesArr,
                    shortDirName,
                    fullDirNameJoinStr);
            }
        }

        private string GetFullDirNamePart(
            string[] existingEntriesArr,
            string shortDirName,
            string fullDirNameJoinStr)
        {
            string dirNameStartStr = string.Concat(
                shortDirName,
                fullDirNameJoinStr);

            string existingEntry = existingEntriesArr.Single(
                entry => entry.StartsWith(dirNameStartStr));

            string fullDirNamePart = Path.GetFileName(existingEntry);
            return fullDirNamePart;
        }

        private string GetArg(
            string[] argsArr,
            int idx,
            string message)
        {
            string arg;

            if (idx < argsArr.Length)
            {
                arg = argsArr[idx];
            }
            else
            {
                arg = GetArg(message);
            }

            arg = arg.Replace(
                ':', ' ').Replace(
                ';', default);

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
        public DirNamesPair(string shortDirName, string fullDirName)
        {
            ShortDirName = shortDirName ?? throw new ArgumentNullException(nameof(shortDirName));
            FullDirName = fullDirName ?? throw new ArgumentNullException(nameof(fullDirName));
        }

        public string ShortDirName { get; }
        public string FullDirName { get; }
    }
}
