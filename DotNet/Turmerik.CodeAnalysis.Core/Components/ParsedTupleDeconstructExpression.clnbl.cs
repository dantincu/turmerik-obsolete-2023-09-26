﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedTupleDeconstructExpression
    {
        public interface IClnbl : ParsedExpression.IClnbl
        {
            IEnumerable<string> GetArgumentNames();
            IEnumerable<ParsedExpression.IClnbl> GetArguments();
        }

        public class Immtbl : ParsedExpression.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                ArgumentNames = src.GetArgumentNames()?.RdnlC();
                Arguments = src.GetArguments().AsImmtblCllctn();
            }

            public ReadOnlyCollection<string> ArgumentNames { get; }
            public ReadOnlyCollection<ParsedExpression.Immtbl> Arguments { get; }

            public IEnumerable<string> GetArgumentNames() => ArgumentNames;
            public IEnumerable<ParsedExpression.IClnbl> GetArguments() => Arguments;
        }

        public class Mtbl : ParsedExpression.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                ArgumentNames = src.GetArgumentNames()?.ToList();
                Arguments = src.GetArguments().AsMtblList();
            }

            public List<string> ArgumentNames { get; set; }
            public List<ParsedExpression.Mtbl> Arguments { get; set; }

            public IEnumerable<string> GetArgumentNames() => ArgumentNames;
            public IEnumerable<ParsedExpression.IClnbl> GetArguments() => Arguments;
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
