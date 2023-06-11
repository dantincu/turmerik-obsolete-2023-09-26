using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.DriveExplorerCore
{
    public enum OfficeLikeFileType
    {
        Docs = 1,
        Sheets,
        Slides
    }

    public partial class DriveItemIdnf<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.IDriveItemIdnf, DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.DriveItemIdnfImmtbl, TClnbl
        where TMtbl : DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.DriveItemIdnfMtbl, TClnbl
    {
        public interface IDriveItemIdnf : IClnblCore
        {
            string Id { get; }
            string Name { get; }

            string PrPath { get; }
            string PrRelPath { get; }

            TClnbl GetPrIdnf();
            TClnbl GetPrBaseIdnf();

            bool Equals(TClnbl other);
        }
    }

    public partial class DriveItemIdnf : DriveItemIdnf<DriveItemIdnf.IClnbl, DriveItemIdnf.Immtbl, DriveItemIdnf.Mtbl>
    {
        public interface IClnbl : IDriveItemIdnf
        {
        }

        public class Immtbl : DriveItemIdnfImmtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : DriveItemIdnfMtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
            }
        }
    }

    public partial class DriveItem : DriveItemIdnf<DriveItem.IClnbl, DriveItem.Immtbl, DriveItem.Mtbl>
    {
        public interface IClnbl : IDriveItemIdnf
        {
            bool? IsFolder { get; }
            string FileNameExtension { get; }
            bool? IsRootFolder { get; }
            DateTime? CreationTime { get; }
            DateTime? LastAccessTime { get; }
            DateTime? LastWriteTime { get; }
            string CreationTimeStr { get; }
            string LastAccessTimeStr { get; }
            string LastWriteTimeStr { get; }
            OfficeLikeFileType? OfficeLikeFileType { get; }
            bool? IsTextFile { get; }
            bool? IsImageFile { get; }
            bool? IsVideoFile { get; }
            bool? IsAudioFile { get; }
            string TextFileContent { get; }
            byte[] RawFileContent { get; }
            long? SizeBytesCount { get; }
            string WebUrl { get; }

            DriveItemIdnf.IClnbl GetIdnf();

            IClnblCollection GetSubFolders();
            IClnblCollection GetFolderFiles();
        }
    }
}
