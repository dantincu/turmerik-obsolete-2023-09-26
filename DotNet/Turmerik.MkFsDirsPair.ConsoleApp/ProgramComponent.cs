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
            var data = new DirNamesPairGenerator().Generate(
                args);

            string workDir = Environment.CurrentDirectory;

            string[] existingEntriesArr = Directory.EnumerateFileSystemEntries(
                workDir).Select(
                entry => Path.GetFileName(entry)).ToArray();

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
            string shortDirPath = Path.Combine(
                workDir,
                data.ShortDirName);

            string fullDirPath = Path.Combine(
                workDir,
                data.FullDirName);

            if (existingEntriesArr.Contains(data.ShortDirName))
            {
                throw new InvalidOperationException(
                    $@"An entry with the name ""{data.ShortDirName}"" already exists at this location");
            }
            else if (existingEntriesArr.Contains(data.FullDirName))
            {
                fullDirPath = Path.GetDirectoryName(fullDirPath);
            }

            Directory.CreateDirectory(shortDirPath);
            Directory.CreateDirectory(fullDirPath);

            string fullDirKeepFilePath = Path.Combine(fullDirPath, ".keep");
            File.WriteAllText(fullDirKeepFilePath, "");

            if (data.DocFileName != null)
            {
                string docFilePath = Path.Combine(
                    shortDirPath,
                    data.DocFileName);

                string docFileExtn = Path.GetExtension(
                    data.DocFileName);

                string docFileText = string.Empty;

                if (docFileExtn == ".md")
                {
                    docFileText = Path.GetFileNameWithoutExtension(
                        data.DocFileName);

                    docFileText = $"# {docFileText}  \n\n";
                }

                File.WriteAllText(
                    docFilePath,
                    docFileText);

                ProcessH.OpenWithDefaultProgram(docFilePath);
            }
        }
    }
}
