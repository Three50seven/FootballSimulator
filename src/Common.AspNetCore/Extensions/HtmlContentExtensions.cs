using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace Common.AspNetCore
{
    public static class HtmlContentExtensions
    {
        /// <summary>
        /// Return encoded HTML string from the provided <paramref name="htmlContent"/>.
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <param name="htmlEncoder"></param>
        /// <returns></returns>
        public static string ToString(this IHtmlContent htmlContent, HtmlEncoder htmlEncoder)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, htmlEncoder);
            return writer.ToString();
        }
    }
}
