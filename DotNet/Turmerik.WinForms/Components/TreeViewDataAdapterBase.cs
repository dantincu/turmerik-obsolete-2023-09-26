using Jint.Native;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Utils;
using Turmerik.WinForms.Controls;
using Turmerik.WinForms.Dependencies;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.Components
{
    public interface ITreeViewDataAdapterCore<TValue>
    {
        TreeView TreeView { get; }
        bool RefreshOnDoubleClick { get; set; }
        int ChildrenRefreshDepth { get; set; }

        TreeViewDataAdapterIconsOptsCore.Immtbl<KeyValuePair<int, string>, TValue> NodeIconKvp { get; }
        TreeViewDataAdapterIconsOptsCore.Immtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue> NodeIconKvpFactory { get; }

        event Action<TreeNodeArg<TValue>> NodeCreated;

        event Action<TreeNodeArg<TValue>> BeforeNodeRefresh;
        event Action<TreeNodeArg<TValue>[]> BeforeNodesRefresh;

        event Action<ITrmrkActionResult<TreeNodeArg<TValue>>> AfterNodeRefresh;
        event Action<ITrmrkActionResult<TreeNodeArg<TValue>[]>> AfterNodesRefresh;

        event Action<TreeNodeArg<TValue>> BeforeNodeAssureLoaded;
        event Action<TreeNodeArg<TValue>[]> BeforeNodesAssureLoaded;

        event Action<ITrmrkActionResult<TreeNodeArg<TValue>>> AfterNodeAssureLoaded;
        event Action<ITrmrkActionResult<TreeNodeArg<TValue>[]>> AfterNodesAssureLoaded;

        DataTree.Immtbl<TValue> GetDataTree();

        void RefreshRootNodes(
            int? childrenRefreshDepth = null);

        void RefreshChildNodes(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null);

        void AssureRootNodesLoaded(
            int? childrenRefreshDepth = null);

        void AssureChildNodesLoaded(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null);
    }

    public interface ITreeViewDataAdapterAsync<TValue> : ITreeViewDataAdapterCore<TValue>
    {
        Task RefreshRootNodesAsync(
            int? childrenRefreshDepth = null);

        Task RefreshChildNodesAsync(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null);
        Task AssureRootNodesLoadedAsync(
            int? childrenRefreshDepth = null);

        Task AssureChildNodesLoadedAsync(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null);
    }

    public abstract class TreeViewDataAdapterBase<TValue> : ITreeViewDataAdapterCore<TValue>
    {
        private Action<TreeNodeArg<TValue>> nodeCreated;

        private Action<TreeNodeArg<TValue>> beforeNodeRefresh;
        private Action<TreeNodeArg<TValue>[]> beforeNodesRefresh;
        private Action<ITrmrkActionResult<TreeNodeArg<TValue>>> afterNodeRefresh;
        private Action<ITrmrkActionResult<TreeNodeArg<TValue>[]>> afterNodesRefresh;
        
        private Action<TreeNodeArg<TValue>> beforeNodeAssureLoaded;
        private Action<TreeNodeArg<TValue>[]> beforeNodesAssureLoaded;
        private Action<ITrmrkActionResult<TreeNodeArg<TValue>>> afterNodeAssureLoaded;
        private Action<ITrmrkActionResult<TreeNodeArg<TValue>[]>> afterNodesAssureLoaded;
        
        public TreeViewDataAdapterBase(
            IAppLoggerCreator appLoggerCreator,
            // IWinFormsActionComponentFactory winFormsActionComponentFactory,
            TreeViewDataAdapterOptsCore.IClnbl<TValue> opts,
            IContextMenuStripFactory contextMenuStripFactory)
        {
            TreeView = opts.TreeView ?? throw new ArgumentNullException(
                nameof(opts.TreeView));

            ContextMenuStripFactory = contextMenuStripFactory ?? throw new ArgumentNullException(
                nameof(contextMenuStripFactory));

            IsLeafPredicate = opts.IsLeafPredicate.FirstNotNull(arg => false);

            NodeTextFactory = opts.NodeTextFactory ?? throw new ArgumentNullException(
                nameof(opts.NodeTextFactory));

            DefaultContextMenuStrip = LazyH.Lazy(CreateDefaultContextMenuStrip);

            this.ContextMenuStripRetriever = opts.ContextMenuStripRetriever.FirstNotNull(
                value => DefaultContextMenuStrip.Value);

            NodeIconKvp = GetIconProps(opts as TreeViewDataAdapterIconsOpts.IClnbl<TValue>);
            NodeIconKvpFactory = GetIconProps(opts as TreeViewDataAdapterIconFactoriesOpts.IClnbl<TValue>);

            this.Logger = appLoggerCreator.GetSharedAppLogger(GetType());
            // this.ActionComponent = winFormsActionComponentFactory.Create(this.Logger);
            this.DataTree = new DataTree.Mtbl<TValue>();

            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
            TreeView.AfterExpand += TreeView_AfterExpand;
        }

        public TreeView TreeView { get; }

        public bool RefreshOnDoubleClick { get; set; }
        public int ChildrenRefreshDepth { get; set; }

        public TreeViewDataAdapterIconsOptsCore.Immtbl<KeyValuePair<int, string>, TValue> NodeIconKvp { get; }
        public TreeViewDataAdapterIconsOptsCore.Immtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue> NodeIconKvpFactory { get; }

        protected IAppLogger Logger { get; private set; }
        // protected IWinFormsActionComponent ActionComponent { get; private set; }
        protected IContextMenuStripFactory ContextMenuStripFactory { get; }
        public Func<TreeNodeArg<TValue>, bool> IsLeafPredicate { get; }
        protected Func<TreeNodeArg<TValue>, string> NodeTextFactory { get; }
        protected Lazy<ContextMenuStrip> DefaultContextMenuStrip { get; }
        protected Func<TreeNodeArg<TValue>, ContextMenuStrip> ContextMenuStripRetriever { get; }
        protected DataTree.Mtbl<TValue> DataTree { get; }

        protected TreeNode CurrentTreeNode { get; set; }

        public event Action<TreeNodeArg<TValue>> NodeCreated
        {
            add => nodeCreated += value;
            remove => nodeCreated -= value;
        }

        public event Action<TreeNodeArg<TValue>> BeforeNodeRefresh
        {
            add => beforeNodeRefresh += value;
            remove => beforeNodeRefresh -= value;
        }

        public event Action<TreeNodeArg<TValue>[]> BeforeNodesRefresh
        {
            add => beforeNodesRefresh += value;
            remove => beforeNodesRefresh -= value;
        }

        public event Action<ITrmrkActionResult<TreeNodeArg<TValue>>> AfterNodeRefresh
        {
            add => afterNodeRefresh += value;
            remove => afterNodeRefresh -= value;
        }

        public event Action<ITrmrkActionResult<TreeNodeArg<TValue>[]>> AfterNodesRefresh
        {
            add => afterNodesRefresh += value;
            remove => afterNodesRefresh -= value;
        }

        public event Action<TreeNodeArg<TValue>> BeforeNodeAssureLoaded
        {
            add => beforeNodeAssureLoaded += value;
            remove => beforeNodeAssureLoaded -= value;
        }

        public event Action<TreeNodeArg<TValue>[]> BeforeNodesAssureLoaded
        {
            add => beforeNodesAssureLoaded += value;
            remove => beforeNodesAssureLoaded -= value;
        }

        public event Action<ITrmrkActionResult<TreeNodeArg<TValue>>> AfterNodeAssureLoaded
        {
            add => afterNodeAssureLoaded += value;
            remove => afterNodeAssureLoaded -= value;
        }

        public event Action<ITrmrkActionResult<TreeNodeArg<TValue>[]>> AfterNodesAssureLoaded
        {
            add => afterNodesAssureLoaded += value;
            remove => afterNodesAssureLoaded -= value;
        }

        public DataTree.Immtbl<TValue> GetDataTree() => DataTree.ToImmtbl();

        public abstract void RefreshRootNodes(
            int? childrenRefreshDepth = null);

        public abstract void RefreshChildNodes(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null);

        public abstract void AssureRootNodesLoaded(
            int? childrenRefreshDepth = null);

        public abstract void AssureChildNodesLoaded(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null);

        protected void OnNodeCreated(
            TreeNodeArg<TValue> value) => nodeCreated?.Invoke(value);

        protected void OnBeforeNodeRefresh(
            TreeNodeArg<TValue> value) => beforeNodeRefresh?.Invoke(value);

        protected void OnBeforeNodesRefresh(
            TreeNodeArg<TValue>[] valuesArr) => beforeNodesRefresh?.Invoke(valuesArr);

        protected void OnAfterNodeRefresh(
            ITrmrkActionResult<TreeNodeArg<TValue>> actionResult) => afterNodeRefresh?.Invoke(actionResult);

        protected void OnAfterNodesRefresh(
            ITrmrkActionResult<TreeNodeArg<TValue>[]> actionResult) => afterNodesRefresh?.Invoke(actionResult);

        protected void OnBeforeNodeAssureLoaded(
            TreeNodeArg<TValue> value) => beforeNodeAssureLoaded?.Invoke(value);

        protected void OnBeforeNodesAssureLoaded(
            TreeNodeArg<TValue>[] valuesArr) => beforeNodesAssureLoaded?.Invoke(valuesArr);

        protected void OnAfterNodeAssureLoaded(
            ITrmrkActionResult<TreeNodeArg<TValue>> actionResult) => afterNodeAssureLoaded?.Invoke(actionResult);

        protected void OnAfterNodesAssureLoaded(
            ITrmrkActionResult<TreeNodeArg<TValue>[]> actionResult) => afterNodesAssureLoaded?.Invoke(actionResult);

        protected virtual void TreeView_NodeMouseDoubleClick(
            object sender,
            TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && RefreshOnDoubleClick)
            {
                /* ActionComponent.Execute(new TrmrkActionComponentOpts
                {
                    ActionName = nameof(TreeView_NodeMouseDoubleClick),
                    Action = () =>
                    {
                        var node = e.Node;
                        CurrentTreeNode = node;

                        var path = WinFormsH.GetPath(TreeView, node);
                        var dataNode = DataTree.FindPath(path);

                        var arg = TreeNodeArgH.Arg(e.Node, path, dataNode);

                        TreeNode_Refresh(arg);
                        return new TrmrkActionResult();
                    }
                }); */
            }
        }

        protected virtual void TreeView_NodeMouseClick(
            object sender,
            TreeNodeMouseClickEventArgs e)
        {
            throw new NotImplementedException();
        } /* => ActionComponent.Execute(new TrmrkActionComponentOpts
            {
                ActionName = nameof(TreeView_NodeMouseClick),
                Action = () =>
                {
                    var node = e.Node;
                    CurrentTreeNode = node;

                    var path = WinFormsH.GetPath(TreeView, node);
                    var dataNode = DataTree.FindPath(path);

                    var arg = TreeNodeArgH.Arg(e.Node, path, dataNode);

                    if (e.Button == MouseButtons.Right)
                    {
                        var contextMenu = ContextMenuStripRetriever(arg);

                        if (contextMenu != null)
                        {
                            contextMenu.Show(
                                TreeView,
                                e.Location);

                            TreeView.SelectedNode = node;
                        }
                    }

                    return new TrmrkActionResult();
                }
            }); */

        protected virtual void TreeView_AfterExpand(
            object sender,
            TreeViewEventArgs e)
        {
            throw new NotImplementedException();
        } /* => ActionComponent.Execute(new TrmrkActionComponentOpts
            {
                ActionName = nameof(TreeView_AfterExpand),
                Action = () =>
                {
                    var node = e.Node;
                    CurrentTreeNode = node;

                    if (ChildrenRefreshDepth > 0)
                    {
                        var path = WinFormsH.GetPath(TreeView, node);
                        var parentDataNode = DataTree.FindPath(path).AsMtbl();
                        var nodesCllctn = node.Nodes;

                        var argsArr = parentDataNode.Children.Select(
                            (dataNode, idx) => TreeNodeArgH.Arg(
                                nodesCllctn[idx],
                                path.Append(idx).ToArray(),
                                dataNode)).ToArray();

                        TreeNodes_AssureLoaded(argsArr);
                    }
                    
                    return new TrmrkActionResult();
                }
            }); */

        protected abstract void TreeNode_Refresh(
            TreeNodeArg<TValue> value);

        protected abstract void TreeNodes_Refresh(
            TreeNodeArg<TValue>[] valuesArr);

        protected abstract void TreeNode_AssureLoaded(
            TreeNodeArg<TValue> value);

        protected abstract void TreeNodes_AssureLoaded(
            TreeNodeArg<TValue>[] valuesArr);

        protected TreeNode CreateTreeNode(
            int[] path,
            DataTreeNode.Mtbl<TValue> node,
            TreeNodeCollection nodeCollection)
        {
            var treeNode = new TreeNode();

            var arg = TreeNodeArgH.Arg(
                treeNode, path, node);

            treeNode.Text = NodeTextFactory(arg);
            nodeCollection.Add(treeNode);

            ApplyImageIndexesIfReq(arg);
            OnNodeCreated(arg);

            return treeNode;
        }

        protected void ApplyImageIndexesIfReq(
            TreeNodeArg<TValue> arg)
        {
            ApplyImageIndexIfReq(arg);
            ApplyStateImageIndexIfReq(arg);
            ApplySelectedImageIndexIfReq(arg);
        }

        protected bool ApplyImageIndexIfReq(
            TreeNodeArg<TValue> arg) => ApplyImageIndexIfReq(
                arg,
                NodeIconKvp.NodeIcon,
                NodeIconKvpFactory.NodeIcon,
                (node, idx) => node.ImageIndex = idx,
                (node, key) => node.ImageKey = key);

        protected bool ApplyStateImageIndexIfReq(
            TreeNodeArg<TValue> arg) => ApplyImageIndexIfReq(
                arg,
                NodeIconKvp.StateNodeIcon,
                NodeIconKvpFactory.StateNodeIcon,
                (node, idx) => node.StateImageIndex = idx,
                (node, key) => node.StateImageKey = key);

        protected bool ApplySelectedImageIndexIfReq(
            TreeNodeArg<TValue> arg) => ApplyImageIndexIfReq(
                arg,
                NodeIconKvp.SelectedNodeIcon,
                NodeIconKvpFactory.SelectedNodeIcon,
                (node, idx) => node.SelectedImageIndex = idx,
                (node, key) => node.SelectedImageKey = key);

        protected bool ApplyImageIndexIfReq(
            TreeNodeArg<TValue> arg,
            KeyValuePair<int, string> iconKvp,
            Func<TreeNodeArg<TValue>, KeyValuePair<int, string>> iconKvpFactory,
            Action<TreeNode, int> imageIdxAssignFunc,
            Action<TreeNode, string> imageKeyAssignFunc)
        {
            bool applied = ApplyImageIndexIfReq(
                arg.TreeNode,
                iconKvp,
                imageIdxAssignFunc,
                imageKeyAssignFunc);

            if (!applied && iconKvpFactory != null)
            {
                iconKvp = iconKvpFactory(arg);

                applied = ApplyImageIndexIfReq(
                    arg.TreeNode,
                    iconKvp,
                    imageIdxAssignFunc,
                    imageKeyAssignFunc);
            }

            return applied;
        }

        protected bool ApplyImageIndexIfReq(
            TreeNode treeNode,
            KeyValuePair<int, string> iconKvp,
            Action<TreeNode, int> imageIdxAssignFunc,
            Action<TreeNode, string> imageKeyAssignFunc) => ApplyImageIndexIfReq(
                treeNode,
                iconKvp.Key,
                imageIdxAssignFunc) || ApplyImageKeyIfReq(
                    treeNode,
                    iconKvp.Value,
                    imageKeyAssignFunc);

        protected bool ApplyImageIndexIfReq(
            TreeNode treeNode,
            int imageIdx,
            Action<TreeNode, int> imageIdxAssignFunc)
        {
            bool apply = imageIdx >= 0;

            if (apply)
            {
                imageIdxAssignFunc(treeNode, imageIdx);
            }

            return apply;
        }

        protected bool ApplyImageKeyIfReq(
            TreeNode treeNode,
            string imageKey,
            Action<TreeNode, string> imageKeyAssignFunc)
        {
            bool apply = imageKey != null;

            if (apply)
            {
                imageKeyAssignFunc(treeNode, imageKey);
            }

            return apply;
        }

        private ContextMenuStrip CreateDefaultContextMenuStrip() => ContextMenuStripFactory.Create(
            new ContextMenuStripOpts.Mtbl
            {
                Items = new ToolStripItemOpts.Mtbl
                {
                    Text = "Refresh",
                    ItemType = typeof(ToolStripMenuItem),
                    ClickHandler = (sender, args) => CurrentTreeNode?.ActWithValue(
                        currentTreeNode =>
                        {
                            var path = WinFormsH.GetPath(TreeView, currentTreeNode);
                            var item = DataTree.FindPath(path);

                            TreeNode_Refresh(
                                TreeNodeArgH.Arg(
                                    currentTreeNode,
                                    path,
                                    item));
                        })
                }.List()
            });

        private TreeViewDataAdapterIconsOptsCore.Immtbl<KeyValuePair<int, string>, TValue> GetIconProps(
            TreeViewDataAdapterIconsOpts.IClnbl<TValue> opts) => new TreeViewDataAdapterIconsOptsCore.Mtbl<KeyValuePair<int, string>, TValue>
            {
                NodeIcon = opts?.NodeIcon ?? TreeViewDataAdapterH.EmptyNodeIconKvp,
                SelectedNodeIcon = opts?.SelectedNodeIcon ?? TreeViewDataAdapterH.EmptyNodeIconKvp,
                StateNodeIcon = opts?.StateNodeIcon ?? TreeViewDataAdapterH.EmptyNodeIconKvp
            }.ToImmtbl();

        private TreeViewDataAdapterIconsOptsCore.Immtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue> GetIconProps(
            TreeViewDataAdapterIconFactoriesOpts.IClnbl<TValue> opts) => new TreeViewDataAdapterIconsOptsCore.Mtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue>
            {
                NodeIcon = opts?.NodeIcon,
                SelectedNodeIcon = opts?.SelectedNodeIcon,
                StateNodeIcon = opts?.StateNodeIcon,
            }.ToImmtbl();
    }
}
