﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Turmerik.Utils;

namespace Turmerik.TreeTraversal
{
    public interface ITreeTraversalComponent<T>
    {
        void Traverse(TreeTraversalComponentOpts.IClnbl<T> opts);
    }

    public class TreeTraversalComponent<T> : ITreeTraversalComponent<T>
    {
        public void Traverse(TreeTraversalComponentOpts.IClnbl<T> opts)
        {
            using (var args = new Args(
                NormalizeOpts(opts.AsMtbl())))
            {
                args.RootTreeNode = GetNextTreeNode(
                    args, args.Opts.RootNode);

                args.CurrentTreeNode = args.RootTreeNode;

                while (args.CurrentTreeNode != null)
                {
                    if (args.Opts.GoNextPredicate(args,
                        args.CurrentTreeNode.Data) && args.CurrentTreeNode.ChildrenNmrtr.Value.MoveNext())
                    {
                        var nextNode = GetNextTreeNode(args,
                            args.CurrentTreeNode.ChildrenNmrtr.Value.Current);

                        args.CurrentTreeNode.CurrentChildTreeNode = nextNode;
                        args.CurrentTreeNode.CurrentChildIdx++;

                        args.CurrentTreeNode = nextNode;
                        args.Opts.OnDescend(args, nextNode.Data);
                    }
                    else
                    {
                        var parentNode = args.CurrentTreeNode.ParentTreeNode;

                        if (args.Opts.DisposeTreeNodes)
                        {
                            args.CurrentTreeNode.Dispose();
                        }
                        
                        args.CurrentTreeNode = parentNode;

                        if (parentNode != null)
                        {
                            parentNode.CurrentChildTreeNode = null;
                            args.Opts.OnAscend(args, parentNode.Data);
                        }
                    }
                }
            }
        }

        protected virtual TreeTraversalComponentNormOpts.Immtbl<T> NormalizeOpts(
            TreeTraversalComponentOpts.Mtbl<T> optsMtbl)
        {
            optsMtbl.OnDescend = optsMtbl.OnDescend.FirstNotNull((args, data) => { });
            optsMtbl.OnAscend = optsMtbl.OnAscend.FirstNotNull((args, data) => { });

            var optsImmtbl = new TreeTraversalComponentNormOpts.Mtbl<T>(optsMtbl)
            {
                DisposeTreeNodes = optsMtbl.DisposeTreeNodes ?? true
            }.ToImmtbl();
            return optsImmtbl;
        }

        private TreeNode GetNextTreeNode(
            Args args,
            T data) => new TreeNode(
                data,
                args.CurrentTreeNode,
                args.Opts.ChildNodesNmrtrRetriever(args, data))
            {
                CurrentChildIdx = -1
            };

        public class Args : IDisposable
        {
            public Args(
                TreeTraversalComponentNormOpts.Immtbl<T> opts)
            {
                Opts = opts ?? throw new ArgumentNullException(nameof(opts));
            }

            public TreeTraversalComponentNormOpts.Immtbl<T> Opts { get; }
            public TreeNode RootTreeNode { get; set; }
            public TreeNode CurrentTreeNode { get; set; }

            public int CurrentLevel => CurrentTreeNode?.CurrentLevel ?? -1;

            public void Dispose()
            {
                RootTreeNode?.Dispose();
            }
        }

        public class TreeNode : IDisposable
        {
            public TreeNode(
                T data,
                TreeNode parentTreeNode,
                Lazy<IEnumerator<T>> childrenNmrtr)
            {
                Data = data;
                ParentTreeNode = parentTreeNode;
                CurrentLevel = (parentTreeNode?.CurrentLevel ?? -1) + 1;

                ChildrenNmrtr = childrenNmrtr;
            }

            public T Data { get; }
            public TreeNode ParentTreeNode { get; }
            public int CurrentLevel { get; }
            public Lazy<IEnumerator<T>> ChildrenNmrtr { get; }
            public TreeNode CurrentChildTreeNode { get; set; }
            public int CurrentChildIdx { get; set; }

            public void Dispose()
            {
                ChildrenNmrtr.Value.Dispose();
            }
        }
    }
}
