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

    public class DriveItemIdnf<TClnbl> : ClnblCore
    {
        public new interface IClnblCore : ClnblCore.IClnblCore
        {
            string Id { get; }
            string Name { get; }

            string PrPath { get; }
            string PrRelPath { get; }

            DriveItemIdnf.IClnbl GetPrIdnf();
            DriveItemIdnf.IClnbl GetPrBaseIdnf();

            bool Equals(TClnbl other);
        }
    }

    public class DriveItemIdnf<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : DriveItemIdnf<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public new interface IClnblCore : DriveItemIdnf<TClnbl>.IClnblCore, ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        {
        }

        public class ImmtblCore : ImmtblCoreBase, IClnblCore
        {
            public ImmtblCore(TClnbl src) : base(src)
            {
                Id = src.Id;
                Name = src.Name;
                PrPath = src.PrPath;
                PrRelPath = src.PrRelPath;

                PrIdnf = src.GetPrIdnf().AsImmtbl();
                PrBaseIdnf = src.GetPrBaseIdnf().AsImmtbl();
            }

            public string Id { get; }
            public string Name { get; }

            public string PrPath { get; }
            public string PrRelPath { get; }

            public DriveItemIdnf.Immtbl PrIdnf { get; }
            public DriveItemIdnf.Immtbl PrBaseIdnf { get; }

            public DriveItemIdnf.IClnbl GetPrIdnf() => PrIdnf;
            public DriveItemIdnf.IClnbl GetPrBaseIdnf() => PrBaseIdnf;

            public bool Equals(TClnbl other)
            {
                bool equals = other.Id == Id || other.PrPath == PrPath;
                return equals;
            }

            public override bool Equals(
                object obj) => obj is TClnbl other && Equals(other);
        }

        public class MtblCore : MtblCoreBase, IClnblCore
        {
            public MtblCore(TClnbl src) : base(src)
            {
                Id = src.Id;
                Name = src.Name;
                PrPath = src.PrPath;
                PrRelPath = src.PrRelPath;

                PrIdnf = src.GetPrIdnf().AsMtbl();
                PrBaseIdnf = src.GetPrBaseIdnf().AsMtbl();
            }

            public MtblCore()
            {
            }

            public string Id { get; set; }
            public string Name { get; set; }

            public string PrPath { get; set; }
            public string PrRelPath { get; set; }

            public DriveItemIdnf.Mtbl PrIdnf { get; set; }
            public DriveItemIdnf.Mtbl PrBaseIdnf { get; set; }

            public DriveItemIdnf.IClnbl GetPrIdnf() => PrIdnf;
            public DriveItemIdnf.IClnbl GetPrBaseIdnf() => PrBaseIdnf;

            public bool Equals(TClnbl other)
            {
                bool equals = other.Id == Id || other.PrPath == PrPath;
                return equals;
            }

            public override bool Equals(
                object obj) => obj is TClnbl other && Equals(other);
        }
    }

    public class DriveItemIdnf : DriveItemIdnf<DriveItemIdnf.IClnbl, DriveItemIdnf.Immtbl, DriveItemIdnf.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
        }

        public class Immtbl : ImmtblCore, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : MtblCore, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
            }
        }
    }

    public class DriveItem : DriveItemIdnf<DriveItem.IClnbl, DriveItem.Immtbl, DriveItem.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            string DisplayName { get; }
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

            IClnblCollection GetSubFolders();
            IClnblCollection GetFolderFiles();
        }

        public class Immtbl : ImmtblCore, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                DisplayName = src.DisplayName;
                IsFolder = src.IsFolder;
                FileNameExtension = src.FileNameExtension;
                IsRootFolder = src.IsRootFolder;
                CreationTime = src.CreationTime;
                LastAccessTime = src.LastAccessTime;
                LastWriteTime = src.LastWriteTime;
                CreationTimeStr = src.CreationTimeStr;
                LastAccessTimeStr = src.LastAccessTimeStr;
                LastWriteTimeStr = src.LastWriteTimeStr;
                OfficeLikeFileType = src.OfficeLikeFileType;
                IsTextFile = src.IsTextFile;
                IsImageFile = src.IsImageFile;
                IsVideoFile = src.IsVideoFile;
                IsAudioFile = src.IsAudioFile;
                TextFileContent = src.TextFileContent;
                RawFileContent = src.RawFileContent;
                SizeBytesCount = src.SizeBytesCount;
                WebUrl = src.WebUrl;

                SubFolders = src.GetSubFolders().AsImmtblCllctn();
                FolderFiles = src.GetFolderFiles().AsImmtblCllctn();
            }

            public string DisplayName { get; }
            public bool? IsFolder { get; }
            public string FileNameExtension { get; }
            public bool? IsRootFolder { get; }
            public DateTime? CreationTime { get; }
            public DateTime? LastAccessTime { get; }
            public DateTime? LastWriteTime { get; }
            public string CreationTimeStr { get; }
            public string LastAccessTimeStr { get; }
            public string LastWriteTimeStr { get; }
            public OfficeLikeFileType? OfficeLikeFileType { get; }
            public bool? IsTextFile { get; }
            public bool? IsImageFile { get; }
            public bool? IsVideoFile { get; }
            public bool? IsAudioFile { get; }
            public string TextFileContent { get; }
            public byte[] RawFileContent { get; }
            public long? SizeBytesCount { get; }
            public string WebUrl { get; }

            public ImmtblCollection SubFolders { get; }
            public ImmtblCollection FolderFiles { get; }

            public IClnblCollection GetFolderFiles() => FolderFiles;
            public IClnblCollection GetSubFolders() => SubFolders;
        }

        public class Mtbl : MtblCore, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                DisplayName = src.DisplayName;
                IsFolder = src.IsFolder;
                FileNameExtension = src.FileNameExtension;
                IsRootFolder = src.IsRootFolder;
                CreationTime = src.CreationTime;
                LastAccessTime = src.LastAccessTime;
                LastWriteTime = src.LastWriteTime;
                CreationTimeStr = src.CreationTimeStr;
                LastAccessTimeStr = src.LastAccessTimeStr;
                LastWriteTimeStr = src.LastWriteTimeStr;
                OfficeLikeFileType = src.OfficeLikeFileType;
                IsTextFile = src.IsTextFile;
                IsImageFile = src.IsImageFile;
                IsVideoFile = src.IsVideoFile;
                IsAudioFile = src.IsAudioFile;
                TextFileContent = src.TextFileContent;
                RawFileContent = src.RawFileContent;
                SizeBytesCount = src.SizeBytesCount;
                WebUrl = src.WebUrl;

                SubFolders = src.GetSubFolders().AsMtblList();
                FolderFiles = src.GetFolderFiles().AsMtblList();
            }

            public string DisplayName { get; set; }
            public bool? IsFolder { get; set; }
            public string FileNameExtension { get; set; }
            public bool? IsRootFolder { get; set; }
            public DateTime? CreationTime { get; set; }
            public DateTime? LastAccessTime { get; set; }
            public DateTime? LastWriteTime { get; set; }
            public string CreationTimeStr { get; set; }
            public string LastAccessTimeStr { get; set; }
            public string LastWriteTimeStr { get; set; }
            public OfficeLikeFileType? OfficeLikeFileType { get; set; }
            public bool? IsTextFile { get; set; }
            public bool? IsImageFile { get; set; }
            public bool? IsVideoFile { get; set; }
            public bool? IsAudioFile { get; set; }
            public string TextFileContent { get; set; }
            public byte[] RawFileContent { get; set; }
            public long? SizeBytesCount { get; set; }
            public string WebUrl { get; set; }

            public MtblList SubFolders { get; set; }
            public MtblList FolderFiles { get; set; }

            public IClnblCollection GetFolderFiles() => FolderFiles;
            public IClnblCollection GetSubFolders() => SubFolders;
        }
    }
}
