using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.DriveExplorerCore;
using Turmerik.Logging;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Dependencies;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.Controls
{
    public abstract class TrmrkTreeView<TValue> : TreeView
    {
        private Action<TrmrkTreeNode<TValue>> nodeCreated;

        public TrmrkTreeView()
        {
            this.SvcProvContnr = ServiceProviderContainer.Instance.Value;
            this.SvcRegistered = SvcProvContnr.AreServicesRegistered();

            if (this.SvcRegistered)
            {
                this.SvcProv = SvcProvContnr.Services;
                this.AppLoggerFactory = this.SvcProv.GetRequiredService<IAppLoggerCreator>();
                this.Logger = this.AppLoggerFactory.GetSharedAppLogger(GetType());
                this.ActionComponentFactory = this.SvcProv.GetRequiredService<IWinFormsActionComponentFactory>();
                this.ActionComponent = this.ActionComponentFactory.Create(this.Logger);

                this.NodeMouseDoubleClick += FsTreeView_NodeMouseDoubleClick;
            }
        }

        public bool RefreshOnDoubleClick { get; set; }

        protected ServiceProviderContainer SvcProvContnr { get; }
        protected bool SvcRegistered { get; }
        protected IServiceProvider SvcProv { get; }

        protected IAppLoggerCreator AppLoggerFactory { get; }
        protected IWinFormsActionComponentFactory ActionComponentFactory { get; }

        protected IAppLogger Logger { get; private set; }
        protected IWinFormsActionComponent ActionComponent { get; private set; }

        protected TValue[] RootItems { get; private set; }
        protected TrmrkTreeNode<TValue>[] RootNodes { get; private set; }

        protected KeyValuePair<int, string> NodeIconKvp { get; private set; } = new KeyValuePair<int, string>(-1, null);
        protected KeyValuePair<int, string> SelectedNodeIconKvp { get; private set; } = new KeyValuePair<int, string>(-1, null);
        protected KeyValuePair<int, string> StateNodeIconKvp { get; private set; } = new KeyValuePair<int, string>(-1, null);

        protected Func<TValue, KeyValuePair<int, string>> NodeIconKvpFactory { get; private set; }
        protected Func<TValue, KeyValuePair<int, string>> SelectedNodeIconKvpFactory { get; private set; }
        protected Func<TValue, KeyValuePair<int, string>> StateNodeIconKvpFactory { get; private set; }

        public IAppLogger GetLogger() => Logger;

        public void SetLogger(
            IAppLogger logger) => this.Logger = logger;

        public IWinFormsActionComponent GetActionComponent() => ActionComponent;

        public void SetActionComponent(
            IWinFormsActionComponent actionComponent) => this.ActionComponent = actionComponent;

        public async Task RefreshRootNodesAsync(int refreshDepth = 1)
        {
            RootItems = await GetRootItemsAsync(refreshDepth);

            this.InvokeIfReq(() =>
            {
                if (RootItems != null)
                {
                    RefreshRootNodes();
                }
                else
                {
                    RootNodes = null;
                    this.Nodes.Clear();
                }
            });
        }

        protected abstract Task<TValue[]> GetRootItemsAsync(
            int refreshDepth = 1);

        protected abstract Task<TValue[]> GetChildItemsAsync(
            TValue parentItem,
            int refreshDepth = 1);

        protected abstract TValue[] GetChildItems(TValue parentItem);
        protected abstract string GetNodeText(TValue item);

        private void RefreshRootNodes()
        {
            RefreshNodesCllctn(
                RootItems,
                Nodes,
                out var rootNodes);

            this.RootNodes = rootNodes;
        }

        private void RefreshChildNodes(TreeNode node)
        {
            var treeNode = node as TrmrkTreeNode<TValue>;

            if (treeNode != null)
            {
                RefreshChildNodes(treeNode);
            }
            else
            {
                throw new NotSupportedException(
                    string.Join(" ",
                        "All nodes of this tree have to be of type",
                        typeof(TrmrkTreeNode<TValue>).FullName));
            }
        }

        private void RefreshChildNodes(
            TrmrkTreeNode<TValue> treeNode) => this.InvokeIfReq(
                () => RefreshNodesCllctn(
                    GetChildItems(treeNode.Data),
                    treeNode.Nodes,
                    out _));

        private void RefreshNodesCllctn(
            TValue[] items,
            TreeNodeCollection nodesCllcnt,
            out TrmrkTreeNode<TValue>[] childNodes)
        {
            nodesCllcnt.Clear();

            childNodes = items?.Select(
                CreateNode).ToArray();

            if (childNodes != null)
            {
                nodesCllcnt.AddRange(childNodes);
            }
        }

        private TrmrkTreeNode<TValue> CreateNode(
            TValue item)
        {
            var node = new TrmrkTreeNode<TValue>(
                item, GetNodeText(item));

            node.ForeColor = this.ForeColor;

            RefreshChildNodes(node);
            ApplyItemIconIdxIfReq(node);

            nodeCreated?.Invoke(node);
            return node;
        }

        private KeyValuePair<int, string> GetNodeIconKvp(
            TValue file,
            KeyValuePair<int, string> defaultNodeIconKvp,
            Func<TValue, KeyValuePair<int, string>> nodeIconKvpFactory)
        {
            KeyValuePair<int, string> nodeIconKvp = defaultNodeIconKvp;

            if (nodeIconKvpFactory != null)
            {
                nodeIconKvp = nodeIconKvpFactory(file);
            }

            return nodeIconKvp;
        }

        private void ApplyItemIconIdxIfReq(
            TrmrkTreeNode<TValue> treeNode)
        {
            KeyValuePair<int, string> iconKvp = GetNodeIconKvp(
                treeNode.Data,
                NodeIconKvp,
                NodeIconKvpFactory);

            KeyValuePair<int, string> selectedIconKvp = GetNodeIconKvp(
                treeNode.Data,
                SelectedNodeIconKvp,
                SelectedNodeIconKvpFactory);

            KeyValuePair<int, string> stateIconKvp = GetNodeIconKvp(
                treeNode.Data,
                StateNodeIconKvp,
                StateNodeIconKvpFactory);

            ApplyIconIdxIfReq(
                treeNode,
                iconKvp,
                selectedIconKvp,
                stateIconKvp);
        }

        private void ApplyIconIdxIfReq(
            TreeNode treeNode,
            KeyValuePair<int, string> iconKvp,
            KeyValuePair<int, string> selectedIconKvp,
            KeyValuePair<int, string> stateIconKvp)
        {
            ApplyIconIdxIfReq(
                treeNode, selectedIconKvp,
                (node, idx) => node.SelectedImageIndex = idx,
                (node, key) => node.SelectedImageKey = key);

            ApplyIconIdxIfReq(
                treeNode, iconKvp,
                (node, idx) => node.ImageIndex = idx,
                (node, key) => node.ImageKey = key);

            ApplyIconIdxIfReq(
                treeNode, stateIconKvp,
                (node, idx) => node.StateImageIndex = idx,
                (node, key) => node.StateImageKey = key);
        }

        private void ApplyIconIdxIfReq(
            TreeNode treeNode,
            KeyValuePair<int, string> iconKvp,
            Action<TreeNode, int> idxAssignFunc,
            Action<TreeNode, string> keyAssignFunc)
        {
            if (iconKvp.Key >= 0)
            {
                idxAssignFunc(treeNode, iconKvp.Key);
            }
            else if (iconKvp.Value != null)
            {
                keyAssignFunc(treeNode, iconKvp.Value);
            }
        }

        #region UI Event Handlers

        private void FsTreeView_NodeMouseDoubleClick(
            object sender,
            TreeNodeMouseClickEventArgs e) => ActionComponent.Execute(new TrmrkActionComponentOpts
            {
                ActionName = nameof(FsTreeView_NodeMouseDoubleClick),
                Action = () =>
                {
                    if (RefreshOnDoubleClick)
                    {
                        RefreshChildNodes(e.Node);
                    }

                    return new TrmrkActionResult();
                },
                LogLevel = Microsoft.Extensions.Logging.LogLevel.Error
            });

        #endregion UI Event Handlers
    }
}
