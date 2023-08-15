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
    public class FsTreeNode<TDriveItem> : TreeNode
        where TDriveItem : DriveItem.IClnbl
    {
        public FsTreeNode(
            TDriveItem data)
        {
            this.Data = data;
        }

        public FsTreeNode(
            TDriveItem data,
            string text) : base(
                text)
        {
            this.Data = data;
        }

        public FsTreeNode(
            TDriveItem data,
            string text,
            TreeNode[] children) : base(
                text,
                children)
        {
            this.Data = data;
        }

        public FsTreeNode(
            TDriveItem data,
            string text,
            int imageIndex,
            int selectedImageIndex) : base(
                text,
                imageIndex,
                selectedImageIndex)
        {
            this.Data = data;
        }

        public FsTreeNode(
            TDriveItem data,
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

        protected FsTreeNode(
            TDriveItem data,
            SerializationInfo serializationInfo,
            StreamingContext context) : base(
                serializationInfo,
                context)
        {
            this.Data = data;
        }

        public TDriveItem Data { get; }
    }
}
