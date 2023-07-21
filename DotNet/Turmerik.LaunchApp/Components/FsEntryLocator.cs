using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LaunchApp.Components
{
    public class FsEntryLocator
    {
        public string AbsPath { get; set; }
        public string RelPath { get; set; }
    }

    public static class FsEntryLocatorH
    {
        public static string GetPath(
            this FsEntryLocator locator,
            string basePath) => locator.AbsPath ?? Path.Combine(
                basePath,
                locator.RelPath);
    }
}
