using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text;

namespace Turmerik.FileSystem
{
    public interface IFsPathNormalizer
    {
        IFsPathNormalizerResult TryNormalizePath(IFsPathNormalizerOpts opts);

        IFsPathNormalizerResult TryNormalizePath(
            string path,
            bool? isUnixStyle = null,
            bool allowStartWithLinkToParent = false,
            int maxStartingSlashesAllowed = 2);
    }

    public class FsPathNormalizer : IFsPathNormalizer
    {
        private const string NULL_INPUT_PATH_ERR_MSG = "Provided path cannot be null";
        private const string ILLEGAL_FILE_NAME_CHARS_ERR_MSG = "At least one of the provided path's segments contains invalid characters";
        private const string SURROUNDING_WHITE_SPACES_ERR_MSG = "Spaces are only allowed inside path segments";
        private const string INVALID_WHITE_SPACES_ERR_MSG = "At least one of the provided path's segments contains invalid white space characters";
        private const string EMPTY_SEGMENTS_ERR_MSG = "Empty segments not allowed";
        private const string SEGMENT_ENDING_IN_DOT_ERR_MSG = "Path segments cannot end with dot";
        private const string SEGMENT_CONTAINING_DOTS_NEAR_WHITE_SPACES_ERR_MSG = "Path segments cannot have dots preceded or succeeded by white space";
        private const string GO_TO_PARENT_LINK_NOT_ALLOWED_ERR_MSG = "Go to parent link segment is not allowed";
        private const string SEGMENT_CONTAINING_TOO_MANY_DOTS_ERR_MSG = "Consecutive dots is only allowed when an the path segment consists of the go to parent link (..)";

        public static readonly Regex InvalidFileNameWsBfDotRegex = new Regex(@"\s\.");
        public static readonly Regex InvalidFileNameWsAfDotRegex = new Regex(@"\.\s");

        public IFsPathNormalizerResult TryNormalizePath(IFsPathNormalizerOpts opts)
        {
            var d = new TempData(opts);
            var immtbl = TryNormalizePath(d);

            return immtbl;
        }

        public IFsPathNormalizerResult TryNormalizePath(
            string path,
            bool? isUnixStyle = null,
            bool allowStartWithLinkToParent = false,
            int maxStartingSlashesAllowed = 2)
        {
            var opts = new FsPathNormalizerOptsMtbl
            {
                Path = path,
                IsUnixStyle = isUnixStyle,
                AllowStartWithLinkToParent = allowStartWithLinkToParent,
                MaxStartingSlashesAllowed = maxStartingSlashesAllowed
            };

            var immtbl = TryNormalizePath(opts);
            return immtbl;
        }

        private IFsPathNormalizerResult TryNormalizePath(TempData d)
        {
            var o = d.Opts;
            string path = o.Path;

            char[] sepsArr = d.PathDirSeparatorChars;
            bool pathIsNotNull = path != null;

            var mtbl = d.Mtbl;

            mtbl.IsEmpty = pathIsNotNull ? string.IsNullOrEmpty(path) : false;
            mtbl.IsValid = true;

            try
            {
                if (Validate(mtbl, pathIsNotNull, NULL_INPUT_PATH_ERR_MSG))
                {
                    ValidateAgainstSurroundingWhiteSpaces(mtbl, ref path);
                }

                if (pathIsNotNull && !mtbl.IsEmpty)
                {
                    int idx = path.IndexOfAny(sepsArr);

                    if (idx > 0)
                    {
                        string firstPart = path.Substring(0, idx);
                        string firstPartTrimmed = firstPart.Trim();

                        string rest = path.Substring(idx);
                        mtbl.ConsistentlyUsedDirSeparator = default(char);

                        if (rest.First() == '/')
                        {
                            if (!rest.Contains('\\'))
                            {
                                mtbl.ConsistentlyUsedDirSeparator = '/';
                            }

                            if (firstPartTrimmed.LastOrDefault() == ':' && rest.StartsWith("//"))
                            {
                                mtbl.IsAbsUri = true;
                                mtbl.AbsUriScheme = firstPartTrimmed.SubStr(0, -1);

                                ValidateAgainstSurroundingWhiteSpaces(mtbl, firstPart, firstPartTrimmed);
                                ValidateAgainstInvalidFileNameChars(mtbl, firstPartTrimmed);

                                path = rest.Substring(2); // Gets rid of the starting "//" from the URI scheme
                            }
                        }
                        else // rest.First() == '\\'
                        {
                            if (!rest.Contains('/'))
                            {
                                mtbl.ConsistentlyUsedDirSeparator = '\\';
                            }
                        }
                    }

                    TryNormalizePathCore(d);

                    mtbl.NormalizedPath = string.Join(
                        Path.DirectorySeparatorChar.ToString(),
                        d.SegmentsList.ToArray());

                    if (mtbl.IsUnixStyle == true && mtbl.IsRooted)
                    {
                        mtbl.NormalizedPath = string.Concat(Path.DirectorySeparatorChar, mtbl.NormalizedPath);
                    }

                    if (mtbl.IsNetworkPath)
                    {
                        mtbl.NormalizedPath = string.Concat(Path.DirectorySeparatorChar, mtbl.NormalizedPath);
                    }
                }
                else if (mtbl.IsEmpty)
                {
                    mtbl.NormalizedPath = string.Empty;
                }
            }
            catch (Exception exc)
            {
                mtbl.IsValid = false;
                mtbl.Exception = exc;
            }

            var immtbl = new FsPathNormalizerResultImmtbl(mtbl);
            return immtbl;
        }

