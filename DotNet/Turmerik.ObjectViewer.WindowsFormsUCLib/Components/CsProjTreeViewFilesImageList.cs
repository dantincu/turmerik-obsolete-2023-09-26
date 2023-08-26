using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Reflection.Cache;
using Turmerik.WinForms.Components;

namespace Turmerik.ObjectViewer.WindowsFormsUCLib.Components
{
    public class CsProjTreeViewFilesImageList : ImageListDecoratorBase
    {
        public CsProjTreeViewFilesImageList(
            ICachedTypesMap cachedTypesMap,
            ImageListDecoratorOpts.IClnbl opts) : base(
                cachedTypesMap,
                opts)
        {
        }

        public int FolderKey { get; protected set; }
        public int FolderOpenKey { get; protected set; }
        public int FolderZipKey { get; protected set; }
        public int HardDriveKey { get; protected set; }
        public int NoteKey { get; protected set; }
        public int CodeKey { get; protected set; }
        public int Package2Key { get; protected set; }
        public int ImageKey { get; protected set; }
        public int AudioFileKey { get; protected set; }
        public int VideoFileKey { get; protected set; }
        public int UnknownDocumentKey { get; protected set; }
    }
}
