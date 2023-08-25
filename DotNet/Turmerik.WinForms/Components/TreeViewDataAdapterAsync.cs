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
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Controls;
using Turmerik.WinForms.Dependencies;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.Components
{
    public interface ITreeViewDataAdapterCore<TValue>
    {
        TreeView TreeView { get; }
        bool RefreshOnDoubleClick { get; set; }

        TreeViewDataAdapterIconsOptsCore.Immtbl<KeyValuePair<int, string>, TValue> NodeIconKvp { get; }
        TreeViewDataAdapterIconsOptsCore.Immtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue> NodeIconKvpFactory { get; }

        event Action<TreeNodeArg<TValue>> NodeCreated;

        event Action<TreeNodeArg<TValue>> BeforeNodeRefresh;
        event Action<ITrmrkActionResult<TreeNodeArg<TValue>>> AfterNodeRefresh;

        event Action<IEnumerable<TValue>> BeforeRootNodesRefresh;
        event Action<ITrmrkActionResult<IEnumerable<TreeNodeArg<TValue>>>> AfterRootNodesRefresh;
    }

    public interface ITreeViewDataAdapterAsync<TValue> : ITreeViewDataAdapterCore<TValue>
    {
        Task RefreshRootNodesAsync();

        Task RefreshChildNodesAsync(
            TreeNodeArg<TValue> arg);
    }

    public abstract class TreeViewDataAdapterBase<TValue> : ITreeViewDataAdapterCore<TValue>
    {
        private Action<TreeNodeArg<TValue>> nodeCreated;
        private Action<TreeNodeArg<TValue>> beforeNodeRefresh;
        private Action<ITrmrkActionResult<TreeNodeArg<TValue>>> afterNodeRefresh;
        private Action<IEnumerable<TValue>> beforeRootNodesRefresh;
        private Action<ITrmrkActionResult<IEnumerable<TreeNodeArg<TValue>>>> afterRootNodesRefresh;

        public TreeViewDataAdapterBase(
            IAppLoggerCreator appLoggerCreator,
            IWinFormsActionComponentFactory winFormsActionComponentFactory,
            TreeViewDataAdapterOptsCore.IClnbl<TValue> opts,
            IContextMenuStripFactory contextMenuStripFactory)
        {
            TreeView = opts.TreeView ?? throw new ArgumentNullException(
                nameof(opts.TreeView));

            ContextMenuStripFactory = contextMenuStripFactory ?? throw new ArgumentNullException(
                nameof(contextMenuStripFactory));

            NodeTextFactory = opts.NodeTextFactory ?? throw new ArgumentNullException(
                nameof(opts.NodeTextFactory));

            DefaultContextMenuStrip = LazyH.Lazy(CreateDefaultContextMenuStrip);

            this.ContextMenuStripRetriever = opts.ContextMenuStripRetriever.FirstNotNull(
                value => DefaultContextMenuStrip.Value);

            NodeIconKvp = GetIconProps(opts as TreeViewDataAdapterIconsOpts.IClnbl<TValue>);
            NodeIconKvpFactory = GetIconProps(opts as TreeViewDataAdapterIconFactoriesOpts.IClnbl<TValue>);

            this.Logger = appLoggerCreator.GetSharedAppLogger(GetType());
            this.ActionComponent = winFormsActionComponentFactory.Create(this.Logger);
            this.DataTree = new DataTree.Mtbl<TValue>();

            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
        }

        public TreeView TreeView { get; }

        public bool RefreshOnDoubleClick { get; set; }

        public TreeViewDataAdapterIconsOptsCore.Immtbl<KeyValuePair<int, string>, TValue> NodeIconKvp { get; }
        public TreeViewDataAdapterIconsOptsCore.Immtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue> NodeIconKvpFactory { get; }

        protected IAppLogger Logger { get; private set; }
        protected IWinFormsActionComponent ActionComponent { get; private set; }
        protected IContextMenuStripFactory ContextMenuStripFactory { get; }
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

        public event Action<ITrmrkActionResult<TreeNodeArg<TValue>>> AfterNodeRefresh
        {
            add => afterNodeRefresh += value;
            remove => afterNodeRefresh -= value;
        }

        public event Action<IEnumerable<TValue>> BeforeRootNodesRefresh
        {
            add => beforeRootNodesRefresh += value;
            remove => beforeRootNodesRefresh -= value;
        }

        public event Action<ITrmrkActionResult<IEnumerable<TreeNodeArg<TValue>>>> AfterRootNodesRefresh
        {
            add => afterRootNodesRefresh += value;
            remove => afterRootNodesRefresh -= value;
        }

        protected void OnNodeCreated(
            TreeNodeArg<TValue> value) => nodeCreated?.Invoke(value);

        protected void OnBeforeNodeRefresh(
            TreeNodeArg<TValue> value) => beforeNodeRefresh?.Invoke(value);

        protected void OnAfterNodeRefresh(
            ITrmrkActionResult<TreeNodeArg<TValue>> actionResult) => afterNodeRefresh?.Invoke(actionResult);

        protected void OnBeforeRootNodesRefresh(
            IEnumerable<TValue> nmrbl) => beforeRootNodesRefresh?.Invoke(nmrbl);

        protected void OnAfterRootNodesRefresh(
            ITrmrkActionResult<IEnumerable<TreeNodeArg<TValue>>> actionResult) => afterRootNodesRefresh?.Invoke(actionResult);

        protected abstract void TreeViewNodeMouseClick(
            TreeNodeMouseClickEventArgs e,
            TreeNodeArg<TValue> arg);

        protected virtual void TreeView_NodeMouseClick(
            object sender,
            TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node;
            CurrentTreeNode = node;

            ActionComponent.Execute(new TrmrkActionComponentOpts
            {
                ActionName = nameof(TreeView_NodeMouseClick),
                Action = () =>
                {
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
                        }
                    }

                    TreeViewNodeMouseClick(e, arg);
                    return new TrmrkActionResult();
                }
            });
        }

        protected abstract void TreeNode_Refresh(
            TreeNodeArg<TValue> value);

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

    public class TreeViewDataAdapterAsync<TValue> : TreeViewDataAdapterBase<TValue>, ITreeViewDataAdapterAsync<TValue>
    {
        public TreeViewDataAdapterAsync(
            IAppLoggerCreator appLoggerCreator,
            IWinFormsActionComponentFactory winFormsActionComponentFactory,
            TreeViewDataAdapterOptsCore.IClnbl<TValue> opts,
            IContextMenuStripFactory contextMenuStripFactory,
            Func<Task<IEnumerable<TValue>>> rootItemsFactory,
            Func<TValue, Task<IEnumerable<TValue>>> childItemsFactory) : base(
                appLoggerCreator,
                winFormsActionComponentFactory,
                opts,
                contextMenuStripFactory)
        {
            RootItemsFactory = rootItemsFactory ?? throw new ArgumentNullException(nameof(rootItemsFactory));
            ChildItemsFactory = childItemsFactory ?? throw new ArgumentNullException(nameof(childItemsFactory));
        }

        protected Func<Task<IEnumerable<TValue>>> RootItemsFactory { get; }
        protected Func<TValue, Task<IEnumerable<TValue>>> ChildItemsFactory { get; }

        public async Task RefreshRootNodesAsync()
        {
            var rootItems = (await RootItemsFactory()).ToArray();

            this.DataTree.RootNodes = rootItems.Select(
                value => new DataTreeNode.Mtbl<TValue>
                {
                    Value = value,
                }).ToList();

            TreeView.Nodes.Clear();

            for (int i = 0; i < this.DataTree.RootNodes.Count; i++)
            {
                CreateTreeNode(
                    i.Arr(),
                    this.DataTree.RootNodes[i],
                    TreeView.Nodes);
            }
        }

        public async Task RefreshChildNodesAsync(
            TreeNodeArg<TValue> arg)
        {
            var items = await ChildItemsFactory(arg.Value);

            var parentNode = DataTree.FindPath(
                arg.NodePath.ToArray()).AsMtbl();

            parentNode.Children = items.Select(
                value => new DataTreeNode.Mtbl<TValue>
                {
                    Value = value,
                    Parent = parentNode
                }).ToList();

            arg.TreeNode.Nodes.Clear();

            for (int i = 0; i < parentNode.Children.Count; i++)
            {
                CreateTreeNode(
                    i.Arr(),
                    parentNode.Children[i],
                    arg.TreeNode.Nodes);
            }
        }

        protected override void TreeViewNodeMouseClick(
            TreeNodeMouseClickEventArgs e,
            TreeNodeArg<TValue> arg)
        {
            if (e.Button == MouseButtons.Left && e.Clicks > 1 && RefreshOnDoubleClick)
            {
                ActionComponent.Execute(new TrmrkActionComponentOpts
                {
                    ActionName = nameof(TreeViewNodeMouseClick),
                    Action = () =>
                    {
                        TreeNodeRefreshAsync(arg);
                        return new TrmrkActionResult();
                    }
                });
            }
        }

        protected override void TreeNode_Refresh(
            TreeNodeArg<TValue> value) => TreeNodeRefreshAsync(value);

        protected Task<ITrmrkActionResult<TreeNodeArg<TValue>>> TreeNodeRefreshAsync(
            TreeNodeArg<TValue> value) => ActionComponent.ExecuteAsync(
            new TrmrkAsyncActionComponentOpts<TreeNodeArg<TValue>>
            {
                ActionName = nameof(TreeNode_Refresh),
                BeforeExecute = () =>
                {
                    OnBeforeNodeRefresh(value);
                },
                Action = async () =>
                {
                    await RefreshChildNodesAsync(value);

                    return new TrmrkActionResult<TreeNodeArg<TValue>>
                    {
                        Data = value
                    };
                },
                AlwaysCallback = actionResult =>
                {
                    TreeView.InvokeIfReq(() =>
                    {
                        OnAfterNodeRefresh(actionResult);
                    });
                }
            });
    }
}
