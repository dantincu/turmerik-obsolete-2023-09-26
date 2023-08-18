using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.DriveExplorerCore;

namespace Turmerik.WinForms.Controls
{
    public class TrmrkTreeNode<TValue> : TreeNode
    {
        public TrmrkTreeNode(
            TValue data)
        {
            this.Data = data;
        }

        public TrmrkTreeNode(
            TValue data,
            string text) : base(
                text)
        {
            this.Data = data;
        }

        public TrmrkTreeNode(
            TValue data,
            string text,
            TreeNode[] children) : base(
                text,
                children)
        {
            this.Data = data;
        }

        public TrmrkTreeNode(
            TValue data,
            string text,
            int imageIndex,
            int selectedImageIndex) : base(
                text,
                imageIndex,
                selectedImageIndex)
        {
            this.Data = data;
        }

        public TrmrkTreeNode(
            TValue data,
            string text,
            int imageIndex,
            int selectedImageIndex,
            TreeNode[] children) : base(
                text,
                imageIndex,
                selectedImageIndex,
                children)
        {
            this.Data = data;
        }

        protected TrmrkTreeNode(
            TValue data,
            SerializationInfo serializationInfo,
            StreamingContext context) : base(
                serializationInfo,
                context)
        {
            this.Data = data;
        }

        public TValue Data { get; }
    }
}
