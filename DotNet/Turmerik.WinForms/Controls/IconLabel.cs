using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.WinForms.Controls
{
    public class IconLabel : Label
    {
        private readonly ServiceProviderContainer svcProvContnr;
        private readonly bool svcRegistered;

        private FontFamily fontFamily;
        /* private Color labelBackColor;
        private int borderRadius; */

        public IconLabel()
        {
            this.svcProvContnr = ServiceProviderContainer.Instance.Value;
            this.svcRegistered = svcProvContnr.AreServicesRegistered();

            if (this.svcRegistered)
            {
                fontFamily = svcProvContnr.IconsFont.Families[0];

                Font = new Font(
                    FontFamily,
                    16f,
                    FontStyle.Regular);
            }

            Cursor = Cursors.Hand;

            // Paint += IconLabel_Paint;
        }

        public FontFamily FontFamily
        {
            get => fontFamily;

            set
            {
                fontFamily = value;

                if (value != null)
                {
                    Font = new Font(
                        FontFamily,
                        Font.Size,
                        Font.Style);
                }

                Invalidate();
            }
        }

        /* public Color LabelBackColor
        {
            get => labelBackColor;

            set
            {
                labelBackColor = value;

                Invalidate();
            }
        }

        public int BorderRadius
        {
            get => borderRadius;

            set
            {
                borderRadius = value;

                Invalidate();
            }
        } */

        private void IconLabel_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
