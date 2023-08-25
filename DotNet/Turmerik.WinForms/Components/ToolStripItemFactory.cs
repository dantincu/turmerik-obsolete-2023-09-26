using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Utils;

namespace Turmerik.WinForms.Components
{
    public interface IToolStripItemFactory
    {
        ToolStripItem Create(ToolStripItemOpts.IClnbl opts);
    }

    public class ToolStripItemFactory : IToolStripItemFactory
    {
        public ToolStripItem Create(ToolStripItemOpts.IClnbl opts)
        {
            var factory = opts.ItemFactory.FirstNotNull(
                itemOpts => opts.ItemType.CreateInstance<ToolStripItem>());

            var item = factory(opts);

            if (opts.Text != null)
            {
                item.Text = opts.Text;
            }

            if (opts.ClickHandler != null)
            {
                item.Click += new EventHandler(opts.ClickHandler);
            }

            return item;
        }
    }
}
