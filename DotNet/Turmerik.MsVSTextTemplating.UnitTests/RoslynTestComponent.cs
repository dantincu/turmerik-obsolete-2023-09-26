using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.CodeAnalysis.Core.Dependencies;
using Turmerik.TreeTraversal;
using Turmerik.Collections;
using Turmerik.Utils;
using static Turmerik.CodeAnalysis.Core.Dependencies.SyntaxTreeTraversal;
using static Turmerik.MsVSTextTemplating.UnitTests.RoslynTestComponent;

namespace Turmerik.MsVSTextTemplating.UnitTests
{
    public interface IRoslynTestComponent
    {
        List<RoslynTestComponentNode> ParseCode(string code);
    }

    public class RoslynTestComponent : SyntaxTreeTraversal, IRoslynTestComponent
    {
        public RoslynTestComponent(
            ITreeTraversalComponentFactory treeTraversalComponentFactory) : base(
                treeTraversalComponentFactory)
        {
        }

        public List<RoslynTestComponentNode> ParseCode(string code) => TraverseTree(
            new SyntaxTreeTraversalOptsCore<Args, RoslynTestComponentNode, List<RoslynTestComponentNode>>.Mtbl
            {
                Code = code,
                OnAscend = (args, trArgs, treeNode) =>
                {
                    args.CurrentTreeNode = treeNode;
                },
                TreeNodeFactory = (args, trArgs, node) => new RoslynTestComponentNode(
                    node,
                    node.Kind(),
                    node.ToString(),
                    node.ToFullString(),
                    args.CurrentTreeNode).ActWithValue(treeNode =>
                    {
                        if (trArgs == null)
                        {
                            args.Result = new List<RoslynTestComponentNode>
                            {
                                treeNode
                            };
                        }
                        else
                        {
                            args.CurrentTreeNode.ChildNodes.Add(treeNode);
                        }

                        args.CurrentTreeNode = treeNode;
                        args.AllNodes.Add(treeNode);
                    }),
                ResultFactory = args => args.Result,
            },
            argsFactory: opts => GetDefaultArgs<List<RoslynTestComponentNode>>(
                opts).WithValue(dfArgs => new Args(
                    opts,
                    dfArgs.SyntaxTree,
                    dfArgs.RootNode)));

        public class Args : Args<Args, RoslynTestComponentNode, List<RoslynTestComponentNode>>
        {
            public Args(
                SyntaxTreeTraversalOptsCore<Args, RoslynTestComponentNode, List<RoslynTestComponentNode>>.Immtbl opts,
                SyntaxTree syntaxTree,
                CompilationUnitSyntax rootNode) : base(
                    opts,
                    syntaxTree,
                    rootNode)
            {
                AllNodes = new List<RoslynTestComponentNode>();
            }

            public List<RoslynTestComponentNode> AllNodes { get; }
        }

        public class RoslynTestComponentNode : TreeNode
        {
            public RoslynTestComponentNode(
                SyntaxNode node,
                SyntaxKind kind,
                string text,
                string fullText,
                RoslynTestComponentNode parentNode) : base(
                    node,
                    kind)
            {
                Text = text;
                FullText = fullText;
                ParentNode = parentNode;

                ChildNodes = new List<RoslynTestComponentNode>();

                ChildKinds = node.ChildNodes().Select(
                    child => child.Kind()).ToArray();
            }

            public string Text { get; }
            public string FullText { get; }
            public RoslynTestComponentNode ParentNode { get; }

            public List<RoslynTestComponentNode> ChildNodes { get; }
            public SyntaxKind[] ChildKinds { get; }
        }
    }

    [Serializable]
    public class RoslynTestComponentNodeSerializable
    {
        public RoslynTestComponentNodeSerializable()
        {
        }

        public RoslynTestComponentNodeSerializable(RoslynTestComponentNode src)
        {
            Kind = src.Kind;
            Text = src.Text;
            FullText = src.FullText;

            ChildNodes = src.ChildNodes?.Select(
                item => new RoslynTestComponentNodeSerializable(item)).ToArray();

            ChildKinds = src.ChildKinds;
        }

        public SyntaxKind Kind { get; set; }
        public string Text { get; set; }
        public string FullText { get; set; }

        public RoslynTestComponentNodeSerializable[] ChildNodes { get; set; }
        public SyntaxKind[] ChildKinds { get; set; }
    }
}
