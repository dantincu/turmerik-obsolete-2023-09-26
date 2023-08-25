using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Collections;
using Turmerik.Text;
using Turmerik.WinForms.Controls;

namespace Turmerik.WinForms.Utils
{
    public static class WinFormsH
    {
        public static void InvokeIfReq(
            this Control control,
            Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Code taken from https://www.syncfusion.com/blogs/post/how-to-use-icon-fonts-in-winforms-windows-forms-applications.aspx
        /// </summary>
        /// <param name="executingAssembly"></param>
        /// <param name="manifestResurceStreamName"></param>
        public static IntPtr AddFontToMemory(
            this PrivateFontCollection pfc,
            Assembly executingAssembly,
            string manifestResurceStreamName)
        {
            byte[] fontAsByte;

            using (Stream fontAsStream = executingAssembly.GetManifestResourceStream(manifestResurceStreamName))
            {
                long streamLen = fontAsStream.Length;
                fontAsByte = new byte[streamLen];

                fontAsStream.Read(
                    fontAsByte,
                    0,
                    (int)streamLen);
            }

            IntPtr memPointer = pfc.AddFontToMemory(fontAsByte);
            return memPointer;
        }

        public static IntPtr AddFontToMemory(
            this PrivateFontCollection pfc,
            byte[] fontAsByte)
        {
            IntPtr memPointer = IntPtr.Zero;

            try
            {
                memPointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(
                    System.Runtime.InteropServices.Marshal.SizeOf(
                        typeof(byte)) * fontAsByte.Length);

                System.Runtime.InteropServices.Marshal.Copy(
                    fontAsByte,
                    0,
                    memPointer,
                    fontAsByte.Length);

                pfc.AddMemoryFont(
                    memPointer,
                    fontAsByte.Length);
            }
            catch
            {
                if (memPointer != IntPtr.Zero)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(memPointer);
                }

                throw;
            }
            
            return memPointer;
        }

        public static ClickToggleIconLabel SetExpandMoreLess(
            this ClickToggleIconLabel control)
        {
            control.SetToggleText(
                Unicodes.ExpandMore,
                Unicodes.ExpandLess);

            return control;
        }

        public static int[] GetPath(
            TreeView treeView,
            TreeNode treeNode) => DataTreeNodeH.GetPath(
                treeNode,
                node => node.Parent,
                node => node.Nodes.OfType<TreeNode>(),
                () => treeView.Nodes.OfType<TreeNode>());
    }
}
