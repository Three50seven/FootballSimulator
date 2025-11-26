using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Core
{
    /// <summary>
    /// Ref - https://github.com/robvolk/Helpers.Net/blob/master/Src/Helpers.Net/StringHtmlExtensions.cs
    /// </summary>
    public static class StringTruncationExtensions
    {
        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters, string trailingText = null)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            // find the spot to truncate
            // count the text characters and ignore tags
            var textCount = 0;
            var charCount = 0;
            var ignore = false;
            foreach (char c in html)
            {
                charCount++;
                if (c == '<')
                    ignore = true;
                else if (!ignore)
                    textCount++;

                if (c == '>')
                    ignore = false;

                // stop once we hit the limit
                if (textCount >= maxCharacters)
                    break;
            }

            // Truncate the html and keep whole words only
            var trunc = new StringBuilder(TruncateWords(html, charCount));

            // keep track of open tags and close any tags left open
            var tags = new Stack<string>();
            var matches = Regex.Matches(trunc.ToString(),
                @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var closeTag = match.Groups["closeTag"].Value;

                    // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                    {
                        tags.Push(tag);
                    }
                    // pop from stack if close tag
                    else if (!string.IsNullOrEmpty(closeTag))
                    {
                        // pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Pop() != closeTag && tags.Count > 0)
                        { }
                    }
                }
            }

            // add the trailing text
            if (html.Length > charCount)
                trunc.Append(trailingText);

            // pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                trunc.Append("</");
                trunc.Append(tags.Pop());
                trunc.Append('>');
            }

            return trunc.ToString();
        }


        /// <summary>
        /// Truncates a string containing HTML to the first occurrence of a delimiter.
        /// </summary>
        /// <param name="html">The HTML string to truncate</param>
        /// <param name="delimiter">The delimiter</param>
        /// <param name="comparison">The delimiter comparison type</param>
        /// <returns></returns>
        public static string TruncateHtmlByDelimiter(this string html, string delimiter, StringComparison comparison = StringComparison.Ordinal)
        {
            var index = html.IndexOf(delimiter, comparison);
            if (index <= 0) return html;

            var value = html.Substring(0, index);
            return TruncateHtml(value, StripHtml(value).Length);
        }

        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Truncate/trim the end of the provided string value based on a given max character length
        /// allowed. Optionally append a closing trail like "...".
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength">Max length the string can be. The resulting string will be truncated up to that length.</param>
        /// <param name="trailingText">The trail that should be appended. Defaults to null (no trail).</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength, string trailingText = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength || maxLength <= 0)
                return value;

            return string.Concat(value.Substring(0, Math.Min(value.Length, maxLength)), trailingText.SetNullToEmpty());
        }


        /// <summary>
        /// Truncates text and discars any partial words left at the end.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters">Max length the string can be. 
        /// The resulting string will be truncated up to that length while not ending in the middle of a word.</param>
        /// <param name="trailingText">The trail that should be appended. Defaults to null (no trail).</param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters, string trailingText = null)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;

            // trunctate the text, then remove the partial word at the end
            return Regex.Replace(Truncate(text, maxCharacters),
                @"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;
        }
    }
}
