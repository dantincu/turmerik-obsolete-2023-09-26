using Microsoft.Extensions.DependencyInjection;
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
using Turmerik.WinForms.Controls;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.WinForms.Components
{
    public interface ITreeViewDataAdapterCore<TValue>
    {
        TreeView TreeView { get; }
        bool RefreshOnDoubleClick { get; set; }

        KeyValuePair<int, string> NodeIconKvp { get; set; }
        KeyValuePair<int, string> SelectedNodeIconKvp { get; set; }
        KeyValuePair<int, string> StateNodeIconKvp { get; set; }

        Func<TValue, KeyValuePair<int, string>> NodeIconKvpFactory { get; set; }
        Func<TValue, KeyValuePair<int, string>> SelectedNodeIconKvpFactory { get; set; }
        Func<TValue, KeyValuePair<int, string>> StateNodeIconKvpFactory { get; set; }

        event Action<TValue, TreeNode> NodeCreated;

        void RefreshRootNodes();

        void RefreshChildNodes(
            TValue value,
            TreeNode treeNode);
    }

    public interface ITreeViewDataAdapterAsync<TValue> : ITreeViewDataAdapterCore<TValue>
    {
        Task RefreshRootNodesAsync();

        Task RefreshChildNodesAsync(
            TValue value,
            TreeNode treeNode);
    }

    public abstract class TreeViewDataAdapterBase<TValue> : ITreeViewDataAdapterCore<TValue>
    {
        private Action<TValue, TreeNode> nodeCreated;

        public TreeViewDataAdapterBase(
            IAppLoggerCreator appLoggerCreator,
            IWinFormsActionComponentFactory winFormsActionComponentFactory,
            TreeView treeView,
            Func<TValue, ContextMenuStrip> contextMenuStripFactory = null)
        {
            TreeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
            this.ContextMenuStripFactory = contextMenuStripFactory.FirstNotNull(value => null);

            this.Logger = appLoggerCreator.GetSharedAppLogger(GetType());
            this.ActionComponent = winFormsActionComponentFactory.Create(this.Logger);
            this.DataTree = new DataTree.Mtbl<TValue>();

            TreeView.NodeMouseDoubleClick += FsTreeView_NodeMouseDoubleClick;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
        }

        public TreeView TreeView { get; }

        public bool RefreshOnDoubleClick { get; set; }

        public KeyValuePair<int, string> NodeIconKvp { get; set; } = TreeViewDataAdapterH.EmptyNodeIconKvp;
        public KeyValuePair<int, string> SelectedNodeIconKvp { get; set; } = TreeViewDataAdapterH.EmptyNodeIconKvp;
        public KeyValuePair<int, string> StateNodeIconKvp { get; set; } = TreeViewDataAdapterH.EmptyNodeIconKvp;

        public Func<TValue, KeyValuePair<int, string>> NodeIconKvpFactory { get; set; }
        public Func<TValue, KeyValuePair<int, string>> SelectedNodeIconKvpFactory { get; set; }
        public Func<TValue, KeyValuePair<int, string>> StateNodeIconKvpFactory { get; set; }

        protected IAppLogger Logger { get; private set; }
        protected IWinFormsActionComponent ActionComponent { get; private set; }
        protected Func<TValue, ContextMenuStrip> ContextMenuStripFactory { get; }
        protected DataTree.Mtbl<TValue> DataTree { get; }

        public event Action<TValue, TreeNode> NodeCreated
        {
            add => nodeCreated += value;
            remove => nodeCreated -= value;
        }

        public virtual void RefreshRootNodes()
        {

        }

        public virtual void RefreshChildNodes(
            TValue value,
            TreeNode treeNode)
        {

        }

        protected void OnNodeCreated(
            TValue value,
            TreeNode treeNode) => nodeCreated?.Invoke(
                value,
                treeNode);

        protected void RefreshChildNodes(TreeNode node)
        {

        }

        protected abstract void FsTreeView_NodeMouseDoubleClick(
            object sender,
            TreeNodeMouseClickEventArgs e);

        protected abstract void TreeView_NodeMouseClick(
            object sender,
            TreeNodeMouseClickEventArgs e);
    }

    public class TreeViewDataAdapterAsync<TValue> : TreeViewDataAdapterBase<TValue>, ITreeViewDataAdapterAsync<TValue>
    {
        public TreeViewDataAdapterAsync(
            IAppLoggerCreator appLoggerCreator,
            IWinFormsActionComponentFactory winFormsActionComponentFactory,
            TreeView treeView,
            Func<TValue, ContextMenuStrip> contextMenuStripFactory = null) : base(
                appLoggerCreator,
                winFormsActionComponentFactory,
                treeView,
                contextMenuStripFactory)
        {
        }

        public override void RefreshRootNodes()
        {
            RefreshRootNodesAsync();
        }

        public override void RefreshChildNodes(
            TValue value,
            TreeNode treeNode)
        {
            RefreshChildNodesAsync(value, treeNode);
        }

        public async Task RefreshRootNodesAsync()
        {

        }

        public async Task RefreshChildNodesAsync(
            TValue value,
            TreeNode treeNode)
        {

        }

        #region UI Event Handlers

        protected override void FsTreeView_NodeMouseDoubleClick(
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

        protected override void TreeView_NodeMouseClick(
            object sender,
            TreeNodeMouseClickEventArgs e) => ActionComponent.Execute(new TrmrkActionComponentOpts
            {
                ActionName = nameof(TreeView_NodeMouseClick),
                Action = () =>
                {


                    return new TrmrkActionResult();
                },
                LogLevel = Microsoft.Extensions.Logging.LogLevel.Error
            });

        #endregion UI Event Handlers
    }
}
