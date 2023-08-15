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
    public class TrmrkAppMetadataCore<TClnbl>
    {
        public interface IClnblCore
        {
            string TrmrkBase64LongUuid { get; }
        }
    }

    public class TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : TrmrkAppMetadataCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public interface IClnblCore : TrmrkAppMetadataCore<TClnbl>.IClnblCore
        {
        }

        public class ImmtblCore : IClnblCore
        {
            public ImmtblCore(TClnbl src)
            {
                TrmrkBase64LongUuid = src.TrmrkBase64LongUuid;
            }

            public string TrmrkBase64LongUuid { get; }
        }

        public class MtblCore : IClnblCore
        {
            public MtblCore()
            {
            }

            public MtblCore(TClnbl src)
            {
                TrmrkBase64LongUuid = src.TrmrkBase64LongUuid;
            }

            public string TrmrkBase64LongUuid { get; set; }
        }

        public static bool TrmrkAppMetadataIsValid(
            IClnblCore trmrkAppMetadataCore,
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
