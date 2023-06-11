using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text;

namespace Turmerik.FileSystem
{
    public interface IFsPathNormalizerOpts
    {
        string Path { get; }
        bool? IsUnixStyle { get; }
        bool AllowStartWithLinkToParent { get; }
        int MaxStartingSlashesAllowed { get; }
    }

    public interface IFsPathNormalizerResult
    {
        string NormalizedPath { get; }
        bool IsValid { get; }
        string ErrorMessage { get; }
        Exception Exception { get; }
        bool IsRooted { get; }
        bool? IsUnixStyle { get; }
        bool? IsAbsUri { get; }
        string AbsUriScheme { get; }
        bool IsNetworkPath { get; }
        bool IsEmpty { get; }
        char? ConsistentlyUsedDirSeparator { get; }
        int StartingSlashesCount { get; }
        IEnumerable<string> GetSegments();
    }

    public class FsPathNormalizerOptsImmtbl : IFsPathNormalizerOpts
    {
        public FsPathNormalizerOptsImmtbl(IFsPathNormalizerOpts src)
        {
            Path = src.Path;
            IsUnixStyle = src.IsUnixStyle;
            AllowStartWithLinkToParent = src.AllowStartWithLinkToParent;
            MaxStartingSlashesAllowed = src.MaxStartingSlashesAllowed;
        }

        public string Path { get; }
        public bool? IsUnixStyle { get; }
        public bool AllowStartWithLinkToParent { get; }
        public int MaxStartingSlashesAllowed { get; }
    }

    public class FsPathNormalizerOptsMtbl : IFsPathNormalizerOpts
    {
        public FsPathNormalizerOptsMtbl()
        {
        }

        public FsPathNormalizerOptsMtbl(IFsPathNormalizerOpts src)
        {
            Path = src.Path;
            IsUnixStyle = src.IsUnixStyle;
            AllowStartWithLinkToParent = src.AllowStartWithLinkToParent;
            MaxStartingSlashesAllowed = src.MaxStartingSlashesAllowed;
        }

        public string Path { get; set; }
        public bool? IsUnixStyle { get; set; }
        public bool AllowStartWithLinkToParent { get; set; }
        public int MaxStartingSlashesAllowed { get; set; }
    }

    public class FsPathNormalizerResultImmtbl : IFsPathNormalizerResult
    {
        public FsPathNormalizerResultImmtbl()
        {
        }

        public FsPathNormalizerResultImmtbl(IFsPathNormalizerResult src)
        {
            NormalizedPath = src.NormalizedPath;
            IsValid = src.IsValid;
            ErrorMessage = src.ErrorMessage;
            Exception = src.Exception;
            IsRooted = src.IsRooted;
            IsUnixStyle = src.IsUnixStyle;
            IsAbsUri = src.IsAbsUri;
            AbsUriScheme = src.AbsUriScheme;
            IsNetworkPath = src.IsNetworkPath;
            IsEmpty = src.IsEmpty;
            ConsistentlyUsedDirSeparator = src.ConsistentlyUsedDirSeparator;
            StartingSlashesCount = src.StartingSlashesCount;
            Segments = GetSegments()?.RdnlC();
        }

        public string NormalizedPath { get; }
        public bool IsValid { get; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
        public bool IsRooted { get; set; }
        public bool? IsUnixStyle { get; }
        public bool? IsAbsUri { get; }
        public string AbsUriScheme { get; }
        public bool IsNetworkPath { get; }
        public bool IsEmpty { get; }
        public char? ConsistentlyUsedDirSeparator { get; }
        public int StartingSlashesCount { get; }
        public ReadOnlyCollection<string> Segments { get; }

        public IEnumerable<string> GetSegments() => Segments;
    }

    public class FsPathNormalizerResultMtbl : IFsPathNormalizerResult
    {
        public FsPathNormalizerResultMtbl()
        {
        }

        public FsPathNormalizerResultMtbl(IFsPathNormalizerResult src)
        {
            NormalizedPath = src.NormalizedPath;
            IsValid = src.IsValid;
            ErrorMessage = src.ErrorMessage;
            Exception = src.Exception;
            IsRooted = src.IsRooted;
            IsUnixStyle = src.IsUnixStyle;
            IsAbsUri = src.IsAbsUri;
            AbsUriScheme = src.AbsUriScheme;
            IsNetworkPath = src.IsNetworkPath;
            IsEmpty = src.IsEmpty;
            ConsistentlyUsedDirSeparator = src.ConsistentlyUsedDirSeparator;
            StartingSlashesCount = src.StartingSlashesCount;
            Segments = GetSegments()?.ToList();
        }

        public string NormalizedPath { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public bool IsRooted { get; set; }
        public bool? IsUnixStyle { get; set; }
        public bool? IsAbsUri { get; set; }
        public string AbsUriScheme { get; set; }
        public bool IsNetworkPath { get; set; }
        public bool IsEmpty { get; set; }
        public char? ConsistentlyUsedDirSeparator { get; set; }
        public int StartingSlashesCount { get; set; }
        public List<string> Segments { get; set; }

        public IEnumerable<string> GetSegments() => Segments;
    }
}
