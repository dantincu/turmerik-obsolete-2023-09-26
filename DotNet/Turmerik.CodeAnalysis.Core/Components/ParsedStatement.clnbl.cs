using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedStatement
    {
        public interface IClnbl
        {
            ParsedStatementType StatementType { get; }
            ParsedExpressionOperatorType OperatorType { get; }

            ParsedExpressionOrStatementsBlock.IClnbl GetFollowingExpressionOrStatementsBlock();

            ParsedLocalVariableDeclaration.IClnbl GetLocalVariableDeclaration();
            IEnumerable<ParsedExpression.IClnbl> GetExpressions();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                StatementType = src.StatementType;
                OperatorType = src.OperatorType;

                FollowingExpressionOrStatementsBlock = src.GetFollowingExpressionOrStatementsBlock().AsImmtbl();
                LocalVariableDeclaration = src.GetLocalVariableDeclaration().AsImmtbl();
                Expressions = src.GetExpressions().AsImmtblCllctn();
            }

            public ParsedStatementType StatementType { get; }
            public ParsedExpressionOperatorType OperatorType { get; }

            public ParsedExpressionOrStatementsBlock.Immtbl FollowingExpressionOrStatementsBlock { get; }
            public ParsedLocalVariableDeclaration.Immtbl LocalVariableDeclaration { get; }
            public IEnumerable<ParsedExpression.Immtbl> Expressions { get; }

            public ParsedExpressionOrStatementsBlock.IClnbl GetFollowingExpressionOrStatementsBlock() => FollowingExpressionOrStatementsBlock;
            public ParsedLocalVariableDeclaration.IClnbl GetLocalVariableDeclaration() => LocalVariableDeclaration;
            public IEnumerable<ParsedExpression.IClnbl> GetExpressions() => Expressions;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                StatementType = src.StatementType;
                OperatorType = src.OperatorType;

                FollowingExpressionOrStatementsBlock = src.GetFollowingExpressionOrStatementsBlock().AsMtbl();
                LocalVariableDeclaration = src.GetLocalVariableDeclaration().AsMtbl();
                Expressions = src.GetExpressions().AsMtblList();
            }

            public ParsedStatementType StatementType { get; set; }
            public ParsedExpressionOperatorType OperatorType { get; set; }

            public ParsedExpressionOrStatementsBlock.Mtbl FollowingExpressionOrStatementsBlock { get; set; }
            public IEnumerable<ParsedExpression.Mtbl> Expressions { get; set; }
            public ParsedLocalVariableDeclaration.Mtbl LocalVariableDeclaration { get; set; }

            public ParsedExpressionOrStatementsBlock.IClnbl GetFollowingExpressionOrStatementsBlock() => FollowingExpressionOrStatementsBlock;
            public ParsedLocalVariableDeclaration.IClnbl GetLocalVariableDeclaration() => LocalVariableDeclaration;
            public IEnumerable<ParsedExpression.IClnbl> GetExpressions() => Expressions;
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
