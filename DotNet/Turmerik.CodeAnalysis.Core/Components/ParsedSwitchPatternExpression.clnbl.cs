using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedSwitchPatternExpression
    {
        public interface IClnbl : ParsedLiteralExpression.IClnbl
        {
            ParsedExpression.IClnbl GetSwitchExpr();

            IEnumerable<ParsedSwitchCase.IClnbl> GetCases();
        }

        public class Immtbl : ParsedLiteralExpression.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                SwitchExpr = src.GetSwitchExpr().AsImmtbl();
                Cases = src.GetCases().AsImmtblCllctn();
            }

            public ParsedExpression.Immtbl SwitchExpr { get; }

            public ReadOnlyCollection<ParsedSwitchCase.Immtbl> Cases { get; }

            public ParsedExpression.IClnbl GetSwitchExpr() => SwitchExpr;

            public IEnumerable<ParsedSwitchCase.IClnbl> GetCases() => Cases;
        }

        public class Mtbl : ParsedLiteralExpression.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                SwitchExpr = src.GetSwitchExpr().AsMtbl();
                Cases = src.GetCases().AsMtblList();
            }

            public ParsedExpression.Mtbl SwitchExpr { get; }

            public List<ParsedSwitchCase.Mtbl> Cases { get; }

            public ParsedExpression.IClnbl GetSwitchExpr() => SwitchExpr;

            public IEnumerable<ParsedSwitchCase.IClnbl> GetCases() => Cases;
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
