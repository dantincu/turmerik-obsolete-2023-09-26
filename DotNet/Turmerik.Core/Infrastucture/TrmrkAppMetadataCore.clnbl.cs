using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.Infrastucture
{
    public partial class TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>.ICoreClnbl
        where TImmtbl : TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>.CoreImmtbl, TClnbl
        where TMtbl : TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>.CoreMtbl, TClnbl
    {
        public interface ICoreClnbl : IClnblCore
        {
            string TrmrkBase64LongUuid { get; }
        }

        public class CoreImmtbl : ImmtblCoreBase, ICoreClnbl
        {
            public CoreImmtbl(TClnbl src) : base(src)
            {
                TrmrkBase64LongUuid = src.TrmrkBase64LongUuid;
            }

            public string TrmrkBase64LongUuid { get; }
        }

        public class CoreMtbl : MtblCoreBase, ICoreClnbl
        {
            public CoreMtbl()
            {
            }

            public CoreMtbl(TClnbl src) : base(src)
            {
                TrmrkBase64LongUuid = src.TrmrkBase64LongUuid;
            }

            public string TrmrkBase64LongUuid { get; set; }
        }

        public static bool TrmrkAppMetadataIsValid(
            ICoreClnbl trmrkAppMetadataCore,
            bool throwIfInvalid = false)
        {
            bool isValid = trmrkAppMetadataCore.TrmrkBase64LongUuid == TrmrkAppsSuite.BASE_64_LONG_UUID;

            if (!isValid && throwIfInvalid)
            {
                throw new InvalidOperationException($"Invalid metadata file");
            }

            return isValid;
        }

        public static TMtbl JsonToTrmrkAppMetadataMtbl(
            string jsonStr,
            bool throwIfInvalidJson = true,
            bool throwIfInvalidMetadata = true)
        {
            TMtbl mtbl = null;

            try
            {
                mtbl = JsonH.FromJson<TMtbl>(jsonStr);
            }
            catch
            {
                if (throwIfInvalidJson)
                {
                    throw;
                }
            }

            if (mtbl != null && throwIfInvalidMetadata)
            {
                TrmrkAppMetadataIsValid(mtbl, true);
            }

            return mtbl;
        }

        public static TImmtbl JsonToTrmrkAppMetadataImmtbl(
            string jsonStr,
            bool throwIfInvalidJson = true,
            bool throwIfInvalidMetadata = true)
        {
            TMtbl mtbl = JsonToTrmrkAppMetadataMtbl(
                jsonStr,
                throwIfInvalidJson,
                throwIfInvalidMetadata);

            TImmtbl immtbl = mtbl?.CreateInstance<TImmtbl>();
            return immtbl;
        }
    }
}
