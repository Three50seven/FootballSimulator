using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common.Core
{
    public static class StringExtensions
    {
        private static readonly string _directorySeparator = Path.DirectorySeparatorChar.ToString();

        private static readonly string _directorySeparatorAlt = Path.AltDirectorySeparatorChar.ToString();

        public static string EncodedNewLine = "&#xD;&#xA;";

        //
        // Summary:
        //     Returns snake_case version of provided string. Consider using a 3rd party library
        //     like Humanizer instead of this extension method.
        //
        // Parameters:
        //   value:
        //
        //   lower:
        //     Whether to lower the string value.
        public static string ToSnakeCase(this string value, bool lower = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = string.Concat(Regex.Replace(value.Trim(), "\\s+", "_").Select(delegate (char x, int i)
            {
                string result;
                if (i <= 0 || !char.IsUpper(x))
                {
                    result = x.ToString();
                }
                else
                {
                    string[] array = new string[1];
                    char reference = '_';
                    ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(ref reference);
                    char reference2 = x;
                    array[0] = string.Concat(readOnlySpan, new ReadOnlySpan<char>(ref reference2));
                    result = string.Concat(array);
                }

                return result;
            })).Replace("__", "_");
            return lower ? value.ToLower() : value;
        }

        //
        // Summary:
        //     Find any individual words from a given string.
        //
        // Parameters:
        //   text:
        //
        //   lower:
        //     Whether resulting words in list should be lowered.
        public static IEnumerable<string> GetWords(this string text, bool lower = false)
        {
            var temp = text.SetEmptyToNull();
            if (string.IsNullOrWhiteSpace(temp))
            {
                return new string[0];
            }

            return (lower ? temp.ToLower() : temp).Split(' ');
        }

        //
        // Summary:
        //     Returns null if string is empty, null, or whitespace.
        //
        // Parameters:
        //   value:
        //
        //   trim:
        //     Whether result should be trimmed. Defaults to true.
        public static string? SetEmptyToNull(this string value, bool trim = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return trim ? value.Trim() : value;
        }

        //
        // Summary:
        //     Returns empty if string is null.
        //
        // Parameters:
        //   value:
        //
        //   trim:
        //     Whether result should be trimmed. Defaults to true.
        public static string SetNullToEmpty(this string value, bool trim = true)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return trim ? value.Trim() : value;
        }

        //
        // Summary:
        //     Remove special characters defined under NetTango.Core.RegularExpressions.SpecialCharacters
        //     from a given string input.
        //
        // Parameters:
        //   input:
        //
        //   customRegEx:
        //     Optionally provide a custom RegEx.
        public static string RemoveSpecialCharacters(this string input, string? customRegEx = null)
        {
            Regex regex = new Regex((customRegEx == null) ? "(?:[^a-z0-9 -]|(?<=['\"])s)" : customRegEx, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return regex.Replace(input, string.Empty);
        }

        //
        // Summary:
        //     Remove area of a string builder where first indexes of start and end string values
        //     are found.
        //
        // Parameters:
        //   sb:
        //
        //   start:
        //
        //   end:
        public static void RemoveArea(this StringBuilder sb, string start, string end)
        {
            int num = sb.ToString().IndexOf(start);
            int num2 = sb.ToString().IndexOf(end);
            sb.Remove(num, num2 - num + end.Length);
        }

        //
        // Summary:
        //     Attempts to pluralize a given string value under English. Consider using a 3rd
        //     party library like Humanizer over this extension method.
        //
        // Parameters:
        //   value:
        public static string ToPlural(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.EndsWith("es"))
            {
                return value;
            }

            if (value.EndsWith("y") && !value.EndsWith("ey"))
            {
                return value.Remove(value.Length - 1) + "ies";
            }

            if (value.EndsWith("s"))
            {
                return value + "es";
            }

            return value + "s";
        }

        //
        // Summary:
        //     Format a string to singular or plural based on count/int value supplied for num.
        //     If num equals 1, the string value is returned unchanged. Else, NetTango.Core.StringExtensions.ToPlural(System.String)
        //     is returned. Optionally supplied the plural suffix/ending for the plural condition
        //     using pluralEnding.
        //
        // Parameters:
        //   value:
        //     Value to format.
        //
        //   num:
        //     Number value to determine singular/plural.
        //
        //   pluralEnding:
        //     Optional plural suffix/ending for the plural condition.
        public static string FormatSingularOrPlural(this string value, int num, string? pluralEnding = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return (num == 1) ? value : (string.IsNullOrWhiteSpace(pluralEnding) ? value.ToPlural() : (value + pluralEnding));
        }

        //
        // Summary:
        //     Format a given string as a more url-friendly result. Removes any non-alphanumeric
        //     characters and replaces spaces spaces with hyphens.
        //
        // Parameters:
        //   value:
        public static string ToUrlFriendlyString(this string value)
        {
            value = value.SetNullToEmpty(trim: false);
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            string text = value.Replace("®", string.Empty).Replace("™", string.Empty).RemoveAccent()
                .ToLower();
            text = text.Replace("-", " ");
            text = Regex.Replace(text, "[^a-z0-9\\s-_]", "");
            text = Regex.Replace(text, "\\s+", " ").Trim();
            return Regex.Replace(text, "\\s", "-");
        }

        private static string RemoveAccent(this string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        //
        // Summary:
        //     Helper extension method to check and format a path to properly end with the correct
        //     directory slash separator. Reference: http://stackoverflow.com/questions/20405965/how-to-ensure-there-is-trailing-directory-separator-in-paths
        //
        //
        // Parameters:
        //   path:
        //
        //   forIO:
        //     Whether to force a System.IO-based slash.
        public static string ToDirectoryPathFormat(this string path, bool forIO = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            path = path.Trim();
            string text = (forIO ? _directorySeparator : _directorySeparatorAlt);
            if (path.EndsWith(text))
            {
                return path;
            }

            return path + text;
        }

        //
        // Summary:
        //     Return new list of strings all lowered.
        //
        // Parameters:
        //   list:
        public static IEnumerable<string> ToLower(this IEnumerable<string> list)
        {
            foreach (string item in list)
            {
                yield return item.ToLower();
            }
        }

        //
        // Summary:
        //     Remove provided suffix value from the end of the input string if string ends
        //     with said suffix value.
        //
        // Parameters:
        //   input:
        //     String to check.
        //
        //   suffixToRemove:
        //     Suffix that should be removed from end of input string if present.
        //
        //   comparisonType:
        //     String comparison.
        public static string? TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }

            return input;
        }

        //
        // Summary:
        //     Returns string with the first character uppercased. Optionally set all remaining
        //     characters to lowercase.
        //
        // Parameters:
        //   value:
        //
        //   lowerRemainingChars:
        //     Whether remaining characters should be lowercased. Defaults to true.
        public static string ToUpperFirstChar(this string value, bool lowerRemainingChars = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (value.Length > 1)
            {
                if (lowerRemainingChars)
                {
                    value = value.ToLower();
                }

                char reference = char.ToUpper(value[0]);
                return string.Concat(new ReadOnlySpan<char>(ref reference), value.Substring(1));
            }

            return value.ToUpper();
        }

        //
        // Summary:
        //     Returns comma-delimited, sentence-friendly list of values from provided string
        //     list. The last item is preceeded by lastItemTextSeparator and includes the Oxford
        //     comma. Example: ["pets", "cats", "kitties"] returns "pets, cats, and kitties"
        //
        //
        // Parameters:
        //   list:
        //
        //   lastItemTextSeparator:
        public static string ToSentenceFriendlyText(this IEnumerable<string> list, string lastItemTextSeparator = "and")
        {
            if (!list.HasItems())
            {
                return string.Empty;
            }

            List<string> list2 = [.. list.Where((string x) => !string.IsNullOrWhiteSpace(x))];
            if (list2.Count == 1)
            {
                return list2[0].SetNullToEmpty();
            }

            if (list2.Count == 2)
            {
                return $"{list2[0]} {lastItemTextSeparator} {list2[1]}";
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= list2.Count; i++)
            {
                stringBuilder.Append(list2[i - 1]);
                if (i != list2.Count)
                {
                    stringBuilder.Append(", ");
                    if (i == list2.Count - 1)
                    {
                        StringBuilder stringBuilder2 = stringBuilder;
                        StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
                        handler.AppendFormatted(lastItemTextSeparator);
                        handler.AppendLiteral(" ");
                        stringBuilder2.Append(ref handler);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        //
        // Summary:
        //     Removes all "whitespace" found between any HTML tags found in the input HTML
        //     string.
        //
        // Parameters:
        //   html:
        public static string RemoveWhitespaceBetweenHtmlMarkup(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            return Regex.Replace(html, "\\s*(<[^>]+>)\\s*", "$1", RegexOptions.Singleline);
        }

        //
        // Summary:
        //     Replaces any System.Environment.NewLine and NetTango.Core.StringExtensions.EncodedNewLine
        //     found in input string with propery HTML tags (p and br)
        //
        // Parameters:
        //   value:
        //
        //   includeClosingParagraphs:
        //     Whether to wrap result inside a paragraph tag.
        public static string ReplaceNewLinesForHtml(this string value, bool includeClosingParagraphs = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = value.Replace(Environment.NewLine + Environment.NewLine, "</p><p>").Replace(EncodedNewLine + EncodedNewLine, "</p><p>").Replace(Environment.NewLine, "<br />")
                .Replace(EncodedNewLine, "<br />")
                .Replace("</p><p>", "</p>" + Environment.NewLine + "<p>");
            if (includeClosingParagraphs)
            {
                value = "<p>" + value + "</p>";
            }

            return value;
        }

        //
        // Summary:
        //     Parse and split a given string input into an array of values. These values are
        //     from splitting on new lines, commas, spaces, and/or semi-colons.
        //
        // Parameters:
        //   value:
        //
        //   lower:
        //     Whether results should be lowered.
        //
        //   distinct:
        //     Whether results should be filtered to only be distinct.
        public static string[] ParseToArray(this string value, bool lower = true, bool distinct = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new string[0];
            }

            IEnumerable<string> source = from x in value.Split(new string[5] { "\r", "\n", ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries)
                                         where !string.IsNullOrWhiteSpace(x)
                                         select x.Trim();
            if (lower)
            {
                source = source.Select((string x) => x.ToLower());
            }

            if (distinct)
            {
                source = source.Distinct();
            }

            return source.ToArray();
        }

        //
        // Summary:
        //     Sanitize possible user input string value.
        //
        // Parameters:
        //   value:
        //
        //   encode:
        //     Whether or not to encode the result. Only available in netstandard2.0+.
        public static string Sanitize(this string value, bool encode = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = value.RemoveNewLines();
            return encode ? HttpUtility.HtmlEncode(value) : value;
        }

        //
        // Summary:
        //     Remove any new line characters (\r, \n) from the string.
        //
        // Parameters:
        //   value:
        public static string RemoveNewLines(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        //
        // Summary:
        //     Trim the start of a string from all exact instances of trimString. Ref - https://stackoverflow.com/questions/4335878/c-sharp-trimstart-with-string-parameter
        //
        //
        // Parameters:
        //   target:
        //
        //   trimString:
        public static string TrimStart(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString) || string.IsNullOrEmpty(trimString))
            {
                return target;
            }

            string text = target;
            while (text.StartsWith(trimString))
            {
                text = text.Substring(trimString.Length);
            }

            return text;
        }

        //
        // Summary:
        //     Trim the end of a string from all exact instances of trimString. Ref - https://stackoverflow.com/questions/4335878/c-sharp-trimstart-with-string-parameter
        //
        //
        // Parameters:
        //   target:
        //
        //   trimString:
        public static string TrimEnd(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString) || string.IsNullOrEmpty(trimString))
            {
                return target;
            }

            string text = target;
            while (text.EndsWith(trimString))
            {
                text = text.Substring(0, text.Length - trimString.Length);
            }

            return text;
        }

        //
        // Summary:
        //     Uses regex to replace collection of spaces together to one space.
        //
        // Parameters:
        //   value:
        public static string? ReplaceMultiSpacesWithSingleSpace(this string value)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Replace(value, "\\s+", " ");
        }

        //
        // Summary:
        //     Replaces any instaces of the provided string array with System.String.Empty.
        //
        //
        // Parameters:
        //   value:
        //
        //   stringsToRemove:
        //     One or more strings to be removed from the value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        public static string RemoveAny(this string value, params string[] stringsToRemove)
        {
            if (stringsToRemove == null || stringsToRemove.Length == 0)
            {
                throw new ArgumentNullException("stringsToRemove");
            }

            foreach (string oldValue in stringsToRemove)
            {
                value = value.Replace(oldValue, string.Empty);
            }

            return value;
        }

        //
        // Summary:
        //     Replaces "special" quotation characters (single and double) with the standard,
        //     non-formatted versions.
        //
        // Parameters:
        //   value:
        public static string? ReplaceSpecialQuotes(this string value)
        {
            return value?.Replace("‘", "'").Replace("’", "'").Replace("“", "'")
                .Replace("”", "'");
        }
    }
}
