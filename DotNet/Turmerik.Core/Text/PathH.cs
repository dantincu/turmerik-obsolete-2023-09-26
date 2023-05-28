using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Collections;

namespace Turmerik.Core.Text
{
    public static class PathH
    {
        public static readonly ReadOnlyCollection<char> InvalidPathChars;
        public static readonly ReadOnlyCollection<char> InvalidFileNameChars;
        public static readonly ReadOnlyCollection<char> DirSeparatorChars;

        static PathH()
        {
            InvalidPathChars = Path.GetInvalidPathChars().RdnlC();
            InvalidFileNameChars = Path.GetInvalidFileNameChars().RdnlC();
            DirSeparatorChars = new char[] { '\\', '/' }.RdnlC();
        }

        public static bool ContainsInvalidPathChars(
            string path,
            bool isFileName = false,
            bool throwIfInvalid = false)
        {
            var invalidPathChars = GetInvalidPathChars(isFileName);
            bool retVal = path.ContainsAny(invalidPathChars);

            if (!retVal && throwIfInvalid)
            {
                char[] contained = path.GetContained(invalidPathChars);
                string containedStr = string.Join(", ", contained);

                throw new ArgumentException(
                    $"Provided path {path} contains the following invalid path chars: {containedStr}");
            }

            return retVal;
        }

        public static string ReplaceInvalidPathChars(
            string path,
            bool isFileName = false,
            Func<char, char> replaceFactory = null)
        {
            var invalidPathChars = GetInvalidPathChars(isFileName);

            string retPath = path.ReplaceChars(
                replaceFactory,
                invalidPathChars);

            return retPath;
        }

        public static ReadOnlyCollection<char> GetInvalidPathChars(
            bool isFileName)
        {
            ReadOnlyCollection<char> invalidPathChars;

            if (isFileName)
            {
                invalidPathChars = InvalidPathChars;
            }
            else
            {
                invalidPathChars = InvalidFileNameChars;
            }

            return invalidPathChars;
        }

        public static string GetDirName(string path)
        {
            path = path?.TrimEnd('/', '\\') ?? string.Empty;
            string dirName = null;

            if (path.LastOrDefault() == ':')
            {
                dirName = path ?? string.Empty;
            }
            else
            {
                dirName = Path.GetFileName(path);
            }

            return dirName;
        }

        public static string GetDirPath(string path)
        {
            path = path?.TrimEnd('/', '\\') ?? string.Empty;
            string dirPath = string.Empty;

            if (path.LastOrDefault() != ':')
            {
                dirPath = Path.GetDirectoryName(path);
            }

            return dirPath;
        }
    }
}
