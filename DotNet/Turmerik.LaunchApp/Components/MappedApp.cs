using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LaunchApp.Components
{
    public class MappedApp
    {
        public string CmdName { get; set; }
        public int ArgsToSkip { get; set; }
        public string AssemblyName { get; set; }
        public FsEntryLocator AssemblyDir { get; set; }
        public FsEntryLocator AssemblyDeployDir { get; set; }
        public FsEntryLocator AssemblyFile { get; set; }
    }
}
