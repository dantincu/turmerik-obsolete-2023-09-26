using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Text;

namespace Turmerik.Infrastucture
{
    public partial class TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.ICoreClnbl
        where TImmtbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.CoreImmtblBase, TClnbl
        where TMtbl : TrmrkMetadataCore<TClnbl, TImmtbl, TMtbl, TMetadata>.CoreMtblBase, TClnbl
        where TMetadata : ClnblCore.IClnblCore
    {
        public interface ICoreClnbl : IClnblCore
        {
            string MetadataTypeName { get; }

            TMetadata GetMetadata();
        }

        public abstract class CoreImmtblBase : ImmtblCoreBase
        {
            public CoreImmtblBase(TClnbl src) : base(src)
            {
                MetadataTypeName = src.MetadataTypeName;
            }

            public string MetadataTypeName { get; }

            public abstract TMetadata GetMetadata();
        }

        public abstract class CoreMtblBase : MtblCoreBase
        {
            public CoreMtblBase()
            {
            }

            public CoreMtblBase(TClnbl src) : base(src)
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
