using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.Infrastucture
{
    public static class TrmrkAppsSuite
    {
        public const string BASE_64_LONG_UUID = "PR8T8Y_iTkS4FoKi-ZSxprbYXnKUYWtKsa2fanB5TQcORe_93tlNRoIZqygr-Ryv";

        public const string LONG_NAME_DIR_METADATA_FILE_NAME = ".trmrk.json";
        public const string SHORT_NAME_DIR_METADATA_DIR_NAME = ".trmrk";

        public static readonly ReadOnlyCollection<string> TrmrkUuidStrs;
        public static readonly ReadOnlyCollection<Guid> TrmrkUuids;

        static TrmrkAppsSuite()
        {
            TrmrkUuidStrs = new string[]
            {
                "f1131f3d-a28f-444e-b816-82a2fd94b1a6",
                "725ed8b6-6194-4a6b-b1ad-9f6a70794d07",
                "bdef450e-d9de-464d-8219-ab282bfd1caf"
            }.RdnlC();

            TrmrkUuids = TrmrkUuidStrs.Select(str => Guid.Parse(str)).RdnlC();
        }
    }
}
