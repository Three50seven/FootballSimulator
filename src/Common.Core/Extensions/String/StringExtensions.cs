using System.Text;
using System.Text.RegularExpressions;

namespace Common.Core
{
    public static class StringExtensions
    {
        private static readonly string _directorySeparator = Path.DirectorySeparatorChar.ToString();
        private static readonly string _directorySeparatorAlt = Path.AltDirectorySeparatorChar.ToString();

        public static string EncodedNewLine = "&#xD;&#xA;";
        
        /// <summary>
        /// Returns snake_case version of provided string.
        /// Consider using a 3rd party library like Humanizer instead of this extension method.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lower">Whether to lower the string value.</param>
        /// <returns></returns>
        public static string ToSnakeCase(this string value, bool lower = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            value = string.Concat(
                Regex.Replace(value.Trim(), @"\s+", "_")
                .Select((x, i) =>

                    (i > 0 && char.IsUpper(x)) ? string.Concat("_" + x.ToString()) : x.ToString())

                ).Replace("__", "_");

            return lower ? value.ToLower() : value;
        }

        /// <summary>
        /// Find any individual words from a given string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lower">Whether resulting words in list should be lowered.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetWords(this string? text, bool lower = false)
        {
            text = text?.SetEmptyToNull();
            if (string.IsNullOrWhiteSpace(text))
                return new string[] { };
            return (lower ? text.ToLower() : text).Split(' ');
        }

        /// <summary>
        /// Returns null if string is empty, null, or whitespace.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trim">Whether result should be trimmed. Defaults to true.</param>
        /// <returns></returns>
        public static string? SetEmptyToNull(this string value, bool trim = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return trim ? value.Trim() : value;
        }

        /// <summary>
        /// Returns empty if string is null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trim">Whether result should be trimmed. Defaults to true.</param>
        /// <returns></returns>
        public static string SetNullToEmpty(this string value, bool trim = true)
        {
            if (value == null)
                return string.Empty;
            else
                return trim ? value.Trim() : value;
        }

        /// <summary>
        /// Remove special characters defined under <see cref="RegularExpressions.SpecialCharacters"/>
        /// from a given string input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="customRegEx">Optionally provide a custom RegEx.</param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string input, string customRegEx = null)
        {
            Regex r = new Regex(customRegEx == null ? RegularExpressions.SpecialCharacters : customRegEx, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }

        /// <summary>
        /// Remove area of a string builder where first indexes of start and end string values are found.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void RemoveArea(this StringBuilder sb, string start, string end)
        {
            int startplaceholderindex = sb.ToString().IndexOf(start);
            int endplaceholderindex = sb.ToString().IndexOf(end);
            sb.Remove(startplaceholderindex, endplaceholderindex - startplaceholderindex + end.Length);
        }

        /// <summary>
        /// Attempts to pluralize a given string value under English.
        /// Consider using a 3rd party library like Humanizer over this extension method.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPlural(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.EndsWith("es"))
                return value;
            if (value.EndsWith("y") && !value.EndsWith("ey"))
                return string.Concat(value.Remove(value.Length - 1), "ies");
            if (value.EndsWith("s"))
                return string.Concat(value, "es");

            return string.Concat(value, "s");
        }

        /// <summary>
        /// Format a string to singular or plural based on count/int value supplied for <paramref name="num"/>.
        /// If num equals 1, the string value is returned unchanged. Else, <see cref="ToPlural(string)"/> is returned.
        /// Optionally supplied the plural suffix/ending for the plural condition using <paramref name="pluralEnding"/>.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <param name="num">Number value to determine singular/plural.</param>
        /// <param name="pluralEnding">Optional plural suffix/ending for the plural condition.</param>
        /// <returns></returns>
        public static string FormatSingularOrPlural(this string value, int num, string pluralEnding = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return num == 1 ? value : string.IsNullOrWhiteSpace(pluralEnding) ? value.ToPlural() : value + pluralEnding;
        }

