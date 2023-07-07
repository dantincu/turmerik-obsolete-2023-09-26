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
    public static partial class ClnblTypesCodeGeneratorConfigCore
    {
        public static class TypeNames
        {
            public interface IClnbl
            {
                string CloneableInterface { get; }
                string Immutable { get; }
                string Mutable { get; }
                string EnumerableInterface { get; }
                string DictionaryCoreInterface { get; }
                string List { get; }
                string Dictionary { get; }
                string ReadOnlyCollection { get; }
                string ReadOnlyDictionary { get; }
                string ClnblNs { get; }
            }

            public class Immtbl : IClnbl
            {
                public Immtbl(IClnbl src)
                {
                }

                public string CloneableInterface { get; }
                public string Immutable { get; }
                public string Mutable { get; }
                public string EnumerableInterface { get; }
                public string DictionaryCoreInterface { get; }
                public string List { get; }
                public string Dictionary { get; }
                public string ReadOnlyCollection { get; }
                public string ReadOnlyDictionary { get; }
                public string ClnblNs { get; }
            }

            public class Mtbl : IClnbl
            {
                public Mtbl()
                {
                }

                public Mtbl(IClnbl src)
                {
                    CloneableInterface = src.CloneableInterface;
                    Immutable = src.Immutable;
                    Mutable = src.Mutable;
                    EnumerableInterface = src.EnumerableInterface;
                    DictionaryCoreInterface = src.DictionaryCoreInterface;
                    List = src.List;
                    Dictionary = src.Dictionary;
                    ReadOnlyCollection = src.ReadOnlyCollection;
                    ReadOnlyDictionary = src.ReadOnlyDictionary;
                    ClnblNs = src.ClnblNs;
                }

                public string CloneableInterface { get; set; }
                public string Immutable { get; set; }
                public string Mutable { get; set; }
                public string EnumerableInterface { get; set; }
                public string DictionaryCoreInterface { get; set; }
                public string List { get; set; }
                public string Dictionary { get; set; }
                public string ReadOnlyCollection { get; set; }
                public string ReadOnlyDictionary { get; set; }
                public string ClnblNs { get; set; }
            }
        }

        public static TypeNames.Immtbl ToImmtbl(
            this TypeNames.IClnbl src) => new TypeNames.Immtbl(src);

        public static TypeNames.Immtbl AsImmtbl(
            this TypeNames.IClnbl src) => (src as TypeNames.Immtbl) ?? src?.ToImmtbl();

        public static TypeNames.Mtbl ToMtbl(
            this TypeNames.IClnbl src) => new TypeNames.Mtbl(src);

        public static TypeNames.Mtbl AsMtbl(
            this TypeNames.IClnbl src) => (src as TypeNames.Mtbl) ?? src?.ToMtbl();

        public static ReadOnlyCollection<TypeNames.Immtbl> ToImmtblCllctn(
            this IEnumerable<TypeNames.IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<TypeNames.Immtbl> AsImmtblCllctn(
            this IEnumerable<TypeNames.IClnbl> src) => (
            src as ReadOnlyCollection<TypeNames.Immtbl>) ?? src?.ToImmtblCllctn();

        public static List<TypeNames.Mtbl> ToMtblList(
            this IEnumerable<TypeNames.IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<TypeNames.Mtbl> AsMtblList(
            this IEnumerable<TypeNames.IClnbl> src) => (src as List<TypeNames.Mtbl>) ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, TypeNames.Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, TypeNames.IClnbl> src) => (
                src as ReadOnlyDictionary<TKey, TypeNames.Immtbl>) ?? (
                src as Dictionary<TKey, TypeNames.Mtbl>)?.ToDictionary(
                    kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, TypeNames.Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, TypeNames.IClnbl> src) => (
                src as Dictionary<TKey, TypeNames.Mtbl>) ?? (
                src as ReadOnlyDictionary<TKey, TypeNames.Immtbl>)?.ToDictionary(
                    kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }
}
