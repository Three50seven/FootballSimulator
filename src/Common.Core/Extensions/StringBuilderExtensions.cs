using System;
using System.Linq;
using System.Text;

namespace Common.Core
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Remove any instances of provided chars if at the end of the string builder.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static StringBuilder RemoveTrailingChars(this StringBuilder sb, params char[] chars)
        {
            if (chars == null || chars.Length == 0)
                return sb;

            if (sb.Length == 0)
                return sb;
            if (!chars.Contains(sb[sb.Length - 1]))
                return sb;

            sb.Remove(sb.Length - 1, 1);
            return RemoveTrailingChars(sb, chars);
        }
    }
}