        /// <summary>
        /// Format a given string as a more url-friendly result. 
        /// Removes any non-alphanumeric characters and replaces spaces spaces with hyphens.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUrlFriendlyString(this string value)
        {
            value = value.SetNullToEmpty(trim: false);
            if (string.IsNullOrEmpty(value))
                return value;

            string friendlyValue = value.Replace("®", string.Empty).Replace("™", string.Empty)
                                    .RemoveAccent()
                                    .ToLower(); //clean and lower

            friendlyValue = friendlyValue.Replace("-", " "); // replace existing hypens with a space
            friendlyValue = Regex.Replace(friendlyValue, @"[^a-z0-9\s-_]", ""); // invalid chars  
            friendlyValue = Regex.Replace(friendlyValue, @"\s+", " ").Trim(); // convert multiple spaces into one space 
            friendlyValue = Regex.Replace(friendlyValue, @"\s", "-"); // hyphens   

            return friendlyValue;
        }

        private static string RemoveAccent(this string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Helper extension method to check and format a path to properly end with the correct
        /// directory slash separator.
        /// Reference: http://stackoverflow.com/questions/20405965/how-to-ensure-there-is-trailing-directory-separator-in-paths
        /// </summary>
        /// <param name="path"></param>
        /// <param name="forIO">Whether to force a System.IO-based slash.</param>
        /// <returns></returns>
        public static string ToDirectoryPathFormat(this string path, bool forIO = false)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            path = path.Trim();

            string separator = forIO ? _directorySeparator : _directorySeparatorAlt;

            if (path.EndsWith(separator))
                return path;

            return string.Concat(path, separator);
        }
        
