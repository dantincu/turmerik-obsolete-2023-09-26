using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedSwitchCase
    {
        public interface IClnbl
        {
            bool IsPatternMatchingCase { get; }

            ParsedExpression.IClnbl GetCaseExpr();
            ParsedExpressionOrStatementsBlock.IClnbl GetFunctionExpressionOrStatementsBlock();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                IsPatternMatchingCase = src.IsPatternMatchingCase;
                CaseExpr = src.GetCaseExpr().AsImmtbl();
                FunctionExpressionOrStatementsBlock = src.GetFunctionExpressionOrStatementsBlock().AsImmtbl();
            }

            public bool IsPatternMatchingCase { get; }

            public ParsedExpression.Immtbl CaseExpr { get; }
            public ParsedExpressionOrStatementsBlock.Immtbl FunctionExpressionOrStatementsBlock { get; }

            public ParsedExpression.IClnbl GetCaseExpr() => CaseExpr;
            public ParsedExpressionOrStatementsBlock.IClnbl GetFunctionExpressionOrStatementsBlock() => FunctionExpressionOrStatementsBlock;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                IsPatternMatchingCase = src.IsPatternMatchingCase;
                CaseExpr = src.GetCaseExpr().AsMtbl();
                FunctionExpressionOrStatementsBlock = src.GetFunctionExpressionOrStatementsBlock().AsMtbl();
            }

            public bool IsPatternMatchingCase { get; set; }

            public ParsedExpression.Mtbl CaseExpr { get; set; }
            public ParsedExpressionOrStatementsBlock.Mtbl FunctionExpressionOrStatementsBlock { get; set; }

            public ParsedExpression.IClnbl GetCaseExpr() => CaseExpr;
            public ParsedExpressionOrStatementsBlock.IClnbl GetFunctionExpressionOrStatementsBlock() => FunctionExpressionOrStatementsBlock;
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
