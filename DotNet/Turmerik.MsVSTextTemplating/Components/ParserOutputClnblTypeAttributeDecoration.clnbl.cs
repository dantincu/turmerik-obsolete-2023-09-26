using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.MsVSTextTemplating.Components
{
    public static partial class ParserOutputClnblTypeAttributeDecoration
    {
        public interface IClnbl : ParserOutputAttributeDecoration.IClnbl
        {
            ParserOutputTypeIdentifier.IClnbl GetImmtblTypeType();
            ParserOutputTypeIdentifier.IClnbl GetMtblTypeType();
            ParserOutputTypeIdentifier.IClnbl GetClnblDecoratorType();
            ParserOutputTypeIdentifier.IClnbl GetClnblTypeDecoratorType();
        }

        public class Immtbl : ParserOutputAttributeDecoration.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                ImmtblTypeType = src.GetImmtblTypeType().AsImmtbl();
                MtblTypeType = src.GetMtblTypeType().AsImmtbl();
                ClnblDecoratorType = src.GetClnblDecoratorType().AsImmtbl();
                ClnblTypeDecoratorType = src.GetClnblTypeDecoratorType().AsImmtbl();
            }

            public ParserOutputTypeIdentifier.Immtbl ImmtblTypeType { get; }
            public ParserOutputTypeIdentifier.Immtbl MtblTypeType { get; }
            public ParserOutputTypeIdentifier.Immtbl ClnblDecoratorType { get; }
            public ParserOutputTypeIdentifier.Immtbl ClnblTypeDecoratorType { get; }

            public ParserOutputTypeIdentifier.IClnbl GetImmtblTypeType() => ImmtblTypeType;
            public ParserOutputTypeIdentifier.IClnbl GetMtblTypeType() => MtblTypeType;
            public ParserOutputTypeIdentifier.IClnbl GetClnblDecoratorType() => ClnblDecoratorType;
            public ParserOutputTypeIdentifier.IClnbl GetClnblTypeDecoratorType() => ClnblTypeDecoratorType;
        }

        public class Mtbl : ParserOutputAttributeDecoration.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                ImmtblTypeType = src.GetImmtblTypeType().AsMtbl();
                MtblTypeType = src.GetMtblTypeType().AsMtbl();
                ClnblDecoratorType = src.GetClnblDecoratorType().AsMtbl();
                ClnblTypeDecoratorType = src.GetClnblTypeDecoratorType().AsMtbl();
            }

            public ParserOutputTypeIdentifier.Mtbl ImmtblTypeType { get; set; }
            public ParserOutputTypeIdentifier.Mtbl MtblTypeType { get; set; }
            public ParserOutputTypeIdentifier.Mtbl ClnblDecoratorType { get; set; }
            public ParserOutputTypeIdentifier.Mtbl ClnblTypeDecoratorType { get; set; }

            public ParserOutputTypeIdentifier.IClnbl GetImmtblTypeType() => ImmtblTypeType;
            public ParserOutputTypeIdentifier.IClnbl GetMtblTypeType() => MtblTypeType;
            public ParserOutputTypeIdentifier.IClnbl GetClnblDecoratorType() => ClnblDecoratorType;
            public ParserOutputTypeIdentifier.IClnbl GetClnblTypeDecoratorType() => ClnblTypeDecoratorType;
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => (src as Immtbl) ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => (src as Mtbl) ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) => (
            src as ReadOnlyCollection<Immtbl>) ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => (src as List<Mtbl>) ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => (src as ReadOnlyDictionary<TKey, Immtbl>) ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => (src as Dictionary<TKey, Mtbl>) ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }
}
