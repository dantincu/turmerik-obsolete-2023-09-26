using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.Infrastucture
{
    public class TrmrkMetadataCore<TClnbl>
    {
        public interface IClnblCore
        {
            string MetadataTypeName { get; }
        }
    }

    public class TrmrkMetadataCore<TClnbl, TMetadata> : TrmrkMetadataCore<TClnbl>
    {
        public interface IClnbl : IClnblCore
        {
            [ClnblIgnoreMethod]
            TMetadata GetMetadata();
        }
    }

    public class TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>
        where TClnbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.IClnblCore
        where TImmtbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.ImmtblCoreBase, TClnbl
        where TMtbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.MtblCoreBase, TClnbl
    {
        public interface IClnblCore : TrmrkMetadataCore<TClnbl, TMetadata>.IClnbl
        {
        }

        public abstract class ImmtblCoreBase : IClnblCore
        {
            public ImmtblCoreBase(TClnbl src)
            {
                MetadataTypeName = src.MetadataTypeName;
            }

            public string MetadataTypeName { get; }

            public abstract TMetadata GetMetadata();
        }

        public new abstract class MtblCoreBase : IClnblCore
        {
            public MtblCoreBase()
            {
            }

            public MtblCoreBase(TClnbl src)
            {
                MetadataTypeName = src.MetadataTypeName;
            }

            public string MetadataTypeName { get; set; }

            public abstract TMetadata GetMetadata();
        }

        public static TMtbl JsonToMtbl(
            string jsonStr) => JsonH.FromJson<TMtbl>(jsonStr);

        public static TImmtbl JsonToImmtbl(
            string jsonStr) => JsonToMtbl(
                jsonStr).IfNotNull(mtbl => mtbl.CreateInstance<TImmtbl>());
    }
}
