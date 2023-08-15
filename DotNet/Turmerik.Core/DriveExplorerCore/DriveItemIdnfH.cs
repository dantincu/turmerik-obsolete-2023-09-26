using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Turmerik.Collections;
using Turmerik.FileSystem;

namespace Turmerik.DriveExplorerCore
{
    public static class DriveItemIdnfH
    {
        public static string GetFullPath(
            this DriveItemIdnf.IClnbl idnf,
            string dirSep) => FsH.CombinePaths(
                (idnf.PrPath ?? idnf.GetPrIdnf(
                    )?.GetFullPath(
                        dirSep)).Arr(
                    idnf.Name), dirSep);

        public static string GetPath(
            this DriveItemIdnf.IClnbl idnf,
            string dirSep) => FsH.CombinePaths(
                idnf.PrPath.Arr(
                    idnf.Name), dirSep);
    }
}
