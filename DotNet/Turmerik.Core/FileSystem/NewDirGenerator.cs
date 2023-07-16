using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Turmerik.Synchronized;

namespace Turmerik.FileSystem
{
    public interface INewDirGenerator
    {
        string Generate(
            string parentPath,
            Func<string, string[], string> dirNameGenerator);
    }

    public class NewDirGenerator : INewDirGenerator
    {
        private readonly IInterProcessConcurrentActionComponentFactory interProcessConcurrentActionComponentFactory;

        public string Generate(
            string parentPath,
            Func<string, string[], string> dirNameGenerator)
        {
            string newDirPath = null;

            using (var parentComponent = interProcessConcurrentActionComponentFactory.Create(
                parentPath))
            {
                parentComponent.Execute(() =>
                {
                    string[] existingEntries = Directory.GetFileSystemEntries(parentPath);

                    string newDirName = dirNameGenerator(
                        parentPath,
                        existingEntries);

                    newDirPath = Path.Combine(
                        parentPath,
                        newDirName);

                    using (var component = interProcessConcurrentActionComponentFactory.Create(
                        newDirPath, true))
                    {
                    }
                });
            }

            return newDirPath;
        }
    }
}
