using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedTypeMethodDeclaration
    {
        public interface IClnbl : ParsedTypeMemberDeclaration.IClnbl
        {
            IDictionaryCore<string, ParsedGenericTypeParameterConstraint.IClnbl> GetGenericTypeParameters();
            IEnumerable<ParsedParameterDefinition.IClnbl> GetParameters();
        }

        public class Immtbl : ParsedTypeMemberDeclaration.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                GenericTypeParameters = src.GetGenericTypeParameters().AsImmtblDictnr();
                Parameters = src.GetParameters().AsImmtblCllctn();
            }

            public ReadOnlyDictionary<string, ParsedGenericTypeParameterConstraint.Immtbl> GenericTypeParameters { get; }
            public ReadOnlyCollection<ParsedParameterDefinition.Immtbl> Parameters { get; }

            public IDictionaryCore<string, ParsedGenericTypeParameterConstraint.IClnbl> GetGenericTypeParameters() => GenericTypeParameters?.ToClnblDictnr();
            public IEnumerable<ParsedParameterDefinition.IClnbl> GetParameters() => Parameters;
        }

        public class Mtbl : ParsedTypeMemberDeclaration.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                GenericTypeParameters = src.GetGenericTypeParameters().AsMtblDictnr();
                Parameters = src.GetParameters().AsMtblList();
            }

            public Dictionary<string, ParsedGenericTypeParameterConstraint.Mtbl> GenericTypeParameters { get; set; }
            public List<ParsedParameterDefinition.Mtbl> Parameters { get; }

            public IDictionaryCore<string, ParsedGenericTypeParameterConstraint.IClnbl> GetGenericTypeParameters() => GenericTypeParameters?.ToClnblDictnr();
            public IEnumerable<ParsedParameterDefinition.IClnbl> GetParameters() => Parameters;
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

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this Dictionary<TKey, Mtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this ReadOnlyDictionary<TKey, Immtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());
    }
}
