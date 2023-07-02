using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Text;

namespace Turmerik.Infrastucture
{
    public class TrmrkMetadataCore<TClnbl> : ClnblCore
    {
        public new interface IClnblCore : ClnblCore.IClnblCore
        {
            string MetadataTypeName { get; }
        }
    }

    public class TrmrkMetadataCore<TClnbl, TMetadata> : TrmrkMetadataCore<TClnbl>
    {
        public new interface IClnblCore : TrmrkMetadataCore<TClnbl>.IClnblCore
        {
            [ClnblIgnoreMethod]
            TMetadata GetMetadata();
        }
    }

    public class TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.IClnblCore
        where TImmtbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.ImmtblCoreBase, TClnbl
        where TMtbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.MtblCoreBase, TClnbl
        where TMetadata : ClnblCore.IClnblCore
    {
        public new interface IClnblCore : TrmrkMetadataCore<TClnbl, TMetadata>.IClnblCore, ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        {
        }

        public new abstract class ImmtblCoreBase : ClnblCore<TClnbl, TImmtbl, TMtbl>.ImmtblCoreBase, IClnblCore
        {
            public ImmtblCoreBase(TClnbl src) : base(src)
            {
                MetadataTypeName = src.MetadataTypeName;
            }

            public string MetadataTypeName { get; }

            public abstract TMetadata GetMetadata();
        }

        public new abstract class MtblCoreBase : ClnblCore<TClnbl, TImmtbl, TMtbl>.MtblCoreBase, IClnblCore
        {
            public MtblCoreBase()
            {
            }

            public MtblCoreBase(TClnbl src) : base(src)
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
                jsonStr).AsImmtbl();
    }
}
