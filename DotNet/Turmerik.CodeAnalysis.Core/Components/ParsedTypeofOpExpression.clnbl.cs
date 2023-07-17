﻿using System;
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
    public static class ParsedTypeofOpExpression
    {
        public interface IClnbl : ParsedOpExpressionCore.IClnbl
        {
            ParsedTypeOrMemberIdentifier.IClnbl GetTypeIdnf();
        }

        public class Immtbl : ParsedOpExpressionCore.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                TypeIdnf = src.GetTypeIdnf().AsImmtbl();
            }

            public ParsedTypeOrMemberIdentifier.Immtbl TypeIdnf { get; }

            public ParsedTypeOrMemberIdentifier.IClnbl GetTypeIdnf() => TypeIdnf;
        }

        public class Mtbl : ParsedOpExpressionCore.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                TypeIdnf = src.GetTypeIdnf().AsMtbl();
            }

            public ParsedTypeOrMemberIdentifier.Mtbl TypeIdnf { get; }

            public ParsedTypeOrMemberIdentifier.IClnbl GetTypeIdnf() => TypeIdnf;
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