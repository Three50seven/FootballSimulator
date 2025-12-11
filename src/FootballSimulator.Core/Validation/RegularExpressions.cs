using System.Text.RegularExpressions;

namespace FootballSimulator
{
    public static class RegularExpressions
    {
        // hrefs must start with "/" or alpha
        public static readonly Regex LinkTrackHref = new Regex(@"^([a-zA-Z]|\/)");

        // check for invalid characters in file name
        public static readonly Regex InvalidFileNameCharacters = new Regex($"[{Regex.Escape(Path.GetInvalidFileNameChars().ToString())}]");

        // folder names can contain alphanumeric and underscores and must have at least one word - they can also have spaces or hyphens (-) in between words
        public static readonly Regex ValidFolderName = new Regex(@"^\w+([\-\ ]\w+)*$");

        public static readonly Regex UrlFriendlyCharacters = new Regex(@"^\w+[\w \-\.~]*$");

        public static readonly Regex AlphaNumeric = new Regex(@"^(?=.*[a-zA-Z])(?=.*[0-9])[A-Za-z0-9]+$");

        public static readonly Regex SingleSpace = new Regex(@"\s");

        public static readonly Regex DoubleSpaces = new Regex(@"\s+");

        public static readonly Regex NumericList = new Regex(@"[^\d\r\n]");

        public static readonly Regex EmailRegex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
