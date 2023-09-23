using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Logging;

namespace Turmerik.WinForms.Components
{
    public interface ITreeViewDataAdapterFactory
    {
        ITreeViewDataAdapterAsync<TValue> Create<TValue>(
            TreeViewDataAdapterIconFactoriesOpts.IClnbl<TValue> opts,
            Func<Task<IEnumerable<TValue>>> rootItemsFactory,
            Func<TValue, Task<IEnumerable<TValue>>> childItemsFactory);

        ITreeViewDataAdapterAsync<TValue> Create<TValue>(
            TreeViewDataAdapterIconsOpts.IClnbl<TValue> opts,
            Func<Task<IEnumerable<TValue>>> rootItemsFactory,
            Func<TValue, Task<IEnumerable<TValue>>> childItemsFactory);
    }

    public class TreeViewDataAdapterFactory : ITreeViewDataAdapterFactory
    {
        private readonly IAppLoggerCreator appLoggerCreator;
        // private readonly IWinFormsActionComponentFactory winFormsActionComponentFactory;
        private readonly IContextMenuStripFactory contextMenuStripFactory;

        public TreeViewDataAdapterFactory(
            IAppLoggerCreator appLoggerCreator,
            // IWinFormsActionComponentFactory winFormsActionComponentFactory,
            IContextMenuStripFactory contextMenuStripFactory)
        {
            this.appLoggerCreator = appLoggerCreator ?? throw new ArgumentNullException(
                nameof(appLoggerCreator));

            /* this.winFormsActionComponentFactory = winFormsActionComponentFactory ?? throw new ArgumentNullException(
                nameof(winFormsActionComponentFactory)); */

            this.contextMenuStripFactory = contextMenuStripFactory ?? throw new ArgumentNullException(
                nameof(contextMenuStripFactory));
        }

        public ITreeViewDataAdapterAsync<TValue> Create<TValue>(
            TreeViewDataAdapterIconFactoriesOpts.IClnbl<TValue> opts,
            Func<Task<IEnumerable<TValue>>> rootItemsFactory,
            Func<TValue, Task<IEnumerable<TValue>>> childItemsFactory) => new TreeViewDataAdapterAsync<TValue>(
                appLoggerCreator,
                // winFormsActionComponentFactory,
                opts,
                contextMenuStripFactory,
                rootItemsFactory,
                childItemsFactory);

        public ITreeViewDataAdapterAsync<TValue> Create<TValue>(
            TreeViewDataAdapterIconsOpts.IClnbl<TValue> opts,
            Func<Task<IEnumerable<TValue>>> rootItemsFactory,
            Func<TValue, Task<IEnumerable<TValue>>> childItemsFactory) => new TreeViewDataAdapterAsync<TValue>(
                appLoggerCreator,
                // winFormsActionComponentFactory,
                opts,
                contextMenuStripFactory,
                rootItemsFactory,
                childItemsFactory);
    }
}
