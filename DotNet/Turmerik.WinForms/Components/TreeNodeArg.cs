using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Collections;

namespace Turmerik.WinForms.Components
{
    public class TreeNodeArg<TValue>
    {
        public TreeNodeArg(
            TreeNode treeNode,
            TValue value,
            ReadOnlyCollection<int> nodePath,
            Lazy<DataTreeNode.Immtbl<TValue>> node)
        {
            TreeNode = treeNode ?? throw new ArgumentNullException(nameof(treeNode));
            Value = value;
            NodePath = nodePath ?? throw new ArgumentNullException(nameof(nodePath));
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public TreeNode TreeNode { get; }
        public TValue Value { get; }
        public ReadOnlyCollection<int> NodePath { get; }
        public Lazy<DataTreeNode.Immtbl<TValue>> Node { get; }
    }
}
