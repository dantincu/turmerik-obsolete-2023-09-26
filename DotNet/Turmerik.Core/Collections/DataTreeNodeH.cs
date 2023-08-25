using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turmerik.Utils;

namespace Turmerik.Collections
{
    public static class DataTreeNodeH
    {
        public static DataTreeNode.IClnbl<TValue> FindPath<TValue>(
            this IEnumerable<DataTreeNode.IClnbl<TValue>> parentNodes,
            int[] path)
        {
            int idx = path.First();
            var node = parentNodes.GetNthVal(idx);

            var childPath = path.Skip(1).ToArray();
            var child = node.FindPath(childPath);

            return child;
        }

        public static DataTreeNode.IClnbl<TValue> FindPath<TValue>(
            this DataTreeNode.IClnbl<TValue> parentNode,
            int[] path) => parentNode.GetChildren().FindPath(path);

        public static DataTreeNode.IClnbl<TValue> FindPath<TValue>(
            this DataTree.IClnbl<TValue> dataTree,
            int[] path) => dataTree.GetRootNodes().FindPath(path);

        public static Func<TNode, TNode, bool> GetDefaultEqualsPredicate<TNode>(
            ) => (a, b) => ReferenceEquals(a, b);

        public static void NormalizeEqualsPredicate<TNode>(
            ref Func<TNode, TNode, bool> equalsPredicate)
        {
            equalsPredicate = equalsPredicate.FirstNotNull(
                (a, b) => ReferenceEquals(a, b));
        }

        public static void AddIdxToPath<TNode>(
            TNode node,
            IEnumerable<TNode> nodesNmrbl,
            List<int> path,
            Func<TNode, TNode, bool> equalsPredicate = null)
        {
            NormalizeEqualsPredicate(ref equalsPredicate);

            var kvp = nodesNmrbl.FindVal(
                n => equalsPredicate(n, node));

            path.Insert(0, kvp.Key);
        }

        public static TNode BuildPath<TNode>(
            TNode node,
            Func<TNode, TNode> parentRetriever,
            Func<TNode, IEnumerable<TNode>> childrenRetriever,
            List<int> path,
            Func<TNode, TNode, bool> equalsPredicate = null,
            Func<TNode, bool> isDefaultPredicate = null)
        {
            var parent = parentRetriever(node);
            var root = node;

            isDefaultPredicate = isDefaultPredicate.FirstNotNull(val => val == null);

            if (parent != null)
            {
                var sibblings = childrenRetriever(parent);

                AddIdxToPath(
                    node,
                    sibblings,
                    path,
                    equalsPredicate);

                var newRoot = BuildPath(
                    parent,
                    parentRetriever,
                    childrenRetriever,
                    path,
                    equalsPredicate);

                if (!isDefaultPredicate(newRoot))
                {
                    root = newRoot;
                }
            }

            return parent;
        }

        public static DataTreeNode.IClnbl<TValue> BuildPath<TValue>(
            this DataTreeNode.IClnbl<TValue> node,
            List<int> path) => BuildPath(
                node,
                n => n.GetParent(),
                n => n.GetChildren(),
                path);

        public static int[] GetPath<TNode>(
            TNode node,
            Func<TNode, TNode> parentRetriever,
            Func<TNode, IEnumerable<TNode>> childrenRetriever,
            Func<TNode, TNode, bool> equalsPredicate = null)
        {
            var path = new List<int>();

            BuildPath(
                node,
                parentRetriever,
                childrenRetriever,
                path,
                equalsPredicate);

            return path.ToArray();
        }

        public static int[] GetPath<TNode>(
            TNode node,
            IEnumerable<TNode> rootNodes,
            Func<TNode, TNode> parentRetriever,
            Func<TNode, IEnumerable<TNode>> childrenRetriever,
            Func<TNode, TNode, bool> equalsPredicate = null)
        {
            var path = new List<int>();

            var root = BuildPath(
                node,
                parentRetriever,
                childrenRetriever,
                new List<int>(),
                equalsPredicate);

            AddIdxToPath(
                root,
                rootNodes,
                path,
                equalsPredicate);

            return path.ToArray();
        }

        public static int[] GetPath<TValue>(
            this DataTreeNode.IClnbl<TValue> node) => GetPath(
                node,
                n => n.GetParent(),
                n => n.GetChildren());

        public static int[] GetPath<TValue>(
            this DataTree.IClnbl<TValue> tree,
            DataTreeNode.IClnbl<TValue> node) => GetPath(
                node,
                tree.GetRootNodes(),
                n => n.GetParent(),
                n => n.GetChildren());
    }
}
