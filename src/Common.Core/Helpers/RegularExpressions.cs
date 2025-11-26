
using System.IO;
using System.Text.RegularExpressions;

namespace Common.Core
{
    public static class RegularExpressions
    {
        public const string SpecialCharacters = "(?:[^a-z0-9 -]|(?<=['\"])s)";
        public const string Email = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        public const string Phone = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
        public const string Zip = @"\d{5}-?(\d{4})?$";
        public const string SSN = @"^\d{9}|\d{3}-\d{2}-\d{4}$";
        public const string FileName = @"^[\w\-. ]+$";
        public const string Url = "^(http|https)://(.*)";

        /// <summary>
        /// Reference: https://stackoverflow.com/a/3652479
        /// Includes .* to the beginning, since RegularExpressionAttribute requires a full string match
        /// </summary>
        public const string YouTubeUrl = @".*youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";

        public const string YouTubeUrlForEmbeded = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";

        public const string GoogleDriveURL = @"^(https:\/\/drive\.google\.com\/)file\/d\/([^\/]+)\/.*$";

        // check for invalid characters in file name
        public static readonly Regex InvalidFileNameCharacters = new Regex($"[{Regex.Escape(Path.GetInvalidFileNameChars().ToString())}]");

        // check for invalid chars in file path / directory
        public static readonly Regex InvalidPathCharacters = new Regex($"[{Regex.Escape(Path.GetInvalidPathChars().ToString())}]");
    }
}
