using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedTypeMemberDeclaration
    {
        public interface IClnbl
        {
            string Name { get; }
            ParsedMemberKind Kind { get; }

            ParsedTypeOrMemberIdentifier.IClnbl GetReturnType();

            IEnumerable<ParsedAttributeDecoration.IClnbl> GetAttributes();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                Name = src.Name;
                Kind = src.Kind;
                ReturnType = src.GetReturnType().AsImmtbl();
                Attributes = src.GetAttributes().AsImmtblCllctn();
            }

            public string Name { get; }
            public ParsedMemberKind Kind { get; }

            public ParsedTypeOrMemberIdentifier.Immtbl ReturnType { get; }

            public ReadOnlyCollection<ParsedAttributeDecoration.Immtbl> Attributes { get; }

            public ParsedTypeOrMemberIdentifier.IClnbl GetReturnType() => ReturnType;

            public IEnumerable<ParsedAttributeDecoration.IClnbl> GetAttributes() => Attributes;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                Name = src.Name;
                Kind = src.Kind;
                ReturnType = src.GetReturnType().AsMtbl();
                Attributes = src.GetAttributes().AsMtblList();
            }

            public string Name { get; set; }
            public ParsedMemberKind Kind { get; set; }

            public ParsedTypeOrMemberIdentifier.Mtbl ReturnType { get; set; }

            public List<ParsedAttributeDecoration.Mtbl> Attributes { get; set; }

            public ParsedTypeOrMemberIdentifier.IClnbl GetReturnType() => ReturnType;

            public IEnumerable<ParsedAttributeDecoration.IClnbl> GetAttributes() => Attributes;
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => src as Immtbl ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => src as Mtbl ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) =>
            src as ReadOnlyCollection<Immtbl> ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => src as List<Mtbl> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as ReadOnlyDictionary<TKey, Immtbl> ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as Dictionary<TKey, Mtbl> ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }
}
