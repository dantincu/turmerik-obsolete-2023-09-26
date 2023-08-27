using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.Components
{
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

        public override void RefreshRootNodes(
            int? childrenRefreshDepth = null) => RefreshRootNodesAsync(
                childrenRefreshDepth);
        public override void RefreshChildNodes(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null) => RefreshChildNodesAsync(
                arg, childrenRefreshDepth);

        public override void AssureRootNodesLoaded(
            int? childrenRefreshDepth = null) => AssureRootNodesLoadedAsync(
                childrenRefreshDepth);

        public override void AssureChildNodesLoaded(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null) => AssureChildNodesLoadedAsync(
                arg, childrenRefreshDepth);

        public Task RefreshRootNodesAsync(
            int? childrenRefreshDepth = null) => RefreshRootNodesAsync(
                childrenRefreshDepth ?? ChildrenRefreshDepth);

        public Task RefreshChildNodesAsync(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null) => RefreshChildNodesAsync(
                arg, childrenRefreshDepth ?? ChildrenRefreshDepth);

        public Task AssureRootNodesLoadedAsync(
            int? childrenRefreshDepth = null) => AssureRootNodesLoadedAsync(
                childrenRefreshDepth ?? ChildrenRefreshDepth);

        public Task AssureChildNodesLoadedAsync(
            TreeNodeArg<TValue> arg,
            int? childrenRefreshDepth = null) => AssureChildNodesLoadedAsync(
                arg, childrenRefreshDepth ?? ChildrenRefreshDepth);

        protected async Task RefreshRootNodesAsync(
            int childrenRefreshDepth)
        {
            var rootItems = (await RootItemsFactory()).ToArray();

            this.DataTree.RootNodes = rootItems.Select(
                value => new DataTreeNode.Mtbl<TValue>
                {
                    Value = value,
                }).ToList();

            var rootNodes = this.DataTree.RootNodes;
            var nodesList = new List<TreeNode>();

            var nodesCllctn = TreeView.Nodes;
            nodesCllctn.Clear();

            for (int i = 0; i < rootNodes.Count; i++)
            {
                var treeNode = CreateTreeNode(
                    i.Arr(),
                    rootNodes[i],
                    nodesCllctn);

                nodesList.Add(treeNode);
            }

            if (childrenRefreshDepth > 0)
            {
                for (int i = 0; i < rootNodes.Count; i++)
                {
                    await RefreshChildNodesAsync(
                        TreeNodeArgH.Arg(
                            nodesList[i],
                            i.Arr(),
                            rootNodes[i]),
                        childrenRefreshDepth - 1);
                }
            }

            this.DataTree.LoadedChildrenDepth = childrenRefreshDepth;
        }

        protected async Task RefreshChildNodesAsync(
            TreeNodeArg<TValue> arg,
            int childrenRefreshDepth)
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

            var childNodes = parentNode.Children;
            var nodesList = new List<TreeNode>();

            var nodesCllctn = arg.TreeNode.Nodes;
            nodesCllctn.Clear();

            for (int i = 0; i < parentNode.Children.Count; i++)
            {
                var treeNode = CreateTreeNode(
                    arg.NodePath.Append(i).ToArray(),
                    parentNode.Children[i],
                    arg.TreeNode.Nodes);

                nodesList.Add(treeNode);
            }

            if (childrenRefreshDepth > 0)
            {
                for (int i = 0; i < childNodes.Count; i++)
                {
                    await RefreshChildNodesAsync(
                        TreeNodeArgH.Arg(
                            nodesList[i],
                            arg.NodePath.Append(i).ToArray(),
                            childNodes[i]),
                        childrenRefreshDepth - 1);
                }
            }

            parentNode.LoadedChildrenDepth = childrenRefreshDepth;
        }

        protected async Task AssureRootNodesLoadedAsync(
            int childrenRefreshDepth)
        {
            if (DataTree.RootNodes == null)
            {
                await RefreshRootNodesAsync(childrenRefreshDepth);
            }
            else
            {
                var rootNodes = DataTree.RootNodes;
                var rootTreeNodes = TreeView.Nodes;

                for (int i = 0; i < rootNodes.Count; i++)
                {
                    await AssureChildNodesLoadedAsync(
                        TreeNodeArgH.Arg(
                            rootTreeNodes[i],
                            i.Arr(),
                            rootNodes[i]),
                        childrenRefreshDepth - 1);
                }
            }
        }

        protected async Task AssureChildNodesLoadedAsync(
            TreeNodeArg<TValue> arg,
            int childrenRefreshDepth)
        {
            var parentTreeNode = arg.TreeNode;
            var path = WinFormsH.GetPath(TreeView, parentTreeNode);
            var parentNode = DataTree.FindPath(path).AsMtbl();

            if (parentNode.Children == null)
            {
                await RefreshChildNodesAsync(arg, childrenRefreshDepth);
            }
            else if (parentNode.LoadedChildrenDepth < childrenRefreshDepth)
            {
                var nodesCllctn = parentNode.Children;
                var treeNodes = parentTreeNode.Nodes;

                for (int i = 0; i < nodesCllctn.Count; i++)
                {
                    await AssureChildNodesLoadedAsync(
                        TreeNodeArgH.Arg(
                            treeNodes[i],
                            path.Append(i).ToArray(),
                            nodesCllctn[i]),
                        childrenRefreshDepth - 1);
                }
            }
        }

        protected override void TreeNode_Refresh(
            TreeNodeArg<TValue> value) => TreeNodeRefreshAsync(value);

        protected override void TreeNodes_Refresh(
            TreeNodeArg<TValue>[] valuesArr) => TreeNodesRefreshAsync(valuesArr);

        protected override void TreeNode_AssureLoaded(
            TreeNodeArg<TValue> value) => TreeNodeAssureLoadedAsync(value);

        protected override void TreeNodes_AssureLoaded(
            TreeNodeArg<TValue>[] valuesArr) => TreeNodesAssureLoadedAsync(valuesArr);

        protected Task<ITrmrkActionResult<TreeNodeArg<TValue>>> TreeNodeRefreshAsync(
            TreeNodeArg<TValue> value) => ActionComponent.ExecuteAsync(
            new TrmrkAsyncActionComponentOpts<TreeNodeArg<TValue>>
            {
                ActionName = nameof(TreeNodeRefreshAsync),
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

        protected Task<ITrmrkActionResult<TreeNodeArg<TValue>[]>> TreeNodesRefreshAsync(
            TreeNodeArg<TValue>[] valuesArr) => ActionComponent.ExecuteAsync(
            new TrmrkAsyncActionComponentOpts<TreeNodeArg<TValue>[]>
            {
                ActionName = nameof(TreeNodesRefreshAsync),
                BeforeExecute = () =>
                {
                    OnBeforeNodesRefresh(valuesArr);
                },
                Action = async () =>
                {
                    foreach (var value in valuesArr)
                    {
                        await RefreshChildNodesAsync(
                            value,
                            ChildrenRefreshDepth - 1);
                    }

                    return new TrmrkActionResult<TreeNodeArg<TValue>[]>
                    {
                        Data = valuesArr
                    };
                },
                AlwaysCallback = actionResult =>
                {
                    TreeView.InvokeIfReq(() =>
                    {
                        OnAfterNodesRefresh(actionResult);
                    });
                }
            });

        protected Task<ITrmrkActionResult<TreeNodeArg<TValue>>> TreeNodeAssureLoadedAsync(
            TreeNodeArg<TValue> value) => ActionComponent.ExecuteAsync(
            new TrmrkAsyncActionComponentOpts<TreeNodeArg<TValue>>
            {
                ActionName = nameof(TreeNodeAssureLoadedAsync),
                BeforeExecute = () =>
                {
                    OnBeforeNodeAssureLoaded(value);
                },
                Action = async () =>
                {
                    await AssureChildNodesLoadedAsync(value);

                    return new TrmrkActionResult<TreeNodeArg<TValue>>
                    {
                        Data = value
                    };
                },
                AlwaysCallback = actionResult =>
                {
                    TreeView.InvokeIfReq(() =>
                    {
                        OnAfterNodeAssureLoaded(actionResult);
                    });
                }
            });

        protected Task<ITrmrkActionResult<TreeNodeArg<TValue>[]>> TreeNodesAssureLoadedAsync(
            TreeNodeArg<TValue>[] valuesArr) => ActionComponent.ExecuteAsync(
            new TrmrkAsyncActionComponentOpts<TreeNodeArg<TValue>[]>
            {
                ActionName = nameof(TreeNodesAssureLoadedAsync),
                BeforeExecute = () =>
                {
                    OnBeforeNodesAssureLoaded(valuesArr);
                },
                Action = async () =>
                {
                    foreach (var value in valuesArr)
                    {
                        await AssureChildNodesLoadedAsync(
                            value,
                            ChildrenRefreshDepth - 1);
                    }

                    return new TrmrkActionResult<TreeNodeArg<TValue>[]>
                    {
                        Data = valuesArr
                    };
                },
                AlwaysCallback = actionResult =>
                {
                    TreeView.InvokeIfReq(() =>
                    {
                        OnAfterNodesAssureLoaded(actionResult);
                    });
                }
            });
    }
}
