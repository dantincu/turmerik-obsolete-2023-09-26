using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Infrastucture
{
    public interface ITrmrkAppMetadataCore
    {
        string TrmrkBase64LongUuid { get; }
    }

    public class TrmrkAppMetadataCoreImmtbl : ITrmrkAppMetadataCore
    {
        public TrmrkAppMetadataCoreImmtbl(ITrmrkAppMetadataCore src)
        {
            TrmrkBase64LongUuid = src.TrmrkBase64LongUuid;
        }

        public string TrmrkBase64LongUuid { get; }
    }

    public class TrmrkAppMetadataCoreMtbl : ITrmrkAppMetadataCore
    {
        public TrmrkAppMetadataCoreMtbl()
        {
        }

        public TrmrkAppMetadataCoreMtbl(ITrmrkAppMetadataCore src)
        {
            TrmrkBase64LongUuid = src.TrmrkBase64LongUuid;
        }

        public string TrmrkBase64LongUuid { get; set; }
    }
}
