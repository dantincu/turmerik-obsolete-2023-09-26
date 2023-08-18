using Jint.Native;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Turmerik.DriveExplorerCore;
using Turmerik.Logging;
using Turmerik.TrmrkAction;
using Turmerik.Utils;
using Turmerik.WinForms.ActionComponent;
using Turmerik.WinForms.Dependencies;

namespace Turmerik.WinForms.Controls
{
    public class TrmrkFsTreeView : TrmrkTreeView<DriveItem.Mtbl>
    {
        private DriveItemIdnf.IClnbl rootItemIdnf;
        private DriveItem.Mtbl rootItem;

        public DriveItemIdnf.IClnbl GetRootItemIdnf() => rootItemIdnf;

        public async Task SetRootItemIdnfAsync(
            DriveItemIdnf.IClnbl rootItemIdnf)
        {
            this.rootItemIdnf = rootItemIdnf;
            await RefreshRootNodesAsync();
        }

        public IDriveItemsRetriever DriveItemsRetriever { get; set; }

        protected override DriveItem.Mtbl[] GetChildItems(
            DriveItem.Mtbl parentItem)
        {
            var childrenList = new List<DriveItem.Mtbl>();

            if (parentItem.SubFolders != null)
            {
                childrenList.AddRange(parentItem.SubFolders);
            }

            if (parentItem.FolderFiles != null)
            {
                childrenList.AddRange(parentItem.FolderFiles);
            }

            return childrenList.ToArray();
        }

        protected override string GetNodeText(
            DriveItem.Mtbl item) => item.Name;

        protected override async Task<DriveItem.Mtbl[]> GetRootItemsAsync(int refreshDepth = 1)
        {
            rootItem = await DriveItemsRetriever.GetFolderAsync(rootItemIdnf);
            var rootItemsArr = GetChildItems(rootItem);

            return rootItemsArr;
        }

        protected override async Task<DriveItem.Mtbl[]> GetChildItemsAsync(
            DriveItem.Mtbl parentItem,
            int refreshDepth = 1)
        {
            var subFolders = parentItem.SubFolders;

            if (refreshDepth > 0)
            {
                for (int i = 0; i < subFolders.Count; i++)
                {
                    subFolders[i] = await DriveItemsRetriever.GetFolderAsync(subFolders[i]);

                    if (refreshDepth > 1)
                    {
                        
                    }
                }
            }

            var childtemsArr = GetChildItems(parentItem);
            return childtemsArr;
        }
    }
}
