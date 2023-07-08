using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.TreeTraversal;
using Microsoft.CodeAnalysis;

namespace Turmerik.CodeAnalysis.Core.Dependencies
{
    public static class SyntaxTreeTraversalOptsCore
    {
        public interface IClnbl
        {
            string Code { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                Code = src.Code;
            }

            public string Code { get; }
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                Code = src.Code;
            }

            public string Code { get; set; }
        }

        public static Immtbl ToImmtbl(this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => (src as Immtbl) ?? src.ToImmtbl();

        public static Mtbl ToMtbl(this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => (src as Mtbl) ?? src.ToMtbl();

        public static SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl ToImmtbl<TArgs, TTreeNode, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl src) => new SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl(src);

        public static SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl AsImmtbl<TArgs, TTreeNode, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl src) => (
            src as SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl) ?? src.ToImmtbl();

        public static SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Mtbl ToMtbl<TArgs, TTreeNode, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl src) => new SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Mtbl(src);

        public static SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Mtbl AsMtbl<TArgs, TTreeNode, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl src) => (
            src as SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Mtbl) ?? src.ToMtbl();

        public static SyntaxTreeTraversalOptsCore<TArgs, TResult>.Immtbl ToImmtbl<TArgs, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TResult>.IClnbl src)
            where TArgs : SyntaxTreeTraversal.Args<TArgs, TResult> => new SyntaxTreeTraversalOptsCore<TArgs, TResult>.Immtbl(src);

        public static SyntaxTreeTraversalOptsCore<TArgs, TResult>.Immtbl AsImmtbl<TArgs, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TResult>.IClnbl src)
            where TArgs : SyntaxTreeTraversal.Args<TArgs, TResult> => (
            src as SyntaxTreeTraversalOptsCore<TArgs, TResult>.Immtbl) ?? src.ToImmtbl();

        public static SyntaxTreeTraversalOptsCore<TArgs, TResult>.Mtbl ToMtbl<TArgs, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TResult>.IClnbl src)
            where TArgs : SyntaxTreeTraversal.Args<TArgs, TResult> => new SyntaxTreeTraversalOptsCore<TArgs, TResult>.Mtbl(src);

        public static SyntaxTreeTraversalOptsCore<TArgs, TResult>.Mtbl AsMtbl<TArgs, TResult>(
            this SyntaxTreeTraversalOptsCore<TArgs, TResult>.IClnbl src)
            where TArgs : SyntaxTreeTraversal.Args<TArgs, TResult> => (
            src as SyntaxTreeTraversalOptsCore<TArgs, TResult>.Mtbl) ?? src.ToMtbl();
    }

    public class SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>
    {
        public interface IClnbl : SyntaxTreeTraversalOptsCore.IClnbl
        {
            Action<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode> OnAscend { get; }
            Action<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode> OnDescend { get; }
            Func<TArgs, TreeTraversalComponent<TTreeNode>.Args, SyntaxNode, TTreeNode> TreeNodeFactory { get; }
            Func<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode, bool> GoNextPredicate { get; }
            Func<TArgs, TResult> ResultFactory { get; }
        }

        public class Immtbl : SyntaxTreeTraversalOptsCore.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                OnAscend = src.OnAscend;
                OnDescend = src.OnDescend;
                TreeNodeFactory = src.TreeNodeFactory;
                GoNextPredicate = src.GoNextPredicate;
                ResultFactory = src.ResultFactory;
            }

            public Action<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode> OnAscend { get; }
            public Action<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode> OnDescend { get; }
            public Func<TArgs, TreeTraversalComponent<TTreeNode>.Args, SyntaxNode, TTreeNode> TreeNodeFactory { get; }
            public Func<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode, bool> GoNextPredicate { get; }
            public Func<TArgs, TResult> ResultFactory { get; }
        }

        public class Mtbl : SyntaxTreeTraversalOptsCore.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base (src)
            {
                OnAscend = src.OnAscend;
                OnDescend = src.OnDescend;
                TreeNodeFactory = src.TreeNodeFactory;
                GoNextPredicate = src.GoNextPredicate;
                ResultFactory = src.ResultFactory;
            }

            public Action<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode> OnAscend { get; set; }
            public Action<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode> OnDescend { get; set; }
            public Func<TArgs, TreeTraversalComponent<TTreeNode>.Args, SyntaxNode, TTreeNode> TreeNodeFactory { get; set; }
            public Func<TArgs, TreeTraversalComponent<TTreeNode>.Args, TTreeNode, bool> GoNextPredicate { get; set; }
            public Func<TArgs, TResult> ResultFactory { get; set; }
        }
    }

    public class SyntaxTreeTraversalOptsCore<TArgs, TResult>
        where TArgs : SyntaxTreeTraversal.Args<TArgs, TResult>
    {
        public interface IClnbl : SyntaxTreeTraversalOptsCore<TArgs, SyntaxTreeTraversal.TreeNode, TResult>.IClnbl
        {
        }

        public class Immtbl : SyntaxTreeTraversalOptsCore<TArgs, SyntaxTreeTraversal.TreeNode, TResult>.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : SyntaxTreeTraversalOptsCore<TArgs, SyntaxTreeTraversal.TreeNode, TResult>.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
            }
        }
    }
}