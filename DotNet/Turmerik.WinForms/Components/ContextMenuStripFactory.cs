using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turmerik.WinForms.Components
{
    public interface IContextMenuStripFactory
    {
        ContextMenuStrip Create(ContextMenuStripOpts.IClnbl opts);
    }

    public class ContextMenuStripFactory : IContextMenuStripFactory
    {
        private readonly IToolStripItemFactory toolStripItemFactory;

        public ContextMenuStripFactory(
            IToolStripItemFactory toolStripItemFactory)
        {
            this.toolStripItemFactory = toolStripItemFactory ?? throw new ArgumentNullException(nameof(toolStripItemFactory));
        }

        public ContextMenuStrip Create(
            ContextMenuStripOpts.IClnbl opts)
        {
            var itemsArr = opts.GetItems().Select(
                itemOpts => toolStripItemFactory.Create(
                    itemOpts)).ToArray();

            var menuStrip = new ContextMenuStrip();
            menuStrip.Items.AddRange(itemsArr);

            return menuStrip;
        }
    }
}
