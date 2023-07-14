using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.TreeTraversal;
using Turmerik.Collections;
using Turmerik.Infrastucture;
using Turmerik.Utils;
using System.Xml.Linq;
using System.Linq;

namespace Turmerik.CodeAnalysis.Core.Dependencies
{
    public class SyntaxTreeTraversal : ComponentCoreBase
    {
        public SyntaxTreeTraversal(
            ITreeTraversalComponentFactory treeTraversalComponentFactory)
        {
            TreeTraversalComponentFactory = treeTraversalComponentFactory ?? throw new ArgumentNullException(
                nameof(treeTraversalComponentFactory));
        }

        protected ITreeTraversalComponentFactory TreeTraversalComponentFactory { get; }

        protected TResult TraverseTree<TOpts, TNormOpts, TArgs, TTreeNode, TResult>(
            TOpts options,
            Func<TOpts, TNormOpts> optsNormalizer = null,
            Func<TNormOpts, TArgs> argsFactory = null)
            where TOpts : SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl
            where TNormOpts : SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl, TOpts
            where TArgs : ArgsCore<TNormOpts, TResult>
            where TTreeNode : TreeNode
        {
            return Run(
                options,
                optsNormalizer.FirstNotNull(
                    opts =>
                    {
                        var optsMtbl = opts.CreateInstance<SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult >.Mtbl>();
                        NormalizeOpts(optsMtbl);

                        var normOpts = optsMtbl.CreateInstance<TNormOpts>();
                        return normOpts;
                    }),
                argsFactory.FirstNotNull(
                    opts => GetDefaultArgs<TResult>(opts).WithValue(
                        args => opts.CreateInstance<TArgs>(
                            null,
                            args.SyntaxTree,
                            args.RootNode))),
                args =>
                {
                    var component = TreeTraversalComponentFactory.Create<TTreeNode>();

                    component.Traverse(
                        new TreeTraversalComponentOpts.Mtbl<TTreeNode>
                        {
                            RootNode = args.Opts.TreeNodeFactory(args, null, args.RootNode),
                            ChildNodesNmrtrRetriever = (trArgs, treeNode) => 
                                LazyH.Lazy(() => treeNode.Node.ChildNodes().ToArray().Select(
                                    node => args.Opts.TreeNodeFactory(args, trArgs, node)).GetEnumerator()),
                            GoNextPredicate = (trArgs, treeNode) => args.Opts.GoNextPredicate(args, trArgs, treeNode),
                            OnAscend = (trArgs, treeNode) => args.Opts.OnAscend(args, trArgs, treeNode),
                            OnDescend = (trArgs, treeNode) => args.Opts.OnDescend(args, trArgs, treeNode)
                        });
                },
                args => args.Opts.ResultFactory(args));
        }

        protected TResult TraverseTree<TArgs, TTreeNode, TResult>(
            SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl options,
            Func<SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl, SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl> optsNormalizer = null,
            Func<SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl, TArgs> argsFactory = null)
            where TArgs : Args<TArgs, TTreeNode, TResult>
            where TTreeNode : TreeNode
        {
            var result = TraverseTree<
                    SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.IClnbl,
                    SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl,
                    TArgs,
                    TTreeNode,
                    TResult>(
                options,
                optsNormalizer,
                argsFactory);

            return result;
        }

        protected TResult TraverseTree<TTreeNode, TResult>(
            SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.IClnbl options,
            Func<SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.IClnbl, SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.Immtbl> optsNormalizer = null,
            Func<SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.Immtbl, Args<TTreeNode, TResult>> argsFactory = null)
            where TTreeNode : TreeNode
        {
            var result = TraverseTree<
                    SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.IClnbl,
                    SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.Immtbl,
                    Args<TTreeNode, TResult>,
                    TTreeNode,
                    TResult>(
                options,
                optsNormalizer,
                argsFactory);

            return result;
        }

        protected ArgsCore<SyntaxTreeTraversalOptsCore.Immtbl, TResult> GetDefaultArgs<TResult>(
            SyntaxTreeTraversalOptsCore.Immtbl opts)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(opts.DefsCode);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var args = new ArgsCore<SyntaxTreeTraversalOptsCore.Immtbl, TResult>(opts, tree, root);
            return args;
        }

        protected void NormalizeOpts<TArgs, TTreeNode, TResult>(
            SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Mtbl opts)
        {
            opts.TreeNodeFactory = opts.TreeNodeFactory.FirstNotNull(
                (args, trArgs, node) => node.CreateInstance<TTreeNode>(
                    null, node.Kind()));

            opts.GoNextPredicate = opts.GoNextPredicate.FirstNotNull(
                (args, trArgs, node) => true);

            opts.OnAscend = opts.OnAscend.FirstNotNull(
                (args, trArgs, node) => { });

            opts.OnDescend = opts.OnDescend.FirstNotNull(
                (args, trArgs, node) => { });
        }

        public class TreeNode
        {
            public TreeNode(
                SyntaxNode node,
                SyntaxKind kind)
            {
                Node = node ?? throw new ArgumentNullException(nameof(node));
                Kind = kind;
            }

            public SyntaxNode Node { get; }
            public SyntaxKind Kind { get; }
        }

        public class ArgsCore<TNormOpts, TResult>
            where TNormOpts : SyntaxTreeTraversalOptsCore.Immtbl
        {
            public ArgsCore(
                TNormOpts opts,
                SyntaxTree syntaxTree,
                CompilationUnitSyntax rootNode)
            {
                Opts = opts ?? throw new ArgumentNullException(nameof(opts));
                SyntaxTree = syntaxTree ?? throw new ArgumentNullException(nameof(syntaxTree));
                RootNode = rootNode ?? throw new ArgumentNullException(nameof(rootNode));
            }

            public TNormOpts Opts { get; }
            public SyntaxTree SyntaxTree { get; }
            public CompilationUnitSyntax RootNode { get; }
            public TResult Result { get; set; }
        }

        public class Args<TArgs, TTreeNode, TResult> : ArgsCore<SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl, TResult>
            where TArgs : Args<TArgs, TTreeNode, TResult>
            where TTreeNode : TreeNode
        {
            public Args(
                SyntaxTreeTraversalOptsCore<TArgs, TTreeNode, TResult>.Immtbl opts,
                SyntaxTree syntaxTree,
                CompilationUnitSyntax rootNode) : base(
                    opts,
                    syntaxTree,
                    rootNode)
            {
            }

            public TTreeNode CurrentTreeNode { get; set; }
        }

        public class Args<TTreeNode, TResult> : Args<Args<TTreeNode, TResult>, TTreeNode, TResult>
            where TTreeNode : TreeNode
        {
            public Args(
                SyntaxTreeTraversalOptsCore<Args<TTreeNode, TResult>, TTreeNode, TResult>.Immtbl opts,
                SyntaxTree syntaxTree,
                CompilationUnitSyntax rootNode) : base(
                    opts,
                    syntaxTree,
                    rootNode)
            {
            }
        }
    }
}