        private void TryNormalizePathCore(
            TempData d)
        {
            var o = d.Opts;
            var mtbl = d.Mtbl;

            char[] invalidFileNameChars = PathH.InvalidFileNameChars.ToArray();
            string[] pathPartsArr = d.PathSegments = o.Path.Split(d.PathDirSeparatorChars);

            int pathPartsLen = pathPartsArr.Length;
            int maxIdx = pathPartsLen - 1;

            bool isStart = true;
            int stEmptySegmentsCount = 0;

            int levels = 0;
            var segmentsList = d.SegmentsList;

            for (int i = 0; i < pathPartsLen; i++)
            {
                var segment = pathPartsArr[i];
                var segLen = segment.Length;

                ValidateAgainstSurroundingWhiteSpaces(mtbl, ref segment);

                if (string.IsNullOrEmpty(segment))
                {
                    bool isLastSegment = i == maxIdx;

                    if (!isLastSegment)
                    {
                        stEmptySegmentsCount++;
                    }

                    Validate(mtbl,
                        (isStart || isLastSegment) &&
                        stEmptySegmentsCount <= o.MaxStartingSlashesAllowed, EMPTY_SEGMENTS_ERR_MSG);
                }
                else
                {
                    isStart = false;
                    int cIdx = segment.IndexOfAny(invalidFileNameChars);

                    if (cIdx >= 0)
                    {
                        if (Validate(mtbl,
                            IsWinDriveLetterSegment(
                            o.IsUnixStyle, i == 0,
                            cIdx, segment, segLen),
                            ILLEGAL_FILE_NAME_CHARS_ERR_MSG))
                        {
                            mtbl.IsRooted = true;
                            mtbl.IsUnixStyle = false;

                            levels++;
                            segmentsList.Add(segment);
                        }
                    }
                    else
                    {
                        ValidateAgainstInvalidWhiteSpaces(mtbl, segment);
                        ValidateAgainstDotsNearWhiteSpaces(mtbl, segment);

                        int dotsCount;

                        if (ContainsOnlyDots(segment, out dotsCount))
                        {
                            if (dotsCount == 2)
                            {
                                if (Validate(mtbl,
                                    (!isStart || o.AllowStartWithLinkToParent) && (levels > 1 || levels > 0 && !mtbl.IsRooted),
                                    GO_TO_PARENT_LINK_NOT_ALLOWED_ERR_MSG))
                                {
                                    segmentsList.RemoveAtIdx(-1);
                                    levels--;
                                }
                            }
                            else
                            {
                                Validate(mtbl,
                                    dotsCount == 1,
                                    SEGMENT_CONTAINING_TOO_MANY_DOTS_ERR_MSG);
                            }
                        }
                        else
                        {
                            Validate(mtbl, segment.Last() != '.', SEGMENT_ENDING_IN_DOT_ERR_MSG);

                            levels++;
                            segmentsList.Add(segment);
                        }
                    }
                }
            }

            if (mtbl.IsUnixStyle != false && (mtbl.IsRooted = stEmptySegmentsCount > 0))
            {
                mtbl.IsUnixStyle = true;
                mtbl.IsNetworkPath = stEmptySegmentsCount > 1;
            }

            mtbl.Segments = segmentsList;
            mtbl.StartingSlashesCount = stEmptySegmentsCount;
        }

