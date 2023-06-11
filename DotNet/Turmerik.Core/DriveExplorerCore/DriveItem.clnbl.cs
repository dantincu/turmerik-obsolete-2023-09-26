using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.DriveExplorerCore
{
    public enum OfficeLikeFileType
    {
        Docs = 1,
        Sheets,
        Slides
    }
    public interface IDriveItemIdnf
    {
        string Id { get; }
        string Name { get; }

        string PrPath { get; }
        string PrRelPath { get; }

        IDriveItemIdnf GetPrIdnf();
        IDriveItemIdnf GetPrBaseIdnf();

        bool Equals(IDriveItemIdnf other);
    }

    public interface IDriveItem
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

        IDriveItemIdnf GetIdnf();

        IDriveItem[] GetSubFolders();
        IDriveItem[] GetFolderFiles();
    }

    public static class DriveItemIdnf
    {
        public static DriveItemIdnfImmtbl ToImmtbl(this IDriveItemIdnf src) => new DriveItemIdnfImmtbl(src);
        public static DriveItemIdnfMtbl ToMtbl(this IDriveItemIdnf src) => new DriveItemIdnfMtbl(src);

        public static string GetFullPathRecursively(
            this IDriveItemIdnf idnf) => Path.Combine(
                idnf.PrPath ?? idnf.GetPrIdnf()?.GetFullPathRecursively(),
                idnf.Name);

        public static string GetFullPath(
            this IDriveItemIdnf idnf) => Path.Combine(
                idnf.PrPath ?? idnf.GetPrIdnf()?.GetPath(),
                idnf.Name);

        public static string GetPath(
            this IDriveItemIdnf idnf) => Path.Combine(
            idnf.PrPath,
            idnf.Name);
    }

    public class DriveItemIdnfImmtbl : IDriveItemIdnf
    {
        public DriveItemIdnfImmtbl(IDriveItemIdnf src)
        {
            Id = src.Id;
            Name = src.Name;

            PrPath = src.PrPath;

            PrRelPath = src.PrRelPath;

            PrIdnf = src.GetPrIdnf()?.ToImmtbl();
            PrBaseIdnf = src.GetPrBaseIdnf()?.ToImmtbl();
        }

        public DriveItemIdnfImmtbl()
        {
        }

        public string Id { get; }
        public string Name { get; }

        public string PrPath { get; }
        public string PrRelPath { get; }

        public DriveItemIdnfImmtbl PrIdnf { get; }
        public DriveItemIdnfImmtbl PrBaseIdnf { get; }

        public IDriveItemIdnf GetPrIdnf() => PrIdnf;
        public IDriveItemIdnf GetPrBaseIdnf() => PrBaseIdnf;

        public bool Equals(IDriveItemIdnf other)
        {
            bool equals = other.Id == Id || other.PrPath == PrPath;
            return equals;
        }

        public override bool Equals(object obj) => obj is DriveItemIdnfImmtbl other && Equals(other);
    }

    public class DriveItemIdnfMtbl : IDriveItemIdnf
    {
        public DriveItemIdnfMtbl(IDriveItemIdnf src)
        {
            Id = src.Id;
            Name = src.Name;

            PrPath = src.PrPath;

            PrRelPath = src.PrRelPath;

            PrIdnf = src.GetPrIdnf()?.ToMtbl();
            PrBaseIdnf = src.GetPrBaseIdnf()?.ToMtbl();
        }

        public DriveItemIdnfMtbl()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string PrPath { get; set; }
        public string PrRelPath { get; set; }

        public DriveItemIdnfMtbl PrIdnf { get; set; }
        public DriveItemIdnfMtbl PrBaseIdnf { get; set; }

        public IDriveItemIdnf GetPrIdnf() => PrIdnf;
        public IDriveItemIdnf GetPrBaseIdnf() => PrBaseIdnf;

        public bool Equals(IDriveItemIdnf other)
        {
            bool equals = other.Id == Id || other.PrPath == PrPath;
            return equals;
        }

        public override bool Equals(object obj) => obj is DriveItemIdnfImmtbl other && Equals(other);
    }

    public class DriveItemImmtbl : IDriveItem
    {
        public DriveItemImmtbl(IDriveItem src)
        {
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

            Idnf = src.GetIdnf()?.ToImmtbl();

            SubFolders = src.GetSubFolders()?.Select(
                item => new DriveItemImmtbl(item)).RdnlC();

            FolderFiles = src.GetFolderFiles()?.Select(
                item => new DriveItemImmtbl(item)).RdnlC();
        }

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

        public DriveItemIdnfImmtbl Idnf { get; }
        public ReadOnlyCollection<DriveItemImmtbl> SubFolders { get; }
        public ReadOnlyCollection<DriveItemImmtbl> FolderFiles { get; }

        public IDriveItemIdnf GetIdnf() => Idnf;

        public IDriveItem[] GetFolderFiles() => FolderFiles?.Cast<IDriveItem>().ToArray();
        public IDriveItem[] GetSubFolders() => SubFolders?.Cast<IDriveItem>().ToArray();
    }

    public class DriveItemMtbl : IDriveItem
    {
        public DriveItemMtbl()
        {
        }

        public DriveItemMtbl(IDriveItem src)
        {
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

            Idnf = src.GetIdnf()?.ToMtbl();

            SubFolders = src.GetSubFolders()?.Select(
                item => new DriveItemMtbl(item)).ToList();

            FolderFiles = src.GetFolderFiles()?.Select(
                item => new DriveItemMtbl(item)).ToList();
        }

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

        public DriveItemIdnfMtbl Idnf { get; set; }
        public List<DriveItemMtbl> SubFolders { get; set; }
        public List<DriveItemMtbl> FolderFiles { get; set; }

        public IDriveItemIdnf GetIdnf() => Idnf;

        public IDriveItem[] GetFolderFiles() => FolderFiles?.Cast<IDriveItem>().ToArray();
        public IDriveItem[] GetSubFolders() => SubFolders?.Cast<IDriveItem>().ToArray();
    }
}
