using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedExpressionOrStatementsBlock
    {
        public interface IClnbl : ParsedSyntaxNode.IClnbl
        {
            bool StatementsNotSurroundedByCurlyBraces { get; }

            ParsedExpression.IClnbl GetExpression();
            ParsedStatement.IClnbl GetSingleStatement();
            IEnumerable<ParsedStatement.IClnbl> GetStatements();
        }

        public class Immtbl : ParsedSyntaxNode.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                StatementsNotSurroundedByCurlyBraces = src.StatementsNotSurroundedByCurlyBraces;
                Expression = src.GetExpression().AsImmtbl();
                SingleStatement = src.GetSingleStatement().AsImmtbl();
                Statements = src.GetStatements().AsImmtblCllctn();
            }

            public bool StatementsNotSurroundedByCurlyBraces { get; }

            public ParsedExpression.Immtbl Expression { get; }
            public ParsedStatement.Immtbl SingleStatement { get; }
            public ReadOnlyCollection<ParsedStatement.Immtbl> Statements { get; }

            public ParsedExpression.IClnbl GetExpression() => Expression;
            public ParsedStatement.IClnbl GetSingleStatement() => SingleStatement;
            public IEnumerable<ParsedStatement.IClnbl> GetStatements() => Statements;
        }

        public class Mtbl : ParsedSyntaxNode.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                StatementsNotSurroundedByCurlyBraces = src.StatementsNotSurroundedByCurlyBraces;
                Expression = src.GetExpression().AsMtbl();
                SingleStatement = src.GetSingleStatement().AsMtbl();
                Statements = src.GetStatements().AsMtblList();
            }

            public bool StatementsNotSurroundedByCurlyBraces { get; set; }

            public ParsedExpression.Mtbl Expression { get; set; }
            public ParsedStatement.Mtbl SingleStatement { get; set; }
            public List<ParsedStatement.Mtbl> Statements { get; set; }

            public ParsedExpression.IClnbl GetExpression() => Expression;
            public ParsedStatement.IClnbl GetSingleStatement() => SingleStatement;
            public IEnumerable<ParsedStatement.IClnbl> GetStatements() => Statements;
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
