using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.WinForms.Components
{
    public static class TreeNodeArgH
    {
        public static TreeNodeArg<TValue> Arg<TValue>(
            TreeNode treeNode,
            int[] nodePath,
            DataTreeNode.IClnbl<TValue> node) => new TreeNodeArg<TValue>(
                treeNode,
                node.Value,
                nodePath.RdnlC(),
                LazyH.Lazy(() => node.AsImmtbl()));
    }
}
