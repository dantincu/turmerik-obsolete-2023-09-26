using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.DriveExplorerCore
{
    public enum OfficeLikeFileType
    {
        Docs = 1,
        Sheets,
        Slides
    }

    public static class DriveItemIdnf
    {
        public interface IClnbl
        {
            string Id { get; }
            string Name { get; }

            string PrPath { get; }

            IClnbl GetPrIdnf();

            bool Equals(IClnbl other);
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                Id = src.Id;
                Name = src.Name;
                PrPath = src.PrPath;
                PrIdnf = src.GetPrIdnf().AsImmtbl();
            }

            public string Id { get; }
            public string Name { get; }

            public string PrPath { get; }

            public Immtbl PrIdnf { get; }

            public IClnbl GetPrIdnf() => PrIdnf;

            public bool Equals(IClnbl other) => AreEqual(this, other);

            public override bool Equals(
                object obj) => obj is IClnbl other && Equals(other);
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                Id = src.Id;
                Name = src.Name;
                PrPath = src.PrPath;
                PrIdnf = src.GetPrIdnf().AsMtbl();
            }

            public string Id { get; set; }
            public string Name { get; set; }

            public string PrPath { get; set; }

            public Mtbl PrIdnf { get; set; }

            public IClnbl GetPrIdnf() => PrIdnf;

            public bool Equals(IClnbl other) => AreEqual(this, other);

            public override bool Equals(
                object obj) => obj is IClnbl other && Equals(other);
        }

        public static bool AreEqual(
            IClnbl left,
            IClnbl right)
        {
            bool equals = left.Id == right.Id && left.Name == right.Name && left.PrPath == right.PrPath;

            if (equals)
            {
                var leftPrIdnf = left.GetPrIdnf();
                var rightPrIdnf = right.GetPrIdnf();

                equals = (leftPrIdnf == null) == (rightPrIdnf == null);

                if (equals && leftPrIdnf != null)
                {
                    equals = leftPrIdnf.Equals(rightPrIdnf);
                }
            }

            return equals;
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => src as Immtbl ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => src as Mtbl ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) =>
            src as ReadOnlyCollection<Immtbl> ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => src as List<Mtbl> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as ReadOnlyDictionary<TKey, Immtbl> ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as Dictionary<TKey, Mtbl> ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this Dictionary<TKey, Mtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this ReadOnlyDictionary<TKey, Immtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());
    }

    public static class DriveItem
    {
        public interface IClnbl : DriveItemIdnf.IClnbl
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

            IEnumerable<IClnbl> GetSubFolders();
            IEnumerable<IClnbl> GetFolderFiles();
        }

        public class Immtbl : DriveItemIdnf.Immtbl, IClnbl
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

            public ReadOnlyCollection<Immtbl> SubFolders { get; }
            public ReadOnlyCollection<Immtbl> FolderFiles { get; }

            public IEnumerable<IClnbl> GetFolderFiles() => FolderFiles;
            public IEnumerable<IClnbl> GetSubFolders() => SubFolders;
        }

        public class Mtbl : DriveItemIdnf.Mtbl, IClnbl
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

            public List<Mtbl> SubFolders { get; set; }
            public List<Mtbl> FolderFiles { get; set; }

            public IEnumerable<IClnbl> GetFolderFiles() => FolderFiles;
            public IEnumerable<IClnbl> GetSubFolders() => SubFolders;
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => src as Immtbl ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => src as Mtbl ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) =>
            src as ReadOnlyCollection<Immtbl> ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => src as List<Mtbl> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as ReadOnlyDictionary<TKey, Immtbl> ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as Dictionary<TKey, Mtbl> ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this Dictionary<TKey, Mtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this ReadOnlyDictionary<TKey, Immtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());
    }
}
