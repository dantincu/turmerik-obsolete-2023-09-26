using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Turmerik.DriveExplorerCore
{
    public static class DriveItemIdnfH
    {
        public static string GetFullPathRecursively(
            this DriveItemIdnf.IClnbl idnf) => Path.Combine(
                idnf.PrPath ?? idnf.GetPrIdnf()?.GetFullPathRecursively(),
                idnf.Name);

        public static string GetFullPath(
            this DriveItemIdnf.IClnbl idnf) => Path.Combine(
                idnf.PrPath ?? idnf.GetPrIdnf()?.GetPath(),
                idnf.Name);

        public static string GetPath(
            this DriveItemIdnf.IClnbl idnf) => Path.Combine(
            idnf.PrPath,
            idnf.Name);
    }
}