        /// <summary>
        /// Return new list of strings all lowered.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToLower(this IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                yield return item.ToLower();
            }
        }

        /// <summary>
        /// Remove provided suffix value from the end of the input string if string ends with said suffix value.
        /// </summary>
        /// <param name="input">String to check.</param>
        /// <param name="suffixToRemove">Suffix that should be removed from end of input string if present.</param>
        /// <param name="comparisonType">String comparison.</param>
        /// <returns></returns>
        public static string? TrimEnd(
            this string input, 
            string suffixToRemove, 
            StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            if (input != null && suffixToRemove != null
              && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            else 
                return input;
        }

        /// <summary>
        /// Returns string with the first character uppercased. Optionally set all remaining characters to lowercase.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lowerRemainingChars">Whether remaining characters should be lowercased. Defaults to true.</param>
        /// <returns></returns>
        public static string ToUpperFirstChar(this string value, bool lowerRemainingChars = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.Length > 1)
            {
                if (lowerRemainingChars)
                    value = value.ToLower();

                return char.ToUpper(value[0]) + value.Substring(1);
            }
            else
                return value.ToUpper();
        }

        /// <summary>
        /// Returns comma-delimited, sentence-friendly list of values from provided string list.
        /// The last item is preceeded by <paramref name="lastItemTextSeparator"/> and includes the Oxford comma.
        /// Example: ["pets", "cats", "kitties"] returns "pets, cats, and kitties"
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lastItemTextSeparator"></param>
        /// <returns></returns>
        public static string ToSentenceFriendlyText(this IEnumerable<string> list, string lastItemTextSeparator = "and")
        {
            if (!list.HasItems())
                return string.Empty;

            var items = new List<string>(list.Where(x => !string.IsNullOrWhiteSpace(x)));

            if (items.Count == 1)
                return items[0].SetNullToEmpty();

            if (items.Count == 2)
                return $"{items[0]} {lastItemTextSeparator} {items[1]}";

            var sb = new StringBuilder();

            for (int i = 1; i <= items.Count; i++)
            {
                sb.Append(items[i - 1]);
                if (i != items.Count)
                {
                    sb.Append(", "); // always have Oxford comma
                    if (i == (items.Count - 1))
                        sb.Append($"{lastItemTextSeparator} ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes all "whitespace" found between any HTML tags found in the input HTML string.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveWhitespaceBetweenHtmlMarkup(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            return Regex.Replace(html, @"\s*(<[^>]+>)\s*", "$1", RegexOptions.Singleline);
        }

        /// <summary>
        /// Replaces any <see cref="Environment.NewLine"/> and <see cref="EncodedNewLine"/> found in input string
        /// with propery HTML tags (p and br)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeClosingParagraphs">Whether to wrap result inside a paragraph tag.</param>
        /// <returns></returns>
        public static string ReplaceNewLinesForHtml(this string value, bool includeClosingParagraphs = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            value = value.Replace(Environment.NewLine + Environment.NewLine, "</p><p>")
                        .Replace(EncodedNewLine + EncodedNewLine, "</p><p>")
                        .Replace(Environment.NewLine, "<br />")
                        .Replace(EncodedNewLine, "<br />")
                        .Replace("</p><p>", "</p>" + Environment.NewLine + "<p>");

            if (includeClosingParagraphs)
                value = string.Concat("<p>", value, "</p>");

            return value;
        }

        /// <summary>
        /// Parse and split a given string input into an array of values.
        /// These values are from splitting on new lines, commas, spaces, and/or semi-colons.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lower">Whether results should be lowered.</param>
        /// <param name="distinct">Whether results should be filtered to only be distinct.</param>
        /// <returns></returns>
        public static string[] ParseToArray(this string value, bool lower = true, bool distinct = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new string[] { };

            var values = value.Split(new string[] { "\r", "\n", ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries)
                              .Where(x => !string.IsNullOrWhiteSpace(x))
                              .Select(x => x.Trim());

            if (lower)
                values = values.Select(x => x.ToLower());

            if (distinct)
                values = values.Distinct();

            return values.ToArray();
        }

        /// <summary>
        /// Sanitize possible user input string value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encode">Whether or not to encode the result. Only available in netstandard2.0+.</param>
        /// <returns></returns>
        public static string Sanitize(this string value, bool encode = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            value = RemoveNewLines(value);

            // add any other sanitation steps needed for logging string values here
            return encode ? System.Web.HttpUtility.HtmlEncode(value) : value;
        }

        /// <summary>
        /// Remove any new line characters (\r, \n) from the string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveNewLines(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// Trim the start of a string from all exact instances of trimString.
        /// Ref - https://stackoverflow.com/questions/4335878/c-sharp-trimstart-with-string-parameter
        /// </summary>
        /// <param name="target"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static string TrimStart(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString) || string.IsNullOrEmpty(trimString)) 
                return target;

            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        /// <summary>
        /// Trim the end of a string from all exact instances of trimString.
        /// Ref - https://stackoverflow.com/questions/4335878/c-sharp-trimstart-with-string-parameter 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static string TrimEnd(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString) || string.IsNullOrEmpty(trimString)) 
                return target;

            string result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }

        /// <summary>
        /// Uses regex to replace collection of spaces together to one space.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? ReplaceMultiSpacesWithSingleSpace(this string value)
        {
            if (value == null)
                return null;

            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Replaces any instaces of the provided string array with <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stringsToRemove">One or more strings to be removed from the value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string RemoveAny(this string value, params string[] stringsToRemove)
        {
            if (stringsToRemove == null || stringsToRemove.Length == 0)
                throw new ArgumentNullException(nameof(stringsToRemove));

            foreach (var s in stringsToRemove)
            {
                value = value.Replace(s, string.Empty);
            }

            return value;
        }

        /// <summary>
        /// Replaces "special" quotation characters (single and double) with the standard, non-formatted versions.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? ReplaceSpecialQuotes(this string value)
        {
            return value?.Replace("‘", "'")
                         .Replace("’", "'")
                         .Replace("“", "'")
                         .Replace("”", "'");
        }
    }
}
