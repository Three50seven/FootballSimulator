using Common.Core.Validation;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Common.Core.Services
{
    public static class PathHelper
    {
        public static string GetLowerExtension(string filename)
        {
            Guard.IsNotNull(filename, nameof(filename));
            return Path.GetExtension(filename).Replace(".", "").ToLower();
        }

        public static string GetDirectoryPath(string path)
        {
            return GetDirectoryPath(path, IsFileSystemPath(path));
        }

        public static string GetDirectoryPath(string path, bool fileSystem, bool appendSlash = false)
        {
            return GetDirectoryPathInternal(path, fileSystem, appendSlash);
        }

        private static string GetDirectoryPathInternal(string path, bool fileSystem, bool appendSlash)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            DetermineCharsToAdjust(fileSystem, out char slashToReplace, out char newSlash);

            path = Path.GetDirectoryName(path).Replace(slashToReplace, newSlash);

            if (appendSlash && !path.EndsWith(newSlash.ToString()))
                path = string.Concat(path, newSlash);

            return path;
        }

        public static string GetTopDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            DetermineCharsToAdjust(true, out char slashToReplace, out char newSlash);

            path = path.Replace(slashToReplace, newSlash);

            if (path.StartsWith(newSlash.ToString()))
                path = path.Substring(1);
            else if (path.Length > 2 && path[1] == ':' && path[2] == '\\')
                path = path.Substring(3);

            return path.Split(newSlash)?[0] ?? path;
        }

        public static string Combine(params string[] paths)
        {
            bool isFileSystem = paths?.Any(path => IsFileSystemPath(path)) ?? false;
            return Combine(fileSystem: isFileSystem, paths: paths);
        }

        public static string Combine(bool fileSystem, params string[] paths)
        {
            return CombineInternal(fileSystem, paths);
        }

        private static string CombineInternal(bool fileSystem, params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                throw new ArgumentNullException(nameof(paths));

            DetermineCharsToAdjust(fileSystem, out char slashToReplace, out char newSlash);

            return Path.Combine(paths).Replace(slashToReplace, newSlash);
        }

        private static void DetermineCharsToAdjust(bool fileSystem, out char slashToReplace, out char newSlash)
        {
            // NOTE: DirectorySeparatorChar = '\\'
            //       AltDirectorySeparatorChar  = '/'

            slashToReplace = fileSystem ? Path.AltDirectorySeparatorChar : Path.DirectorySeparatorChar;
            newSlash = fileSystem ? Path.DirectorySeparatorChar : Path.AltDirectorySeparatorChar;
        }

        private static bool IsFileSystemPath(string path)
        {
            return path?.Contains(Path.DirectorySeparatorChar.ToString()) ?? false;
        }

        // Ref - https://stackoverflow.com/a/35218619
        public static string GetAbsolutePath(string relativePath, string basePath)
        {
            if (relativePath == null)
                return null;

            relativePath = CleanForAbsolutePath(relativePath);

            if (basePath == null)
                basePath = Path.GetFullPath("."); // quick way of getting current working directory
            else
                basePath = GetAbsolutePath(basePath, null); // to be REALLY sure

            string path;
            // specific for windows paths starting on \ - they need the drive added to them.
            if (!Path.IsPathRooted(relativePath) || "\\".Equals(Path.GetPathRoot(relativePath)))
            {
                if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                    path = Path.Combine(basePath, relativePath.TrimStart(Path.DirectorySeparatorChar));
                else
                    path = Path.Combine(basePath, relativePath);
            }
            else
                path = relativePath;

            // resolves any internal "..\" to get the true full path.
            return Path.GetFullPath(path);
        }

        private static string CleanForAbsolutePath(string path)
        {
            return path.SetNullToEmpty()?.Replace("/", "\\");
        }

        public static string EncodeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            fileName = Path.GetFileName(fileName);
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            return WebUtility.HtmlEncode(fileName);
        }

        public static string SetupAbsoluteFilePath(string baseDirectory, params string[] paths)
        {
            string path = Combine(paths);
            path = GetAbsolutePath(path, baseDirectory);
            Directory.CreateDirectory(GetDirectoryPath(path));
            return path;
        }

        public static bool IsAbsolutePath(string path)
        {
            // Ref - https://stackoverflow.com/a/47569899/9882811

            if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 || !Path.IsPathRooted(path))
                return false;

            string pathRoot = Path.GetPathRoot(path);
            if (pathRoot.Length <= 2 && pathRoot != "/") // Accepts X:\ and \\UNC\PATH, rejects empty string, \ and X:, but accepts / to support Linux
                return false;

            if (pathRoot[0] != '\\' || pathRoot[1] != '\\')
                return true; // Rooted and not a UNC path

            return pathRoot.Trim('\\').IndexOf('\\') != -1; // A UNC server name without a share name (e.g "\\NAME" or "\\NAME\") is invalid
        }

        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            path = path.Trim();

            // if has trailing slash then it's a directory
            if (new[] { "\\", "/" }.Any(x => path.EndsWith(x)))
                return true;

            // if has extension then it's a file
            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }

        public static string GetUniqueFileName(string fileName, int numCharsToAdd = 10)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            if (numCharsToAdd <= 0)
                numCharsToAdd = 10;

            string uniqueId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                "_",
                uniqueId.Substring(uniqueId.Length - numCharsToAdd),
                Path.GetExtension(fileName));
        }
    }
}
