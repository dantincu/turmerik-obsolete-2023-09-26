using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Turmerik.LaunchApp.Components
{
    public class AppConfig
    {
        public AppLogLevel MinLogLevel { get; set; }
        public FsEntryLocator EnvDir { get; set; }
        public FsEntryLocator EnvBinDir { get; set; }
        public MappedApp[] Apps { get; set; }
    }
}
