using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MkFsDirsPair.ConsoleApp
{
    public class ProgramComponent
    {
        public void Run(string[] args)
        {
            string workDir = Environment.CurrentDirectory;

            string[] existingEntriesArr = Directory.EnumerateFileSystemEntries(
                workDir).Select(
                entry => Path.GetFileName(entry)).ToArray();

            var data = new DirNamesPairGenerator().Generate(
                args,
                existingEntriesArr);

            CreateDirsPair(
                data,
                workDir,
                existingEntriesArr);
        }

        private void CreateDirsPair(
            DirNamesPair data,
            string workDir,
            string[] existingEntriesArr)
        {
            if (existingEntriesArr.Contains(data.ShortDirName))
            {
                throw new InvalidOperationException(
                    $@"An entry with the name ""{data.ShortDirName}"" already exists at this location");
            }
            else if (existingEntriesArr.Contains(data.FullDirName))
            {
                throw new InvalidOperationException(
                    $@"An entry with the name ""{data.FullDirName}"" already exists at this location");
            }

            string shortDirPath = Path.Combine(
                workDir,
                data.ShortDirName);

            string fullDirPath = Path.Combine(
                workDir,
                data.FullDirName);

            Directory.CreateDirectory(shortDirPath);
            Directory.CreateDirectory(fullDirPath);

            string fullDirKeepFilePath = Path.Combine(fullDirPath, ".keep");
            File.WriteAllText(fullDirKeepFilePath, "");
        }
    }
}
