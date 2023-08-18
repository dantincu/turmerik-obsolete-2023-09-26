using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Logging;
using Turmerik.WinForms.ActionComponent;

namespace Turmerik.WinForms.Components
{
    public interface ITreeViewDataAdapterFactory
    {
        ITreeViewDataAdapterAsync<TValue> CreateAsyncWithIconFactories<TValue>(
            TreeView treeView,
            Func<TValue, KeyValuePair<int, string>> nodeIconKvpFactory = null,
            Func<TValue, KeyValuePair<int, string>> selectedNodeIconKvpFactory = null,
            Func<TValue, KeyValuePair<int, string>> stateNodeIconKvpFactory = null,
            Func<TValue, ContextMenuStrip> contextMenuStripFactory = null);

        ITreeViewDataAdapterAsync<TValue> CreateAsyncWithIcons<TValue>(
            TreeView treeView,
            KeyValuePair<int, string>? nodeIconKvp = null,
            KeyValuePair<int, string>? selectedNodeIconKvp = null,
            KeyValuePair<int, string>? stateNodeIconKvp = null,
            Func<TValue, ContextMenuStrip> contextMenuStripFactory = null);
    }

    public class TreeViewDataAdapterFactory : ITreeViewDataAdapterFactory
    {
        private readonly IAppLoggerCreator appLoggerCreator;
        private readonly IWinFormsActionComponentFactory winFormsActionComponentFactory;

        public TreeViewDataAdapterFactory(
            IAppLoggerCreator appLoggerCreator,
            IWinFormsActionComponentFactory winFormsActionComponentFactory)
        {
            this.appLoggerCreator = appLoggerCreator ?? throw new ArgumentNullException(
                nameof(appLoggerCreator));

            this.winFormsActionComponentFactory = winFormsActionComponentFactory ?? throw new ArgumentNullException(
                nameof(winFormsActionComponentFactory));
        }

        public ITreeViewDataAdapterAsync<TValue> CreateAsyncWithIconFactories<TValue>(
            TreeView treeView,
            Func<TValue, KeyValuePair<int, string>> nodeIconKvpFactory = null,
            Func<TValue, KeyValuePair<int, string>> selectedNodeIconKvpFactory = null,
            Func<TValue, KeyValuePair<int, string>> stateNodeIconKvpFactory = null,
            Func<TValue, ContextMenuStrip> contextMenuStripFactory = null) => new TreeViewDataAdapterAsync<TValue>(
                appLoggerCreator,
                winFormsActionComponentFactory,
                treeView,
                contextMenuStripFactory)
            {
                NodeIconKvpFactory = nodeIconKvpFactory,
                SelectedNodeIconKvpFactory = selectedNodeIconKvpFactory,
                StateNodeIconKvpFactory = stateNodeIconKvpFactory
            };

        public ITreeViewDataAdapterAsync<TValue> CreateAsyncWithIcons<TValue>(
            TreeView treeView,
            KeyValuePair<int, string>? nodeIconKvp = null,
            KeyValuePair<int, string>? selectedNodeIconKvp = null,
            KeyValuePair<int, string>? stateNodeIconKvp = null,
            Func<TValue, ContextMenuStrip> contextMenuStripFactory = null) => new TreeViewDataAdapterAsync<TValue>(
                appLoggerCreator,
                winFormsActionComponentFactory,
                treeView,
                contextMenuStripFactory)
            {
                NodeIconKvp = nodeIconKvp ?? TreeViewDataAdapterH.EmptyNodeIconKvp,
                SelectedNodeIconKvp = selectedNodeIconKvp ?? TreeViewDataAdapterH.EmptyNodeIconKvp,
                StateNodeIconKvp = stateNodeIconKvp ?? TreeViewDataAdapterH.EmptyNodeIconKvp
            };
    }
}
