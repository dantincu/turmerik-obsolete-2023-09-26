using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Turmerik.FileSystem;
using Turmerik.Utils;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.TrmrkRepo
{
    public static class TrmrkRepoH
    {
        public static readonly string TrmrkRepoPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            StringH.JoinStrRange(5, FsH.ParentDir));
    }
}