        private bool ContainsOnlyDots(string segment, out int dotsCount)
        {
            int dotsCountVal = 0;

            bool containsOnlyDots = segment.All((c, cI) =>
            {
                bool retVal = c == '.';

                if (retVal)
                {
                    dotsCountVal++;
                }
                else
                {
                    retVal = c == ' ';
                }

                return retVal;
            });

            dotsCount = dotsCountVal;
            return containsOnlyDots;
        }

        private bool IsWinDriveLetterSegment(
            bool? isUnixStyle,
            bool isFirstLine,
            int cIdx,
            string pathPart,
            int partLen)
        {
            bool retVal = isFirstLine && isUnixStyle != true && cIdx == 1 && partLen == 2 && char.IsLetter(pathPart.First());
            return retVal;
        }

        private bool Validate(FsPathNormalizerResultMtbl mtbl, bool isValid, string invalidMessage)
        {
            if (!isValid && mtbl.IsValid)
            {
                mtbl.IsValid = false;
                mtbl.ErrorMessage = invalidMessage;
            }

            return isValid;
        }

        private bool ValidateAgainstInvalidWhiteSpaces(FsPathNormalizerResultMtbl mtbl, string segment)
        {
            bool retVal = Validate(mtbl,
                segment.AllWhiteSpacesAre(),
                INVALID_WHITE_SPACES_ERR_MSG);

            return retVal;
        }

        private bool ValidateAgainstSurroundingWhiteSpaces(FsPathNormalizerResultMtbl mtbl, ref string segment)
        {
            string segmentTrimmed = segment.Trim();
            bool retVal = ValidateAgainstSurroundingWhiteSpaces(mtbl, segment, segmentTrimmed);

            segment = segmentTrimmed;
            return retVal;
        }

        private bool ValidateAgainstSurroundingWhiteSpaces(FsPathNormalizerResultMtbl mtbl, string segment, string trimmedSegment = null)
        {
            trimmedSegment = trimmedSegment ?? segment.Trim();

            bool retVal = Validate(mtbl,
                segment == trimmedSegment,
                SURROUNDING_WHITE_SPACES_ERR_MSG);

            return retVal;
        }

        private bool ValidateAgainstInvalidFileNameChars(FsPathNormalizerResultMtbl mtbl, string segment)
        {
            bool retVal = Validate(mtbl,
                !segment.ContainsAny(PathH.InvalidFileNameChars),
                ILLEGAL_FILE_NAME_CHARS_ERR_MSG);

            return retVal;
        }

        private bool ValidateAgainstDotsNearWhiteSpaces(FsPathNormalizerResultMtbl mtbl, string segment)
        {
            bool retVal = Validate(mtbl,
                !(InvalidFileNameWsBfDotRegex.IsMatch(segment) || InvalidFileNameWsAfDotRegex.IsMatch(segment)),
                SEGMENT_CONTAINING_DOTS_NEAR_WHITE_SPACES_ERR_MSG);

            return retVal;
        }

        private class TempData
        {
            public TempData(
                IFsPathNormalizerOpts opts) : this(
                new FsPathNormalizerOptsImmtbl(opts))
            {
            }

            public TempData(FsPathNormalizerOptsImmtbl opts)
            {
                Opts = opts ?? throw new ArgumentNullException(nameof(opts));
                Mtbl = new FsPathNormalizerResultMtbl();
                SegmentsList = new List<string>();
            }

            public char[] PathDirSeparatorChars = new char[] { '\\', '/' };
            public FsPathNormalizerOptsImmtbl Opts { get; }
            public FsPathNormalizerResultMtbl Mtbl { get; }
            public List<string> SegmentsList { get; }
            public string[] PathSegments { get; set; }
        }
    }
}
