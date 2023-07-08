﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.MsVSTextTemplating.Components
{
    public static partial class ParserOutputClnblTypeMemberDeclaration
    {
        public interface IClnbl
        {
            string Name { get; }
            ParserOutputClnblTypeMemberKind Kind { get; }
            bool HasImplementation { get; }
            bool HasAutoGeneratedImplementation { get; }

            ParserOutputTypeIdentifier.IClnbl GetReturnType();

            IEnumerable<ParserOutputAttributeDecoration.IClnbl> GetAttributes();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                Name = src.Name;
                Kind = src.Kind;
                HasImplementation = src.HasImplementation;
                HasAutoGeneratedImplementation = src.HasAutoGeneratedImplementation;
                ReturnType = src.GetReturnType().AsImmtbl();
                Attributes = src.GetAttributes().AsImmtblCllctn();
            }

            public string Name { get; }
            public ParserOutputClnblTypeMemberKind Kind { get; }
            public bool HasImplementation { get; }
            public bool HasAutoGeneratedImplementation { get; }

            public ParserOutputTypeIdentifier.Immtbl ReturnType { get; }

            public ReadOnlyCollection<ParserOutputAttributeDecoration.Immtbl> Attributes { get; }

            public ParserOutputTypeIdentifier.IClnbl GetReturnType() => ReturnType;

            public IEnumerable<ParserOutputAttributeDecoration.IClnbl> GetAttributes() => Attributes;
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
                HasImplementation = src.HasImplementation;
                HasAutoGeneratedImplementation = src.HasAutoGeneratedImplementation;
                ReturnType = src.GetReturnType().AsMtbl();
                Attributes = src.GetAttributes().AsMtblList();
            }

            public string Name { get; set; }
            public ParserOutputClnblTypeMemberKind Kind { get; set; }
            public bool HasImplementation { get; }
            public bool HasAutoGeneratedImplementation { get; set; }

            public ParserOutputTypeIdentifier.Mtbl ReturnType { get; set; }

            public List<ParserOutputAttributeDecoration.Mtbl> Attributes { get; set; }

            public ParserOutputTypeIdentifier.IClnbl GetReturnType() => ReturnType;

            public IEnumerable<ParserOutputAttributeDecoration.IClnbl> GetAttributes() => Attributes;
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